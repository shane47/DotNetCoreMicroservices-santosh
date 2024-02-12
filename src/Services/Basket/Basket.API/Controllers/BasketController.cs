using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using System;
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
        private readonly IPublishEndpoint publishTransmit;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IPublishEndpoint publishTransmit)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            this.publishTransmit = publishTransmit ?? throw new ArgumentNullException(nameof(publishTransmit));
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckoutEvent basketEvent)
        {
            var basket = await repository.GetBasketAsync(basketEvent.OrdersDto.UserName);

            // if basket is null 
            if (basket == null)
                return BadRequest();

            basketEvent.OrdersDto.TotalPrice = basket.TotalPrice;
            await publishTransmit.Publish(basketEvent);
            // remove the basket
            await repository.DeleteBasketAsync(basket.UserName);

            return Accepted();
        }
    }
}
