using ASP_.NET_InvoiceManagment.Common;
using ASP_.NET_InvoiceManagment.DTOs.Auth;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Authorization controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="authService"></param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(request);
        return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result));
    }

    /// <summary>
    /// Logs in a user and returns a JWT token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDTO>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginUserAsync(request);
        return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result));
    }
}