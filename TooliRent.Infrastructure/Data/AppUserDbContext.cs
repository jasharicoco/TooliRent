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
            var adminRole = new IdentityRole("Admin")
            {
                Id = "1",
                NormalizedName = "ADMIN"
            };

            var userRole = new IdentityRole("User")
            {
                Id = "2",
                NormalizedName = "USER"
            };

            builder.Entity<IdentityRole>().HasData(adminRole, userRole);

            // --- Användare ---
            var hasher = new PasswordHasher<AppUser>();

            var adminUser = new AppUser
            {
                Id = "a1",
                UserName = "admin@asd.com",
                NormalizedUserName = "ADMIN@ASD.COM",
                Email = "admin@asd.com",
                NormalizedEmail = "ADMIN@ASD.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null!, "admin123#")
            };

            var regularUser = new AppUser
            {
                Id = "u1",
                UserName = "user@asd.com",
                NormalizedUserName = "USER@ASD.COM",
                Email = "user@asd.com",
                NormalizedEmail = "USER@ASD.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null!, "user123#")
            };

            builder.Entity<AppUser>().HasData(adminUser, regularUser);

            // --- Tilldela roller ---
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "a1", RoleId = "1" }, // admin
                new IdentityUserRole<string> { UserId = "u1", RoleId = "2" }  // user
            );
        }
    }
}
