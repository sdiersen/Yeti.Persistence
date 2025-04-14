using Dapper;

using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;
internal class RoleParams
{

    internal DynamicParameters FullRoleParamsNoId(Role row)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@RoleName", row.RoleName);
        parameters.Add("@RoleNumber", row.RoleNumber);
        parameters.Add("@Description", row.Description);
        return parameters;
    }
    internal DynamicParameters FullRoleParams(Role row)
    {
        var parameters = FullRoleParamsNoId(row);
        parameters.Add("@Id", row.Id);
        return parameters;
    }
    internal DynamicParameters RoleNameParams(string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@RoleName", name);
        return parameters;
    }
}
