using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.WebAPI.Auth;

namespace TooliRent.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _tokens;

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IJwtTokenService tokens)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _tokens = tokens;
        }

        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var customerLookup = await _context.Customers
                .AsNoTracking()
                .ToDictionaryAsync(c => c.UserId, c => c.Id);

            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserWithRolesDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                customerLookup.TryGetValue(user.Id, out var customerId);

                // Beräkna om användaren är aktiv
                var isActive = !user.LockoutEnabled || (user.LockoutEnd <= DateTimeOffset.UtcNow);

                // Hämta bokningar om du vill inkludera dem
                var rentals = await _context.Rentals
                    .AsNoTracking()
                    .Where(r => r.CustomerId == customerId)
                    .ToListAsync();

                var activeRentalIds = rentals
                    .Where(r => r.Status == RentalStatus.Pending || r.Status == RentalStatus.Confirmed)
                    .Select(r => r.Id)
                    .ToList();

                var pastRentalIds = rentals
                    .Where(r => r.EndDate < DateTime.UtcNow)
                    .Select(r => r.Id)
                    .ToList();

                result.Add(new UserWithRolesDto(
                    customerId,
                    user.Id,
                    user.Email ?? "",
                    user.FirstName + " " + user.LastName,
                    roles,
                    isActive,
                    activeRentalIds,
                    pastRentalIds
                ));
            }

            return Ok(result);
        }

        [HttpGet("user/{customerId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int customerId)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return NotFound(new { Errors = new[] { "Customer not found." } });

            var user = await _userManager.FindByIdAsync(customer.UserId);
            if (user == null)
                return NotFound(new { Errors = new[] { "Associated user not found." } });

            var isActive = !user.LockoutEnabled || (user.LockoutEnd <= DateTimeOffset.UtcNow);

            var rentals = await _context.Rentals
                .AsNoTracking()
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();

            var activeRentalIds = rentals
                .Where(r => r.Status == RentalStatus.Pending || r.Status == RentalStatus.Confirmed)
                .Select(r => r.Id)
                .ToList();

            var pastRentalIds = rentals
                .Where(r => r.EndDate < DateTime.UtcNow)
                .Select(r => r.Id)
                .ToList();

            return Ok(new
            {
                CustomerId = customer.Id,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FirstName + " " + user.LastName,
                IsActive = isActive,
                ActiveRentalIds = activeRentalIds,
                PastRentalIds = pastRentalIds
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return NotFound(new { Errors = new[] { "Invalid email or password." } });
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return NotFound(new { Errors = new[] { "Wrong password." } });
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokens.CreateToken(user, roles);

            return Ok(new AuthResponseDto(token));
        }

        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterDto dto)
        {
            // 1. Skapa AppUser
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            // 2. Säkerställ rollen "Customer"
            if (!await _roleManager.RoleExistsAsync("Customer"))
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

            // 3. Lägg till användaren i rollen
            await _userManager.AddToRoleAsync(user, "Customer");

            // 4. Skapa Customer via navigation property (lösning för FK-problem)
            var customer = new Customer
            {
                User = user
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // 5. Generera JWT via JwtTokenService
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokens.CreateToken(user, roles);

            // 6. Returnera resultat med unika property-namn
            return Ok(new
            {
                UserId = user.Id,
                user.Email,
                CustomerId = customer.Id,
                Roles = roles,
                Token = token
            });
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }

            // Standardroll "User"
            var roleName = "User";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            await _userManager.AddToRoleAsync(user, roleName);

            return StatusCode(201);
        }

        [HttpPost("create-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }

            // Skapa rollen "Admin" om den inte finns
            var roleName = "Admin";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Tilldela användaren rollen Admin
            await _userManager.AddToRoleAsync(user, roleName);

            return StatusCode(201);
        }

        [HttpPatch("update-role/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(string userId, [FromBody] UpdateRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Errors = new[] { "User not found." } });
            }

            // Kolla om rollen finns, annars skapa den
            if (!await _roleManager.RoleExistsAsync(dto.NewRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(dto.NewRole));
            }

            // Hämta nuvarande roller
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Ta bort alla nuvarande roller (eller bara den vi vill ersätta)
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return BadRequest(new { Errors = removeResult.Errors.Select(e => e.Description) });
            }

            // Lägg till den nya rollen
            var addResult = await _userManager.AddToRoleAsync(user, dto.NewRole);
            if (!addResult.Succeeded)
            {
                return BadRequest(new { Errors = addResult.Errors.Select(e => e.Description) });
            }

            return Ok(new { UserId = user.Id, NewRole = dto.NewRole });
        }

        [HttpPatch("toggle-active/{customerId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleUserActiveStatus(int customerId)
        {
            // Hämta customer utan att inkludera User
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                return NotFound(new { Errors = new[] { "Customer not found." } });

            // Hämta AppUser separat via UserManager
            var user = await _userManager.FindByIdAsync(customer.UserId);
            if (user == null)
                return NotFound(new { Errors = new[] { "Associated user not found." } });

            // Växla status
            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                // Aktivera användaren
                user.LockoutEnd = DateTimeOffset.UtcNow;
            }
            else
            {
                // Inaktivera användaren (t.ex. 100 år framåt)
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(new { Errors = updateResult.Errors.Select(e => e.Description) });
            }

            return Ok(new
            {
                CustomerId = customer.Id,
                UserId = user.Id,
                IsActive = !user.LockoutEnabled || (user.LockoutEnd <= DateTimeOffset.UtcNow)
            });
        }
    }
}
