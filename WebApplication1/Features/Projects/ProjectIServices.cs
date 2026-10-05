using WebApplication1.Models.Dtos;

namespace WebApplication1.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(UpsertProjectDto dto);
    Task<bool> UpdateAsync(int id, UpsertProjectDto dto);
    Task<bool> DeleteAsync(int id);
}