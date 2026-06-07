namespace Catalog.Products.Features.GetProductByName;

//public record GetProductByNameRequest(string Name);
public record GetProductByNameResponse(IEnumerable<ProductDto> Products);

public class GetProductByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/name/{Name}", async (string Name, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByNameQuery(Name));

            var response = result.Adapt<GetProductByNameResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductByName")
        .Produces<GetProductByNameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Name")
        .WithDescription("Get Product By Name");
    }
}
