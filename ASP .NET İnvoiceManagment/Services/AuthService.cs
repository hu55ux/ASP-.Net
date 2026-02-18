using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASP_.NET_InvoiceManagment.DTOs.Auth;
using ASP_.NET_InvoiceManagment.Models;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ASP_.NET_InvoiceManagment.Services;

/// <summary>
/// Service implementation for handling authentication-related business logic.
/// This class manages user registration, credential verification, and JWT generation.
/// </summary>
public class AuthService : IAuthService
{

    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="userManager"></param>
    public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
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
    /// <exception cref="NotImplementedException">Currently thrown as the method is not yet implemented.</exception>
    public async Task<AuthResponseDTO> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        // İstifadəçi yoxdursa və ya şifrə yanlışdırsa (nida işarəsinə diqqət!)
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid login or password!");
        }

        return CreateTokenAsync(user);
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
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User registration failed: {errors}");
        }

        return new AuthResponseDTO
        {
            Email = user.Email
        };
    }

    private async Task<AuthResponseDTO> CreateTokenAsync(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("JWTSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationInMinutes = int.Parse(jwtSettings["ExpirationInMinutes"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokens = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
            signingCredentials: credentials);

        var tokenString=

        return new AuthResponseDTO
        {
            AccessToken=
        }


    }
}