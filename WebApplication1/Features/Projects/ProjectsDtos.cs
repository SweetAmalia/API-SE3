using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.Dtos;

// Wat de client meestuurt bij POST of PUT (zonder Id)
public record UpsertProjectDto(
    [property: Required(ErrorMessage = "Naam is verplicht.")] string Naam,
    [property: Required(ErrorMessage = "Omschrijving is verplicht.")] string Omschrijving,
    [property: Required(ErrorMessage = "Duration is verplicht.")] string Duration,
    [property: Required(ErrorMessage = "Status is verplicht.")] string Status,
    [property: Required(ErrorMessage = "Categorie is verplicht.")] string Categorie,
    [property: Range(1900, 2100, ErrorMessage = "Geef een geldig jaar op.")] int Jaar,
    string? Link
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
