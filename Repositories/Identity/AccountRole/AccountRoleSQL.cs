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
}
