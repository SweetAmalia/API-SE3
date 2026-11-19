using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models.Dtos;

    public record UpsertBlogDto(
        [property: Required(ErrorMessage = "Id is verplicht.")] int Id,
        [property: Required(ErrorMessage = "Title is verplicht.")] string Title,
        [property: Required(ErrorMessage = "Inhoud is verplicht.")] string Inhoud,
        [property: Required(ErrorMessage = "PublicatieDatum is verplicht.")] DateTime PublicatieDatum
    );


    public record BlogDto(
        int Id,
        string Title,
        string Inhoud,
        DateTime PublicatieDatum
    );