using System;
using EsApp.Domain.Payments;
using EsApp.Persistence.Context;
using EsApp.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Repository;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly EsAppDbContext _dbContext;

    public PaymentsRepository(EsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(Payment payment)
    {
        var paymentId = payment.paymentId == Guid.Empty ? Guid.NewGuid() : payment.paymentId;
        var entity = new PaymentsServicesEntity
        {
            PaymentsId = paymentId,
            UsersMasterId = payment.usersMasterId,
            CustomerId = payment.customerId,
            ServiceProviderId = payment.serviceProviderId,
            Amount = payment.amount,
            Status = payment.status,
            StateRecord = "A",
            CreationDate = payment.createdAt
        };

        _dbContext.PaymentsServices.Add(entity);
        await _dbContext.SaveChangesAsync();
        return paymentId;
    }

    public async Task<IReadOnlyList<Payment>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbContext.PaymentsServices
            .AsNoTracking()
            .Include(item => item.ServiceProvider)
            .Where(item => item.StateRecord == "A" && item.CustomerId == customerId)
            .OrderByDescending(item => item.CreationDate)
            .Select(item => new Payment
            {
                paymentId = item.PaymentsId,
                customerId = item.CustomerId,
                serviceProviderId = item.ServiceProviderId,
                amount = item.Amount,
                status = item.Status,
                createdAt = item.CreationDate,
                serviceProvider = item.ServiceProvider.Service
            })
            .ToListAsync();
    }
}
