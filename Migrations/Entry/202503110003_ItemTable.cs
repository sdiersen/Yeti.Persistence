using FluentMigrator;
using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503110003)]
    public class ItemTable : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.ITEM_TABLE}")
                .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbItemTable.NAME}").AsString().NotNullable()
                .WithColumn($"{DbItemTable.DESCRIPTION}").AsString().Nullable()
                .WithColumn($"{DbItemTable.AMOUNT}").AsDecimal().NotNullable()
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table($"{DbTableNames.ITEM_TABLE}");
        }
    }
}
