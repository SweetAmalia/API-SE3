namespace WebApplication1.Models.Dtos;

    public record UpsertBlogDto(
        int Id,
        string Title,
        string Inhoud,
        DateTime PublicatieDatum
    );


    public record BlogDto(
        int Id,
        string Title,
        string Inhoud,
        DateTime PublicatieDatum
    );