using ASP_.NET_InvoiceManagementAuth.DTOs;

namespace ASP_.NET_InvoiceManagementAuth.Services.Interfaces;

/// <summary>
/// Defines the contract for authentication services, including user registration, 
/// login, and secure token lifecycle management.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user in the system and initializes their account.
    /// </summary>
    /// <param name="request">The registration details including email, password, and profile data.</param>
    /// <returns>An <see cref="AuthResponseDTO"/> containing the newly created user info and initial access tokens.</returns>
    Task<AuthResponseDTO> RegisterUserAsync(RegisterRequest request);

    /// <summary>
    /// Authenticates a user based on their credentials (email and password).
    /// </summary>
    /// <param name="request">The login credentials provided by the user.</param>
    /// <returns>An <see cref="AuthResponseDTO"/> containing identity tokens if successful; otherwise, failure details.</returns>
    Task<AuthResponseDTO> LoginUserAsync(LoginRequest request);

    /// <summary>
    /// Generates a new Access Token using a valid Refresh Token. 
    /// This allows users to stay logged in without re-entering credentials.
    /// </summary>
    /// <param name="refreshTokenRequest">The request containing the expired access token and the valid refresh token.</param>
    /// <returns>A new <see cref="AuthResponseDTO"/> with updated tokens, or null if the refresh token is invalid/expired.</returns>
    Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

    /// <summary>
    /// Revokes a specific refresh token, effectively logging the user out from that session.
    /// </summary>
    /// <param name="refreshToken">The unique token string to be invalidated in the database.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RevokeRefreshTokenAsync(string refreshToken);
}