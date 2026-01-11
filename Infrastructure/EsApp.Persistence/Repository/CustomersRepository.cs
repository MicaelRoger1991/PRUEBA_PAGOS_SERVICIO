using System;
using EsApp.Domain.Customers;
using EsApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Repository;

public class CustomersRepository : ICustomersRepository
{
    private readonly EsAppDbContext _dbContext;

    public CustomersRepository(EsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetByDocumentNumberAsync(string documentNumber)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(item => item.StateRecord == "A" && item.DocumentNumber == documentNumber)
            .Select(item => new Customer
            {
                customerId = item.CustomerId,
                firstName = item.FirstName,
                lastName = item.LastName,
                documentNumber = item.DocumentNumber
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid customerId)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(item => item.StateRecord == "A" && item.CustomerId == customerId)
            .Select(item => new Customer
            {
                customerId = item.CustomerId,
                firstName = item.FirstName,
                lastName = item.LastName,
                documentNumber = item.DocumentNumber
            })
            .FirstOrDefaultAsync();
    }
}
