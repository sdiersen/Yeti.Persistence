
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Transaction;
[Migration(202503110003)]
public class ItemTable : Migration
{
    public override void Up()
    {
        Create.Table($"{DbTableNames.ITEM_TABLE}")
            .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
            .WithColumn($"{DbItemTable.NAME}").AsString().NotNullable()
            .WithColumn($"{DbItemTable.NOTE}").AsString().Nullable()
            .WithColumn($"{DbItemTable.IS_EXPENSE}").AsBoolean().NotNullable()
            .WithColumn($"{DbItemTable.BUDGET_AMOUNT}").AsDecimal().NotNullable()
            .WithColumn($"{DbItemTable.CURRENT_AMOUNT}").AsDecimal().NotNullable()
            .WithColumn($"{DbItemTable.CATEGORY_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.CATEGORY_TABLE, DbCommonColumns.ID)
            .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
            .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table($"{DbTableNames.ITEM_TABLE}");
    }
}