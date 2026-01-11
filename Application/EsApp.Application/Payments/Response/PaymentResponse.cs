using System;

namespace EsApp.Application.Payments.Response;

public record PaymentResponse
(
    Guid paymentId,
    string serviceProvider,
    decimal amount,
    string status,
    DateTime createdAt
);
