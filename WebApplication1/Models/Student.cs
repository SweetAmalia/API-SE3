using System.Collections.Generic;

namespace WebApplication1.Models;

public class Student 
{
    public int Id { get; set; }
    public string Studentnummer { get; set; }
    public string Naam { get; set; }

    public static List<Student> Students { get; set; } = new()
    {
        new Student(25012345L, "Jelle") { Studentnummer = "25012345" }
    };

    public Student(long id, string naam)
    {
        Id = (int)id;
        Naam = naam;
    }
}