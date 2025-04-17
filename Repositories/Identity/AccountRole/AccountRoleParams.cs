using Dapper;

using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;
internal class AccountRoleParams
{
    internal DynamicParameters FullAccountRoleParamsNoId(AccountRole row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AccountId", row.AccountId);
        parameters.Add("@RoleId", row.RoleId);
        parameters.Add("@CreatedOn", row.CreatedOn);
        parameters.Add("@ModifiedOn", row.ModifiedOn);

        return parameters;
    }
    internal DynamicParameters FullAccountRoleParams(AccountRole row)
    {
        var parameters = FullAccountRoleParamsNoId(row);
        parameters.Add("@Id", row.Id);

        return parameters;
    }
    internal DynamicParameters RoleAndAccountIdParams(AccountRole row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@AccountId", row.AccountId);
        parameters.Add("@RoleId", row.RoleId);
        return parameters;
    }
}
