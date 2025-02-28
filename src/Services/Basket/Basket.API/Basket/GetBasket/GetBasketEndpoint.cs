using Basket.API.Basket.CheckoutBasket;
using Microsoft.AspNetCore.Http;

namespace Basket.API.Basket.GetBasket;
//public record GetBasketRequest(string Username);
public record GetBasketResponse(ShoppingCart Cart);
public class GetBasketEndpoint(ILogger<CheckoutBasketEndpoints> logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (HttpContext httpContext, string userName, ISender sender) =>
        {
            var requestUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            logger.LogInformation("Sending request to {Url}", requestUrl);

            var result = await sender.Send(new GetBasketQuery(userName));
            var response = result.Adapt<GetBasketResponse>();

            return Results.Ok(response);
        })
        .WithName("GetBasket")
        .Produces<GetBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Basket")
        .WithDescription("Get Basket");
    }
}
