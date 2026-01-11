using System;

namespace EsApp.Application.Payments.Response;

public record RegisterPaymentResponse
(
    Guid paymentId,
    string status
);
