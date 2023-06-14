using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace RxMed.Models
{
    public class Medicine
    {
        [Key]
        public int med_id { get; set; }

        public string? med_name { get; set; }

        public string? med_pharma { get; set; }

        public string? description { get; set; }

        public string? rx { get; set; }

        public int price { get; set; }

        public int total_reviews { get; set; }

        public int avg_reviews { get; set; }

        public bool out_of_stock { get; set; }

        public string? image_url { get; set; }

        public int quantity { get; set; }

        public List<Review>? Reviews { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }


public static class MedicineEndpoints
{
	public static void MapMedicineEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Medicine", () =>
        {
            return new [] { new Medicine() };
        })
        .WithName("GetAllMedicines")
        .Produces<Medicine[]>(StatusCodes.Status200OK);

        routes.MapGet("/api/Medicine/{id}", (int id) =>
        {
            //return new Medicine { ID = id };
        })
        .WithName("GetMedicineById")
        .Produces<Medicine>(StatusCodes.Status200OK);

        routes.MapPut("/api/Medicine/{id}", (int id, Medicine input) =>
        {
            return Results.NoContent();
        })
        .WithName("UpdateMedicine")
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Medicine/", (Medicine model) =>
        {
            //return Results.Created($"/Medicines/{model.ID}", model);
        })
        .WithName("CreateMedicine")
        .Produces<Medicine>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Medicine/{id}", (int id) =>
        {
            //return Results.Ok(new Medicine { ID = id });
        })
        .WithName("DeleteMedicine")
        .Produces<Medicine>(StatusCodes.Status200OK);
    }
}}
