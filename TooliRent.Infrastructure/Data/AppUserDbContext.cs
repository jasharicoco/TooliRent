using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Data
{
    public class AppUserDbContext : IdentityDbContext<AppUser>
    {
        public AppUserDbContext(DbContextOptions<AppUserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- Roller ---
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            // --- Användare ---
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = "a1",
                    UserName = "admin@asd.com",
                    NormalizedUserName = "ADMIN@ASD.COM",
                    Email = "admin@asd.com",
                    NormalizedEmail = "ADMIN@ASD.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "STATIC-ADMIN-SECURITYSTAMP",
                    ConcurrencyStamp = "STATIC-ADMIN-ROLE-CONCURRENCYSTAMP",
                    CreatedAt = new DateTime(2025, 9, 4), // statiskt datum
                    FirstName = "Admin",
                    LastName = "User",
                    PasswordHash = "AQAAAAIAAYagAAAAEHfdPaPu3AoXt73wEtI9kk74dORAiPsgVJVbKJDfU6UNi2wjuO11LCYGHrCwUxlthQ=="
                },
                new AppUser
                {
                    Id = "u1",
                    UserName = "user@asd.com",
                    NormalizedUserName = "USER@ASD.COM",
                    Email = "user@asd.com",
                    NormalizedEmail = "USER@ASD.COM",
                    EmailConfirmed = true,
                    SecurityStamp = "STATIC-ADMIN-SECURITYSTAMP",
                    ConcurrencyStamp = "STATIC-ADMIN-ROLE-CONCURRENCYSTAMP",
                    CreatedAt = new DateTime(2025, 9, 4), // statiskt datum
                    FirstName = "Regular",
                    LastName = "User",
                    PasswordHash = "AQAAAAIAAYagAAAAEJHOQD8WJ9FhT7jFt5WPjdw+iA6FmLgQSsWA+9ranctpdC3Xy2v4vtign4B+sADe+g=="
                }
            );

            // --- Koppla roller ---
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "a1", RoleId = "1" },
                new IdentityUserRole<string> { UserId = "u1", RoleId = "2" }
            );
        }
    }
}
