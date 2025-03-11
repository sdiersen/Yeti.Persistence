using Dapper;
using Microsoft.Data.SqlClient;
using Persistence.ModelValidations;
using Persistence.Models;
using ErrorHandling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">The Subclass that uses this Superclass </typeparam>
    /// <typeparam name="T2">The Model class that is being accessed in the database.</typeparam>
    public abstract class DbBaseLayer<T1, T2> where T2 : IBaseModel
    {
        protected readonly string _connectionString;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<T1> _logger;
        protected readonly IModelValidation<T2> _modelValidation;
        protected int Id { get; set; }
        protected string TableName { get; set; } = string.Empty;
        protected DateTime CreatedOn { get; set; }
        protected DateTime ModifiedOn { get; set; }

        public DbBaseLayer(ILogger<T1> logger,
                            IConfiguration configuration,
                            IModelValidation<T2> modelValidation)
        {
            _logger = logger;
            _configuration = configuration;
            _modelValidation = modelValidation;

            // try to get YETI_DB_CONNECTION_STRING from environment variables
            string? connectionString = Environment.GetEnvironmentVariable("YETI_DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                // try to get YETI_DB_CONNECTION_STRING from appsettings.json
                connectionString = _configuration.GetConnectionString("YETI_DB_CONNECTION_STRING");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException("YETI_DB_CONNECTION_STRING is not set");
                }
            }
            _connectionString = connectionString;
        }

        //******************************************************************************
        // Get Row
        //******************************************************************************

        public virtual ReturnValue<T2> GetRow(int id)
        {
            ReturnValue<T2> returnValue = new ReturnValue<T2>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT * FROM {TableName} WHERE Id = @Id";

                try
                {
                    connection.Open();
                    var row = connection.QueryFirstOrDefault<T2>(query, new { Id = id });
                    if (row != null)
                    {
                        returnValue.Success = true;
                        returnValue.Messages.Add($"{TableName} row with Id: {id} found.");
                        returnValue.Data = row;
                    }
                    else
                    {
                        returnValue.Messages.Add($"No {TableName} row found with Id: {id}.");
                    }
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        public virtual async Task<ReturnValue<T2>> GetRowAsync(int id)
        {
            ReturnValue<T2> returnValue = new ReturnValue<T2>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT * FROM {TableName} WHERE Id = @Id";

                try
                {
                    await connection.OpenAsync();
                    var row = await connection.QueryFirstOrDefaultAsync<T2>(query, new { Id = id });
                    if (row != null)
                    {
                        returnValue.Success = true;
                        returnValue.Messages.Add($"{TableName} row with Id: {id} found.");
                        returnValue.Data = row;
                    }
                    else
                    {
                        returnValue.Messages.Add($"No {TableName} row found with Id: {id}.");
                    }
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Get the first numberOfRows from teh table
        /// if numberOfRows is <= 0, return all rows
        /// </summary>
        /// <param name="numberOfRows"></param>
        /// <returns>ReturnValue of type List of table type</returns>
        public virtual ReturnValue<List<T2>> GetFirstXRows(int numberOfRows)
        {
            ReturnValue<List<T2>> returnValue = new ReturnValue<List<T2>>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = numberOfRows >= 0 ? $"SELECT TOP @rows * FROM {TableName}" :
                                                   $"SELECT * FROM {TableName}";
                try
                {
                    var rows = connection.Query<T2>(query, new { rows = numberOfRows });
                    returnValue.Success = true;
                    returnValue.Data = rows.ToList();
                    var message = rows.Any() ? $"Get {numberOfRows} rows from {TableName} returned successfully, {rows.Count()} were returned." :
                                               $"Get {numberOfRows} rows from {TableName} returned successfully, but 0 rows were returned.";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
                return returnValue;
            }
        }

        public virtual async Task<ReturnValue<List<T2>>> GetFirstXRowsAsync(int numberOfRows)
        {
            ReturnValue<List<T2>> returnValue = new ReturnValue<List<T2>>();
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = numberOfRows >= 0 ? $"SELECT TOP @rows * FROM {TableName}" :
                                                   $"SELECT * FROM {TableName}";
                try
                {
                    await connection.OpenAsync();
                    var rows = await connection.QueryAsync<T2>(query, new { rows = numberOfRows });
                    returnValue.Success = true;
                    returnValue.Data = rows.ToList();
                    var message = rows.Any() ? $"Get {numberOfRows} rows from {TableName} returned successfully, {rows.Count()} were returned." :
                                               $"Get {numberOfRows} rows from {TableName} returned successfully, but 0 rows were returned.";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
                return returnValue;
            }
        }

        //******************************************************************************
        // Delete Row
        //******************************************************************************
        public virtual ReturnValue DeleteRow(int id)
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"DELETE FROM {TableName} WHERE Id = @Id";
                    int rowsAffected = connection.Execute(query, new { Id = id });
                    returnValue.Success = true;
                    var message = rowsAffected == 0 ? "Delete query returned successfully, but 0 rows were affected." :
                                                        $"Delete query returned successfully. Number of rows affected: {rowsAffected}";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Success = false;
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        public virtual async Task<ReturnValue> DeleteRowAsync(int id)
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = $"DELETE FROM {TableName} WHERE Id = @Id";
                    int rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                    returnValue.Success = true;
                    var message = rowsAffected == 0 ? "Delete query returned successfully, but 0 rows were affected." :
                                                        $"Delete query returned successfully. Number of rows affected: {rowsAffected}";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Success = false;
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        /*
        Protected Methods:
        Inserting and Updating require information specific to the derived class, so these methods are protected.
        the parameter string sql for each method is how the derived classes will pass specific data for the queries.
        These methods are here to handle the dapper calls to the database and to create the ReturnValue object that is returned.
        */

        protected ReturnValue InsertRowBase(string sql, DynamicParameters parameters)
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    int rowsAffected = connection.Execute(sql, parameters);
                    returnValue.Success = true;
                    var message = rowsAffected == 0 ? $"Insert row into {TableName} returned successfully, but 0 rows were affected." :
                                                     $"Insert row into {TableName} returned successfully. Number of rows affected: {rowsAffected}";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        protected async Task<ReturnValue> InsertRowBaseAsync(string sql, DynamicParameters parameters)
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await connection.ExecuteAsync(sql, parameters);
                    returnValue.Success = true;
                    var message = rowsAffected == 0 ? $"Insert row into {TableName} async returned successfully, but 0 rows were affected." :
                                                     $"Insert row into {TableName} async returned successfully. Number of rows affected: {rowsAffected}";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        protected ReturnValue UpdateRowBase(string sql, DynamicParameters parameters)
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    int id = parameters.Get<int>("@Id");
                    int rowsAffected = connection.Execute(sql, parameters);
                    returnValue.Success = true;
                    var message = rowsAffected == 0 ? $"Update {TableName} row {id} returned successfully, but 0 rows were affected." :
                                                     $"Update {TableName} row {id} returned successfully. Number of rows affected: {rowsAffected}";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        protected async Task<ReturnValue> UpdateRowBaseAsync(string sql, DynamicParameters parameters)
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    int id = parameters.Get<int>("@Id");
                    int rowsAffected = await connection.ExecuteAsync(sql, parameters);
                    returnValue.Success = true;
                    var message = rowsAffected == 0 ? $"Update {TableName} row {id} returned successfully, but 0 rows were affected." :
                                                     $"Update {TableName} row {id} returned successfully. Number of rows affected: {rowsAffected}";
                    returnValue.Messages.Add(message);
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        protected ReturnValue ExecuteQuery(string sql,
                                            DynamicParameters parameters,
                                            string errorMessage = "",
                                            string successMessage = "")
        {
            ReturnValue returnValue = new ReturnValue();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Execute(sql, parameters);
                    returnValue.Success = true;
                    if (successMessage != "")
                    {
                        returnValue.Messages.Add(successMessage);
                    }
                }
                catch (Exception ex)
                {
                    returnValue.Errors.Add(ex.Message);
                    if (errorMessage != "")
                    {
                        returnValue.Messages.Add(errorMessage);
                    }
                }
            }
            return returnValue;
        }
    }
}