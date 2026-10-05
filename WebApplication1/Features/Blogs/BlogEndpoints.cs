using WebApplication1.Models.Dtos;
using WebApplication1.Services;
namespace WebApplication1.Features.Blogs;

public static class BlogEndpoints
{
    public static void MapBlogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/blogs");

        // GET: Alle blogs opvragen
        group.MapGet("/", async (IBlogService service) =>
        {
            var blogs = await service.GetAllAsync();
            return Results.Ok(blogs);
        });

        // GET: Eén blog opvragen op ID
        group.MapGet("/{id:int}", async (int id, IBlogService service) =>
        {
            var blog = await service.GetByIdAsync(id);
            return blog is not null ? Results.Ok(blog) : Results.NotFound();
        });

        // POST: Blog aanmaken
        group.MapPost("/", async (UpsertBlogDto dto, IBlogService service) =>
        {
            var created = await service.CreateAsync(dto);
            return Results.Created($"/api/blogs/{created.Id}", created);
        });

        // PUT: Blog wijzigen
        group.MapPut("/{id:int}", async (int id, UpsertBlogDto dto, IBlogService service) =>
        {
            var success = await service.UpdateAsync(id, dto);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE: Blog verwijderen
        group.MapDelete("/{id:int}", async (int id, IBlogService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}