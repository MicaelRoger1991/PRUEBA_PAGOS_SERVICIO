using System;

namespace EsApp.Application.Payments.Request;

public record RegisterPaymentRequest
(
    Guid customerId,
    Guid serviceProviderId,
    decimal amount
);
