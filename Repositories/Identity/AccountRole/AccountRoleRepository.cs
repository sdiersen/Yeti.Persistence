
using Dapper;

using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;

public class AccountRoleRepository : BaseRepository<AccountRole, AccountRoleRepository>, IRepository<AccountRole>
{
    private readonly AccountRoleParams _params;

    public AccountRoleRepository(ILogger<AccountRoleRepository> logger, SqlConnection connection, SqlTransaction transaction) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.ACCOUNT_ROLE_TABLE;
        _params = new AccountRoleParams();
    }

    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<AccountRole> GetRow(AccountRole row)
    {
        return GetRow(row.Id);
    }
    public async Task<ReturnValue<AccountRole>> GetRowAsync(AccountRole row)
    {
        return await GetRowAsync(row.Id);
    }

    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(AccountRole row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return InsertRowBase(AccountRoleSQL.InsertRowSQL, _params.FullAccountRoleParamsNoId(row));
    }
    public async Task<ReturnValue> InsertRowAsync(AccountRole row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowBaseAsync(AccountRoleSQL.InsertRowSQL, _params.FullAccountRoleParamsNoId(row));
    }
    public ReturnValue<int> InsertRowAndGetId(AccountRole row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return InsertRowAndGetIdBase(AccountRoleSQL.InsertRowAndGetIdSQL, _params.FullAccountRoleParamsNoId(row));
    }
    public async Task<ReturnValue<int>> InsertRowAndGetIdAsync(AccountRole row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowAndGetIdBaseAsync(AccountRoleSQL.InsertRowAndGetIdSQL, _params.FullAccountRoleParamsNoId(row));
    }

    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(AccountRole row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        return UpdateRowBase(AccountRoleSQL.UpdateRowSQL, _params.FullAccountRoleParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(AccountRole row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        return await UpdateRowBaseAsync(AccountRoleSQL.UpdateRowSQL, _params.FullAccountRoleParams(row));
    }
    public ReturnValue UpdateRow(int id, AccountRole row)
    {
        row.Id = id;
        return UpdateRow(row);
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, AccountRole row)
    {
        row.Id = id;
        return await UpdateRowAsync(row);
    }
    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(AccountRole row)
    {
        try
        {
            var id = Connection.QuerySingleOrDefault<int>(AccountRoleSQL.GetIdFromRoleAndAccountIdSQL, _params.RoleAndAccountIdParams(row), Transaction);
            if (id == 0)
            {
                var returnValue = new ReturnValue();
                returnValue.AddMessage("accountrole", "No matching AccountRole found for deletion.");
                return returnValue;
            }
            return DeleteRow(id);
        }
        catch (Exception ex)
        {
            var returnValue = new ReturnValue();
            returnValue.AddError("accountrole", $"Error deleting AccountRole: {ex.Message}");
            return returnValue;
        }
    }
    public async Task<ReturnValue> DeleteRowAsync(AccountRole row)
    {
        try
        {
            var id = await Connection.QuerySingleOrDefaultAsync<int>(AccountRoleSQL.GetIdFromRoleAndAccountIdSQL, _params.RoleAndAccountIdParams(row), Transaction);
            if (id == 0)
            {
                var returnValue = new ReturnValue();
                returnValue.AddMessage("accountrole", "No matching AccountRole found for deletion.");
                return returnValue;
            }
            return await DeleteRowAsync(id);
        }
        catch (Exception ex)
        {
            var returnValue = new ReturnValue();
            returnValue.AddError("accountrole", $"Error deleting AccountRole: {ex.Message}");
            return returnValue;
        }
    }
    //******************************************************************************************************
    // AccountRole specific methods
    //******************************************************************************************************

    //Get a list of AccountRoles for a given AccountId
    public ReturnValue<List<AccountRole>> GetRolesForAccountId(int accountId)
    {
        var returnValue = new ReturnValue<List<AccountRole>>
        {
            Data = []
        };
        try
        {
            var roles = Connection.Query<AccountRole>(AccountRoleSQL.GetRolesForAccountIdSQL, new { AccountId = accountId }, Transaction).ToList();
            if (roles.Count > 0)
            {
                returnValue.Data = roles;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No roles found for AccountId {accountId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving roles for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<AccountRole>>> GetRolesForAccountIdAsync(int accountId)
    {
        var returnValue = new ReturnValue<List<AccountRole>>
        {
            Data = []
        };
        try
        {
            var roles = (await Connection.QueryAsync<AccountRole>(AccountRoleSQL.GetRolesForAccountIdSQL, new { AccountId = accountId }, Transaction)).ToList();
            if (roles.Count > 0)
            {
                returnValue.Data = roles;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No roles found for AccountId {accountId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving roles for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }

    // Get a list of Role IDs for a given AccountId
    public ReturnValue<List<int>> GetRoleIdsForAccountId(int accountId)
    {
        var returnValue = new ReturnValue<List<int>>()
        {
            Data = []
        };
        try
        {
            var roleIds = Connection.Query<int>(AccountRoleSQL.GetRoleIdsForAccountIdSQL, new { AccountId = accountId }, Transaction).ToList();
            if (roleIds.Count > 0)
            {
                returnValue.Data = roleIds;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No role IDs found for AccountId {accountId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving role IDs for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<int>>> GetRoleIdsForAccountIdAsync(int accountId)
    {
        var returnValue = new ReturnValue<List<int>>
        {
            Data = []
        };
        try
        {
            var roleIds = (await Connection.QueryAsync<int>(AccountRoleSQL.GetRoleIdsForAccountIdSQL, new { AccountId = accountId }, Transaction)).ToList();
            if (roleIds.Count > 0)
            {
                returnValue.Data = roleIds;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No role IDs found for AccountId {accountId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving role IDs for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }

    // Get a List of AccountRoles for a given RoleId
    public ReturnValue<List<AccountRole>> GetAccountRolesForRoleId(int roleId)
    {
        var returnValue = new ReturnValue<List<AccountRole>>
        { 
            Data = [] 
        };
        try
        {
            var accountRoles = Connection.Query<AccountRole>(AccountRoleSQL.GetAccountRolesForRoleIdSQL, new { RoleId = roleId }, Transaction).ToList();
            if (accountRoles.Count > 0)
            {
                returnValue.Data = accountRoles;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No AccountRoles found for RoleId {roleId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving AccountRoles for RoleId {roleId}: {ex.Message}");
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<AccountRole>>> GetAccoutRolesForRoleIdAsync(int roleId)
    {
        var returnValue = new ReturnValue<List<AccountRole>>
        {
            Data = []
        };
        try
        {
            var accountRoles = (await Connection.QueryAsync<AccountRole>(AccountRoleSQL.GetAccountRolesForRoleIdSQL, new { RoleId = roleId }, Transaction)).ToList();
            if (accountRoles.Count > 0)
            {
                returnValue.Data = accountRoles;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No AccountRoles found for RoleId {roleId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving AccountRoles for RoleId {roleId}: {ex.Message}");
        }
        return returnValue;
    }

    // Get a List of AccountIds for a given RoleId
    public ReturnValue<List<int>> GetAccountIdsForRoleId(int roleId)
    {
        var returnValue = new ReturnValue<List<int>>
        {
            Data = []
        };
        try
        {
            var accountIds = Connection.Query<int>(AccountRoleSQL.GetAccountIdsForRoleIdSQL, new { RoleId = roleId }, Transaction).ToList();
            if (accountIds.Count > 0)
            {
                returnValue.Data = accountIds;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No AccountIds found for RoleId {roleId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving AccountIds for RoleId {roleId}: {ex.Message}");
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<int>>> GetAccountIdsForRoleIdAsync(int roleId)
    {
        var returnValue = new ReturnValue<List<int>>
        {
            Data = []
        };
        try
        {
            var accountIds = (await Connection.QueryAsync<int>(AccountRoleSQL.GetAccountIdsForRoleIdSQL, new { RoleId = roleId }, Transaction)).ToList();
            if (accountIds.Count > 0)
            {
                returnValue.Data = accountIds;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("accountrole", $"No AccountIds found for RoleId {roleId}.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error retrieving AccountIds for RoleId {roleId}: {ex.Message}");
        }
        return returnValue;
    }

    // Update the RoleIds for a specific AccountId
    public ReturnValue UpdateRolesForAccountId(int accountId, List<int> roleIds)
    {
        var returnValue = new ReturnValue();
        try
        {
            // First, delete existing roles for the account
            var deleteReturnValue = DeleteRow(new AccountRole { AccountId = accountId });
            if (!deleteReturnValue.Success)
            {
                return deleteReturnValue;
            }
            // Then, insert the new roles
            foreach (var roleId in roleIds)
            {
                var accountRole = new AccountRole { AccountId = accountId, RoleId = roleId };
                var insertReturnValue = InsertRow(accountRole);
                if (!insertReturnValue.Success)
                {
                    return insertReturnValue;
                }
            }
            returnValue.Success = true;
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error updating RoleIds for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }
    public async Task<ReturnValue> UpdateRolesForAccountIdAsync(int accountId, List<int> roleIds)
    {
        var returnValue = new ReturnValue();
        try
        {
            // First, delete existing roles for the account
            var deleteReturnValue = await DeleteRowAsync(new AccountRole { AccountId = accountId });
            if (!deleteReturnValue.Success)
            {
                return deleteReturnValue;
            }
            // Then, insert the new roles
            foreach (var roleId in roleIds)
            {
                var accountRole = new AccountRole { AccountId = accountId, RoleId = roleId };
                var insertReturnValue = await InsertRowAsync(accountRole);
                if (!insertReturnValue.Success)
                {
                    return insertReturnValue;
                }
            }
            returnValue.Success = true;
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error updating RoleIds for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }

    // Delete all roles for a specific AccountId
    public ReturnValue DeleteRolesForAccountId(int accountId)
    {
        var returnValue = new ReturnValue();
        try
        {
            var results = Connection.Execute(AccountRoleSQL.DeleteRolesForAccountIdSQL, new { AccountId = accountId }, Transaction);
            if (results > 0)
            {
                returnValue.AddMessage("accountrole", $"Successfully deleted {results} roles for AccountId {accountId}.");
            }
            returnValue.Success = true;
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error deleting roles for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }
    public async Task<ReturnValue> DeleteRolesForAccountIdAsync(int accountId)
    {
        var returnValue = new ReturnValue();
        try
        {
            var results = await Connection.ExecuteAsync(AccountRoleSQL.DeleteRolesForAccountIdSQL, new { AccountId = accountId }, Transaction);
            if (results > 0)
            {
                returnValue.AddMessage("accountrole", $"Successfully deleted {results} roles for AccountId {accountId}.");
            }
            returnValue.Success = true;
        }
        catch (Exception ex)
        {
            returnValue.AddError("accountrole", $"Error deleting roles for AccountId {accountId}: {ex.Message}");
        }
        return returnValue;
    }
}