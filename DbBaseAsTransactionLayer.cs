using Dapper;

using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models;
using Persistence.ModelValidations;

namespace Persistence;

/// <summary>
/// Abstract class to serve as base for services using transactions.
/// provides virtual methods for a number of the common operations in the IDbServiceAsTransactionInterface methods.
/// </summary>
/// <typeparam name="T1">The Subclass that uses this Superclass </typeparam>
/// <typeparam name="T2">The Model class that is being accessed in the database.</typeparam>
public abstract class DbBaseAsTransactionLayer<T1, T2> where T2 : IBaseModel
{
    /// <summary>
    /// The connection string to the database
    /// </summary>
    protected readonly string _connectionString;
    /// <summary>
    /// The configuration object that is used to get the connection string from the appsettings.json file
    /// </summary>
    protected readonly IConfiguration _configuration;
    /// <summary>
    /// The logger object that is used to log messages to the console or a file.
    /// </summary>
    protected readonly ILogger<T1> _logger;
    /// <summary>
    /// The model validation object that is used to validate the model before inserting or updating the database.
    /// </summary>
    protected readonly IModelValidation<T2> _modelValidation;
    /// <summary>
    /// The Id of the row in the database
    /// </summary>
    protected int Id { get; set; }
    /// <summary>
    /// The name of the table in the database
    /// </summary>
    protected static string TableName { get; set; } = string.Empty;
    /// <summary>
    /// The date and time the row was created
    /// </summary>
    protected DateTime CreatedOn { get; set; }
    /// <summary>
    /// The date and time the row was last modified
    /// </summary>
    protected DateTime ModifiedOn { get; set; }

