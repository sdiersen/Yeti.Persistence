#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503110002)]
    public class EntryCategoryTable : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.ENTRY_CATEGORY_TABLE}")
                .WithColumn($"{DbCategoryTable.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbCategoryTable.NAME}").AsString().NotNullable()
                .WithColumn($"{DbCategoryTable.DESCRIPTION}").AsString().Nullable()
                .WithColumn($"{DbCategoryTable.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCategoryTable.MODIFIED_ON}").AsDateTime().NotNullable();
        }
        public override void Down()
        {
            Delete.Table($"{DbTableNames.ENTRY_CATEGORY_TABLE}");
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
