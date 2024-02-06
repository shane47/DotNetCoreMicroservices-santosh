using MediatR;
using Orders.Application.Features.Queries.GetOrdersList;

namespace Orders.Application.Features.Commands.UpdateOrder 
{ 
    public class UpdateOrderCommand : IRequest
    {
        public OrderDto OrderDto { get; set; }
    }
}
