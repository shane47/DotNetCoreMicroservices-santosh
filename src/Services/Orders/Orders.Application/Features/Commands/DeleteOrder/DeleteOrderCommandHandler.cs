using MediatR;
using Microsoft.Extensions.Logging;
using Orders.Application.Contracts.Persistence;
using Orders.Application.Exceptions;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.Application.Features.Commands.DeleteOrder
{
    class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly ILogger<DeleteOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepos;

        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IOrderRepository orderRepos)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.orderRepos = orderRepos ?? throw new ArgumentNullException(nameof(orderRepos));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await orderRepos.GetByIdAsync(request.Id);
            if (null == orderToDelete)
            {
                logger.LogError($"No such order : {request.Id} exist");
                throw new NotFoundException(nameof(Order),request.Id);
            }
                

            await orderRepos.DeleteAsync(orderToDelete);

            logger.LogInformation($"Deleted the Order Successfully");

            return Unit.Value;
        }
    }
}
