using System;
using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Domain.Token;

public static class TokenError
{
    public static readonly Error NotFound = new(
        "Token.NotFound",
        "No existe la sesión."
    );
    public static readonly Error InvalidSession = new(
        "Token.InvalidSession",
        "Token de sesión invalida."
    );
    public static readonly Error Expired = new(
        "Token.Expired",
        "Su sesión ha expirado, por favor inicie sesión nuevamente."
    );
    public static readonly Error NotUpdated = new(
        "Token.NotUpdated",
        "Ups, tuvimos un problema al actualizar su sesión, por favor intente nuevamente."
    );
    public static readonly Error CurrentSession = new(
        "Token.CurrentSession",
        "Su sesión sigue vigente."
    );
}