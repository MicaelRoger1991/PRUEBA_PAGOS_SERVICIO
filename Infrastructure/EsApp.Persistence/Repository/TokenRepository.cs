using EsApp.Domain.Token;
using EsApp.Persistence.Context;
using EsApp.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Repository;

public class TokenRepository : ITokenRepository
{
    private readonly EsAppDbContext _dbContext;

    public TokenRepository(EsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static DateTime EnsureUtc(DateTime value) =>
        value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);

    public async Task<bool> addTokenByIdUser(UserSession token)
    {
        var sessionId = token.usersSessionsMasterId == Guid.Empty ? Guid.NewGuid() : token.usersSessionsMasterId;
        var entity = new UsersSessionsMasterEntity
        {
            UsersSessionsMasterId = sessionId,
            UsersMasterId = token.usersMasterId,
            Token = token.token,
            RefreshToken = token.refreshToken,
            Description = token.description,
            DateGenerate = EnsureUtc(token.dateGenerate),
            DateExpiration = EnsureUtc(token.dateExpiration),
            AmountToken = token.amountToken,
            CreationDate = DateTime.UtcNow
        };

        _dbContext.UsersSessionsMaster.Add(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> updateTokenByIdUser(UserSession token)
    {
        var entity = await _dbContext.UsersSessionsMaster
            .FirstOrDefaultAsync(session =>
                session.UsersSessionsMasterId == token.usersSessionsMasterId &&
                session.UsersMasterId == token.usersMasterId);

        if (entity == null)
        {
            return false;
        }

        entity.Token = token.token;
        entity.RefreshToken = token.refreshToken;
        entity.Description = token.description;
        entity.DateGenerate = EnsureUtc(token.dateGenerate);
        entity.DateExpiration = EnsureUtc(token.dateExpiration);
        entity.ModificationDate = DateTime.UtcNow;

        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<UserSession> GetSessionByTokenAsync(string token, string refreshToken, string user)
    {
        if (!Guid.TryParse(user, out var userId))
        {
            return new UserSession();
        }

        var session = await _dbContext.UsersSessionsMaster
            .AsNoTracking()
            .FirstOrDefaultAsync(item =>
                item.UsersMasterId == userId &&
                item.Token == token &&
                item.RefreshToken == refreshToken);

        if (session == null)
        {
            return new UserSession();
        }

        return UserSession.GetSession(
            usersSessionsMasterId: session.UsersSessionsMasterId,
            usersMasterId: session.UsersMasterId,
            token: session.Token,
            refreshToken: session.RefreshToken,
            description: session.Description,
            dateGenerate: session.DateGenerate,
            dateExpiration: session.DateExpiration,
            amountToken: session.AmountToken
        );
    }
}
