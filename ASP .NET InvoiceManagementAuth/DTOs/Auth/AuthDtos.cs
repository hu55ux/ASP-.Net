namespace ASP_.NET_InvoiceManagementAuth.DTOs.Auth;

/// <summary>
/// RegisterRequest: This class represents the data transfer object (DTO) for user registration.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// FirstName
    /// </summary>
    /// <example>John</example>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// LastName
    /// </summary>
    /// <example>Doe</example>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// Email
    /// </summary>
    /// <example>john@doe.com</example>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Password
    /// </summary>
    /// <example>Pass123</example>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// Confirmed Password
    /// </summary>
    /// <example>Pass123</example>
    public string ConfirmedPassword { get; set; } = string.Empty;
}


/// <summary>
/// LoginRequest: This class represents the data transfer object (DTO) for user login.
/// </summary>
public class LoginRequest
{

    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    /// <example>someString@gmail.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user. This property 
    /// is required for login and is used to authenticate the user when they log in to the system.
    /// </summary>
    /// <example> "Password123!"</example>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// AuthResponseDTO: This class represents the data transfer object 
/// (DTO) for the response returned after a successful authentication (login).
/// </summary>
public class AuthResponseDTO
{

    /// <summary>
    /// Access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token Expires date 
    /// </summary>
    public DateTimeOffset ExpiresAt { get; set; }

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// RefreshToken Expires date 
    /// </summary>
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }

    /// <summary>
    /// User Email
    /// </summary>
    /// <example>john@doe.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Roles of the user. This property is a collection of 
    /// strings that represents the roles assigned to the authenticated user.
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}

/// <summary>
/// RefreshTokenRequest: This class represents the data transfer object 
/// (DTO) for requesting a new access token using a refresh token.
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// String property named "RefreshToken" that is used to store the refresh token value.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
