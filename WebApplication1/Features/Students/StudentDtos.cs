namespace WebApplication1.Models.Dtos;

// Wat de client meestuurt bij POST of PUT (zonder Id)
public record UpsertStudentDto(
    string Studentnummer,
    string Naam
);

// Wat de API teruggeeft aan de client (inclusief Id)
public record StudentDto(
    int Id,
    string Studentnummer,
    string Naam
);