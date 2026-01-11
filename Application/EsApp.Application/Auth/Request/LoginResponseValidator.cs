using System;
using FluentValidation;

namespace EsApp.Application.Auth.Request;

public class LoginResponseValidator : AbstractValidator<LoginRequest>
{
    public LoginResponseValidator()
    {
        RuleFor(x => x.user)
            .NotEmpty()
            .WithMessage("El usuario es obligatorio.");

        RuleFor(x => x.password)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }
}