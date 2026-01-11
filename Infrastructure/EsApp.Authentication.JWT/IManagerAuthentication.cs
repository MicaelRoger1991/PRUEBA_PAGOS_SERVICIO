using System;

namespace EsApp.Authentication.JWT;

public interface IManagerAuthentication
{
    AuthenticationToken getToken(AuthenticationRequest request);
    bool getValidateTokenFromRefresh(string token);
    AuthenticationToken getRefreshToken(AuthenticationToken request, AuthenticationRequest requestLogin);
}
