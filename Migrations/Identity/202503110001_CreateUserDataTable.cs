
using FluentMigrator;
using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503110001)]
    public class UserDataTable : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.USER_DATA_TABLE}")
                .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbUserDataTable.USERNAME}").AsString().NotNullable()
                .WithColumn($"{DbUserDataTable.EMAIL}").AsString().NotNullable()
                .WithColumn($"{DbUserDataTable.PASSWORD}").AsString().NotNullable()
                .WithColumn($"{DbUserDataTable.LAST_LOGIN}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table($"{DbTableNames.USER_DATA_TABLE}");
        }
    }
}
