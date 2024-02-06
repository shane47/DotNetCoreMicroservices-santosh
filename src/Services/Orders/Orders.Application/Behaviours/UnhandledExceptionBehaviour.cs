using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TReponse> : IPipelineBehavior<TRequest, TReponse>
    {
        private readonly ILogger<TRequest> logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TReponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TReponse> next)
        {
            try
            {
                return await next();
            }
            catch(Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}", 
                    requestName, request);
                throw;
            }
        }
    }
}
