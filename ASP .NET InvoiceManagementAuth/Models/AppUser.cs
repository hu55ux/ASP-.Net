using Microsoft.AspNetCore.Identity;

namespace ASP_.NET_InvoiceManagementAuth.Models;

/// <summary>
/// Represents a customized identity user within the system. 
/// Extends the base <see cref="IdentityUser"/> to include profile-specific 
/// information and auditing timestamps.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the user's legal first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's legal last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Additional contact information for the user, such as a physical address or mailing address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the user account was created.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the user profile was last updated.
    /// Returns null if the profile has not been modified since creation.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; } = null!;
}