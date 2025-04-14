using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
public class CategoryItemRepository : BaseRepository<CategoryItem, CategoryItemRepository>, IRepository<CategoryItem>
{
    private readonly CategoryItemParams _params;
    public CategoryItemRepository(ILogger<CategoryItemRepository> logger, SqlConnection connection, SqlTransaction transaction) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.CATEGORY_ITEM_TABLE;
        _params = new CategoryItemParams();
    }
    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<CategoryItem> GetRow(CategoryItem row)
    {
        return GetRow(row.Id);
    }
    public async Task<ReturnValue<CategoryItem>> GetRowAsync(CategoryItem row)
    {
        return await GetRowAsync(row.Id);
    }
    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(CategoryItem row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return InsertRowBase(CategoryItemSQL.InsertRowSQL, _params.FullCategoryItemParamsNoId(row));
    }
    public async Task<ReturnValue> InsertRowAsync(CategoryItem row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowBaseAsync(CategoryItemSQL.InsertRowSQL, _params.FullCategoryItemParamsNoId(row));
    }
    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(CategoryItem row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        return UpdateRowBase(CategoryItemSQL.UpdateRowSQL, _params.FullCategoryItemParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(CategoryItem row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        return await UpdateRowBaseAsync(CategoryItemSQL.UpdateRowSQL, _params.FullCategoryItemParams(row));
    }
    public ReturnValue UpdateRow(int id, CategoryItem row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        row.Id = id;
        return UpdateRowBase(CategoryItemSQL.UpdateRowSQL, _params.FullCategoryItemParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, CategoryItem row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        row.Id = id;
        return await UpdateRowBaseAsync(CategoryItemSQL.UpdateRowSQL, _params.FullCategoryItemParams(row));
    }
    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(CategoryItem row)
    {
        return DeleteRow(row.Id);
    }
    public async Task<ReturnValue> DeleteRowAsync(CategoryItem row)
    {
        return await DeleteRowAsync(row.Id);
    }
}
