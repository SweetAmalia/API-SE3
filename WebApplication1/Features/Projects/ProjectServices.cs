using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;

namespace WebApplication1.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        return await _context.Projects
            .Select(p => new ProjectDto(p.Id, p.Naam, p.Omschrijving, p.Duration, p.Status, p.Categorie, p.Jaar, p.Link))
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var p = await _context.Projects.FindAsync(id);
        if (p == null) return null;

        return new ProjectDto(p.Id, p.Naam, p.Omschrijving, p.Duration, p.Status, p.Categorie, p.Jaar, p.Link);
    }

    public async Task<ProjectDto> CreateAsync(UpsertProjectDto dto)
    {
        var project = new Project
        {
            Naam = dto.Naam,
            Omschrijving = dto.Omschrijving,
            Duration = dto.Duration,
            Status = dto.Status,
            Categorie = dto.Categorie,
            Jaar = dto.Jaar,
            Link = dto.Link
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return new ProjectDto(project.Id, project.Naam, project.Omschrijving, project.Duration, project.Status, project.Categorie, project.Jaar, project.Link);
    }

    public async Task<bool> UpdateAsync(int id, UpsertProjectDto dto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return false;

        project.Naam = dto.Naam;
        project.Omschrijving = dto.Omschrijving;
        project.Duration = dto.Duration;
        project.Status = dto.Status;
        project.Categorie = dto.Categorie;
        project.Jaar = dto.Jaar;
        project.Link = dto.Link;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
}