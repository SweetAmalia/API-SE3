namespace WebApplication1.Models.Entities;

public class Project
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public string Omschrijving { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public int Jaar { get; set; }
    public string Link { get; set; } = string.Empty;

    // EF Core heeft een parameterloze constructor nodig
    public Project() { }

    public Project(string naam, string omschrijving, string duration, string status, string categorie, int jaar, string link)
    {
        Naam = naam;
        Omschrijving = omschrijving;
        Duration = duration;
        Status = status;
        Categorie = categorie;
        Jaar = jaar;
        Link = link;
    }

}