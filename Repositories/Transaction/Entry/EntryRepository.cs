using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
public class EntryRepository : BaseRepository<Entry, EntryRepository>, IRepository<Entry>
{
    private readonly EntryParams _params;
    public EntryRepository(ILogger<EntryRepository> logger, SqlConnection connection, SqlTransaction transaction) : base(logger, connection, transaction)
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
}
