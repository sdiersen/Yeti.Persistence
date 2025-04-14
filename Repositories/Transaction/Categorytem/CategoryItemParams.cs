
using Dapper;

using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
internal class CategoryItemParams
{
    internal DynamicParameters FullCategoryItemParamsNoId(CategoryItem row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CategoryId", row.CategoryId);
        parameters.Add("@ItemId", row.ItemId);
        parameters.Add("@CreatedOn", row.CreatedOn);
        parameters.Add("@ModifiedOn", row.ModifiedOn);

        return parameters;
    }

    internal DynamicParameters FullCategoryItemParams(CategoryItem row)
    {
        var parameters = FullCategoryItemParamsNoId(row);
        parameters.Add("@Id", row.Id);

        return parameters;
    }
}
