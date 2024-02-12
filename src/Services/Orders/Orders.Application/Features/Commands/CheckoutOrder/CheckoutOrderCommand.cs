using MediatR;
using Orders.Application.Features.Queries.GetOrdersList;


namespace Orders.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>
    {
        public OrderDto OrderDto { get; set; }
    }
}
