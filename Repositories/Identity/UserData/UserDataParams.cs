
using Dapper;

using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;
internal class UserDataParams
{
    internal DynamicParameters FullUserDataParamsNoId(UserData userData)
    {
        var dp = new DynamicParameters();
        dp.Add("@FirstName", userData.FirstName);
        dp.Add("@LastName", userData.LastName);
        dp.Add("@DateOfBirth", userData.DateOfBirth);
        dp.Add("@CreatedOn", userData.CreatedOn);
        dp.Add("@ModifiedOn", userData.ModifiedOn);
        return dp;
    }

    internal DynamicParameters FullUserDataParams(UserData userData)
    {
        var dp = FullUserDataParamsNoId(userData);
        dp.Add("@Id", userData.Id);
        return dp;
    }

}
