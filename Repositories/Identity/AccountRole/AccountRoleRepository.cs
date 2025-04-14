
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
        return DeleteRow(row.Id);
    }
    public async Task<ReturnValue> DeleteRowAsync(AccountRole row)
    {
        return await DeleteRowAsync(row.Id);
    }


}
