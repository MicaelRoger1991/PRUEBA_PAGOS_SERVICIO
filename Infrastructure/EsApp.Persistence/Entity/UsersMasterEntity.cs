using System;

namespace EsApp.Persistence.Entity;

public class UsersMasterEntity
{
    public Guid UsersMasterId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserRegistration { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string StateRecord { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }

    public ICollection<UsersSessionsMasterEntity> Sessions { get; set; } = new List<UsersSessionsMasterEntity>();
}
