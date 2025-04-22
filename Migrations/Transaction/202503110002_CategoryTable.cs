#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503110002)]
    public class CategoryTable : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.CATEGORY_TABLE}")
                .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbCategoryTable.NAME}").AsString().NotNullable()
                .WithColumn($"{DbCategoryTable.DESCRIPTION}").AsString().Nullable()
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
        }
        public override void Down()
        {
            // Delete the dependent CategoryItemTable first, if it exists
            Delete.Table($"{DbTableNames.CATEGORY_ITEM_TABLE}").IfExists();

            Delete.Table($"{DbTableNames.CATEGORY_TABLE}");
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
