using ASP_.NET_InvoiceManagementAuth.DTOs.Auth;

namespace ASP_.NET_InvoiceManagementAuth.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Authenticates a user based on the provided email and password. 
    /// This method checks the user's credentials against the stored user data and returns a boolean value indicating whether the authentication was successful or not.
    /// </summary>
    /// <returns>AuthResponseDTO containing the result of the authentication process, 
    /// including user information and any relevant tokens or messages.</returns>
    Task<AuthResponseDTO> RegisterUserAsync(RegisterRequest request);

    /// <summary>
    /// Logins a user by validating the provided email and password against the stored user data.
    /// </summary>
    /// <returns>AuthResponseDTO containing the result of the authentication process, 
    /// including user information and any relevant tokens or messages.</returns>
    Task<AuthResponseDTO> LoginUserAsync(LoginRequest request);
    Task<AuthResponseDTO?> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task RevokeRefreshTokenAsync(string refreshToken);
}
