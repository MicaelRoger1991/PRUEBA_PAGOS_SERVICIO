using System;

namespace EsApp.Domain.Auth;

public class Auth
{
    public Guid usersMasterId { get; set; }
    public string lastName { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string userRegistration { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;

    public static Auth Create(
        Guid usersMasterId,
        string lastName,
        string firstName,
        string userRegistration,
        string role
        )
    {
        return new Auth
        {
            usersMasterId = usersMasterId,
            lastName = lastName,
            firstName = firstName,
            userRegistration = userRegistration,
            role = role
        };
    }
}