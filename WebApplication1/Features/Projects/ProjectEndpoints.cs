using WebApplication1.Models.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Features.Projects;


public class ProjectEndpoints 
{
    public void MapProjectEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/projects");

        // GET: Alle projecten opvragen
        group.MapGet("/", async (IProjectService service) =>
        {
            var projects = await service.GetAllAsync();
            return Results.Ok(projects);
        });

        // GET: Eén project opvragen op ID
        group.MapGet("/{id:int}", async (int id, IProjectService service) =>
        {
            var project = await service.GetByIdAsync(id);
            return project is not null ? Results.Ok(project) : Results.NotFound();
        });

        // POST: Project aanmaken
        group.MapPost("/", async (UpsertProjectDto dto, IProjectService service) =>
        {
            var created = await service.CreateAsync(dto);
            return Results.Created($"/api/projects/{created.Id}", created);
        });

        // PUT: Project wijzigen
        group.MapPut("/{id:int}", async (int id, UpsertProjectDto dto, IProjectService service) =>
        {
            var success = await service.UpdateAsync(id, dto);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE: Project verwijderen
        group.MapDelete("/{id:int}", async (int id, IProjectService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}