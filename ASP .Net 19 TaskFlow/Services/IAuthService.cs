using ASP_.Net_19_TaskFlow.DTOs;

namespace ASP_.Net_19_TaskFlow.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequest registerRequest);
    Task<AuthResponseDto> LoginAsync(LoginRequest loginRequest);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task RevokeRefreshTokenAsync(string refreshToken);

}
