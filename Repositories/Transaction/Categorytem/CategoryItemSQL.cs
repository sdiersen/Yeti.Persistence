using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Transaction;
internal class CategoryItemSQL
{
    internal const string InsertRowSQL = @$"
            INSERT INTO {DbTableNames.CATEGORY_ITEM_TABLE}
            (
                {DbCategoryItemTable.CATEGORY_ID},
                {DbCategoryItemTable.ITEM_ID},
                {DbCommonColumns.CREATED_ON},
                {DbCommonColumns.MODIFIED_ON}
            )
            VALUES
            (
                @CategoryId,
                @ItemId,
                @CreatedOn,
                @ModifiedOn
            );
        ";
    internal const string InsertRowAndGetIdSQL = @$"
            INSERT INTO {DbTableNames.CATEGORY_ITEM_TABLE}
            (
                {DbCategoryItemTable.CATEGORY_ID},
                {DbCategoryItemTable.ITEM_ID},
                {DbCommonColumns.CREATED_ON},
                {DbCommonColumns.MODIFIED_ON}
            )
            OUTPUT INSERTED.{DbCommonColumns.ID}
            VALUES
            (
                @CategoryId,
                @ItemId,
                @CreatedOn,
                @ModifiedOn
            );
        ";

    internal const string UpdateRowSQL = @$"
            UPDATE {DbTableNames.CATEGORY_ITEM_TABLE}
            SET
                {DbCategoryItemTable.CATEGORY_ID} = @CategoryId,
                {DbCategoryItemTable.ITEM_ID} = @ItemId,
                {DbCommonColumns.MODIFIED_ON} = @ModifiedOn
            WHERE
                {DbCommonColumns.ID} = @Id
            ;
        ";
}
