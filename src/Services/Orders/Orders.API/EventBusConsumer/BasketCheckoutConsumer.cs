using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Orders.Application.Features.Commands.CheckoutOrder;
using System;
using System.Threading.Tasks;


namespace Orders.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<BasketCheckoutConsumer> logger;

        public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            CheckoutOrderCommand command = new() { OrderDto = context.Message.OrdersDto } ;
            var result = await mediator.Send(command);

            logger.LogInformation("BasketCheckoutEvent consumed succssfully. Created Order Id: {newOrderId}", result);
        }
    }
}
