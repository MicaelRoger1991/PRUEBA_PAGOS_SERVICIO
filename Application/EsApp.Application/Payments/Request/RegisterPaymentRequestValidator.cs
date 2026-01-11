using FluentValidation;

namespace EsApp.Application.Payments.Request;

public class RegisterPaymentRequestValidator : AbstractValidator<RegisterPaymentRequest>
{
    public RegisterPaymentRequestValidator()
    {
        RuleFor(x => x.customerId).NotEmpty();
        RuleFor(x => x.serviceProviderId).NotEmpty();
        RuleFor(x => x.amount).GreaterThan(0);
    }
}
