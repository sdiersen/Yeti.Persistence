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
        /// <returns>ReturnValue object, Success=true on valid connection string. Success=false and exception in Errors on invalid connection string</returns>
        public ReturnValue IsConnectionStringValid()
        {
            var returnValue = new ReturnValue();
            try
            {
                using (var connection = new SqlConnection(ConectionStringWithoutDatabase()))
                {
                    connection.Open();
                }
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.Errors.Add(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// Checks if the database is valid by opening a connection and executing a simple query.
        /// </summary>
        /// <returns>ReturnValue object, Success=true on valid database. Success=false and exception in Errors on invalid or non-existent database.</returns>
        public ReturnValue IsDatabaseValid()
        {
            var returnValue = new ReturnValue();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand($"SELECT database_id FROM sys.databases WHERE name = '{GetDatabaseName()}'", connection);
                    var result = command.ExecuteScalar();
                    returnValue.Success = result != null;
                    if (!returnValue.Success)
                    {
                        returnValue.Messages.Add("Database does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Errors.Add(ex.Message);
            }
            return returnValue;
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
                using (var connection = new SqlConnection(ConectionStringWithoutDatabase()))
                {
                    connection.Open();
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

        /// <summary>
        /// This is a single method that will check the database connection string, check if the database is valid, create the database if necessary
        /// and migrate the databse to the laatest version.
        /// </summary>
        /// <returns></returns>
        public ReturnValue DatabaseStartUp()
        {
            var returnValue = new ReturnValue();
            var connectionStringReturnValue = IsConnectionStringValid();
            if (!connectionStringReturnValue.Success)
            {
                returnValue.Messages.AddRange(connectionStringReturnValue.Messages);
                returnValue.Errors.AddRange(connectionStringReturnValue.Errors);
                return returnValue;
            }
            var databaseValidReturnValue = IsDatabaseValid();
            if (!databaseValidReturnValue.Success)
            {
                returnValue.Messages.AddRange(databaseValidReturnValue.Messages);
                returnValue.Errors.AddRange(databaseValidReturnValue.Errors);
                var createDatabaseReturnValue = CreateDatabase();
                if (!createDatabaseReturnValue.Success)
                {
                    returnValue.Messages.AddRange(createDatabaseReturnValue.Messages);
                    returnValue.Errors.AddRange(createDatabaseReturnValue.Errors);
                    return returnValue;
                }

                // Retry mechanism to ensure the database is available
                int retryCount = 5;
                int delay = 2000; // 2 seconds
                while (retryCount > 0)
                {
                    Console.WriteLine($"Waiting for the database to be available... try: {retryCount}");
                    if (IsDatabaseValid().Success)
                    {
                        break;
                    }
                    Thread.Sleep(delay);
                    retryCount--;
                }
            }
            var migrateUpReturnValue = MigrateUp();
            if (!migrateUpReturnValue.Success)
            {
                returnValue.Messages.AddRange(migrateUpReturnValue.Messages);
                returnValue.Errors.AddRange(migrateUpReturnValue.Errors);
                return returnValue;
            }
            returnValue.Success = true;
            return returnValue;
        }

        // TODO: this seems like it should work wth InitialCatalog and Database, but does it work for other
        // ways of naming the database, such as Data Source (ODBC), DBQ (Oracle and Access), or legacy terms. 
        private string GetDatabaseName()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            return builder.InitialCatalog;
        }

        private string ConectionStringWithoutDatabase()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            builder.InitialCatalog = string.Empty; // Remove the database name
            return builder.ToString();
        }
    }
}
