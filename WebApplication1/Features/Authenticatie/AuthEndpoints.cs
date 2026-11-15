using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Features.Authenticatie;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        // Built-in Identity API endpoints (/login, /refresh, etc.)
        group.MapIdentityApi<IdentityUser>();

        // Custom registration endpoint assigning 'Gast' role
        group.MapPost("/register-gast", async (RegisterDto dto, UserManager<IdentityUser> userManager) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return Results.BadRequest(new { message = "Email and password are required." });
            }

            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return Results.BadRequest(new { message = "User already exists." });
            }

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            await userManager.AddToRoleAsync(user, "Gast");

            return Results.Ok(new { message = "User registered successfully with Gast role." });
        });
    }
}