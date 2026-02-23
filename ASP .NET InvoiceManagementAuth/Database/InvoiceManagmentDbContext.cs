using ASP_.NET_InvoiceManagementAuth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_InvoiceManagementAuth.Database;

/// <summary>
/// The database context for the Invoice Management system.
/// Handles the configuration and mapping of Customer, Invoice, and InvoiceRow entities.
/// </summary>
public class InvoiceManagmentDbContext : IdentityDbContext<AppUser>
{
    /// <summary>
    /// Constructor for the InvoiceManagmentDbContext class. It takes 
    /// DbContextOptions as a parameter and passes it to the base DbContext constructor.
    /// </summary>
    /// <param name="options"></param>
    public InvoiceManagmentDbContext(DbContextOptions options) : base(options)
    { }

    /// <summary>
    /// Customers: Represents the customers in the system. Each customer can have multiple invoices.
    /// </summary>
    public DbSet<Customer> Customers => Set<Customer>();

    /// <summary>
    /// Invoices: Represents the invoices in the system. Each invoice is associated with 
    /// one customer and can have multiple invoice rows.
    /// </summary>
    public DbSet<Invoice> Invoices => Set<Invoice>();

    /// <summary>
    /// InvoiceRows: Represents the individual line items on an invoice. Each row is associated with
    /// invoice and contains details about the service provided, quantity, amount, and calculated sum.
    /// </summary>
    public DbSet<InvoiceRow> InvoiceRows => Set<InvoiceRow>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>
    /// Configures the database schema and entity relationships using Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

            customer.HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Invoice>(invoice =>
        {
            invoice.HasKey(i => i.Id);

            invoice.Property(i => i.Status)
                .IsRequired();

            invoice.Property(i => i.TotalSum)
                .HasPrecision(18, 2);

            invoice.Property(i => i.StartDate)
                .IsRequired();

            invoice.Property(i => i.EndDate)
                .IsRequired();

            invoice.Property(i => i.Comment)
                .HasMaxLength(500);

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

            row.Property(r => r.Quantity)
                .HasPrecision(18, 3);

            row.Property(r => r.Amount)
                .HasPrecision(18, 2);

            row.Property(r => r.Sum)
               .HasPrecision(18, 2)
               .HasComputedColumnSql("[Quantity] * [Amount]");

        });

        modelBuilder.Entity<RefreshToken>(
            refresh =>
            {
                refresh.HasKey(rt => rt.Id);
                refresh.HasIndex(rt => rt.JwtId).IsUnique();
                refresh.Property(rt => rt.JwtId).IsRequired().HasMaxLength(64);
                refresh.Property(rt => rt.UserId).IsRequired().HasMaxLength(64);
            }
            );

    }
}