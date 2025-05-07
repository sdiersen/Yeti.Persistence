using Dapper;

using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Persistence.Migrations.Constants;
using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
public class EntryRepository : BaseRepository<Entry, EntryRepository>, IRepository<Entry>
{
    private readonly EntryParams _params;
    public EntryRepository(ILogger<EntryRepository> logger, SqlConnection connection, SqlTransaction? transaction = null) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.ENTRY_TABLE;
        _params = new EntryParams();
    }
    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<Entry> GetRow(Entry row)
    {
        return GetRow(row.Id);
    }
    public async Task<ReturnValue<Entry>> GetRowAsync(Entry row)
    {
        return await GetRowAsync(row.Id);
    }
    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(Entry row)
    {
        row.ModifiedOn = DateTime.Now;
        row.CreatedOn = DateTime.Now;
        return InsertRowBase(EntrySQL.InsertRowSQL, _params.FullEntryParamsNoId(row));
    }
    public async Task<ReturnValue> InsertRowAsync(Entry row)
    {
        row.ModifiedOn = DateTime.Now;
        row.CreatedOn = DateTime.Now;
        return await InsertRowBaseAsync(EntrySQL.InsertRowSQL, _params.FullEntryParamsNoId(row));
    }
    public ReturnValue<int> InsertRowAndGetId(Entry row)
    {
        row.ModifiedOn = DateTime.Now;
        row.CreatedOn = DateTime.Now;
        return InsertRowAndGetIdBase(EntrySQL.InsertRowAndGetIdSQL, _params.FullEntryParamsNoId(row));
    }
    public async Task<ReturnValue<int>> InsertRowAndGetIdAsync(Entry row)
    {
        row.ModifiedOn = DateTime.Now;
        row.CreatedOn = DateTime.Now;
        return await InsertRowAndGetIdBaseAsync(EntrySQL.InsertRowAndGetIdSQL, _params.FullEntryParamsNoId(row));
    }
    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(int id, Entry row)
    {
        row.ModifiedOn = DateTime.Now;
        row.Id = id;
        return UpdateRowBase(EntrySQL.UpdateRowSQL, _params.FullEntryParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, Entry row)
    {
        row.ModifiedOn = DateTime.Now;
        row.Id = id;
        return await UpdateRowBaseAsync(EntrySQL.UpdateRowSQL, _params.FullEntryParams(row));
    }
    public ReturnValue UpdateRow(Entry row)
    {
        return UpdateRow(row.Id, row);
    }
    public async Task<ReturnValue> UpdateRowAsync(Entry row)
    {
        return await UpdateRowAsync(row.Id, row);
    }
    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(Entry row)
    {
        return DeleteRow(row.Id);
    }
    public async Task<ReturnValue> DeleteRowAsync(Entry row)
    {
        return await DeleteRowAsync(row.Id);
    }
    //******************************************************************************************************
    // Entry specific methods
    //******************************************************************************************************
    public ReturnValue<List<Entry>> GetAllEntriesForItemId(int itemId)
    {
        var returnValue = new ReturnValue<List<Entry>>();
        try
        {
            var result = Connection.Query<Entry>(EntrySQL.GetAllEntriesForItemIdSQL, new { ItemId = itemId },
                Transaction
            );
            returnValue.Data = result.ToList();
            returnValue.Success = true;
            if (returnValue.Data.Count <= 0)
            {
                returnValue.AddMessage("database", "No entries found for item id: " + itemId);
            }            
        }
        catch (SqlException ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<Entry>>> GetAllEntriesForItemIdAsync(int itemId)
    {
        var returnValue = new ReturnValue<List<Entry>>();
        try
        {
            var result = await Connection.QueryAsync<Entry>(EntrySQL.GetAllEntriesForItemIdSQL, new { ItemId = itemId },
                Transaction
            );
            returnValue.Data = result.ToList();
            returnValue.Success = true;
            if (returnValue.Data.Count <= 0)
            {
                returnValue.AddMessage("database", "No entries found for item id: " + itemId);
            }

        }
        catch (SqlException ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }

}
