using System;
using EsApp.Api.Abstractions;
using EsApp.Application.Auth;
using EsApp.Application.Auth.Request;
using EsApp.Application.Auth.Response;
using EsApp.CROSS.Shared.Abstractions;
using FluentValidation;

namespace EsApp.Api.EndPoints.Auth;

public class RefreshToken : IEndPoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut($"{Tags.Auth}/RefreshToken", Handler)
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Auth);
    }

    private async Task<IResult> Handler(
        IAuthService authService,
        RefreshTokenRequest request,
        IValidator<RefreshTokenRequest> validator,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(request, validator, cancellationToken);
        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok(result.Value);
    }
}
