using System;
using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Domain.Customers;

public static class CustomersError
{
    public static readonly Error NotFound = new(
        "Customers.NotFound",
        "Cliente no encontrado."
    );
}
