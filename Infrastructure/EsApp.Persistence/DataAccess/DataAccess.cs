using System;
using System.Data;
using EsApp.CROSS.Encrypt;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Npgsql;

namespace EsApp.Persistence.DataAccess;

public class DataAccess : IDataAccess
{
    private readonly ISecurityEncrypt _securityEncrypt;
    private readonly ConnectionString _connectingString;

    public DataAccess(ISecurityEncrypt securityEncrypt, string nameDb, IOptions<DataBaseSettings> dataBaseSettings)
    {
        _securityEncrypt = securityEncrypt;
        _connectingString = dataBaseSettings.Value.ConnectionStrings
            .FirstOrDefault(x => x.Name.Equals(nameDb))
            ?? throw new ArgumentNullException($"Connection string {nameDb} not found");
    }

    public HealthCheckResult CheckHealth()
    {
        try
        {
            var conexion = new NpgsqlConnection(Connection());
            conexion.Open();
            conexion.Close();
            var response = HealthCheckResult.Healthy($"Data Base: {_connectingString.DataBase}; Server: {_connectingString.Server}; User: {_connectingString.User}");
            return response;
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Could not connection to Data Base: {_connectingString.DataBase}; Server: {_connectingString.Server}; User: {_connectingString.User}; Error: {ex.Message}; Exception: {ex.Message}");
        }
    }
    public async Task<bool> ExecuteStoredProcedure(string nameDB, string querySP, List<NpgsqlParameter> parameters)
    {
        using var conexion = new NpgsqlConnection(Connection());
        try
        {
            using var comando = new NpgsqlCommand(querySP, conexion);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = _connectingString.Timeout;

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    if (item.Value == null)
                        item.Value = DBNull.Value;
                    comando.Parameters.Add(item);
                }
            }

            conexion.Open();
            comando.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            NpgsqlConnection.ClearAllPools();
        }
    }

    public async Task<DataTable> SelectStoredProcedure(string nameDB, string querySP, List<NpgsqlParameter> parameters)
    {
        DataTable consulta = new DataTable();
        using var conexion = new NpgsqlConnection(Connection());
        try
        {
            using var comando = new NpgsqlCommand(querySP, conexion);
            comando.CommandType = CommandType.Text;
            comando.CommandTimeout = _connectingString.Timeout;

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    if (item.Value == null)
                        item.Value = DBNull.Value;
                    comando.Parameters.Add(item);
                }
            }

            await conexion.OpenAsync();
            using var reader = await comando.ExecuteReaderAsync();
            consulta.Load(reader);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            await conexion.CloseAsync();
        }
        return consulta;
    }

    private string Connection()
    {
        string pass = _securityEncrypt.dencrypt(_connectingString.Password);
        var connection = $"Host={_connectingString.Server};Database={_connectingString.DataBase};Username={_connectingString.User};Password={pass};Timeout={_connectingString.Timeout}";
        //var connection = $"Host={_connectingString.Server};Database={_connectingString.DataBase};Username={_connectingString.User};Password={pass};Timeout={_connectingString.Timeout}";
        return connection;
    }
}
