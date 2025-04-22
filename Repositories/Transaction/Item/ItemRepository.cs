using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
public class ItemRepository : BaseRepository<Item, ItemRepository>, IRepository<Item>
{
    private readonly ItemParams _params;
    public ItemRepository(ILogger<ItemRepository> logger, SqlConnection connection, SqlTransaction? transaction = null) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.ITEM_TABLE;
        _params = new ItemParams();
    }
    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<Item> GetRow(Item row)
    {
        return GetRow(row.Id);
    }
    public async Task<ReturnValue<Item>> GetRowAsync(Item row)
    {
        return await GetRowAsync(row.Id);
    }
    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(Item row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return InsertRowBase(ItemSQL.InsertRowSQL, _params.FullItemParamsNoId(row));
    }
    public async Task<ReturnValue> InsertRowAsync(Item row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowBaseAsync(ItemSQL.InsertRowSQL, _params.FullItemParamsNoId(row));
    }
    public ReturnValue<int> InsertRowAndGetId(Item row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return InsertRowAndGetIdBase(ItemSQL.InsertRowAndGetIdSQL, _params.FullItemParamsNoId(row));
    }
    public async Task<ReturnValue<int>> InsertRowAndGetIdAsync(Item row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowAndGetIdBaseAsync(ItemSQL.InsertRowAndGetIdSQL, _params.FullItemParamsNoId(row));
    }
    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(int id, Item row)
    {
        row.Id = id;
        row.ModifiedOn = DateTime.UtcNow;
        return UpdateRowBase(ItemSQL.UpdateRowSQL, _params.FullItemParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, Item row)
    {
        row.Id = id;
        row.ModifiedOn = DateTime.UtcNow;
        return await UpdateRowBaseAsync(ItemSQL.UpdateRowSQL, _params.FullItemParams(row));
    }
    public ReturnValue UpdateRow(Item row)
    {
        return UpdateRow(row.Id, row);
    }
    public async Task<ReturnValue> UpdateRowAsync(Item row)
    {
        return await UpdateRowAsync(row.Id, row);
    }
    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(Item row)
    {
        return DeleteRow(row.Id);
    }
    public async Task<ReturnValue> DeleteRowAsync(Item row)
    {
        return await DeleteRowAsync(row.Id);
    }
}
