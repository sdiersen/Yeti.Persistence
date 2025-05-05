using Dapper;

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

    //******************************************************************************************************
    // Item specific methods
    //******************************************************************************************************
    public ReturnValue<List<Item>> GetAllItemsByCategoryId(int categoryId)
    {
        var returnValue = new ReturnValue<List<Item>>();

        try
        {
            var sql = ItemSQL.GetAllItemsByCategoryIdSQL;
            var items = Connection.Query<Item>(sql, new { CategoryId = categoryId }, Transaction).AsList();
            if (items.Count > 0)
            {
                returnValue.Data = items;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("getitembycategoryid", "No items found for the specified category ID.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<Item>>> GetAllItemsByCategoryIdAsync(int categoryId)
    {
        var returnValue = new ReturnValue<List<Item>>();

        try
        {
            var sql = ItemSQL.GetAllItemsByCategoryIdSQL;
            var items = (await Connection.QueryAsync<Item>(sql, new { CategoryId = categoryId }, Transaction)).AsList();
            if (items.Count > 0)
            {
                returnValue.Data = items;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("getitembycategoryid", "No items found for the specified category ID.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }

    public ReturnValue<int> GetUnattachedId()
    {
        var returnValue = new ReturnValue<int>();
        try
        {
            var sql = ItemSQL.GetUnattachedIdSQL;
            var id = Connection.QuerySingleOrDefault<int>(sql, Transaction);
            if (id > 0)
            {
                returnValue.Data = id;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", "No row in Item table with Name='unattached'.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public async Task<ReturnValue<int>> GetUnattachedIdAsync()
    {
        var returnValue = new ReturnValue<int>();
        try
        {
            var sql = ItemSQL.GetUnattachedIdSQL;
            var id = await Connection.QuerySingleOrDefaultAsync<int>(sql, Transaction);
            if (id > 0)
            {
                returnValue.Data = id;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", "No row in Item table with Name='unattached'.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
}
