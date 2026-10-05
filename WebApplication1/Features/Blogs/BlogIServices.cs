using WebApplication1.Models.Dtos;

namespace WebApplication1.Services;

public interface IBlogService
{
    Task<IEnumerable<BlogDto>> GetAllAsync();
    Task<BlogDto?> GetByIdAsync(int id);
    Task<BlogDto> CreateAsync(UpsertBlogDto dto);
    Task<bool> UpdateAsync(int id, UpsertBlogDto dto);
    Task<bool> DeleteAsync(int id);
}

