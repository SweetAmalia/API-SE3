using WebApplication1.Models.Dtos;

namespace WebApplication1.Services;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(UpsertStudentDto dto);
    Task<bool> UpdateAsync(int id, UpsertStudentDto dto);
    Task<bool> DeleteAsync(int id);
}