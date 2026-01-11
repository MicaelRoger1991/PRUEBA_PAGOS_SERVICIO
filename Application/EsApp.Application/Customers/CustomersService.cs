using System;
using EsApp.Application.Customers.Request;
using EsApp.Application.Customers.Response;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.Domain.Customers;
using FluentValidation;

namespace EsApp.Application.Customers;

public class CustomersService : ICustomersService
{
    private readonly ICustomersRepository _customersRepository;

    public CustomersService(ICustomersRepository customersRepository)
    {
        _customersRepository = customersRepository;
    }

    public async Task<Result<CustomerResponse>> GetCustomerByDocumentNumberAsync(
        GetCustomerByDocumentNumberRequest request,
        IValidator<GetCustomerByDocumentNumberRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _customersRepository.GetByDocumentNumberAsync(request.documentNumber);
        if (customer == null)
        {
            return Result.Failure<CustomerResponse>(CustomersError.NotFound);
        }

        return new CustomerResponse(
            customer.customerId,
            customer.firstName,
            customer.lastName
        );
    }
}
