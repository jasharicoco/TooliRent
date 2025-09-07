using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tool> Tools => Set<Tool>();
        public DbSet<ToolCategory> ToolCategories => Set<ToolCategory>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tabellnamn
            builder.Entity<Tool>().ToTable("Tool");
            builder.Entity<ToolCategory>().ToTable("ToolCategory");
            builder.Entity<Rental>().ToTable("Rental");
            builder.Entity<Payment>().ToTable("Payment");
            builder.Entity<Customer>().ToTable("Customer");
            builder.Entity<Review>().ToTable("Review");

            // Index / relationer
            builder.Entity<Review>()
                .HasIndex(r => r.RentalId)
                .IsUnique();

            builder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ToolCategory>()
                .HasMany(tc => tc.Tools)
                .WithOne(t => t.ToolCategory)
                .HasForeignKey(t => t.ToolCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tool>()
                .HasMany(t => t.Rentals)
                .WithOne(r => r.Tool)
                .HasForeignKey(r => r.ToolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Customer>()
                .HasMany(c => c.Rentals)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Rental>()
                .HasMany(r => r.Payments)
                .WithOne(p => p.Rental)
                .HasForeignKey(p => p.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Rental>()
                .HasOne(r => r.Review)
                .WithOne(rv => rv.Rental)
                .HasForeignKey<Review>(rv => rv.RentalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enum → string (mer läsbart)
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

            // Defaultvärden för timestamps
            builder.Entity<Rental>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<Rental>()
                .Property(r => r.ModifiedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Entity<Payment>()
                .Property(p => p.PaymentDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Seed kategorier
            builder.Entity<ToolCategory>().HasData(
                new ToolCategory { Id = 1, Name = "Borrmaskiner", Description = "Elektriska borrmaskiner för trä, metall och betong" },
                new ToolCategory { Id = 2, Name = "Sågar", Description = "Handsäger och elektriska sågar för olika material" },
                new ToolCategory { Id = 3, Name = "Trädgårdsverktyg", Description = "Verktyg för trädgårdsskötsel och underhåll" }
            );

            // Seed verktyg
            builder.Entity<Tool>().HasData(
                new Tool
                {
                    Id = 1,
                    Name = "Bosch Borrmaskin",
                    Description = "Slagborrmaskin 500W med flera hastigheter",
                    ImageUrl = "https://example.com/images/bosch-drill.jpg",
                    Condition = ToolCondition.Good,
                    Price = 199.00m,
                    ToolCategoryId = 1
                },
                new Tool
                {
                    Id = 2,
                    Name = "Makita Slagborr",
                    Description = "Kraftfull slagborrmaskin för betong",
                    ImageUrl = "https://example.com/images/makita-drill.jpg",
                    Condition = ToolCondition.New,
                    Price = 299.00m,
                    ToolCategoryId = 1
                },
                new Tool
                {
                    Id = 3,
                    Name = "Cirkelsåg",
                    Description = "Elektrisk cirkelsåg för precis kapning",
                    ImageUrl = "https://example.com/images/circular-saw.jpg",
                    Condition = ToolCondition.Good,
                    Price = 249.00m,
                    ToolCategoryId = 2
                },
                new Tool
                {
                    Id = 4,
                    Name = "Handsåg",
                    Description = "Klassisk handsåg för trä",
                    ImageUrl = null,
                    Condition = ToolCondition.Used,
                    Price = 49.00m,
                    ToolCategoryId = 2
                },
                new Tool
                {
                    Id = 5,
                    Name = "Grästrimmer",
                    Description = "Elektrisk trimmer för gräskanter",
                    ImageUrl = "https://example.com/images/grass-trimmer.jpg",
                    Condition = ToolCondition.Good,
                    Price = 149.00m,
                    ToolCategoryId = 3
                },
                new Tool
                {
                    Id = 6,
                    Name = "Häcksax",
                    Description = "Elektrisk häcksax för buskar och häckar",
                    ImageUrl = null,
                    Condition = ToolCondition.New,
                    Price = 179.00m,
                    ToolCategoryId = 3
                }
            );
        }
    }
}
