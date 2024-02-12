using Orders.Domain.Entities;
using Orders.Application.Features.Queries.GetOrdersList;

namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public BasketCheckoutEvent(){}

        public OrderDto OrdersDto {get; set;}
    }
}
