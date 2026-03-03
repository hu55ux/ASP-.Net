namespace ASP_.NET_InvoiceManagementAuth.Models;

/// <summary>
/// Represents a persistent refresh token stored in the database to manage long-lived user sessions.
/// This entity allows the system to issue new Access Tokens without requiring user re-authentication.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Unique identifier for the refresh token record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The unique identifier (JTI) of the JWT Access Token that this refresh token is associated with.
    /// Used to prevent "Replay Attacks."
    /// </summary>
    public string JwtId { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the user who owns this token.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp when this refresh token will no longer be valid.
    /// </summary>
    public DateTimeOffset ExpiresAt { get; set; }

    /// <summary>
    /// The timestamp when the refresh token was initially issued.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The timestamp when the token was manually revoked (e.g., during logout or security breach).
    /// If null, the token has not been revoked.
    /// </summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>
    /// If this token was swapped for a new one (Token Rotation), this stores the ID of the new token.
    /// </summary>
    public string? ReplacedByJwtId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the token has been explicitly revoked.
    /// </summary>
    public bool IsRevoked => RevokedAt.HasValue;

    /// <summary>
    /// Gets a value indicating whether the token has passed its expiration date.
    /// </summary>
    public bool IsExpired => ExpiresAt <= DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets a value indicating whether the token is currently valid and usable.
    /// A token is active only if it is not revoked and not expired.
    /// </summary>
    public bool IsActive => !IsRevoked && !IsExpired;
}