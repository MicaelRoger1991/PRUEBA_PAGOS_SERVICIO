using System;

namespace EsApp.Domain.Payments;

public interface IPaymentsRepository
{
    Task<Guid> CreateAsync(Payment payment);
    Task<IReadOnlyList<Payment>> GetByCustomerIdAsync(Guid customerId);
}
