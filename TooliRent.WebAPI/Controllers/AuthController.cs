using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList(); // Hämta alla användare
            var result = new List<UserWithRolesDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserWithRolesDto(
                    user.Id,
                    user.Email ?? "",
                    user.UserName ?? "",
                    roles
                ));
            }

            return Ok(result);
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

    }
}
