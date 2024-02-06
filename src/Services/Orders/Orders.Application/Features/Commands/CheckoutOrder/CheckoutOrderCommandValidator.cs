using FluentValidation;

namespace Orders.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.OrderDto).NotNull();
            RuleFor(p => p.OrderDto.UserName).NotEmpty().WithMessage("{UserName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{MaximumLength} canot be exceed");

            RuleFor(p => p.OrderDto.EmailAddress)
                .EmailAddress().WithMessage("Valid {EmailAddress} is required.")
               .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p => p.OrderDto.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
