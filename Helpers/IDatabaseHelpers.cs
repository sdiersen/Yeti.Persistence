using ErrorHandling;
/// <summary>
/// This class is used to help with database operations such as creating a database, checking if the database is valid, and migrating the database up and down.
/// </summary>
public interface IDatabaseHelpers
{
    /// <summary>
    /// Creates a new database with the name specified in the connection string.
    /// If the database already exists, this method will throw an exception.
    /// </summary>
    /// <returns>
    /// ReturnValue.Success is true if the database is created.
    /// If the database is not created: ReturnValue.Success = false, ReturnValue.Messages will have a vague creating database error message. ReturnValue.Errors will have the exception message
    /// </returns>
    ReturnValue CreateDatabase();
    /// <summary>
    /// Checks if the connection string is valid by opening a connection and executing a simple query to the sql server.
    /// </summary>
    /// <returns>ReturnValue object, Success=true on valid connection string. Success=false and exception in Errors on invalid connection string</returns>
    ReturnValue IsConnectionStringValid();
    /// <summary>
    /// Checks if the database is valid by opening a connection and executing a simple query.
    /// </summary>
    /// <returns>ReturnValue object, Success=true on valid database. Success=false and exception in Errors on invalid or non-existent database.</returns>
    ReturnValue IsDatabaseValid();
    /// <summary>
    /// Reverts the database to the version specified in the version parameter.
    /// </summary>
    /// <param name="version">this is in the form of YYYYMMDDXXXX where YYYY is the year of the version, MM is the month of the version, DD is the day of the version and XXXX is the specific version for that day</param>
    /// <returns>
    /// ReturnValue.Success = true if the migration works correctly.
    /// Otherwise, ReturnValue.Success = false, ReturnValue.Messages will have a vague message about not being able to migrate and ReturnValue.Errors will have the exception message
    /// </returns>
    ReturnValue MigrateDown(long version);

    /// <summary>
    /// Does any new migrations to the database
    /// </summary>
    /// <returns>
    /// ReturnValue.Success = true if the migration works correctly.
    /// Otherwise, ReturnValue.Success = false, ReturnValue.Messages will have a vague message about not being able to migrate and ReturnValue.Errors will have the exception message
    /// </returns>
    ReturnValue MigrateUp();

    /// <summary>
    /// This is a single method that will check the database connection string, check if the database is valid, create the database if necessary
    /// and migrate the databse to the laatest version.
    /// </summary>
    /// <returns>Based on the four methods: IsConnectionStringValid, IsDatabaseValid, CreateDatabase, MigrateUp</returns>
    ReturnValue DatabaseStartUp();
}