#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503280002)]
    public class AccountRole : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.ACCOUNT_ROLE_TABLE}")
                .WithColumn($"{DbAccountRoleTable.ACCOUNT_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ACCOUNT_TABLE, DbAccountTable.ID)
                .WithColumn($"{DbAccountRoleTable.ROLE_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ROLE_TABLE, DbRoleTable.ID)
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table($"{DbTableNames.ACCOUNT_ROLE_TABLE}");
        }
    }
}