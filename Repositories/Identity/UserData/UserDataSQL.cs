using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Identity;
internal class UserDataSQL
{
    internal const string InsertUserDataSQL = $@"
                            INSERT INTO {DbTableNames.USER_DATA_TABLE} 
                            (
                                {DbUserDataTable.FIRST_NAME}, 
                                {DbUserDataTable.LAST_NAME}, 
                                {DbUserDataTable.DATE_OF_BIRTH},
                                {DbCommonColumns.MODIFIED_ON},
                                {DbCommonColumns.CREATED_ON}
                            ) 
                            VALUES 
                            (
                                @FirstName, 
                                @LastMame, 
                                @DateOfBirth,
                                @ModifiedOn,
                                @CreatedOn
                            )
                        ;"
    ;

    internal const string UpdateUserDataSQL = $@"
                            UPDATE {DbTableNames.USER_DATA_TABLE} 
                            SET                                 
                                {DbUserDataTable.FIRST_NAME} = @FirstName, 
                                {DbUserDataTable.LAST_NAME} = @LastName,
                                {DbUserDataTable.DATE_OF_BIRTH} = @DateOfBirth, 
                                {DbCommonColumns.CREATED_ON} = @CreatedOn,
                                {DbCommonColumns.MODIFIED_ON} = @ModifiedOn
                            WHERE {DbCommonColumns.ID} = @Id;
                        "
        ;
}
