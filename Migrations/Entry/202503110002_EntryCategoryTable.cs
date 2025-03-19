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
                .WithColumn($"{DbEntryCategoryTable.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbEntryCategoryTable.NAME}").AsString().NotNullable()
                .WithColumn($"{DbEntryCategoryTable.DESCRIPTION}").AsString().Nullable()
                .WithColumn($"{DbEntryCategoryTable.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbEntryCategoryTable.MODIFIED_ON}").AsDateTime().NotNullable();
        }
        public override void Down()
        {
            Delete.Table($"{DbTableNames.ENTRY_CATEGORY_TABLE}");
        }
    }
}
