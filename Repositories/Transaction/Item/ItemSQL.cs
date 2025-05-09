using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Transaction;
internal class ItemSQL
{
    internal const string InsertRowSQL = $@"
                            INSERT INTO {DbTableNames.ITEM_TABLE} 
                            (
                                {DbCommonColumns.CREATED_ON},
                                {DbCommonColumns.MODIFIED_ON},
                                {DbItemTable.NAME},
                                {DbItemTable.NOTE},
                                {DbItemTable.BUDGET_AMOUNT},
                                {DbItemTable.CURRENT_AMOUNT},
                                {DbItemTable.CATEGORY_ID},
                                {DbItemTable.IS_EXPENSE}

                            )
                            VALUES
                            (
                                @CreatedOn,
                                @ModifiedOn,
                                @Name,
                                @Note,
                                @BudgetAmount,
                                @CurrentAmount,
                                @CategoryId,
                                @IsExpense
                            );
                        ";
    internal const string InsertRowAndGetIdSQL = $@"
                            INSERT INTO {DbTableNames.ITEM_TABLE} 
                            (
                                {DbCommonColumns.CREATED_ON},
                                {DbCommonColumns.MODIFIED_ON},
                                {DbItemTable.NAME},
                                {DbItemTable.NOTE},
                                {DbItemTable.BUDGET_AMOUNT},
                                {DbItemTable.CURRENT_AMOUNT},
                                {DbItemTable.CATEGORY_ID},
                                {DbItemTable.IS_EXPENSE}
                            )
                            OUTPUT INSERTED.{DbCommonColumns.ID}
                            VALUES
                            (
                                @CreatedOn,
                                @ModifiedOn,
                                @Name,
                                @Note,
                                @BudgetAmount,
                                @CurrentAmount,
                                @CategoryId,
                                @IsExpense
                            );
                        ";
    internal const string UpdateRowSQL = $@"
                            UPDATE {DbTableNames.ITEM_TABLE}
                            SET
                                {DbCommonColumns.MODIFIED_ON} = @ModifiedOn,
                                {DbItemTable.NAME} = @Name,
                                {DbItemTable.NOTE} = @Note,
                                {DbItemTable.BUDGET_AMOUNT} = @BudgetAmount,
                                {DbItemTable.CURRENT_AMOUNT} = @CurrentAmount,
                                {DbItemTable.CATEGORY_ID} = @CategoryId,
                                {DbItemTable.IS_EXPENSE} = @IsExpense
                            WHERE
                                {DbCommonColumns.ID} = @Id;
                        ";
    internal const string GetAllItemsByCategoryIdSQL = $@"
                            SELECT 
                                *
                            FROM 
                                {DbTableNames.ITEM_TABLE}
                            WHERE 
                                {DbItemTable.CATEGORY_ID} = @CategoryId
                            ORDER BY 
                                {DbItemTable.NAME};
                        ";
    internal const string GetUnattachedIdSQL = $@"
                            SELECT 
                                {DbCommonColumns.ID}
                            FROM 
                                {DbTableNames.ITEM_TABLE}
                            WHERE 
                                {DbItemTable.NAME} = 'Unattached'
                        ";
    internal const string GetItemsByIdsSQL = $@"
                            SELECT 
                                *
                            FROM 
                                {DbTableNames.ITEM_TABLE}
                            WHERE 
                                {DbCommonColumns.ID} IN (@ItemIds)
                        ";
    internal const string IsValidIdSQL = $@"
                            SELECT CASE WHEN EXISTS (
                                SELECT 1
                                FROM {DbTableNames.ITEM_TABLE}
                                WHERE {DbCommonColumns.ID} = @Id
                            )
                            THEN 1
                            ELSE 0
                            END;
                        ";
}
