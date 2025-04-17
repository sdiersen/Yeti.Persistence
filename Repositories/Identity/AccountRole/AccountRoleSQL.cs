using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Identity;
internal class AccountRoleSQL
{
    //****************************************************************************
    // Private string constant sql queries
    //****************************************************************************
    internal const string InsertRowSQL = $@"
                                    INSERT INTO {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    (
                                        {DbAccountRoleTable.ACCOUNT_ID}, 
                                        {DbAccountRoleTable.ROLE_ID}, 
                                        {DbCommonColumns.CREATED_ON},
                                        {DbCommonColumns.MODIFIED_ON} 
                                    )
                                    VALUES 
                                    (
                                        @AccountId, 
                                        @RoleId, 
                                        @CreatedOn, 
                                        @ModifiedOn
                                    )
                                ;"
                        ;
    internal const string InsertRowAndGetIdSQL = $@"
                                    INSERT INTO {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    (
                                        {DbAccountRoleTable.ACCOUNT_ID}, 
                                        {DbAccountRoleTable.ROLE_ID}, 
                                        {DbCommonColumns.CREATED_ON},
                                        {DbCommonColumns.MODIFIED_ON} 
                                    )
                                    OUTPUT INSERTED.{DbCommonColumns.ID}
                                    VALUES 
                                    (
                                        @AccountId, 
                                        @RoleId, 
                                        @CreatedOn, 
                                        @ModifiedOn
                                    )
                                ;"
                        ;
    internal const string UpdateRowSQL = $@"
                                    UPDATE {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    SET 
                                        {DbAccountRoleTable.ACCOUNT_ID} = @AccountId, 
                                        {DbAccountRoleTable.ROLE_ID} = @RoleId, 
                                        {DbCommonColumns.CREATED_ON} = @CreatedOn, 
                                        {DbCommonColumns.MODIFIED_ON} = @ModifiedOn
                                    WHERE {DbCommonColumns.ID} = @Id
                                ;"
                        ;
    internal const string GetIdFromRoleAndAccountIdSQL = $@"
                                    SELECT {DbCommonColumns.ID}
                                    FROM {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    WHERE {DbAccountRoleTable.ACCOUNT_ID} = @AccountId 
                                    AND {DbAccountRoleTable.ROLE_ID} = @RoleId
                                ;"
                        ;
    internal const string GetRolesForAccountIdSQL = $@"
                                    SELECT {DbCommonColumns.ID}, {DbAccountRoleTable.ACCOUNT_ID}, {DbAccountRoleTable.ROLE_ID}, {DbCommonColumns.CREATED_ON}, {DbCommonColumns.MODIFIED_ON}
                                    FROM {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    WHERE {DbAccountRoleTable.ACCOUNT_ID} = @AccountId
                                ;"
                        ;
    internal const string GetRoleIdsForAccountIdSQL = $@"
                                    SELECT {DbAccountRoleTable.ROLE_ID}
                                    FROM {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    WHERE {DbAccountRoleTable.ACCOUNT_ID} = @AccountId
                                ;"
                        ;
    internal const string GetAccountRolesForRoleIdSQL = $@"
                                    SELECT {DbCommonColumns.ID}, {DbAccountRoleTable.ACCOUNT_ID}, {DbAccountRoleTable.ROLE_ID}, {DbCommonColumns.CREATED_ON}, {DbCommonColumns.MODIFIED_ON}
                                    FROM {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    WHERE {DbAccountRoleTable.ROLE_ID} = @RoleId
                                ;"
                        ;
    internal const string GetAccountIdsForRoleIdSQL = $@"
                                    SELECT {DbAccountRoleTable.ACCOUNT_ID}
                                    FROM {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    WHERE {DbAccountRoleTable.ROLE_ID} = @RoleId
                                ;"
                        ;
    internal const string DeleteRolesForAccountIdSQL = $@"
                                    DELETE FROM {DbTableNames.ACCOUNT_ROLE_TABLE} 
                                    WHERE {DbAccountRoleTable.ACCOUNT_ID} = @AccountId
                                ;"
                        ;
}
