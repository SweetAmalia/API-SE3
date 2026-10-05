using WebApplication1.Models.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Features.Students;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/students");

        // GET: Alle studenten opvragen
        group.MapGet("/", async (IStudentService service) =>
        {
            var students = await service.GetAllAsync();
            return Results.Ok(students);
        });

        // GET: Eén student opvragen op ID
        group.MapGet("/{id:int}", async (int id, IStudentService service) =>
        {
            var student = await service.GetByIdAsync(id);
            return student is not null ? Results.Ok(student) : Results.NotFound();
        });

        // POST: Student aanmaken
        group.MapPost("/", async (UpsertStudentDto dto, IStudentService service) =>
        {
            var created = await service.CreateAsync(dto);
            return Results.Created($"/api/students/{created.Id}", created);
        });

        // PUT: Student wijzigen
        group.MapPut("/{id:int}", async (int id, UpsertStudentDto dto, IStudentService service) =>
        {
            var success = await service.UpdateAsync(id, dto);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE: Student verwijderen
        group.MapDelete("/{id:int}", async (int id, IStudentService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}