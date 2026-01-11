using System;

namespace EsApp.Domain.Token;

public interface ITokenRepository
{
    Task<bool> addTokenByIdUser(UserSession token);
    Task<UserSession> GetSessionByTokenAsync(string token, string refreshToken, string user);
    Task<bool> updateTokenByIdUser(UserSession token);
}
