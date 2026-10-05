using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Dtos;
using WebApplication1.Models.Entities;

namespace WebApplication1.Services;

public class BlogService : IBlogService
{
    private readonly ApplicationDbContext _context;

    public BlogService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BlogDto>> GetAllAsync()
    {
        return await _context.Blogs
            .Select(b => new BlogDto(b.Id, b.Title, b.Inhoud, b.PublicatieDatum))
            .ToListAsync();
    }

    public async Task<BlogDto?> GetByIdAsync(int id)
    {
        var b = await _context.Blogs.FindAsync(id);
        if (b == null) return null;

        return new BlogDto(b.Id, b.Title, b.Inhoud, b.PublicatieDatum);
    }

    public async Task<BlogDto> CreateAsync(UpsertBlogDto dto)
    {
        var blog = new Blog
        {
            Title = dto.Title,
            Inhoud = dto.Inhoud,
            PublicatieDatum = dto.PublicatieDatum
        };

        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

        return new BlogDto(blog.Id, blog.Title, blog.Inhoud, blog.PublicatieDatum);
    }

    public async Task<bool> UpdateAsync(int id, UpsertBlogDto dto)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return false;

        blog.Title = dto.Title;
        blog.Inhoud = dto.Inhoud;
        blog.PublicatieDatum = dto.PublicatieDatum;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return false;

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return true;
    }
}