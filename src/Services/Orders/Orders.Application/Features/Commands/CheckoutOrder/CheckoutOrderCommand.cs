using MediatR;
using Orders.Application.Features.Queries.GetOrdersList;
using Orders.Application.Mappings;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>
    {
        public OrderDto OrderDto { get; set; }
    }
}
