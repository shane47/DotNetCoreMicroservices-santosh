using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await redisCache.GetAsync(userName);
            if (null == basket)
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart cart)
        {
            await redisCache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
            return await GetBasketAsync(cart.UserName);
        }
        public async Task DeleteBasketAsync(string userName)
        {
            await redisCache.RemoveAsync(userName);
        }
    }
}
