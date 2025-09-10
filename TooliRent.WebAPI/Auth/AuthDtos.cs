namespace TooliRent.WebAPI.Auth
{
    public record RegisterDto(string Email, string Password, string FirstName, string LastName);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string AccessToken);
    public record UpdateRoleDto(string NewRole);
    public record UserWithRolesDto(
    int? CustomerId,
    string Id,
    string Email,
    string FullName,
    IEnumerable<string> Roles,
    bool IsActive,
    IEnumerable<int> ActiveRentalIds,
    IEnumerable<int> PastRentalIds);
}
