using System;

namespace EsApp.Persistence;

public class DataBaseSettings
{
    public required List<ConnectionString> ConnectionStrings { get; set; }
}

public class ConnectionString
{
    public required string Name { get; set; }
    public required string Server { get; set; }
    public required string DataBase { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
    public int Timeout { get; set; }
}
