namespace WebApplication1.Models.Dtos;

// Wat de client meestuurt bij POST of PUT (zonder Id)
public record UpsertProjectDto(
    string Naam,
    string Omschrijving,
    string Duration,
    string Status,
    string Categorie,
    int Jaar,
    string Link
);

// Wat de API teruggeeft aan de client (inclusief Id)
public record ProjectDto(
    int Id,
    string Naam,
    string Omschrijving,
    string Duration,
    string Status,
    string Categorie,
    int Jaar,
    string Link
);



