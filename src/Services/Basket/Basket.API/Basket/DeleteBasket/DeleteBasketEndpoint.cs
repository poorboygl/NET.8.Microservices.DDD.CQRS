
using Basket.API.Basket.CheckoutBasket;

namespace Basket.API.Basket.DeleteBasket;
//public record DeleteBasketRequest(string UserName)
public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint(ILogger<CheckoutBasketEndpoints> logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (HttpContext httpContext,string userName, ISender sender) =>
        {
            var requestUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            logger.LogInformation("Sending request to {Url}", requestUrl);
            var result = await sender.Send(new DeleteBasketCommand(userName));
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        })
         .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
    }
}
