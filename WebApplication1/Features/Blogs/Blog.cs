namespace WebApplication1.Models.Entities;

public class Blog 
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Inhoud { get; set; } = string.Empty;
    public DateTime PublicatieDatum { get; set; } = DateTime.UtcNow;

    // EF Core heeft een parameterloze constructor nodig
    public Blog() { }

    public Blog(string title, string content)
    {
        Title = title;
        Inhoud = content;
        PublicatieDatum  = DateTime.UtcNow;
    }
}