using ASP_.NET_InvoiceManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_InvoiceManagment.Database;

/// <summary>
/// The database context for the Invoice Management system.
/// Handles the configuration and mapping of Customer, Invoice, and InvoiceRow entities.
/// </summary>
public class InvoiceManagmentDbContext : DbContext
{
    public InvoiceManagmentDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceRow> InvoiceRows => Set<InvoiceRow>();

    /// <summary>
    /// Configures the database schema and entity relationships using Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Customer Entity Configuration ---
        modelBuilder.Entity<Customer>(customer =>
        {
            customer.HasKey(c => c.Id);

            customer.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            customer.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            // One-to-Many: Customer has many Invoices. 
            // Restrict delete to prevent losing invoices if a customer is deleted.
            customer.HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Invoice Entity Configuration ---
        modelBuilder.Entity<Invoice>(invoice =>
        {
            invoice.HasKey(i => i.Id);

            invoice.Property(i => i.Status)
                .IsRequired();

            // High precision for currency values
            invoice.Property(i => i.TotalSum)
                .HasPrecision(18, 2);

            invoice.Property(i => i.StartDate)
                .IsRequired();

            invoice.Property(i => i.EndDate)
                .IsRequired();

            invoice.Property(i => i.Comment)
                .HasMaxLength(500);

            // One-to-Many: Invoice has many Rows.
            // Cascade delete: If invoice is removed, its rows are also removed.
            invoice.HasMany(i => i.Rows)
                .WithOne()
                .HasForeignKey(r => r.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // --- InvoiceRow Entity Configuration ---
        modelBuilder.Entity<InvoiceRow>(row =>
        {
            row.HasKey(row => row.Id);

            row.Property(r => r.Service)
                .IsRequired()
                .HasMaxLength(250);

            // Precision for quantities (3 decimal places for fractional weights/hours)
            row.Property(r => r.Quantity)
                .HasPrecision(18, 3);

            row.Property(r => r.Amount)
                .HasPrecision(18, 2);

            // Calculated property mapping
            row.Property(r => r.Sum)
                .HasPrecision(18, 2);
        });
    }
}