
namespace Basket.API.Data
{
    public class BasketRepository : IBasketRepository
    {
        public Task<ShoppingCart> DeleteBasket(string userName, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
