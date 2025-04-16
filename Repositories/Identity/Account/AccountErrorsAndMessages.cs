
using System.Data;

using ErrorHandling;

using Microsoft.Data.SqlClient;

namespace Persistence.Repositories.Identity;
internal class AccountErrorsAndMessages
{
    internal static ReturnValue AccountModifyExceptions(Exception ex, ReturnValue returnValue)
    {
        switch (ex)
        {
            case SqlException sqlException:
                if (sqlException.Number == 2627) // Unique constraint error number
                {
                    returnValue.AddMessage("database", "UserName already exists.");
                }
                else
                {
                    returnValue.AddMessage("database", "Database error: " + sqlException.Message);
                }
                break;
            case Exception exception:
                returnValue.AddMessage("database", "Error: " + exception.Message);
                break;
        }
        return returnValue;
    }
    internal static ReturnValue<int> AccountModifyExceptions(Exception ex, ReturnValue<int> returnValue)
    {
        switch (ex)
        {
            case SqlException sqlException:
                if (sqlException.Number == 2627) // Unique constraint error number
                {
                    returnValue.AddMessage("database", "UserName already exists.");
                }
                else
                {
                    returnValue.AddMessage("database", "Database error: " + sqlException.Message);
                }
                break;
            case Exception exception:
                returnValue.AddMessage("database", "Error: " + exception.Message);
                break;
        }
        return returnValue;
    }
}
