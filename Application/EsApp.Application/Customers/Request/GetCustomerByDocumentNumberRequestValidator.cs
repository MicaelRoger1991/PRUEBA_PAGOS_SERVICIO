using FluentValidation;

namespace EsApp.Application.Customers.Request;

public class GetCustomerByDocumentNumberRequestValidator : AbstractValidator<GetCustomerByDocumentNumberRequest>
{
    public GetCustomerByDocumentNumberRequestValidator()
    {
        RuleFor(x => x.documentNumber).NotEmpty().MaximumLength(15);
    }
}
