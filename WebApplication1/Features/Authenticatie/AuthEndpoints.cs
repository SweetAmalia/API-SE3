using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApplication1.Features.Authenticatie;

public sealed record PromoteGastDto(string Email);

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        // Custom login endpoint
        group.MapPost("/login", async (LoginDto dto, SignInManager<IdentityUser> signInManager) =>
        {
            var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
            if (!result.Succeeded)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new { message = "Login successful." });
        });

        //Custom logout endpoint
        group.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok(new { message = "Logout successful." });
        }).RequireAuthorization();

        // Custom GET/me endpoints , reads ClaimsPrincipal user to verify user.Identity?.IsAuthenticated and returns user's email and ClaimTypes.Role claims or 401 unauthorized if not authenticated, allowing to restore login state after refreshing the page
        group.MapGet("/me", (ClaimsPrincipal user) =>
        {
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                return Results.Unauthorized();
            }

            var email = user.FindFirstValue(ClaimTypes.Email);
            var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            return Results.Ok(new { email, roles });
        }).RequireAuthorization();
       
        // Custom registration endpoint assigning 'Gast' role
        group.MapPost("/register", async (RegisterDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) =>
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

            if (!await roleManager.RoleExistsAsync("Gast"))
            {
                await roleManager.CreateAsync(new IdentityRole("Gast"));
            }

            await userManager.AddToRoleAsync(user, "Gast");

            return Results.Ok(new { message = "User registered successfully with Gast role." });
        });

        // Custom endpoint to assign editor role to a user, only accessible by Admins
        group.MapPost("/assign-editor", async (PromoteGastDto dto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Results.BadRequest(new { message = "Email is required." });
            }
            
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Results.NotFound(new { message = "User not found." });
            }

            if (!await roleManager.RoleExistsAsync("Editor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Editor"));
            }

            if (!await userManager.IsInRoleAsync(user, "Editor"))
            {
                var result = await userManager.AddToRoleAsync(user, "Editor");
                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }
            }

            return Results.Ok(new { message = "User assigned to Editor role successfully." });
        }).RequireAuthorization("RequireAdmin");
    }
}