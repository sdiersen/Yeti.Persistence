
using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
public class CategoryRepository : BaseRepository<Category, CategoryRepository>, IRepository<Category>
{
    private readonly CategoryParams _params;
    public CategoryRepository(ILogger<CategoryRepository> logger, SqlConnection connection, SqlTransaction transaction) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.CATEGORY_TABLE;
        _params = new CategoryParams();
    }
    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<Category> GetRow(Category row)
    {
        return GetRow(row.Id);
    }
    public async Task<ReturnValue<Category>> GetRowAsync(Category row)
    {
        return await GetRowAsync(row.Id);
    }
    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(Category row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;

        return InsertRowBase(CategorySQL.InsertRowSQL, _params.FullCategoryParamsNoId(row));
    }
    public async Task<ReturnValue> InsertRowAsync(Category row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowBaseAsync(CategorySQL.InsertRowSQL, _params.FullCategoryParamsNoId(row));
    }
    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(int id, Category row)
    {
        row.Id = id;
        row.ModifiedOn = DateTime.UtcNow;
        return UpdateRowBase(CategorySQL.UpdateRowSQL, _params.FullCategoryParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, Category row)
    {
        row.Id = id;
        row.ModifiedOn = DateTime.UtcNow;
        return await UpdateRowBaseAsync(CategorySQL.UpdateRowSQL, _params.FullCategoryParams(row));
    }
    public ReturnValue UpdateRow(Category row)
    {
        return UpdateRow(row.Id, row);
    }
    public async Task<ReturnValue> UpdateRowAsync(Category row)
    {
        return await UpdateRowAsync(row.Id, row);
    }
    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(Category row)
    {
        return DeleteRow(row.Id);
    }
    public async Task<ReturnValue> DeleteRowAsync(Category row)
    {
        return await DeleteRowAsync(row.Id);
    }
}
