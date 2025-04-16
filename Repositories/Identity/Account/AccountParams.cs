using Dapper;

using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;
internal class AccountParams
{
    internal DynamicParameters AccountParamsNoId(Account row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserName", row.UserName);
        parameters.Add("@Password", row.Password);
        parameters.Add("@LastLogin", row.LastLogin);
        parameters.Add("@IsActive", row.IsActive);
        parameters.Add("@IsLocked", row.IsLocked);
        parameters.Add("@CreatedOn", row.CreatedOn);
        parameters.Add("@ModifiedOn", row.ModifiedOn);
        return parameters;
    }
    internal DynamicParameters AccountParamsWithId(Account row)
    {
        var parameters = AccountParamsNoId(row);
        parameters.Add("@Id", row.Id);
        return parameters;
    }
    internal DynamicParameters UsernameAndPasswordParam(string username, string password)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserName", username);
        parameters.Add("@Password", password);
        return parameters;
    }
}
