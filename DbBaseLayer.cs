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
        protected string TableName { get; set; } = string.Empty;
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

        /// <summary>
        /// Get a row from the table by the Id
        /// </summary>
        /// <param name="id">id of the row to return</param>
        /// <returns>
        /// A ReturnValue Object.
        /// If the row is found, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was found, and ReturnValue.Data contains the row.
        /// If the row is not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
        /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
        /// </returns>
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

        /// <summary>
        /// Get a row from the table by the Id asynchronously
        /// </summary>
        /// <param name="id">id of the row to return</param>
        /// <returns>
        /// A ReturnValue Object.
        /// If the row is found, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was found, and ReturnValue.Data contains the row.
        /// If the row is not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
        /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
        /// </returns>
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
        /// if numberOfRows is less than or equal to 0, return all rows
        /// </summary>
        /// <param name="numberOfRows">the number of rows to return from the top of the table</param>
        /// <returns>
        /// A ReturnValue Object.
        /// If any rows are found, ReturnValue.Success is true, ReturnValue.Messages contains a message with the number of rows found, and ReturnValue.Data contains List<typeparamref name="T2"/> objects.
        /// If now rows are not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
        /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
        /// </returns>
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
        /// <summary>
        /// Get the first numberOfRows from teh table asynchronously 
        /// if numberOfRows is less than or equal to 0, return all rows
        /// </summary>
        /// <param name="numberOfRows">the number of rows to return from the top of the table</param>
        /// <returns>
        /// A ReturnValue Object.
        /// If any rows are found, ReturnValue.Success is true, ReturnValue.Messages contains a message with the number of rows found, and ReturnValue.Data contains List<typeparamref name="T2"/> objects.
        /// If now rows are not found, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not found, and ReturnValue.Data is null. 
        /// If there is an exception, ReturnValue.Success is false, ReturnValue.Errors contains the exception message, and ReturnValue.Data is null.
        /// </returns>
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
        /// <summary>
        /// Delete a row from the table by the Id
        /// </summary>
        /// <param name="id">the id of the row to be deleted.</param>
        /// <returns>
        /// A ReturnValue object.
        /// If the row is deleted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was deleted.
        /// If the row is not deleted, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not deleted, and ReturnValue.Errors contains the exception message.
        /// </returns>
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
                    returnValue.Messages.Add("Delete query was not successful.");
                    returnValue.Errors.Add(ex.Message);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Delete a row from the table by the Id asynchronously
        /// </summary>
        /// <param name="id">the id of the row to be deleted.</param>
        /// <returns>
        /// A ReturnValue object.
        /// If the row is deleted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was deleted.
        /// If the row is not deleted, ReturnValue.Success is false, ReturnValue.Messages contains a message that the row was not deleted, and ReturnValue.Errors contains the exception message.
        /// </returns>
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
                    returnValue.Messages.Add("Delete query was not successful.");
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

        /// <summary>
        /// Insert a row into the table
        /// </summary>
        /// <param name="sql">the sql INSERT query to be executed for the specific table</param>
        /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
        /// <code>
        /// <para>Example of parameter construction:</para>
        /// var sql = $@"INSERT INTO {TableName} (Name, Description, CreatedOn, ModifiedOn) VALUES (@Name, @Description, @CreatedOn, @ModifiedOn);";
        /// var parameters = new DynamicParameters();
        /// parameters.Add("@Name", model.Name);
        /// parameters.Add("@Description", model.Description);
        /// parameters.Add("@CreatedOn", CreatedOn);
        /// parameters.Add("@ModifiedOn", ModifiedOn);
        /// </code>
        /// <returns>
        /// A ReturnValue object.
        /// If the row is inserted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was inserted.
        /// If the row is not inserted, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
        /// </returns>
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

        /// <summary>
        /// Insert a row into the table asynchronously
        /// </summary>
        /// <param name="sql">the sql INSERT query to be executed for the specific table</param>
        /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
        /// <code>
        /// <para>Example of parameter construction:</para>
        /// var sql = $@"INSERT INTO {TableName} ({Name}, {Description}, {CreatedOn}, {ModifiedOn}) VALUES (@Name, @Description, @CreatedOn, @ModifiedOn);";
        /// var parameters = new DynamicParameters();
        /// parameters.Add("@Name", model.Name);
        /// parameters.Add("@Description", model.Description);
        /// parameters.Add("@CreatedOn", CreatedOn);
        /// parameters.Add("@ModifiedOn", ModifiedOn);
        /// </code>
        /// <returns>
        /// A ReturnValue object.
        /// If the row is inserted, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was inserted.
        /// If the row is not inserted, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
        /// </returns>
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

        /// <summary>
        /// Update a row in the table
        /// </summary>
        /// <param name="sql">the sql UPDATE query to be executed for the specific table</param>
        /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
        /// <code>
        /// <para>Example of parameter construction:</para>
        /// var sql = $@"UPDATE {TableName} SET ({Name}=@Name, {Description}=@Description, {ModifiedOn}=@ModifiedOn) WHERE {Id}=@Id;";
        /// var parameters = new DynamicParameters();
        /// parameters.Add("@Name", model.Name);
        /// parameters.Add("@Description", model.Description);
        /// parameters.Add("@ModifiedOn", ModifiedOn);
        /// parameters.Add("@Id", Id);
        /// </code>
        /// <returns>
        /// A ReturnValue object.
        /// If the row is updated, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was updated.
        /// If the row is not updated, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
        /// </returns>
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

        /// <summary>
        /// Update a row in the table asynchronously
        /// </summary>
        /// <param name="sql">the sql UPDATE query to be executed for the specific table</param>
        /// <param name="parameters">the parameters for the sql query using Dapper's DynamicParameters</param>
        /// <code>
        /// <para>Example of parameter construction:</para>
        /// var sql = $@"UPDATE {TableName} SET ({Name}=@Name, {Description}=@Description, {ModifiedOn}=@ModifiedOn) WHERE {Id}=@Id;";
        /// var parameters = new DynamicParameters();
        /// parameters.Add("@Name", model.Name);
        /// parameters.Add("@Description", model.Description);
        /// parameters.Add("@ModifiedOn", ModifiedOn);
        /// parameters.Add("@Id", Id);
        /// </code>
        /// <returns>
        /// A ReturnValue object.
        /// If the row is updated, ReturnValue.Success is true, ReturnValue.Messages contains a message that the row was updated.
        /// If the row is not updated, ReturnValue.Success is false, ReturnValue.Errors contains the exception message.
        /// </returns>
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

        /// <summary>
        /// Executes a user query with Dapper DynamicParameters
        /// </summary>
        /// <param name="sql">A user defined SQL query</param>
        /// <param name="parameters">The DynamicParameters to be used in this query</param>
        /// <param name="errorMessage">User defined Error message to be returned in ReturnValue.Messages, defaults to ""</param>
        /// <param name="successMessage">User defined Success message to be returned in ReturnValue.Messages, defaults to ""</param>
        /// <returns>
        /// A ReturnValue object.
        /// If the query is successful, ReturnValue.Success is true, ReturnValue.Messages contains successMessage or nothing if the successMessage parameter was default.
        /// If the query is not successful, ReturnValue.Success is false, ReturnValue.Messages contains errorMessage or nothing if the errorMessage parameter was default, and ReturnValue.Errors contains the exception message.
        /// </returns>
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