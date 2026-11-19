using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Features.Authenticatie;

namespace WebApplication1.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"PortfolioTestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Verwijder de bestaande SQL Server DbContext-registratie uit Program.cs
            var dbDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                            d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>))
                .ToList();

            foreach (var descriptor in dbDescriptors)
            {
                services.Remove(descriptor);
            }

            // 2. Registreer een geïsoleerde InMemory database voor deze test-instantie
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));

            // 3. Seed rollen en standaard testgebruikers
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            db.Database.EnsureCreated();

            string[] roles = ["Gast", "Editor", "Admin"];
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }

            SeedUser(userManager, "gast@test.nl", "Test1234!", "Gast");
            SeedUser(userManager, "editor@test.nl", "Test1234!", "Editor");
            SeedUser(userManager, "admin@test.nl", "Test1234!", "Admin");
        });
    }

    private static void SeedUser(UserManager<IdentityUser> userManager, string email, string password, string role)
    {
        if (userManager.FindByEmailAsync(email).GetAwaiter().GetResult() is null)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
            }
        }
    }

    // Helper om direct een ingelogde HttpClient met sessiecookie aan te maken
    public async Task<HttpClient> CreateAuthenticatedClientAsync(string email, string password)
    {
        var client = CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true
        });

        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginDto(email, password));
        response.EnsureSuccessStatusCode();

        return client;
    }
}