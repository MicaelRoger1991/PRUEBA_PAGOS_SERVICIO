using EsApp.Application.Customers.Request;
using EsApp.Application.Customers.Response;
using EsApp.CROSS.Shared.Abstractions;
using FluentValidation;

namespace EsApp.Application.Customers;

public interface ICustomersService
{
    Task<Result<CustomerResponse>> GetCustomerByDocumentNumberAsync(
        GetCustomerByDocumentNumberRequest request,
        IValidator<GetCustomerByDocumentNumberRequest> validator,
        CancellationToken cancellationToken);
}
