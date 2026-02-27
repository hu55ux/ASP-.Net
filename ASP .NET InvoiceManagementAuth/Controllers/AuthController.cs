using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_InvoiceManagementAuth.Controllers;

/// <summary>
/// Provides endpoints for user authentication, registration, and token management.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> with the required authentication service.
    /// </summary>
    /// <param name="authService">The service handling authentication logic.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account within the system.
    /// </summary>
    /// <param name="request">The user's registration details (email, password, etc.).</param>
    /// <returns>A standard API response containing the authentication result and tokens.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(request);
        return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result));
    }

    /// <summary>
    /// Authenticates a user and issues a JWT Access Token and a Refresh Token.
    /// </summary>
    /// <param name="request">The login credentials (email and password).</param>
    /// <returns>A standard API response containing user info and security tokens.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginUserAsync(request);
        return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result));
    }

    /// <summary>
    /// Refreshes an expired access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshTokenRequest">Request containing the expired access token and current refresh token.</param>
    /// <returns>A new set of access and refresh tokens.</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenRequest);
        return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result));
    }

    /// <summary>
    /// Revokes a refresh token, effectively logging the user out of the specific session.
    /// </summary>
    /// <param name="refreshTokenRequest">Request containing the token to be revoked.</param>
    /// <returns>A confirmation message indicating the token was successfully revoked.</returns>
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Revoke([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        await _authService.RevokeRefreshTokenAsync(refreshTokenRequest.RefreshToken);
        return Ok(ApiResponse<string>.SuccessResponse("Refresh token revoked"));
    }
}