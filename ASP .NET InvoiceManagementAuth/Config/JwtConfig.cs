namespace ASP_.NET_InvoiceManagementAuth.Config;

/// <summary>
/// Represents the configuration settings for JSON Web Token (JWT) generation and validation.
/// This class is bound to the "JWTSettings" section in the application configuration.
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// The name of the configuration section in appsettings.json.
    /// </summary>
    public const string SectionName = "JWTSettings";

    /// <summary>
    /// The primary secret key used to sign and verify Access Tokens.
    /// Should be a long, random string stored securely.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// An additional secret key used specifically for Refresh Token validation logic, 
    /// adding an extra layer of security.
    /// </summary>
    public string RefreshTokenSecretKey { get; set; } = string.Empty;

    /// <summary>
    /// The legitimate issuer of the token (typically the URL of the API).
    /// Used to verify that the token was created by this system.
    /// </summary>
    /// <example>https://api.invoicemanagement.com</example>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The intended recipient of the token. 
    /// Used to ensure the token is being used by the correct client application.
    /// </summary>
    /// <example>https://invoicemanagement-app.com</example>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// The duration in minutes for which an Access Token remains valid.
    /// Short lifetimes (e.g., 15 minutes) are recommended for security.
    /// </summary>
    public int ExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// The duration in days for which a Refresh Token remains valid.
    /// These are longer-lived to allow users to stay logged in across sessions.
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}