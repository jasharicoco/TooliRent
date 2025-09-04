namespace TooliRent.WebAPI.Auth
{
    public record RegisterDto(string Email, string Password, string FirstName, string LastName);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string AccessToken);
    public record UpdateRoleDto(string NewRole);
    public record UserWithRolesDto(string Id, string Email, string FullName, IEnumerable<string> Roles, int? CustomerId);
}
