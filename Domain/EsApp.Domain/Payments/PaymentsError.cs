using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Domain.Payments;

public static class PaymentsError
{
    public static readonly Error AmountExceeded = new(
        "Payments.AmountExceeded",
        "El monto no puede ser mayor a 1500 Bs."
    );

    public static readonly Error CurrencyNotAllowed = new(
        "Payments.CurrencyNotAllowed",
        "No se permiten pagos en dólares."
    );

    public static readonly Error CustomerNotFound = new(
        "Payments.CustomerNotFound",
        "Cliente no encontrado."
    );

    public static readonly Error ServiceProviderNotFound = new(
        "Payments.ServiceProviderNotFound",
        "Servicio no encontrado."
    );
}
