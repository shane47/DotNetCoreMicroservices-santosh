using MediatR;
using System;
using System.Collections.Generic;

namespace Orders.Application.Features.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrderDto>>
    {
        public string UserName { get; set; }

        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
