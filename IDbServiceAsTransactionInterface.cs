using ErrorHandling;
using Microsoft.Data.SqlClient;

namespace Persistence;

/// <summary>
/// Interface for the database services.
/// All Models will have a service that implements this interface.
/// The service will be responsible for all database operations that require a transaction.
/// Service will implement this via the DbLayerBase&lt;T&gt; class. 
/// which is an abstract class providing basic functionality for some methods.
/// When writing a single database interaction, user should use either the AsTransaction service or the base service
/// don't mix services. These services will be a little heavier than the base service.
/// </summary>
/// <typeparam name="T">T is the Model object</typeparam>
public interface IDbServiceAsTransactionInterface<T>
{
    /// <summary>
    /// Updates a row in the database as part of a transaction.
    /// </summary>
    /// <param name="id">the id of the row to update</param>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue UpdateRowAsTransaction(int id, T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Updates a row in the database as part of a transaction.
    /// </summary>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue UpdateRowAsTransaction(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Updates a row in the database as part of a transaction asynchronously.
    /// </summary>
    /// <param name="id">the id of the row to update</param>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> UpdateRowAsTransactionAsync(int id, T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Updates a row in the database as part of a transaction asynchronously. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> UpdateRowAsTransactionAsync(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Inserts a row into the database as part of a transaction. 
    /// </summary>
    /// <param name="row">The row of type T that will be inserted into the database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was inserted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the insert failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue InsertRowAsTransaction(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Inserts a row into the database as part of a transaction asynchronously. 
    /// </summary>
    /// <param name="row">The row of type T that will be inserted into the database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was inserted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the insert failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> InsertRowAsTransactionAsync(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Deletes a row from the database as part of a transaction. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The row of type T that will be deleted from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue DeleteRowAsTransaction(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Deletes a row from the database asynchronously as part of a transaction. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The row of type T that will be deleted from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> DeleteRowAsTransactionAsync(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Gets a row from the database as part of a transaction. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The row of type T that will be deleted from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns></returns>
    ReturnValue<T> GetRowAsTransaction(T row, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Gets a row from the database asynchronously as part of a transaction. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The row of type T that will be deleted from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue<T>> GetRowAsTransactionAsync(T row, SqlConnection connection, SqlTransaction transaction);

    // The following are usually provided by an abstract class
    // currently that is DbBaseLayerAsTransaction<T>

    /// <summary>
    /// Deletes a row from the database as part of a transaction.
    /// </summary>
    /// <param name="id">The id of the row that will be deleted from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue DeleteRowAsTransaction(int id, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Deletes a row from the database asynchronously as part of a transaction.
    /// </summary>
    /// <param name="id">The id of the row that will be deleted from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> DeleteRowAsTransactionAsync(int id, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Gets a row from the database as part of a transaction.
    /// </summary>
    /// <param name="id">The id of the row to get from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue<T> GetRowAsTransaction(int id, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Gets a row from the database asynchronously as part of a transaction.
    /// </summary>
    /// <param name="id">The id of the row to get from database</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue<T>> GetRowAsTransactionAsync(int id, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Gets up to the first x or all the rows from the database as part of a transaction.
    /// </summary>
    /// <param name="x">x is the number of rows to return. An x &lt;=0 will return all rows.</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the rows were found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue<List<T>> GetFirstXRowsAsTransaction(int x, SqlConnection connection, SqlTransaction transaction);
    /// <summary>
    /// Gets up to the first x or all the rows from the database asynchronously as part of a transaction.
    /// </summary>
    /// <param name="x">x is the number of rows to return. An x &lt;= 0 will return all rows.</param>
    /// <param name="connection">the connection to the database</param>
    /// <param name="transaction">the transaction to use</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the rows were found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue<List<T>>> GetFirstXRowsAsTransactionAsync(int x, SqlConnection connection, SqlTransaction transaction);
}
