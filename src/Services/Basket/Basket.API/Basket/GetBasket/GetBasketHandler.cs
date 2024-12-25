using Basket.API.Models;
using BuildingBlock.CQRS;

namespace Basket.API.Basket.GetBasket;
public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);
public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        //TODO: get basket from database
        //var basket = await _repository.GetBasket(request.UserName)

        return new GetBasketResult(new ShoppingCart("swn"));
    }
}
