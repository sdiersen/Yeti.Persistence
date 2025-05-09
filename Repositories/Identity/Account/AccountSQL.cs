using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Identity;
internal class AccountSQL
{
    internal const string InsertRowSQL = $@"
                                INSERT INTO {DbTableNames.ACCOUNT_TABLE} 
                                (
                                    {DbAccountTable.USERNAME}, 
                                    {DbAccountTable.PASSWORD}, 
                                    {DbAccountTable.LAST_LOGIN},
                                    {DbAccountTable.IS_ACTIVE},
                                    {DbAccountTable.IS_LOCKED},
                                    {DbCommonColumns.CREATED_ON},
                                    {DbCommonColumns.MODIFIED_ON}
                                )
                                OUTPUT INSERTED.{DbCommonColumns.ID}
                                VALUES 
                                (
                                    @Username, 
                                    @Password, 
                                    @LastLogin,
                                    @IsActive,
                                    @IsLocked,
                                    @CreatedOn,
                                    @ModifiedOn
                                )
                            ;"
            ;
    internal const string InsertRowAndGetIdSQL = $@"
                                INSERT INTO {DbTableNames.ACCOUNT_TABLE} 
                                (
                                    {DbAccountTable.USERNAME}, 
                                    {DbAccountTable.PASSWORD}, 
                                    {DbAccountTable.LAST_LOGIN},
                                    {DbAccountTable.IS_ACTIVE},
                                    {DbAccountTable.IS_LOCKED},
                                    {DbCommonColumns.CREATED_ON},
                                    {DbCommonColumns.MODIFIED_ON}
                                )
                                OUTPUT INSERTED.{DbCommonColumns.ID}
                                VALUES 
                                (
                                    @Username, 
                                    @Password, 
                                    @LastLogin,
                                    @IsActive,
                                    @IsLocked,
                                    @CreatedOn,
                                    @ModifiedOn
                                )
                            ;"
        ;
    internal const string UpdateRowSQL = $@"
                                UPDATE {DbTableNames.ACCOUNT_TABLE} WITH (ROWLOCK)
                                SET 
                                    {DbAccountTable.USERNAME} = @Username, 
                                    {DbAccountTable.PASSWORD} = @Password, 
                                    {DbAccountTable.LAST_LOGIN} = @LastLogin,
                                    {DbAccountTable.IS_ACTIVE} = @IsActive,
                                    {DbAccountTable.IS_LOCKED} = @IsLocked,
                                    {DbCommonColumns.CREATED_ON} = @CreatedOn,
                                    {DbCommonColumns.MODIFIED_ON} = @ModifiedOn
                                WHERE {DbCommonColumns.ID} = @Id
                            ;"
        ;
    internal const string GetIdByUsernameAndPasword = $@"
                                SELECT {DbCommonColumns.ID} 
                                FROM {DbTableNames.ACCOUNT_TABLE}
                                WHERE {DbAccountTable.USERNAME} = @Username AND
                                {DbAccountTable.PASSWORD} = @Password
                            ;"
        ;
    internal const string GetAccountByUsernameAndPassword = $@"
                                SELECT * 
                                FROM {DbTableNames.ACCOUNT_TABLE}
                                WHERE {DbAccountTable.USERNAME} = @Username AND
                                {DbAccountTable.PASSWORD} = @Password
                            ;"
        ;
}
