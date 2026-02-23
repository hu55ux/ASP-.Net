using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASP_.NET_InvoiceManagementAuth.Config;
using ASP_.NET_InvoiceManagementAuth.Database;
using ASP_.NET_InvoiceManagementAuth.DTOs.Auth;
using ASP_.NET_InvoiceManagementAuth.Models;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASP_.NET_InvoiceManagementAuth.Services;

/// <summary>
/// Service implementation for handling authentication-related business logic.
/// This class manages user registration, credential verification, and JWT generation.
/// </summary>
public class AuthService : IAuthService
{

    private const string RefreshTokenType = "refresh";
    private readonly UserManager<AppUser> _userManager;
    private readonly InvoiceManagmentDbContext _context;
    private readonly JwtConfig _config;

    /// <summary>
    /// Constructor for AuthService, utilizing dependency injection to receive necessary services.
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="configuration"></param>
    /// <param name="context"></param>
    public AuthService(
        UserManager<AppUser> userManager,
        InvoiceManagmentDbContext context,
        IOptions<JwtConfig> config)
    {
        _userManager = userManager;
        _context = context;
        _config = config.Value;
    }


    /// <summary>
    /// Authenticates a user based on provided credentials and generates a session token.
    /// </summary>
    /// <remarks>
    /// The process involves:
    /// 1. Locating the user by email.
    /// 2. Verifying the hashed password.
    /// 3. Generating a JWT if the credentials are valid.
    /// </remarks>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>An <see cref="AuthResponseDTO"/> containing user details and the generated access token.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when credentials are invalid.</exception>
    public async Task<AuthResponseDTO> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateTokensAsync(user);
    }

    /// <summary>
    /// Generates JWT access and refresh tokens for a given user, including role claims and token expiration details.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<AuthResponseDTO> GenerateTokensAsync(AppUser user)
    {
        var accessToken = await GenerateAccessTokenAsync(user, _config.ExpirationMinutes);
        var (refreshEntity, refreshjwt) = await CreateRefreshTokenJwtAsync(user.Id, _config.RefreshTokenExpirationDays);

        var roles = await _userManager.GetRolesAsync(user);

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_config.ExpirationMinutes),
            RefreshToken = refreshjwt,
            RefreshTokenExpiresAt = refreshEntity.ExpiresAt,
            Email = user.Email ?? string.Empty,
            Roles = roles
        };
    }

    private async Task<(RefreshToken entity, string jwt)> CreateRefreshTokenJwtAsync(string userId, int expirationDays)
    {
        var jti = Guid.NewGuid().ToString();
        var expiresAt = DateTime.UtcNow.AddDays(expirationDays);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.RefreshTokenSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("token_type", RefreshTokenType)
        };

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

        var entity = new RefreshToken
        {
            JwtId = jti,
            UserId = userId,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
        _context.RefreshTokens.Add(entity);
        await _context.SaveChangesAsync();

        return (entity, jwtString);
    }

    private async Task<string> GenerateAccessTokenAsync(AppUser user, int accessExpirationMinutes)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email,user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(accessExpirationMinutes),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var (principal, jti) = ValidateRefreshJwtAndGetJti(refreshTokenRequest.RefreshToken);

        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.JwtId == jti);
        if (storedToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!storedToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token is no longer active.");

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        storedToken.RevokedAt = DateTimeOffset.UtcNow;

        var newTokens = await GenerateTokensAsync(user);
        var newStored = await _context.RefreshTokens
             .FirstOrDefaultAsync(rt => rt.JwtId == GetJtiFromRefreshToken(newTokens.RefreshToken));
        if (newStored is not null)
            storedToken.ReplacedByJwtId = newStored.JwtId;

        await _context.SaveChangesAsync();
        return newTokens;
    }


    private (ClaimsPrincipal principal, string jti) ValidateRefreshJwtAndGetJti(string refreshToken, bool validateLifetime = true)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.RefreshTokenSecretKey));

        var principal = handler.ValidateToken(refreshToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _config.Issuer,
            ValidateAudience = true,
            ValidAudience = _config.Audience,
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwt)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var tokenType = jwt.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
        if (tokenType != RefreshTokenType)
            throw new UnauthorizedAccessException("Invalid token type.");

        var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        return (principal, jti);
    }

    /// <summary>
    /// Handles the creation of a new user account in the system.
    /// </summary>
    /// <remarks>
    /// The process involves:
    /// 1. Checking if the email is already registered.
    /// 2. Mapping the DTO to the AppUser entity.
    /// 3. Persisting the user to the database via Identity UserManager.
    /// </remarks>
    /// <param name="request">The registration data provided by the user.</param>
    /// <returns>An <see cref="AuthResponseDTO"/> indicating successful registration and initial login state.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the user already exists or identity creation fails.</exception>
    public async Task<AuthResponseDTO> RegisterUserAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = null
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        await _userManager.AddToRoleAsync(user, "User");
        return await GenerateTokensAsync(user);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        string? jti;
        try
        {
            (_, jti) = ValidateRefreshJwtAndGetJti(refreshToken, validateLifetime: false);
        }
        catch (Exception)
        {
            return;
        }

        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.JwtId == jti);
        if (storedToken is null || !storedToken.IsActive)
            return;

        storedToken.RevokedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
    }

    private static string GetJtiFromRefreshToken(string refreshJwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(refreshJwt))
            return string.Empty;
        var jwt = handler.ReadJwtToken(refreshJwt);
        return jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;
    }

}