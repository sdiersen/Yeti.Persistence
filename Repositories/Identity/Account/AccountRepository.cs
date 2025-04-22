
using ErrorHandling;

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;
public class AccountRepository : BaseRepository<Account, AccountRepository>, IRepository<Account>
{
    private readonly AccountParams _params;

    public AccountRepository(ILogger<AccountRepository> logger, SqlConnection connection, SqlTransaction? transaction = null) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.ACCOUNT_TABLE;
        _params = new AccountParams();
    }

    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<Account> GetRow(Account row)
    {
        var returnValue = new ReturnValue<Account>();
        try
        {
            var parameters = _params.UsernameAndPasswordParam(row.UserName, row.Password);
            var sql = AccountSQL.GetAccountByUsernameAndPassword;
            var account = Connection.QuerySingleOrDefault<Account>(sql, parameters, Transaction);
            if (account != null)
            {
                returnValue.Success = true;
                returnValue.Data = account;
            }
            else
            {
                returnValue.AddMessage("database", "Invalid username or password");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddMessage("database", ex.Message);
        }
        return returnValue;
    }
    public async Task<ReturnValue<Account>> GetRowAsync(Account row)
    {
        var returnValue = new ReturnValue<Account>();
        try
        {
            var parameters = _params.UsernameAndPasswordParam(row.UserName, row.Password);
            var sql = AccountSQL.GetAccountByUsernameAndPassword;
            var account = await Connection.QuerySingleOrDefaultAsync<Account>(sql, parameters, Transaction);
            if (account != null)
            {
                returnValue.Success = true;
                returnValue.Data = account;
            }
            else
            {
                returnValue.AddMessage("database", "Invalid username or password");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddMessage("database", ex.Message);
        }
        return returnValue;
    }

    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(Account row)
    {
        row = SetCreationValues(row);

        var returnValue = new ReturnValue();
        try
        {
            var parameters = _params.AccountParamsNoId(row);
            var sql = AccountSQL.InsertRowSQL;
            var result = Connection.Execute(sql, parameters, Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", "Account created successfully.");
            }
            else
            {
                returnValue.AddMessage("database", "Failed to create account.");
            }
        }
        catch (Exception ex)
        {
            returnValue = AccountErrorsAndMessages.AccountModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    public async Task<ReturnValue> InsertRowAsync(Account row)
    {
        row = SetCreationValues(row);
        var returnValue = new ReturnValue();
        try
        {
            var parameters = _params.AccountParamsNoId(row);
            var sql = AccountSQL.InsertRowSQL;
            var result = await Connection.ExecuteAsync(sql, parameters, Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", "Account created successfully.");
            }
            else
            {
                returnValue.AddMessage("database", "Failed to create account.");
            }
        }
        catch (Exception ex)
        {
            returnValue = AccountErrorsAndMessages.AccountModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    public ReturnValue<int> InsertRowAndGetId(Account row)
    {
        row = SetCreationValues(row);
        var returnValue = new ReturnValue<int>();
        try
        {
            var parameters = _params.AccountParamsNoId(row);
            var sql = AccountSQL.InsertRowAndGetIdSQL;
            var result = Connection.ExecuteScalar<int>(sql, parameters, Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", "Account created successfully.");
                returnValue.Data = result;
            }
            else
            {
                returnValue.AddMessage("database", "Failed to create account.");
            }
        }
        catch (Exception ex)
        {
            returnValue = AccountErrorsAndMessages.AccountModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    public async Task<ReturnValue<int>> InsertRowAndGetIdAsync(Account row)
    {
        row = SetCreationValues(row);
        var returnValue = new ReturnValue<int>();
        try
        {
            var parameters = _params.AccountParamsNoId(row);
            var sql = AccountSQL.InsertRowAndGetIdSQL;
            var result = await Connection.ExecuteScalarAsync<int>(sql, parameters, Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", "Account created successfully.");
                returnValue.Data = result;
            }
            else
            {
                returnValue.AddMessage("database", "Failed to create account.");
            }
        }
        catch (Exception ex)
        {
            returnValue = AccountErrorsAndMessages.AccountModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }

    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(Account row)
    {
        var resultValue = GetRowId(row.UserName, row.Password);
        if (!resultValue.Success)
        {
            var returnValue = new ReturnValue();
            returnValue.AddMessage("database", "Account not found.");
            return returnValue;
        }
        return UpdateRow(resultValue.Data, row);
    }
    public async Task<ReturnValue> UpdateRowAsync(Account row)
    {
        var resultValue = await GetRowIdAsync(row.UserName, row.Password);
        if (!resultValue.Success)
        {
            var returnValue = new ReturnValue();
            returnValue.AddMessage("database", "Account not found.");
            return returnValue;
        }
        return await UpdateRowAsync(resultValue.Data, row);
    }
    public ReturnValue UpdateRow(int id, Account row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        row.Id = id;
        var returnValue = new ReturnValue();
        try
        {
            var parameters = _params.AccountParamsWithId(row);
            var sql = AccountSQL.UpdateRowSQL;
            var result = Connection.Execute(sql, parameters, Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", "Account updated successfully.");
            }
            else
            {
                returnValue.AddMessage("database", "Failed to update account.");
            }
        }
        catch (Exception ex)
        {
            returnValue = AccountErrorsAndMessages.AccountModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, Account row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        row.Id = id;
        var returnValue = new ReturnValue();
        try
        {
            var parameters = _params.AccountParamsWithId(row);
            var sql = AccountSQL.UpdateRowSQL;
            var result = await Connection.ExecuteAsync(sql, parameters, Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
                returnValue.AddMessage("database", "Account updated successfully.");
            }
            else
            {
                returnValue.AddMessage("database", "Failed to update account.");
            }
        }
        catch (Exception ex)
        {
            returnValue = AccountErrorsAndMessages.AccountModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }

    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(Account row)
    {
        var resultValue = GetRowId(row.UserName, row.Password);
        if (!resultValue.Success)
        {
            var rv = new ReturnValue();
            rv.AddMessage("database", "Account not found.");
            return rv;
        }
        return DeleteRow(resultValue.Data);
    }
    public async Task<ReturnValue> DeleteRowAsync(Account row)
    {
        var resultValue = await GetRowIdAsync(row.UserName, row.Password);
        if (!resultValue.Success)
        {
            var rv = new ReturnValue();
            rv.AddMessage("database", "Account not found.");
            return rv;
        }
        return await DeleteRowAsync(resultValue.Data);
    }


    //****************************************************************************************************
    // Private Helpers
    //****************************************************************************************************
    private static Account SetCreationValues(Account row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        row.LastLogin = DateTime.UtcNow;
        row.IsActive = true;
        row.IsLocked = false;
        return row;
    }
    private ReturnValue<int> GetRowId(string username, string hashedPassword)
    {
        var returnValue = new ReturnValue<int>();
        try
        {
            var parameters = _params.UsernameAndPasswordParam(username, hashedPassword);
            var sql = AccountSQL.GetIdByUsernameAndPasword;
            var accountId = Connection.QuerySingleOrDefault<int>(sql, parameters, Transaction);
            if (accountId > 0)
            {
                returnValue.Success = true;
                returnValue.Data = accountId;
            }
            else
            {
                returnValue.AddMessage("database", "Invalid username or password.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    private async Task<ReturnValue<int>> GetRowIdAsync(string username, string hashedPassword)
    {
        var returnValue = new ReturnValue<int>();
        try
        {
            var parameters = _params.UsernameAndPasswordParam(username, hashedPassword);
            var sql = AccountSQL.GetIdByUsernameAndPasword;
            var accountId = await Connection.QuerySingleOrDefaultAsync<int>(sql, parameters, Transaction);
            if (accountId > 0)
            {
                returnValue.Success = true;
                returnValue.Data = accountId;
            }
            else
            {
                returnValue.AddMessage("database", "Invalid username or password.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
}
