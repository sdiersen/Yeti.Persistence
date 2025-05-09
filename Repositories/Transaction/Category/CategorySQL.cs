using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Transaction;
internal class CategorySQL
{
    internal const string InsertRowSQL = @$"
            INSERT INTO {DbTableNames.CATEGORY_TABLE}
            (
                {DbCategoryTable.NAME},
                {DbCategoryTable.DESCRIPTION},
                {DbCommonColumns.CREATED_ON},
                {DbCommonColumns.MODIFIED_ON}    
            )
            VALUES
            (
                @Name,
                @Description,
                @CreatedOn,
                @ModifiedOn
            );
        ";
    internal const string InsertRowAndGetIdSQL = @$"
            INSERT INTO {DbTableNames.CATEGORY_TABLE}
            (
                {DbCategoryTable.NAME},
                {DbCategoryTable.DESCRIPTION},
                {DbCommonColumns.CREATED_ON},
                {DbCommonColumns.MODIFIED_ON}    
            )
            OUTPUT INSERTED.{DbCommonColumns.ID}
            VALUES
            (
                @Name,
                @Description,
                @CreatedOn,
                @ModifiedOn
            );
        ";

    internal const string UpdateRowSQL = @$"
            UPDATE {DbTableNames.CATEGORY_TABLE} WITH (ROWLOCK)
            SET
                {DbCategoryTable.NAME} = @Name,
                {DbCategoryTable.DESCRIPTION} = @Description,
                {DbCommonColumns.MODIFIED_ON} = @ModifiedOn
            WHERE
                {DbCommonColumns.ID} = @Id;
        ";

    internal const string GetUnattachedIdSQL = @$"
            SELECT {DbCommonColumns.ID}
            FROM {DbTableNames.CATEGORY_TABLE}
            WHERE {DbCategoryTable.NAME} = 'unattached';
        ";
    internal const string IsValidIdSQL = $@"
            SELECT CASE WHEN EXISTS (
                SELECT 1
                FROM {DbTableNames.CATEGORY_TABLE}
                WHERE {DbCommonColumns.ID} = @Id
                )
            THEN 1
            ELSE 0
            END;
        ";
}
