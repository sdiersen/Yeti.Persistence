
using Dapper;

using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
internal class CategoryParams
{
    internal DynamicParameters FullCategoryParamsNoId(Category category)
    {
        var dp = new DynamicParameters();
        dp.Add("@Name", category.Name);
        dp.Add("@Description", category.Description);
        dp.Add("@CreatedOn", category.CreatedOn);
        dp.Add("@ModifiedOn", category.ModifiedOn);
        return dp;
    }

    internal DynamicParameters FullCategoryParams(Category category)
    {
        var dp = FullCategoryParamsNoId(category);
        dp.Add("@Id", category.Id);
        return dp;
    }
}
