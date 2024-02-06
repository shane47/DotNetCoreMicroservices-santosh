using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Orders.Application.Exceptions.ValidationException;

namespace Orders.Application.Behaviours
{
    class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationRes = await Task.WhenAll(validators.Select(async v => await v.ValidateAsync(context,
                                                            cancellationToken)));
                var failures = validationRes.SelectMany(res => res.Errors).Where(f => f != null);

                if (failures.Count() > 0)
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
