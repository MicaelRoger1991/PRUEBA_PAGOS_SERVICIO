using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Domain.Auth;

public class AuthError
{
    public static readonly Error NotFound = new(
        "Login.NotFound",
        "No se encontro al usuario, asegurese de que el usuario y la contraseña sean correctos."
    );
    public static readonly Error NotToken = new(
        "Login.NotToken",
        "Ups, tuvimos un problema al iniciar su sesión, por favor intente nuevamente."
    );
    public static readonly Error NotRecord = new(
        "Login.NotRecord",
        "Ups, tuvimos un problema al guardar su sesión, por favor intente nuevamente."
    );
}