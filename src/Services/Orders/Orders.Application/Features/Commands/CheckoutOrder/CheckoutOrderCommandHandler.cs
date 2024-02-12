using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Orders.Application.Contracts.Models.EmailService;
using Orders.Application.Contracts.Persistence;
using Orders.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly ILogger<CheckoutOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepos;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public CheckoutOrderCommandHandler(ILogger<CheckoutOrderCommandHandler> logger, IOrderRepository orderRepos,
            IMapper mapper, IEmailService emailService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            this.orderRepos = orderRepos ?? throw new ArgumentNullException(nameof(orderRepos)); ;
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService)); ;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = mapper.Map<Order>(request.OrderDto);
            var newOrder = await orderRepos.AddAsync(orderEntity);

            logger.LogInformation($"Order {newOrder.Id} is Successfully created");

            var email = new Email() { To = "ezozkme@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            //try
            //{
            //    // await emailService.SendMailAsync(email);
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError($"Order {request.OrderDto.Id} failed due to an error with the mail service: {ex.Message}");
            //}

            return newOrder.Id;
        }
    }
}
