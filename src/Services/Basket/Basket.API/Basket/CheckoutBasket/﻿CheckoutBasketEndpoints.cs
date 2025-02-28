namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoints(ILogger<CheckoutBasketEndpoints> logger) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (HttpContext httpContext, CheckoutBasketRequest request, ISender sender) =>
        {
            var requestUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            logger.LogInformation("Sending request to {Url}", requestUrl);
            var command = request.Adapt<CheckoutBasketCommand>();
            var result = await sender.Send(command);

            var response = result.Adapt<CheckoutBasketResponse>();

            return Results.Ok(response);
        })

        .WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket");

    }
}