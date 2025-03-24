using Dapper;

using ErrorHandling;

using FluentMigrator.Runner;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence.Migrations.Constants;

using System.Reflection;

namespace Persistence.Helpers
{
    //TODO: This class should be refactored into a private class with methods that deal specifically with the database in sql terms:
    //  - CreateDatabase
    //  - DropDatabase
    //  - Valid Connection string
    //  - Valid Database
    // There should be a public class that uses the fluentRunner to migrate the database up and down.
    // this might change later, for testing we'll keep it as is.
    /// <summary>
    /// This class is used to help with database operations such as creating a database, checking if the database is valid, and migrating the database up and down.
    /// </summary>
    public class DatabaseHelpers : IDatabaseHelpers
    {
        private readonly string _connectionString;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor for the DatabaseHelpers class.
        /// </summary>
        /// <param name="configuration">IConfiguration object which needs to have a connection string for the database</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DatabaseHelpers(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration));
            _serviceProvider = CreateServices();
        }

        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(_connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
        /// <summary>
        /// Checks if the connection string is valid by opening a connection and executing a simple query to the sql server.
        /// </summary>
        /// <returns>True if able to connect to the sql server with the connection string. False otherwise.</returns>
        public bool IsConnectionStringValid()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    connection.Execute("SELECT 1");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the database is valid by opening a connection and executing a simple query.
        /// </summary>
        /// <returns>True if able to connect to the database and execute the basic query. False otherwise.</returns>
        public bool IsDatabaseValid()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand($"SELECT 1 FROM {DbTableNames.USER_DATA_TABLE}", connection);
                    command.ExecuteScalar();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a new database with the name specified in the connection string.
        /// If the database already exists, this method will throw an exception.
        /// </summary>
        /// <returns>
        /// ReturnValue.Success is true if the database is created.
        /// If the database is not created: ReturnValue.Success = false, ReturnValue.Messages will have a vague creating database error message. ReturnValue.Errors will have the exception message
        /// </returns>
        public ReturnValue CreateDatabase()
        {
            var retrunValue = new ReturnValue();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = $"CREATE DATABASE {GetDatabaseName()}";
                    var command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                retrunValue.Success = true;
            }
            catch (Exception ex)
            {
                retrunValue.Messages.Add("Error creating database.");
                retrunValue.Errors.Add(ex.Message);
            }
            return retrunValue;
        }
        /// <summary>
        /// Does any new migrations to the database
        /// </summary>
        /// <returns>
        /// ReturnValue.Success = true if the migration works correctly.
        /// Otherwise, ReturnValue.Success = false, ReturnValue.Messages will have a vague message about not being able to migrate and ReturnValue.Errors will have the exception message
        /// </returns>
        public ReturnValue MigrateUp()
        {
            var returnValue = new ReturnValue();
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                    runner.MigrateUp();
                    returnValue.Success = true;
                }
            }
            catch (Exception ex)
            {
                returnValue.Messages.Add("Error migrating database up.");
                returnValue.Errors.Add(ex.Message);
            }
            return returnValue;

        }

        /// <summary>
        /// Reverts the database to the version specified in the version parameter.
        /// </summary>
        /// <param name="version">this is in the form of YYYYMMDDXXXX where YYYY is the year of the version, MM is the month of the version, DD is the day of the version and XXXX is the specific version for that day</param>
        /// <returns>
        /// ReturnValue.Success = true if the migration works correctly.
        /// Otherwise, ReturnValue.Success = false, ReturnValue.Messages will have a vague message about not being able to migrate and ReturnValue.Errors will have the exception message
        /// </returns>
        public ReturnValue MigrateDown(long version)
        {
            var returnValue = new ReturnValue();
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                    runner.MigrateDown(version);
                    returnValue.Success = true;
                }
            }
            catch (Exception ex)
            {
                returnValue.Messages.Add("Error migrating database down.");
                returnValue.Errors.Add(ex.Message);
            }
            return returnValue;
        }

        // TODO: this seems like it should work wth InitialCatalog and Database, but does it work for other
        // ways of naming the database, such as Data Source (ODBC), DBQ (Oracle and Access), or legacy terms. 
        private string GetDatabaseName()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            return builder.InitialCatalog;
        }
    }
}
