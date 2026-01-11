using System;

namespace EsApp.Api.Abstractions;

public interface IEndPoint
{
    void MapEndpoints(IEndpointRouteBuilder app);
}
