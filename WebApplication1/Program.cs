using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Features.Projects;
using WebApplication1.Features.Blogs;
using WebApplication1.Features.Authenticatie;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Datalaag (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity API
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cookies configureren zodat we 401 Unauthorized en 403 Forbidden teruggeven in plaats van redirects naar login pagina's
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// RBAC Policy Noteren
builder.Services.AddAuthorizationBuilder()
.AddPolicy("RequireEditorOrAdmin", policy => policy.RequireRole("Editor", "Admin"))
.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));

// Registreer je Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map de Minimal API endpoints voor Projecten, Blogs en Authenticatie
app.MapAuthEndpoints();
app.MapProjectEndpoints();
app.MapBlogEndpoints();

app.Run();