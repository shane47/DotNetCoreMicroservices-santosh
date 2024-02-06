using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Orders.Application.Contracts.Persistence;
using Orders.Application.Exceptions;
using Orders.Application.Features.Commands.CheckoutOrder;
using Orders.Application.Features.Queries.GetOrdersList;
using Orders.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.Application.Features.Commands.UpdateOrder
{
    class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly ILogger<UpdateOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepos;
        private readonly IMapper mapper;

        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepos,
            IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.orderRepos = orderRepos ?? throw new ArgumentNullException(nameof(orderRepos));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await orderRepos.GetByIdAsync(request.OrderDto.Id);
            if (null == orderToUpdate)
            {
                logger.LogError($"No such order : {request.OrderDto.Id} exist");
                throw new NotFoundException(nameof(Order), request.OrderDto.Id);
            }

            mapper.Map(request.OrderDto, orderToUpdate,  typeof(OrderDto), typeof(Order));
            await orderRepos.UpdateAsync(orderToUpdate);

            logger.LogInformation($"Update Order completed Successfully");

            return Unit.Value;
        }
    }
}
