
using Dapper;

using ErrorHandling;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Migrations.Constants;
using Persistence.Models.Identity;

namespace Persistence.Repositories.Identity;

public class RoleRepository : BaseRepository<Role, RoleRepository>, IRepository<Role>
{
    private readonly RoleParams _params;
    /// <summary>
    /// A service class for the Role entity. These database services are not Transactions. To use Transaction
    /// </summary>
    public RoleRepository(ILogger<RoleRepository> logger, SqlConnection connection, SqlTransaction? transaction = null) : base(logger, connection, transaction)
    {
        TableName = DbTableNames.ROLE_TABLE;
        _params = new RoleParams();
    }

    //****************************************************************************************************
    // GetRow
    //****************************************************************************************************
    /// <summary>
    /// Get a single row for the Role entity based on the RoleName that is passed in the row parameter.
    /// </summary>
    /// <param name="row">A Role object that contains the Rolename of the Role to find in the database.</param>
    /// <returns>
    /// A ReturnValue object that contains the Role object with the RoleName that was passed in the row parameter in the Data property.
    /// Success is false if not found or there are exceptions. Messages property will contain a message if not found.
    /// </returns>
    public ReturnValue<Role> GetRow(Role row)
    {
        var returnValue = new ReturnValue<Role>();
        try
        {
            var result = Connection.QuerySingleOrDefault<Role>(RoleSQL.GetRowByRoleNameSQL, _params.RoleNameParams(row.RoleName), Transaction);
            if (result != null)
            {
                returnValue.Data = result;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Role with name {row.RoleName} not found.");
            }
        }
        catch (SqlException ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    /// <summary>
    /// Asynchronously get a single row for the Role entity based on the RoleName that is passed in the row parameter.
    /// </summary>
    /// <param name="row">A Role object that contains the Rolename of the Role to find in the database.</param>
    /// <returns>
    /// A ReturnValue object that contains the Role object with the RoleName that was passed in the row parameter in the Data property.
    /// Success is false if not found or there are exceptions. Messages property will contain a message if not found.
    /// </returns>
    public async Task<ReturnValue<Role>> GetRowAsync(Role row)
    {
        var returnValue = new ReturnValue<Role>();
        try
        {
            var result = await Connection.QuerySingleOrDefaultAsync<Role>(RoleSQL.GetRowByRoleNameSQL, _params.RoleNameParams(row.RoleName), Transaction);
            if (result != null)
            {
                returnValue.Data = result;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Role with name {row.RoleName} not found.");
            }
        }
        catch (SqlException ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }

    //*****************************************************************************************************
    // InsertRow
    //*****************************************************************************************************
    /// <summary>
    /// Adds a new role to the database.
    /// The RoleName must be unique in the database.
    /// </summary>
    /// <param name="row">The Role entity containing the new role to add to the database.</param>
    /// <returns>
    /// A ReturnValue object. 
    /// Success is true if the role was added to the database.
    /// Success is false if the role was not added to the database. There will be messages in the Messages and/or Errors properties.
    /// </returns>
    public ReturnValue InsertRow(Role row)
    {
        var returnValue = new ReturnValue();
        row.CreatedOn = DateTime.Now;
        row.ModifiedOn = DateTime.Now;
        try
        {
            var result = Connection.Execute(RoleSQL.InsertRoleSQL, _params.FullRoleParamsNoId(row), Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Failed to insert role {row.RoleName}.");
            }
        }
        catch (Exception ex)
        {
            returnValue = RoleErrorsAndMessages.RoleModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    /// <summary>
    /// Adds a new role to the database asynchronously.
    /// The RoleName must be unique in the database.
    /// </summary>
    /// <param name="row">The Role entity containing the new role to add to the database.</param>
    /// <returns>
    /// A ReturnValue object. 
    /// Success is true if the role was added to the database.
    /// Success is false if the role was not added to the database. There will be messages in the Messages and/or Errors properties.
    /// </returns>
    public async Task<ReturnValue> InsertRowAsync(Role row)
    {
        var returnValue = new ReturnValue();
        row.CreatedOn = DateTime.Now;
        row.ModifiedOn = DateTime.Now;
        try
        {
            var result = await Connection.ExecuteAsync(RoleSQL.InsertRoleSQL, _params.FullRoleParamsNoId(row), Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Failed to insert role {row.RoleName}.");
            }
        }
        catch (Exception ex)
        {
            returnValue = RoleErrorsAndMessages.RoleModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    public ReturnValue<int> InsertRowAndGetId(Role row)
    {
        var returnValue = new ReturnValue<int>();
        row.CreatedOn = DateTime.Now;
        row.ModifiedOn = DateTime.Now;
        try
        {
            var result = Connection.ExecuteScalar<int>(RoleSQL.InsertRoleAndGetIdSQL, _params.FullRoleParamsNoId(row), Transaction);
            if (result > 0)
            {
                returnValue.Data = result;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Failed to insert role {row.RoleName}.");
            }
        }
        catch (Exception ex)
        {
            returnValue = RoleErrorsAndMessages.RoleModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    public async Task<ReturnValue<int>> InsertRowAndGetIdAsync(Role row)
    {
        var returnValue = new ReturnValue<int>();
        row.CreatedOn = DateTime.Now;
        row.ModifiedOn = DateTime.Now;
        try
        {
            var result = await Connection.ExecuteScalarAsync<int>(RoleSQL.InsertRoleAndGetIdSQL, _params.FullRoleParamsNoId(row), Transaction);
            if (result > 0)
            {
                returnValue.Data = result;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Failed to insert role {row.RoleName}.");
            }
        }
        catch (Exception ex)
        {
            returnValue = RoleErrorsAndMessages.RoleModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }

    //*****************************************************************************************************
    // UpdateRow
    //*****************************************************************************************************
    /// <summary>
    /// Update a row for the Role entity based id passed in. This is the method for changing the RoleName.
    /// Remember, the RoleName must be unique in the database.
    /// </summary>
    /// <param name="id">the id of the row in the database to update</param>
    /// <param name="row">the Role instance that will be used to update the databse</param>
    /// <returns>
    /// A ReturnValue object.
    /// If Success is true, the row was updated.
    /// If Success is false, the row was not updated and there will be messages in the Messages and/or Errors properties.
    /// </returns>
    public ReturnValue UpdateRow(int id, Role row)
    {
        var returnValue = new ReturnValue();
        row.ModifiedOn = DateTime.Now;
        row.Id = id;

        try
        {
            var result = Connection.Execute(RoleSQL.UpdateRoleSQL, _params.FullRoleParams(row), Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Failed to update role {row.RoleName}.");
            }
        }
        catch (Exception ex)
        {
            returnValue = RoleErrorsAndMessages.RoleModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    /// <summary>
    /// Asynchronously update a row for the Role entity based id passed in. This is the method for changing the RoleName.
    /// Remember, the RoleName must be unique in the database.
    /// </summary>
    /// <param name="id">the id of the row in the database to update</param>
    /// <param name="row">the Role instance that will be used to update the databse</param>
    /// <returns>
    /// A ReturnValue object.
    /// If Success is true, the row was updated.
    /// If Success is false, the row was not updated and there will be messages in the Messages and/or Errors properties.
    /// </returns>
    public async Task<ReturnValue> UpdateRowAsync(int id, Role row)
    {
        var returnValue = new ReturnValue();
        row.ModifiedOn = DateTime.Now;
        row.Id = id;

        try
        {
            var result = await Connection.ExecuteAsync(RoleSQL.UpdateRoleSQL, _params.FullRoleParams(row), Transaction);
            if (result > 0)
            {
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", $"Failed to update role {row.RoleName}.");
            }
        }
        catch (SqlException ex)
        {
            RoleErrorsAndMessages.RoleModifyExceptions(ex, returnValue);
        }
        return returnValue;
    }
    /// <summary>
    /// Update a row for the Role entity based RoleName passed in through the row parameter.
    /// Remember, the RoleName must be unique in the database.
    /// </summary>
    /// <param name="row">the Role instance that will be used to update the databse</param>
    /// <returns>
    /// A ReturnValue object.
    /// If Success is true, the row was updated.
    /// If Success is false, the row was not updated and there will be messages in the Messages and/or Errors properties.
    /// </returns>
    public ReturnValue UpdateRow(Role row)
    {
        var getRowResult = GetRow(row);
        if (!getRowResult.Success)
        {
            return new ReturnValue
            {
                Success = false,
                Messages = getRowResult.Messages,
                Errors = getRowResult.Errors

            };
        }
        row.Id = getRowResult.Data!.Id; // if Success = true then Data is not null, hence Data! is safe to use
        return UpdateRow(row.Id, row);
    }
    /// <summary>
    /// Asynchronously update a row for the Role entity based RoleName passed in through the row parameter.
    /// Remember, the RoleName must be unique in the database.
    /// </summary>
    /// <param name="row">the Role instance that will be used to update the databse</param>
    /// <returns>
    /// A ReturnValue object.
    /// If Success is true, the row was updated.
    /// If Success is false, the row was not updated and there will be messages in the Messages and/or Errors properties.
    /// </returns>
    public async Task<ReturnValue> UpdateRowAsync(Role row)
    {
        var getRowResult = await GetRowAsync(row);
        if (!getRowResult.Success)
        {
            return new ReturnValue
            {
                Success = false,
                Messages = getRowResult.Messages,
                Errors = getRowResult.Errors

            };
        }
        row.Id = getRowResult.Data!.Id; // if Success = true then Data is not null, hence Data! is safe to use
        return await UpdateRowAsync(row.Id, row);
    }

    //*****************************************************************************************************
    // DeleteRow
    //*****************************************************************************************************
    /// <summary>
    /// Delete a row in the Role table based on the RoleName passed in through the row parameter.
    /// </summary>
    /// <param name="row">A Role entity that has RoleName set to the name of the role to be removed from the database.</param>
    /// <returns></returns>
    public ReturnValue DeleteRow(Role row)
    {
        var getRowResult = GetRow(row);
        if (!getRowResult.Success)
        {
            return new ReturnValue
            {
                Success = false,
                Messages = getRowResult.Messages,
                Errors = getRowResult.Errors
            };
        }
        row.Id = getRowResult.Data!.Id; // if Success = true then Data is not null, hence Data! is safe to use
        return DeleteRow(row.Id);
    }
    /// <summary>
    /// Asynchronously delete a row in the Role table based on the RoleName passed in through the row parameter.
    /// </summary>
    /// <param name="row">A Role entity that has RoleName set to the name of the role to be removed from the database.</param>
    /// <returns></returns>
    public async Task<ReturnValue> DeleteRowAsync(Role row)
    {
        var getRowResult = await GetRowAsync(row);
        if (!getRowResult.Success)
        {
            return new ReturnValue
            {
                Success = false,
                Messages = getRowResult.Messages,
                Errors = getRowResult.Errors
            };
        }
        row.Id = getRowResult.Data!.Id; // if Success = true then Data is not null, hence Data! is safe to use
        return await DeleteRowAsync(row.Id);
    }

    //******************************************************************************************************
    // Role specific methods
    //******************************************************************************************************

    //check a list of RoleNumbers to see if they exist in the database
    public ReturnValue CheckRolesExist(List<int> roleIds)
    {
        var returnValue = new ReturnValue();
        if (roleIds == null || roleIds.Count == 0)
        {
            returnValue.AddMessage("database", "No roles provided to check for existence.");
            return returnValue;
        }
        try
        {
            var rolesExist = Connection.ExecuteScalar<int>(RoleSQL.AllRolesExistSQL, new { RoleIds = roleIds, RoleCount = roleIds.Count }, Transaction);
            if (rolesExist > 0)
            {
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", "Not all roles exist in the database.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public async Task<ReturnValue> CheckRolesExistAsync(List<int> roleIds)
    {
        var returnValue = new ReturnValue();
        if (roleIds == null || roleIds.Count == 0)
        {
            returnValue.AddMessage("database", "No roles provided to check for existence.");
            return returnValue;
        }
        try
        {
            var rolesExist = await Connection.ExecuteScalarAsync<int>(RoleSQL.AllRolesExistSQL, new { RoleIds = roleIds, RoleCount = roleIds.Count }, Transaction);
            if (rolesExist > 0)
            {
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", "Not all roles exist in the database.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }

    //get role names for a list of role ids
    public ReturnValue<List<string>> GetRoleNamesForRoleIds(List<int> roleIds)
    {
        var returnValue = new ReturnValue<List<string>>();
        if (roleIds == null || roleIds.Count == 0)
        {
            returnValue.AddMessage("database", "No roles provided to get names for.");
            return returnValue;
        }
        try
        {
            var roleNames = Connection.Query<string>(RoleSQL.GetRoleNamesForRoleIdsSQL, new { RoleIds = roleIds }, Transaction).ToList();
            if (roleNames.Count > 0)
            {
                returnValue.Data = roleNames;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", "No roles found for the provided ids.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
    public async Task<ReturnValue<List<string>>> GetRoleNamesForRoleIdsAsync(List<int> roleIds)
    {
        var returnValue = new ReturnValue<List<string>>();
        if (roleIds == null || roleIds.Count == 0)
        {
            returnValue.AddMessage("database", "No roles provided to get names for.");
            return returnValue;
        }
        try
        {
            var roleNames = (await Connection.QueryAsync<string>(RoleSQL.GetRoleNamesForRoleIdsSQL, new { RoleIds = roleIds }, Transaction)).ToList();
            if (roleNames.Count > 0)
            {
                returnValue.Data = roleNames;
                returnValue.Success = true;
            }
            else
            {
                returnValue.AddMessage("database", "No roles found for the provided ids.");
            }
        }
        catch (Exception ex)
        {
            returnValue.AddError("database", ex.Message);
        }
        return returnValue;
    }
}
