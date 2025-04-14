using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;
public class UserDataRepository : BaseRepository<UserData, UserDataRepository>, IRepository<UserData>
{
    private readonly UserDataParams _params;
    public UserDataRepository(ILogger<UserDataRepository> logger, SqlConnection connection, SqlTransaction transaction) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.USER_DATA_TABLE;
        _params = new UserDataParams();
    }
    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    public ReturnValue<UserData> GetRow(UserData row)
    {
        return GetRow(row.Id);
    }
    public async Task<ReturnValue<UserData>> GetRowAsync(UserData row)
    {
        return await GetRowAsync(row.Id);
    }
    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    public ReturnValue InsertRow(UserData row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return InsertRowBase(UserDataSQL.InsertUserDataSQL, _params.FullUserDataParamsNoId(row));
    }
    public async Task<ReturnValue> InsertRowAsync(UserData row)
    {
        row.CreatedOn = DateTime.UtcNow;
        row.ModifiedOn = DateTime.UtcNow;
        return await InsertRowBaseAsync(UserDataSQL.InsertUserDataSQL, _params.FullUserDataParamsNoId(row));
    }
    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    public ReturnValue UpdateRow(UserData row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        return UpdateRowBase(UserDataSQL.UpdateUserDataSQL, _params.FullUserDataParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(UserData row)
    {
        row.ModifiedOn = DateTime.UtcNow;
        return await UpdateRowBaseAsync(UserDataSQL.UpdateUserDataSQL, _params.FullUserDataParams(row));
    }
    public ReturnValue UpdateRow(int id, UserData row)
    {
        row.Id = id;
        row.ModifiedOn = DateTime.UtcNow;
        return UpdateRowBase(UserDataSQL.UpdateUserDataSQL, _params.FullUserDataParams(row));
    }
    public async Task<ReturnValue> UpdateRowAsync(int id, UserData row)
    {
        row.Id = id;
        row.ModifiedOn = DateTime.UtcNow;
        return await UpdateRowBaseAsync(UserDataSQL.UpdateUserDataSQL, _params.FullUserDataParams(row));
    }
    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    public ReturnValue DeleteRow(UserData row)
    {
        return DeleteRow(row.Id);
    }
    public async Task<ReturnValue> DeleteRowAsync(UserData row)
    {
        return await DeleteRowAsync(row.Id);
    }
}
