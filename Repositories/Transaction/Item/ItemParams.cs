using Dapper;

using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
internal class ItemParams
{
    internal DynamicParameters FullItemParamsNoId(Item row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("CreatedOn", row.CreatedOn);
        parameters.Add("ModifiedOn", row.ModifiedOn);
        parameters.Add("Name", row.Name);
        parameters.Add("Note", row.Note);
        parameters.Add("BudgetAmount", row.BudgetAmount);
        parameters.Add("CurrentAmount", row.CurrentAmount);
        parameters.Add("CategoryId", row.CategoryId);
        parameters.Add("IsExpense", row.IsExpense);
        return parameters;
    }
    internal DynamicParameters FullItemParams(Item row)
    {
        var parameters = FullItemParamsNoId(row);
        parameters.Add("Id", row.Id);
        return parameters;
    }
}
