using Basket.API.Basket.CheckoutBasket;
using Microsoft.AspNetCore.Http;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);
public class StoreBasketEndpoint(ILogger<CheckoutBasketEndpoints> logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (HttpContext httpContext,StoreBasketRequest request, ISender sender) =>
        {
            var requestUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            logger.LogInformation("Sending request to {Url}", requestUrl);
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<StoreBasketResponse>();
            return Results.Created($"/basket/{response.UserName}", response);
        })
        .WithName("StoreBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Store Basket")
        .WithDescription("Store Basket");
    }
}
