using System;
using EsApp.Application.Payments.Request;
using EsApp.Application.Payments.Response;
using EsApp.CROSS.Shared.Abstractions;
using FluentValidation;

namespace EsApp.Application.Payments;

public interface IPaymentsService
{
    Task<Result<RegisterPaymentResponse>> RegisterPaymentAsync(
        RegisterPaymentRequest request,
        Guid usersMasterId,
        IValidator<RegisterPaymentRequest> validator,
        CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<PaymentResponse>>> GetPaymentsByCustomerAsync(Guid customerId, CancellationToken cancellationToken);
}
