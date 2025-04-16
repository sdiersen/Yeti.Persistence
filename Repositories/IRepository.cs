using ErrorHandling;

using Persistence.Models;

namespace Persistence.Repositories;
/// <summary>
/// IRepository interface
/// </summary>
/// <typeparam name="T">Persistence.Models entity</typeparam>
public interface IRepository<T> where T : IBaseModel
{
    /// <summary>
    /// Updates a row in the database.
    /// </summary>
    /// <param name="id">the id of the row to update</param>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue UpdateRow(int id, T row);

    /// <summary>
    /// Updates a row in the database.
    /// Used when the id of the row is not known. It is up to the implementing class
    /// to determine how to find the row to update.
    /// </summary>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue UpdateRow(T row);

    /// <summary>
    /// Inserts a row into the database asynchronously.
    /// </summary>
    /// <param name="id">the id of the row to update</param>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> UpdateRowAsync(int id, T row);

    /// <summary>
    /// Updates a row in the database asynchronously.
    /// Used when the id of the row is not known. It is up to the implementing class
    /// to determine how to find the row to update.
    /// </summary>
    /// <param name="row">The T object that will set the updated values of the row.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was updated, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the update failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> UpdateRowAsync(T row);

    /// <summary>
    /// Inserts a row into the database. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The T object that will become a new row in the database.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was inserted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the insert failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue InsertRow(T row);

    /// <summary>
    /// Inserts a row into the database asynchronously. 
    /// </summary>
    /// <param name="row">The T object that will become a new row in the database.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was inserted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the insert failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> InsertRowAsync(T row);

    ReturnValue<int> InsertRowAndGetId(T row);
    Task<ReturnValue<int>> InsertRowAndGetIdAsync(T row);

    /// <summary>
    /// Deletes a row from the database. It is up to the implementing class
    /// to determine how to find the row to delete.
    /// </summary>
    /// <param name="row">The T object that will be used to determine which row gets deleteed.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue DeleteRow(T row);

    /// <summary>
    /// Deletes a row from the database asynchronously. It is up to the implementing class
    /// to determine how to find the row to delete.
    /// </summary>
    /// <param name="row">The T object that will be used to determine which row gets deleteed.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> DeleteRowAsync(T row);

    /// <summary>
    /// Gets a row from the database. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The T object that will be used to determine which row gets returned.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue<T> GetRow(T row);

    /// <summary>
    /// Gets a row from the database asynchronously. It is up to the implementing class
    /// to determine how to find the row that will be returned.
    /// </summary>
    /// <param name="row">The T object that will be used to determine which row gets returned.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue<T>> GetRowAsync(T row);

    // The following are usually provided by an abstract class
    // currently that is DbBaseLayer<T>

    /// <summary>
    /// Deletes a row from the database.
    /// </summary>
    /// <param name="id">The id of the row to be deleted</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue DeleteRow(int id);

    /// <summary>
    /// Deletes a row from the database asynchronously.
    /// </summary>
    /// <param name="id">The id of the row to be deleted</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was deleted, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the delete failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue> DeleteRowAsync(int id);

    /// <summary>
    /// Gets a row from the database.
    /// </summary>
    /// <param name="id">The id of the row to be returned</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue<T> GetRow(int id);

    /// <summary>
    /// Gets a row from the database asynchronously.
    /// </summary>
    /// <param name="id">The id of the row to be returned</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the row was found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue<T>> GetRowAsync(int id);

    /// <summary>
    /// Gets all the rows from the database.
    /// </summary>
    /// <param name="x">x is the number of rows to return. An x &lt;= 0 will return all rows.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the rows were found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    ReturnValue<List<T>> GetFirstXRows(int x);

    /// <summary>
    /// Gets all the rows from the database asynchronously.
    /// </summary>
    /// <param name="x">x is the number of rows to return. An x &lt;= 0 will return all rows.</param>
    /// <returns>
    /// A ReturnValue object with Success = true if the rows were found, false otherwise.
    /// If Success = false, the ReturnValue.Messages will contain the logical reasons why
    /// the get failed. ReturnValue.Errors will contain any exceptions messages that were thrown.
    /// </returns>
    Task<ReturnValue<List<T>>> GetFirstXRowsAsync(int x);
}
