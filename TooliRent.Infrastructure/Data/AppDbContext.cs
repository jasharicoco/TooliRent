using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tool> Tools => Set<Tool>();
        public DbSet<ToolCategory> ToolCategories => Set<ToolCategory>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Tool>().ToTable("Tool");
            builder.Entity<ToolCategory>().ToTable("ToolCategory");
            builder.Entity<Rental>().ToTable("Rental");
            builder.Entity<Payment>().ToTable("Payment");
            builder.Entity<Customer>().ToTable("Customer");
            builder.Entity<Review>().ToTable("Review");

            // Unik review per Rental
            builder.Entity<Review>()
                .HasIndex(r => r.RentalId)
                .IsUnique();

            // Customer ↔ User (1-1)
            builder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne() // Ingen navigeringsegenskap i AppUser
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ToolCategory ↔ Tool (1-M)
            builder.Entity<ToolCategory>()
                .HasMany(tc => tc.Tools)
                .WithOne(t => t.ToolCategory)
                .HasForeignKey(t => t.ToolCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tool ↔ Rental (1-M)
            builder.Entity<Tool>()
                .HasMany(t => t.Rentals)
                .WithOne(r => r.Tool)
                .HasForeignKey(r => r.ToolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer ↔ Rental (1-M)
            builder.Entity<Customer>()
                .HasMany(c => c.Rentals)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rental ↔ Payment (1-M)
            builder.Entity<Rental>()
                .HasMany(r => r.Payments)
                .WithOne(p => p.Rental)
                .HasForeignKey(p => p.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Rental ↔ Review (1-1)
            builder.Entity<Rental>()
                .HasOne(r => r.Review)
                .WithOne(rv => rv.Rental)
                .HasForeignKey<Review>(rv => rv.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enums → lagras som strängar (mer läsbart än int)
            builder.Entity<Tool>()
                .Property(t => t.Condition)
                .HasConversion<string>();

            builder.Entity<Rental>()
                .Property(r => r.Status)
                .HasConversion<string>();

            builder.Entity<Payment>()
                .Property(p => p.PaymentMethod)
                .HasConversion<string>();

            builder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}