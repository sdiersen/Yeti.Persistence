using Dapper;

using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models;

namespace Persistence.Repositories;

public abstract class BaseRepository<T1, T2>
    where T1 : IBaseModel
    where T2 : IRepository<T1>
{
    protected int Id { get; set; }
    protected static string TableName { get; set; } = string.Empty;
    protected DateTime CreatedOn { get; set; }
    protected DateTime ModifiedOn { get; set; }
    protected ILogger<T2> Logger { get; }
    protected SqlConnection Connection { get; }
    protected SqlTransaction? Transaction { get; }

    public BaseRepository(ILogger<T2> logger, SqlConnection connection, SqlTransaction? transaction = null)
    {
        Logger = logger;
        Connection = connection;
        Transaction = transaction;
    }

    //******************************************************************************
    // Get Row
    //******************************************************************************
    public virtual ReturnValue<T1> GetRow(int id)
    {
        var returnValue = new ReturnValue<T1>();
        try
        {
            var row = Connection.QuerySingleOrDefault<T1>(_getRowByIdSQL, new { Id = id }, Transaction);
            if (row != null)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", $"{TableName} row with Id: {id} found.");
                returnValue.Data = row;
            }
            else
            {
                returnValue.AddMessage("database", $"No {TableName} row found with Id: {id}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public virtual async Task<ReturnValue<T1>> GetRowAsync(int id)
    {
        var returnValue = new ReturnValue<T1>();
        try
        {
            var row = await Connection.QuerySingleOrDefaultAsync<T1>(_getRowByIdSQL, new { Id = id }, Transaction);
            if (row != null)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", $"{TableName} row with Id: {id} found.");
                returnValue.Data = row;
            }
            else
            {
                returnValue.AddMessage("database", $"No {TableName} row found with Id: {id}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public virtual ReturnValue<List<T1>> GetFirstXRows(int numberOfRows)
    {
        var returnValue = new ReturnValue<List<T1>>();
        string query = GetSQLForTopX(numberOfRows);
        try
        {
            var rows = Connection.Query<T1>(query, new { rows = numberOfRows }, Transaction);
            returnValue.Success = true;
            returnValue.Data = rows.ToList();
            var message = rows.Any() ? $"Get {numberOfRows} rows from {TableName} returned successfully, {rows.Count()} were returned." :
                           $"Get {numberOfRows} rows from {TableName} returned successfully, but 0 rows were returned.";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public virtual async Task<ReturnValue<List<T1>>> GetFirstXRowsAsync(int numberOfRows)
    {
        var returnValue = new ReturnValue<List<T1>>();
        string query = GetSQLForTopX(numberOfRows);
        try
        {
            var rows = await Connection.QueryAsync<T1>(query, new { rows = numberOfRows }, Transaction);
            returnValue.Success = true;
            returnValue.Data = rows.ToList();
            var message = rows.Any() ? $"Get {numberOfRows} rows from {TableName} returned successfully, {rows.Count()} were returned." :
                           $"Get {numberOfRows} rows from {TableName} returned successfully, but 0 rows were returned.";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    //******************************************************************************
    // Delete Row
    //******************************************************************************
    public virtual ReturnValue DeleteRow(int id)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = Connection.Execute(_deleteByIdSQL, new { Id = id }, Transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? "Delete query returned successfully, but 0 rows were affected." :
                                    $"Delete query returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddMessage("database", "Delete query was not successful.");
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public virtual async Task<ReturnValue> DeleteRowAsync(int id)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = await Connection.ExecuteAsync(_deleteByIdSQL, new { Id = id }, Transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? "Delete query returned successfully, but 0 rows were affected." :
                                    $"Delete query returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddMessage("database", "Delete query was not successful.");
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    //******************************************************************************
    // Protected Methods:
    // Inserting and Updating require information specific to the derived class, so these methods are protected.
    // the parameter string sql for each method is how the derived classes will pass specific data for the queries.
    // These methods are here to handle the dapper calls to the database and to create the ReturnValue object that is returned.
    //******************************************************************************
    protected ReturnValue InsertRowBase(string sql, DynamicParameters parameters)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = Connection.Execute(sql, parameters, Transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? $"Insert row into {TableName} returned successfully, but 0 rows were affected." :
                                             $"Insert row into {TableName} returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    protected async Task<ReturnValue> InsertRowBaseAsync(string sql, DynamicParameters parameters)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = await Connection.ExecuteAsync(sql, parameters, Transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? $"Insert row into {TableName} returned successfully, but 0 rows were affected." :
                                             $"Insert row into {TableName} returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    protected ReturnValue<int> InsertRowAndGetIdBase(string sql, DynamicParameters parameters)
    {
        var returnValue = new ReturnValue<int>();
        try
        {
            int id = Connection.ExecuteScalar<int>(sql, parameters, Transaction);
            returnValue.Success = true;
            returnValue.Data = id;
            returnValue.AddMessage("database", $"Insert row into {TableName} returned successfully. Id of inserted row: {id}");
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    protected async Task<ReturnValue<int>> InsertRowAndGetIdBaseAsync(string sql, DynamicParameters parameters)
    {
        var returnValue = new ReturnValue<int>();
        try
        {
            int id = await Connection.ExecuteScalarAsync<int>(sql, parameters, Transaction);
            returnValue.Success = true;
            returnValue.Data = id;
            returnValue.AddMessage("database", $"Insert row into {TableName} returned successfully. Id of inserted row: {id}");
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    protected ReturnValue UpdateRowBase(string sql, DynamicParameters parameters)
    {
        var returnValue = new ReturnValue();
        try
        {
            int id = parameters.Get<int>("@Id");
            int rowsAffected = Connection.Execute(sql, parameters, Transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? $"Update {TableName} row {id} returned successfully, but 0 rows were affected." :
                                             $"Update {TableName} row {id} returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    protected async Task<ReturnValue> UpdateRowBaseAsync(string sql, DynamicParameters parameters)
    {
        var returnValue = new ReturnValue();
        try
        {
            int id = parameters.Get<int>("@Id");
            int rowsAffected = await Connection.ExecuteAsync(sql, parameters, Transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? $"Update {TableName} row {id} returned successfully, but 0 rows were affected." :
                                             $"Update {TableName} row {id} returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    protected ReturnValue ExecuteQuery(string sql, DynamicParameters parameters, string errorMessage = "", string successMessage = "")
    {
        var returnValue = new ReturnValue();
        try
        {
            Connection.Execute(sql, parameters, Transaction);
            returnValue.Success = true;
            if (successMessage != string.Empty)
                returnValue.AddMessage("database", successMessage);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != string.Empty)
                returnValue.AddError("database", errorMessage);
        }
        return returnValue;
    }
    protected async Task<ReturnValue> ExecuteQueryAsync(string sql, DynamicParameters parameters, string errorMessage = "", string successMessage = "")
    {
        var returnValue = new ReturnValue();
        try
        {
            await Connection.ExecuteAsync(sql, parameters, Transaction);
            returnValue.Success = true;
            if (successMessage != string.Empty)
                returnValue.AddMessage("database", successMessage);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != string.Empty)
                returnValue.AddError("database", errorMessage);
        }
        return returnValue;
    }
    protected ReturnValue<T1> ExecuteRowQuery(string sql, DynamicParameters parameters, string errorMessage = "", string successMessage = "")
    {
        var returnValue = new ReturnValue<T1>();
        try
        {
            var row = Connection.QuerySingleOrDefault<T1>(sql, parameters, Transaction);
            if (row != null)
            {
                returnValue.Success = true;
                if(successMessage != string.Empty)
                    returnValue.AddMessage("database", successMessage);
                returnValue.Data = row;
            }
            else
            {
                if (errorMessage != string.Empty)
                    returnValue.AddMessage("database", errorMessage);
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != string.Empty)
                returnValue.AddMessage("database", errorMessage);
        }
        return returnValue;
    }
    protected async Task<ReturnValue<T1>> ExecuteRowQueryAsync(string sql, DynamicParameters parameters, string errorMessage = "", string successMessage = "")
    {
        var returnValue = new ReturnValue<T1>();
        try
        {
            var row = await Connection.QuerySingleOrDefaultAsync<T1>(sql, parameters, Transaction);
            if (row != null)
            {
                returnValue.Success = true;
                if (successMessage != string.Empty)
                    returnValue.AddMessage("database", successMessage);
                returnValue.Data = row;
            }
            else
            {
                if (errorMessage != string.Empty)
                    returnValue.AddMessage("database", errorMessage);
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != string.Empty)
                returnValue.AddMessage("database", errorMessage);
        }
        return returnValue;
    }
    protected Task<ReturnValue<List<T1>>> ExecuteEnumerableQuery(string sql, DynamicParameters parameters, string errorMessage = "", string successMessage = "")
    {
        var returnValue = new ReturnValue<List<T1>>();
        try
        {
            var rows = Connection.Query<T1>(sql, parameters, Transaction);
            returnValue.Success = true;
            if (successMessage != string.Empty)
                returnValue.AddMessage("database", successMessage);
            returnValue.Data = rows.ToList();
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != string.Empty)
                returnValue.AddMessage("database", errorMessage);
        }
        return Task.FromResult(returnValue);
    }
    protected async Task<ReturnValue<List<T1>>> ExecuteEnumerableQueryAsync(string sql, DynamicParameters parameters, string errorMessage = "", string successMessage = "")
    {
        var returnValue = new ReturnValue<List<T1>>();
        try
        {
            var rows = await Connection.QueryAsync<T1>(sql, parameters, Transaction);
            returnValue.Success = true;
            if (successMessage != string.Empty)
                returnValue.AddMessage("database", successMessage);
            returnValue.Data = rows.ToList();
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != string.Empty)
                returnValue.AddMessage("database", errorMessage);
        }
        return returnValue;
    }
    //******************************************************************************
    // Private string constant sql queries
    //******************************************************************************
    private readonly string _getRowByIdSQL = $@"
                                    SELECT * 
                                    FROM {TableName} 
                                    WHERE {DbCommonColumns.ID} = @Id
                                ;"
                        ;
    private readonly string _deleteByIdSQL = $@"
                                    DELETE FROM {TableName} 
                                    WHERE {DbCommonColumns.ID} = @Id
                                ;"
                        ;

    private static string GetSQLForTopX(int numberOfRows)
    {
        return numberOfRows > 0 ? $"SELECT TOP @rows * FROM {TableName}" :
                                    $"SELECT * FROM {TableName}";
    }
}
