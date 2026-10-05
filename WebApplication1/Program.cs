using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Features.Projects;
using WebApplication1.Features.Blogs;
using WebApplication1.Features.Students;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Datalaag (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registreer je Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map de Minimal API endpoints voor Studenten, Projecten en Blogs
app.MapStudentEndpoints();
app.MapProjectEndpoints();
app.MapBlogEndpoints();

app.Run();