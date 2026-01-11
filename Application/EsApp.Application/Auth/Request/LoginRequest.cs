namespace EsApp.Application.Auth.Request;

public record LoginRequest
(
    string user,
    string password
);