using System;

namespace EsApp.Persistence.Entity;

public class UsersSessionsMasterEntity
{
    public Guid UsersSessionsMasterId { get; set; }
    public Guid UsersMasterId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateGenerate { get; set; }
    public DateTime DateExpiration { get; set; }
    public int AmountToken { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }

    public UsersMasterEntity UsersMaster { get; set; } = null!;
}
