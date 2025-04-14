using ErrorHandling;

using Microsoft.Data.SqlClient;

namespace Persistence.Repositories.Identity;
internal class RoleErrorsAndMessages
{
    internal static ReturnValue RoleModifyExceptions(Exception ex, ReturnValue returnValue)
    {
        switch (ex)
        {
            case SqlException sqlException:
                if (sqlException.Number == 2627) // Unique constraint error number
                {
                    returnValue.AddMessage("database", "Role name already exists.");
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
