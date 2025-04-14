using Dapper;

using Persistence.Models.Transaction;

namespace Persistence.Repositories.Transaction;
internal class EntryParams
{
    internal DynamicParameters FullEntryParamsNoId(Entry row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("CreatedOn", row.CreatedOn);
        parameters.Add("ModifiedOn", row.ModifiedOn);
        parameters.Add("Amount", row.Amount);
        parameters.Add("IsExpense", row.IsExpense);
        parameters.Add("EntryDate", row.EntryDate);
        parameters.Add("CategoryId", row.CategoryId);
        parameters.Add("ItemId", row.ItemId);
        parameters.Add("Note", row.Note);
        return parameters;
    }

    internal DynamicParameters FullEntryParams(Entry row)
    {
        var parameters = FullEntryParamsNoId(row);
        parameters.Add("Id", row.Id);
        return parameters;
    }
}
