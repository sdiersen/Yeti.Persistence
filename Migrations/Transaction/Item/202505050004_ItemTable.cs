
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Transaction;
[Migration(202505050004)]
public class AddUnAttachedItemTableRow : Migration
{
    public override void Up()
    {
        //Insert.IntoTable($"{DbTableNames.ITEM_TABLE}")
        //    .Row(new
        //    {
        //        Name = "Unattached",
        //        Note = "This is an item for entries that are not attached to any item. Typically used as a temporary holding area until a proper item is created.",
        //        BudgetAmount = 0.0m,
        //        CurrentAmount = 0.0m,
        //        CategoryId = -1, // TODO: Change this to the Id of the Unattached Category when it is created
        //        IsExpense = true,
        //        CreatedOn = DateTime.Now,
        //        ModifiedOn = DateTime.Now
        //    });
        Execute.Sql($@"
            INSERT INTO {DbTableNames.ITEM_TABLE}
            (
                {DbItemTable.NAME},
                {DbItemTable.NOTE},
                {DbItemTable.BUDGET_AMOUNT},
                {DbItemTable.CURRENT_AMOUNT},
                {DbItemTable.CATEGORY_ID},
                {DbItemTable.IS_EXPENSE},
                {DbCommonColumns.CREATED_ON},
                {DbCommonColumns.MODIFIED_ON}
            )
            VALUES
            (
                'Unattached',
                'This is an item for entries that are not attached to any item. Typically used as a temporary holding area until a proper item is created.',
                0.0,
                0.0,
                (SELECT {DbCommonColumns.ID} FROM {DbTableNames.CATEGORY_TABLE} WHERE {DbCategoryTable.NAME} = 'Unattached'),
                1, -- True for Expense
                GETDATE(),
                GETDATE()
            );
        ");
    }
    public override void Down()
    {
        Delete.FromTable($"{DbTableNames.ITEM_TABLE}")
            .Row(new
            {
                Name = "Unattached",
            });
    }
}
