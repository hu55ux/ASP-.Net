using ASP_.NET_InvoiceManagementAuth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_InvoiceManagementAuth.Database;

/// <summary>
/// The primary database context for the Invoice Management system.
/// Inherits from <see cref="IdentityDbContext{TUser}"/> to integrate ASP.NET Core Identity
/// for secure authentication alongside business data management.
/// </summary>
public class InvoiceManagmentDbContext : IdentityDbContext<AppUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceManagmentDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext (Connection string, Provider, etc.).</param>
    public InvoiceManagmentDbContext(DbContextOptions options) : base(options)
    { }

    /// <summary>
    /// Gets or sets the Customers collection. Represents the legal entities or individuals receiving invoices.
    /// </summary>
    public DbSet<Customer> Customers => Set<Customer>();

    /// <summary>
    /// Gets or sets the Invoices collection. Tracks billing cycles, total amounts, and payment status.
    /// </summary>
    public DbSet<Invoice> Invoices => Set<Invoice>();

    /// <summary>
    /// Gets or sets the InvoiceRows collection. Stores detailed line items for each individual service provided.
    /// </summary>
    public DbSet<InvoiceRow> InvoiceRows => Set<InvoiceRow>();

    /// <summary>
    /// Gets or sets the RefreshTokens collection. Manages security sessions and long-lived authentication logic.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>
    /// Configures the database schema using the Fluent API.
    /// Defines primary keys, property constraints, relationships, and computed columns.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Essential: Calls the base Identity implementation to configure Identity tables (AspNetUsers, etc.)
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.Id);

            customer.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            customer.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            // One-to-Many: A customer can have many invoices. 
            // OnDelete(Restrict) prevents deleting a customer who still has invoices linked.
            customer.HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Invoice>(invoice =>
        {
            invoice.HasKey(i => i.Id);

            invoice.Property(i => i.Status).IsRequired();

            // Precision is set for financial data accuracy (18 total digits, 2 after decimal)
            invoice.Property(i => i.TotalSum).HasPrecision(18, 2);

            invoice.Property(i => i.StartDate).IsRequired();
            invoice.Property(i => i.EndDate).IsRequired();

            invoice.Property(i => i.Comment).HasMaxLength(500);

            // One-to-Many: An invoice has many rows. 
            // OnDelete(Cascade) ensures that if an invoice is deleted, its line items are too.
            invoice.HasMany(i => i.Rows)
                .WithOne()
                .HasForeignKey(r => r.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceRow>(row =>
        {
            row.HasKey(row => row.Id);

            row.Property(r => r.Service)
                .IsRequired()
                .HasMaxLength(250);

            row.Property(r => r.Quantity).HasPrecision(18, 3);
            row.Property(r => r.Amount).HasPrecision(18, 2);

            /// <summary>
            /// Database-side calculation of the row total. 
            /// This ensures the 'Sum' is always consistent with Quantity and Amount.
            /// </summary>
            row.Property(r => r.Sum)
               .HasPrecision(18, 2)
               .HasComputedColumnSql("[Quantity] * [Amount]");
        });

        modelBuilder.Entity<RefreshToken>(refresh =>
        {
            refresh.HasKey(rt => rt.Id);

            // Unique Index on JwtId allows for rapid lookup during token rotation
            refresh.HasIndex(rt => rt.JwtId).IsUnique();

            refresh.Property(rt => rt.JwtId).IsRequired().HasMaxLength(128);
            refresh.Property(rt => rt.UserId).IsRequired().HasMaxLength(128);
        });
    }
}