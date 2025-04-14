using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Identity;
internal class RoleSQL
{
    //*****************************************************************************************************
    // Private string constant sql queries
    //*****************************************************************************************************
    internal const string InsertRoleSQL = $@"
                            INSERT INTO {DbTableNames.ROLE_TABLE} 
                            (
                                {DbRoleTable.ROLE_NAME}, 
                                {DbRoleTable.ROLE_NUMBER}, 
                                {DbRoleTable.DESCRIPTION}
                            )
                            VALUES 
                            (
                                @RoleName, 
                                @RoleNumber, 
                                @Description
                            )
                        ;"
                ;

    internal const string UpdateRoleSQL = $@"
                            UPDATE {DbTableNames.ROLE_TABLE} 
                            SET                                 
                                {DbRoleTable.ROLE_NAME} = @RoleName, 
                                {DbRoleTable.ROLE_NUMBER} = @RoleNumber,
                                {DbRoleTable.DESCRIPTION} = @Description
                            WHERE {DbCommonColumns.ID} = @Id
                        ;"
                ;

    internal const string GetRowByRoleNameSQL = $@"
                            SELECT *
                            FROM {DbTableNames.ROLE_TABLE} 
                            WHERE {DbRoleTable.ROLE_NAME} = @RoleName
                        ;"
                ;
}
