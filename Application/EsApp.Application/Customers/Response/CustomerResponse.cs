using System;

namespace EsApp.Application.Customers.Response;

public record CustomerResponse
(
    Guid customerId,
    string firstName,
    string lastName
);
