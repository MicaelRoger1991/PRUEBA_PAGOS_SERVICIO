using System;
using EsApp.Application.Auth.Request;
using EsApp.Application.Auth.Response;
using EsApp.CROSS.Shared.Abstractions;
using FluentValidation;

namespace EsApp.Application.Auth;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, IValidator<LoginRequest> validator, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request, IValidator<RefreshTokenRequest> validator, CancellationToken cancellationToken);
}
