using System;

namespace EsApp.Domain.Token;

public class UserSession
{
    public Guid usersSessionsMasterId { get; init; }
    public Guid usersMasterId { get; init; }
    public string token { get; init; } = string.Empty;
    public string refreshToken { get; init; } = string.Empty;
    public string description { get; init; } = string.Empty;
    public DateTime dateGenerate { get; init; }
    public DateTime dateExpiration { get; init; }
    public int amountToken { get; init; }

    public static UserSession GetSession(
        Guid usersSessionsMasterId,
        Guid usersMasterId,
        string token,
        string refreshToken,
        string description,
        DateTime dateGenerate,
        DateTime dateExpiration,
        int amountToken
    )
    {
        return new UserSession
        {
            usersSessionsMasterId = usersSessionsMasterId,
            usersMasterId = usersMasterId,
            token = token,
            refreshToken = refreshToken,
            description = description,
            dateGenerate = dateGenerate,
            dateExpiration = dateExpiration,
            amountToken = amountToken
        };
    }
    public static UserSession SaveSession(
        Guid usersMasterId,
        string token,
        string refreshToken,
        string description,
        DateTime dateGenerate,
        DateTime dateExpiration
    )
    {
        return new UserSession
        {
            usersMasterId = usersMasterId,
            token = token,
            refreshToken = refreshToken,
            description = description,
            dateGenerate = dateGenerate,
            dateExpiration = dateExpiration
        };
    }
}