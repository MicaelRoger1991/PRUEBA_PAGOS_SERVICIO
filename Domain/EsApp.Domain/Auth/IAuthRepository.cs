using System;

namespace EsApp.Domain.Auth;

public interface IAuthRepository
{
    Task<Auth> GetLoginByUserAndPasswordAsync(string userRegistration, string password);
}
