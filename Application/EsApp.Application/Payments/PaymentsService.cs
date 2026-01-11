using System;
using EsApp.Application.Payments.Request;
using EsApp.Application.Payments.Response;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.Domain.Customers;
using EsApp.Domain.Payments;
using EsApp.Domain.ServiceProvider;
using FluentValidation;

namespace EsApp.Application.Payments;

public class PaymentsService : IPaymentsService
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IServiceProviderRepository _serviceProviderRepository;
    private readonly ICustomersRepository _customersRepository;

    public PaymentsService(
        IPaymentsRepository paymentsRepository,
        IServiceProviderRepository serviceProviderRepository,
        ICustomersRepository customersRepository)
    {
        _paymentsRepository = paymentsRepository;
        _serviceProviderRepository = serviceProviderRepository;
        _customersRepository = customersRepository;
    }

    public async Task<Result<RegisterPaymentResponse>> RegisterPaymentAsync(
        RegisterPaymentRequest request,
        Guid usersMasterId,
        IValidator<RegisterPaymentRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (request.amount > 1500)
        {
            return Result.Failure<RegisterPaymentResponse>(PaymentsError.AmountExceeded);
        }

        var customer = await _customersRepository.GetByIdAsync(request.customerId);
        if (customer == null)
        {
            return Result.Failure<RegisterPaymentResponse>(PaymentsError.CustomerNotFound);
        }

        var serviceProvider = await _serviceProviderRepository.GetByIdAsync(request.serviceProviderId);
        if (serviceProvider == null)
        {
            return Result.Failure<RegisterPaymentResponse>(PaymentsError.ServiceProviderNotFound);
        }

        if (string.Equals(serviceProvider.currency, "USD", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure<RegisterPaymentResponse>(PaymentsError.CurrencyNotAllowed);
        }

        var payment = new Payment
        {
            customerId = request.customerId,
            serviceProviderId = request.serviceProviderId,
            usersMasterId = usersMasterId,
            amount = request.amount,
            status = "pendiente",
            createdAt = DateTime.UtcNow
        };

        var paymentId = await _paymentsRepository.CreateAsync(payment);
        return new RegisterPaymentResponse(paymentId, payment.status);
    }

    public async Task<Result<IReadOnlyList<PaymentResponse>>> GetPaymentsByCustomerAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var payments = await _paymentsRepository.GetByCustomerIdAsync(customerId);
        var result = payments
            .Select(item => new PaymentResponse(
                item.paymentId,
                item.serviceProvider,
                item.amount,
                item.status,
                item.createdAt
            ))
            .ToList();

        return Result.Success<IReadOnlyList<PaymentResponse>>(result);
    }
}
