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
    public class DatabaseHelpers
    {
        private readonly string _connectionString;
        private readonly IServiceProvider _serviceProvider;

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
