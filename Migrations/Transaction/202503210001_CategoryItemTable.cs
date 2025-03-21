#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Entry
{
    [Migration(202503210001)]
    public class CategoryItem : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.CATEGORY_ITEM_TABLE}")
                .WithColumn($"{DbCategoryItemTable.ITEM_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ITEM_TABLE, DbItemTable.ID)
                .WithColumn($"{DbCategoryItemTable.CATEGORY_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.CATEGORY_TABLE, DbCategoryTable.ID);
        }

        public override void Down()
        {
            Delete.Table($"{DbTableNames.CATEGORY_ITEM_TABLE}");
        }
    }
}

#pragma warning restore 1591 // Missing XML comment for publicly visible type or member