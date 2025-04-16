using Persistence.Migrations.Constants;

namespace Persistence.Repositories.Transaction;
internal class EntrySQL
{
    internal const string InsertRowSQL = @$"
                            INSERT INTO {DbTableNames.ENTRY_TABLE} 
                            (
                                {DbCommonColumns.CREATED_ON},
                                {DbCommonColumns.MODIFIED_ON},
                                {DbEntryTable.AMOUNT},
                                {DbEntryTable.IS_EXPENSE}, 
                                {DbEntryTable.ENTRY_DATE},
                                {DbEntryTable.CATEGORY_ID},
                                {DbEntryTable.ITEM_ID},
                                {DbEntryTable.NOTE}
                            )
                            VALUES 
                            (
                                @CreatedOn,
                                @ModifiedOn,
                                @Amount, 
                                @IsExpense,
                                @EntryDate,
                                @CategoryId,
                                @ItemId,
                                @Note
                            );
                        ";
    internal const string InsertRowAndGetIdSQL = @$"
                            INSERT INTO {DbTableNames.ENTRY_TABLE} 
                            (
                                {DbCommonColumns.CREATED_ON},
                                {DbCommonColumns.MODIFIED_ON},
                                {DbEntryTable.AMOUNT},
                                {DbEntryTable.IS_EXPENSE}, 
                                {DbEntryTable.ENTRY_DATE},
                                {DbEntryTable.CATEGORY_ID},
                                {DbEntryTable.ITEM_ID},
                                {DbEntryTable.NOTE}
                            )
                            OUTPUT INSERTED.{DbCommonColumns.ID}
                            VALUES 
                            (
                                @CreatedOn,
                                @ModifiedOn,
                                @Amount, 
                                @IsExpense,
                                @EntryDate,
                                @CategoryId,
                                @ItemId,
                                @Note
                            );
                        ";
    internal const string UpdateRowSQL = @$"
                            UPDATE {DbTableNames.ENTRY_TABLE} 
                            SET 
                                {DbCommonColumns.MODIFIED_ON} = @ModifiedOn,
                                {DbEntryTable.AMOUNT} = @Amount,
                                {DbEntryTable.IS_EXPENSE} = @IsExpense,
                                {DbEntryTable.ENTRY_DATE} = @EntryDate,
                                {DbEntryTable.CATEGORY_ID} = @CategoryId,
                                {DbEntryTable.ITEM_ID} = @ItemId,
                                {DbEntryTable.NOTE} = @Note
                            WHERE {DbCommonColumns.ID} = @Id
                            ;
                        ";
}
