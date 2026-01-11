namespace EsApp.Authentication.JWT;

public record AuthenticationToken
(
    string token,
    string refreshToken
);