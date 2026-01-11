namespace EsApp.Application.Auth.Response;

public record LoginResponse
(
    string token,
    string refreshToken
);