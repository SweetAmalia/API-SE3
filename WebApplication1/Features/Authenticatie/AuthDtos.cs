namespace WebApplication1.Features.Authenticatie;

public record RegisterDto(string Email, string Password);
public record LoginDto(string Email, string Password);
public record UserInfoDto(string Id, string Email, IEnumerable<string> Roles);