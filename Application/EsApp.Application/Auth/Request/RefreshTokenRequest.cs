using System;

namespace EsApp.Application.Auth.Request;

public record RefreshTokenRequest
(
    string Token,
    string RefreshToken
);