    /// <summary>
    /// Constructor for the DbBaseLayer class.
    /// </summary>
    /// <param name="logger">ILogger<typeparamref name="T1"/> where T1 is the subclass that uses this class</param>
    /// <param name="configuration">IConfiguation object that needs to contain the connection string for the database</param>
    /// <param name="modelValidation">IModelValidation<typeparamref name="T2"/> where T2 is the concrete data model class</param>
    /// <exception cref="ArgumentNullException">Throws ArgumentNullException if the connection string is not an environment variable or in the appsettings.json file</exception>
    public DbBaseAsTransactionLayer(ILogger<T1> logger,
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
            //connectionString = _configuration.GetConnectionString("YETI_DB_CONNECTION_STRING");
            connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("YETI_DB_CONNECTION_STRING is not set.");
            }
        }
        _connectionString = connectionString;
    }

    //******************************************************************************
    // Get Row
    //******************************************************************************
    /// <summary>
    /// Get a row from the table by the Id using a transaction
    /// </summary>
    /// <param name="id">id of the row to return</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <returns>        
    /// A ReturnValue Object.
    /// If the row is found, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was found, and ReturnValue.Data contains the row.
    /// If the row is not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
    /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
    /// </returns>
    public virtual ReturnValue<T2> GetRowAsTransaction(int id, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue<T2>();
        try
        {
            var row = connection.QueryFirstOrDefault<T2>(_getRowByIdSQL, new { Id = id }, transaction);
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
    /// <summary>
    /// Get a row from the table by the Id asynchronously using a transaction
    /// </summary>
    /// <param name="id">id of the row to return</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <returns>        
    /// A ReturnValue Object.
    /// If the row is found, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was found, and ReturnValue.Data contains the row.
    /// If the row is not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
    /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
    /// </returns>
    public virtual async Task<ReturnValue<T2>> GetRowAsTransactionAsync(int id, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue<T2>();
        try
        {
            var row = await connection.QueryFirstOrDefaultAsync<T2>(_getRowByIdSQL, new { Id = id }, transaction);
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
    /// <summary>
    /// Get the first numberOfRows from the table using a transaction
    /// </summary>
    /// <param name="numberOfRows">the number of rows to return from the top of the table</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <returns>
    /// A ReturnValue Object.
    /// If any rows are found, ReturnValue.Success is true, ReturnValue.Messages contains a message with the number of rows found, and ReturnValue.Data contains List<typeparamref name="T2"/> objects.
    /// If now rows are not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
    /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
    /// </returns>
    public virtual ReturnValue<List<T2>> GetFirstXRowsAsTransaction(int numberOfRows, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue<List<T2>>();
        string query = GetSQLForTopX(numberOfRows);
        try
        {
            var rows = connection.Query<T2>(query, new { rows = numberOfRows }, transaction);
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
    /// <summary>
    /// Get the first numberOfRows from the table using a transaction asynchronously.
    /// </summary>
    /// <param name="numberOfRows">the number of rows to return from the top of the table</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <returns>
    /// A ReturnValue Object.
    /// If any rows are found, ReturnValue.Success is true, ReturnValue.Messages contains a message with the number of rows found, and ReturnValue.Data contains List<typeparamref name="T2"/> objects.
    /// If now rows are not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
    /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
    /// </returns>
    public virtual async Task<ReturnValue<List<T2>>> GetFirstXRowsAsTransactionAsync(int numberOfRows, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue<List<T2>>();
        string query = GetSQLForTopX(numberOfRows);
        try
        {
            var rows = await connection.QueryAsync<T2>(query, new { rows = numberOfRows }, transaction);
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
    /// <summary>
    /// Delete a row from the table by the Id using a transaction
    /// </summary>
    /// <param name = "id" > the id of the row to be deleted.</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <returns>
    /// A ReturnValue object.
    /// If the row is deleted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was deleted.
    /// If the row is not deleted, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not deleted, and ReturnValue.Errors contains the exception message.
    /// </returns>
    public virtual ReturnValue DeleteRowAsTransaction(int id, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = connection.Execute(_deleteByIdSQL, new { Id = id }, transaction);
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
    /// <summary>
    /// Delete a row from the table by the Id asynchronously using a transaction
    /// </summary>
    /// <param name = "id" > the id of the row to be deleted.</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <returns>
    /// A ReturnValue object.
    /// If the row is deleted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was deleted.
    /// If the row is not deleted, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not deleted, and ReturnValue.Errors contains the exception message.
    /// </returns>
    public virtual async Task<ReturnValue> DeleteRowAsTransactionAsync(int id, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = await connection.ExecuteAsync(_deleteByIdSQL, new { Id = id }, transaction);
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
    /*
    Protected Methods:
    Inserting and Updating require information specific to the derived class, so these methods are protected.
    the parameter string sql for each method is how the derived classes will pass specific data for the queries.
    These methods are here to handle the dapper calls to the database and to create the ReturnValue object that is returned.
    */
    //******************************************************************************
    /// <summary>
    /// Insert a row into the table using a transaction
    /// </summary>
    /// <param name="sql">the sql INSERT query to be executed for the specific table</param>
    /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <example>
    /// <code>
    /// <para>Example of parameter construction:</para>
    /// var sql = $@"INSERT INTO {TableName} (Name, Description, CreatedOn, ModifiedOn) VALUES (@Name, @Description, @CreatedOn, @ModifiedOn);";
    /// var parameters = new DynamicParameters();
    /// parameters.Add("@Name", model.Name);
    /// parameters.Add("@Description", model.Description);
    /// parameters.Add("@CreatedOn", CreatedOn);
    /// parameters.Add("@ModifiedOn", ModifiedOn);
    /// </code>
    /// </example>
    /// <returns>
    /// A ReturnValue object.
    /// If the row is inserted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was inserted.
    /// If the row is not inserted, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
    /// </returns>
    protected ReturnValue InsertRowBaseAsTransaction(string sql, DynamicParameters parameters, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = connection.Execute(sql, parameters, transaction);
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
    /// <summary>
    /// Insert a row into the table asynchronously using a transaction
    /// </summary>
    /// <param name="sql">the sql INSERT query to be executed for the specific table</param>
    /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <example>
    /// <code>
    /// <para>Example of parameter construction:</para>
    /// var sql = $@"INSERT INTO {TableName} ({Name}, {Description}, {CreatedOn}, {ModifiedOn}) VALUES (@Name, @Description, @CreatedOn, @ModifiedOn);";
    /// var parameters = new DynamicParameters();
    /// parameters.Add("@Name", model.Name);
    /// parameters.Add("@Description", model.Description);
    /// parameters.Add("@CreatedOn", CreatedOn);
    /// parameters.Add("@ModifiedOn", ModifiedOn);
    /// </code>
    /// </example>
    /// <returns>
    /// A ReturnValue object.
    /// If the row is inserted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was inserted.
    /// If the row is not inserted, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
    /// </returns>
    protected async Task<ReturnValue> InsertRowBaseAsTransactionAsync(string sql, DynamicParameters parameters, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue();
        try
        {
            int rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction);
            returnValue.Success = true;
            var message = rowsAffected == 0 ? $"Insert row into {TableName} async returned successfully, but 0 rows were affected." :
                                                 $"Insert row into {TableName} async returned successfully. Number of rows affected: {rowsAffected}";
            returnValue.AddMessage("database", message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    /// <summary>
    /// Update a row in the table using a transaction
    /// </summary>
    /// <param name="sql">the sql UPDATE query to be executed for the specific table</param>
    /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <example>
    /// <code>
    /// <para>Example of parameter construction:</para>
    /// var sql = $@"UPDATE {TableName} SET ({Name}=@Name, {Description}=@Description, {ModifiedOn}=@ModifiedOn) WHERE {Id}=@Id;";
    /// var parameters = new DynamicParameters();
    /// parameters.Add("@Name", model.Name);
    /// parameters.Add("@Description", model.Description);
    /// parameters.Add("@ModifiedOn", ModifiedOn);
    /// parameters.Add("@Id", Id);
    /// </code>
    /// </example>
    /// <returns>
    /// A ReturnValue object.
    /// If the row is updated, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was updated.
    /// If the row is not updated, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
    /// </returns>
    protected ReturnValue UpdateRowBaseAsTransaction(string sql, DynamicParameters parameters, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue();
        try
        {
            int id = parameters.Get<int>("@Id");
            int rowsAffected = connection.Execute(sql, parameters, transaction);
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
    /// <summary>
    /// Update a row in the table asynchronously using a transaction
    /// </summary>
    /// <param name="sql">the sql UPDATE query to be executed for the specific table</param>
    /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <example>
    /// <code>
    /// <para>Example of parameter construction:</para>
    /// var sql = $@"UPDATE {TableName} SET ({Name}=@Name, {Description}=@Description, {ModifiedOn}=@ModifiedOn) WHERE {Id}=@Id;";
    /// var parameters = new DynamicParameters();
    /// parameters.Add("@Name", model.Name);
    /// parameters.Add("@Description", model.Description);
    /// parameters.Add("@ModifiedOn", ModifiedOn);
    /// parameters.Add("@Id", Id);
    /// </code>
    /// </example>
    /// <returns>
    /// A ReturnValue object.
    /// If the row is updated, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was updated.
    /// If the row is not updated, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
    /// </returns>
    protected async Task<ReturnValue> UpdateRowBaseAsTransactionAsync(string sql, DynamicParameters parameters, SqlConnection connection, SqlTransaction transaction)
    {
        var returnValue = new ReturnValue();
        try
        {
            int id = parameters.Get<int>("@Id");
            int rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction);
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

    //******************************************************************************
    // Generic Query methods
    //******************************************************************************
    /// <summary>
    /// Executes a user query with Dapper DynamicParameters using a transaction
    /// </summary>
    /// <param name="sql">A user defined SQL query</param>
    /// <param name="parameters">The DynamicParameters to be used in this query</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <param name="errorMessage">User defined Error message to be returned in ReturnValue.Messages, defaults to ""</param>
    /// <param name="successMessage">User defined Success message to be returned in ReturnValue.Messages, defaults to ""</param>
    /// <returns>
    /// A ReturnValue object.
    /// If the query is successful, ReturnValue.Success is true, ReturnValue.Messages contains successMessage or nothing if the successMessage parameter was default.
    /// If the query is not successful, ReturnValue.Success is false, ReturnValue.Messages contains errorMessage or nothing if the errorMessage parameter was default, and ReturnValue.Errors contains the exception message.
    /// </returns>
    protected ReturnValue ExecuteQueryAsTransaction(string sql,
                                        DynamicParameters parameters,
                                        SqlConnection connection,
                                        SqlTransaction transaction,
                                        string errorMessage = "",
                                        string successMessage = "")
    {
        var returnValue = new ReturnValue();
        try
        {
            connection.Execute(sql, parameters, transaction);
            returnValue.Success = true;
            if (successMessage != "")
            {
                returnValue.AddMessage("database", successMessage);
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != "")
            {
                returnValue.AddMessage("database", errorMessage);
            }
        }
        return returnValue;
    }
    /// <summary>
    /// Executes a user query with Dapper DynamicParameters asynchronously
    /// </summary>
    /// <param name="sql">A user defined SQL query</param>
    /// <param name="parameters">The DynamicParameters to be used in this query</param>
    /// <param name="connection">The SqlConnection for this transaction.</param>
    /// <param name="transaction">The SqlTransaction that this query is going to be part of.</param>
    /// <param name="errorMessage">User defined Error message to be returned in ReturnValue.Messages, defaults to ""</param>
    /// <param name="successMessage">User defined Success message to be returned in ReturnValue.Messages, defaults to ""</param>
    /// <returns>
    /// A ReturnValue object.
    /// If the query is successful, ReturnValue.Success is true, ReturnValue.Messages contains successMessage or nothing if the successMessage parameter was default.
    /// If the query is not successful, ReturnValue.Success is false, ReturnValue.Messages contains errorMessage or nothing if the errorMessage parameter was default, and ReturnValue.Errors contains the exception message.
    /// </returns>
    protected async Task<ReturnValue> ExecuteQueryAsTransactionAsync(string sql,
                                        DynamicParameters parameters,
                                        SqlConnection connection,
                                        SqlTransaction transaction,
                                        string errorMessage = "",
                                        string successMessage = "")
    {
        var returnValue = new ReturnValue();
        try
        {
            await connection.ExecuteAsync(sql, parameters, transaction);
            returnValue.Success = true;
            if (successMessage != "")
            {
                returnValue.AddMessage("database", successMessage);
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
            if (errorMessage != "")
            {
                returnValue.AddMessage("database", errorMessage);
            }
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
        return numberOfRows >= 0 ? $"SELECT TOP @rows * FROM {TableName}" :
                                    $"SELECT * FROM {TableName}";
    }
}
