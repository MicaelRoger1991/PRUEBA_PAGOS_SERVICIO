using System;

namespace EsApp.Domain.Customers;

public interface ICustomersRepository
{
    Task<Customer?> GetByDocumentNumberAsync(string documentNumber);
    Task<Customer?> GetByIdAsync(Guid customerId);
}
