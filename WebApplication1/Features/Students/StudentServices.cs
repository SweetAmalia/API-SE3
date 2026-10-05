using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;

namespace WebApplication1.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        return await _context.Students
            .Select(s => new StudentDto(s.Id, s.Studentnummer, s.Naam))
            .ToListAsync();
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var s = await _context.Students.FindAsync(id);
        if (s == null) return null;

        return new StudentDto(s.Id, s.Studentnummer, s.Naam);
    }

    public async Task<StudentDto> CreateAsync(UpsertStudentDto dto)
    {
        var student = new Student
        {
            Studentnummer = dto.Studentnummer,
            Naam = dto.Naam
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return new StudentDto(student.Id, student.Studentnummer, student.Naam);
    }

    public async Task<bool> UpdateAsync(int id, UpsertStudentDto dto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return false;

        student.Studentnummer = dto.Studentnummer;
        student.Naam = dto.Naam;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}