using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository repository;
        private readonly DiscountGrpcService discountGrpcService;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
            this.discountGrpcService = discountGrpcService ?? throw new System.ArgumentNullException(nameof(discountGrpcService));
        }

        [HttpGet("{userName}", Name= "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket =  await repository.GetBasketAsync(userName);

            // if basket is null means a new user has been added, so returning the empty basket
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart basket)
        {
            foreach (var item in basket.Items)
                item.Price -= (await discountGrpcService.GetDiscount(item.ProductName)).Amount;

            return Ok(await repository.UpdateBasketAsync(basket));
        }

        [HttpDelete("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteBasket(string userName)
        {
            await repository.DeleteBasketAsync(userName);
        }
    }
}
