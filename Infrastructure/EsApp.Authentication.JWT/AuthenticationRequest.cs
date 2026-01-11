using System;

namespace EsApp.Authentication.JWT;

public record AuthenticationRequest
(
    string user,
    string pass,
    string role,
    string name
);