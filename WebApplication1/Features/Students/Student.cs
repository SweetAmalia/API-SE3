namespace WebApplication1.Models.Entities;

public class Student 
{
    public int Id { get; set; }
    public string Studentnummer { get; set; }
    public string Naam { get; set; }

    // EF Core heeft een parameterloze constructor nodig
    public Student() { }

    public Student(string studentnummer, string naam)
    {
        Studentnummer = studentnummer;
        Naam = naam;
    }
}