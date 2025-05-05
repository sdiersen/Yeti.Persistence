
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Transaction;

[Migration(202503210001)]
public class CategoryItem : Migration
{
    public override void Up()
    {
        Create.Table($"{DbTableNames.CATEGORY_ITEM_TABLE}")
            .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
            .WithColumn($"{DbCategoryItemTable.ITEM_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ITEM_TABLE, DbCommonColumns.ID)
            .WithColumn($"{DbCategoryItemTable.CATEGORY_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.CATEGORY_TABLE, DbCommonColumns.ID)
            .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
            .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table($"{DbTableNames.CATEGORY_ITEM_TABLE}");
    }
}