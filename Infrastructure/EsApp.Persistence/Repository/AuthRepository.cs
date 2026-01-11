using EsApp.Domain.Auth;
using EsApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly EsAppDbContext _dbContext;

    public AuthRepository(EsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Auth> GetLoginByUserAndPasswordAsync(string userRegistration, string password)
    {
        var userLogin = await _dbContext.UsersMaster
            .AsNoTracking()
            .Where(user => user.UserRegistration == userRegistration && user.Password == password)
            .Select(user => Auth.Create(
                user.UsersMasterId,
                user.LastName,
                user.FirstName,
                user.UserRegistration,
                user.Role
            ))
            .FirstOrDefaultAsync();

        return userLogin!;
    }
}
