using System;
using System.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace EsApp.Persistence.DataAccess;

public interface IDataAccess
{
    HealthCheckResult CheckHealth();
    Task<DataTable> SelectStoredProcedure(string nameDB, string querySP, List<NpgsqlParameter>? parameters = default);
    Task<bool> ExecuteStoredProcedure(string nameDB, string querySP, List<NpgsqlParameter>? parameters = default);
}
