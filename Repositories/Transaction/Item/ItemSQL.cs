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
}
