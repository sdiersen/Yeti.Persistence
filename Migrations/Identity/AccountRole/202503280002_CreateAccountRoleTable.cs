
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Identity;
[Migration(202503280002)]
public class AccountRole : Migration
{
    public override void Up()
    {
        Create.Table($"{DbTableNames.ACCOUNT_ROLE_TABLE}")
            .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
            .WithColumn($"{DbAccountRoleTable.ACCOUNT_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ACCOUNT_TABLE, DbCommonColumns.ID)
            .WithColumn($"{DbAccountRoleTable.ROLE_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ROLE_TABLE, DbCommonColumns.ID)
            .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
            .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();

        Insert.IntoTable($"{DbTableNames.ACCOUNT_ROLE_TABLE}")
            .Row(new
            {
                AccountId = 1, // cippio
                RoleId = 1, // Admin
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                AccountId = 1, // cippio
                RoleId = 2, // User
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                AccountId = 2, // admin
                RoleId = 1, // Admin
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                AccountId = 2, // admin
                RoleId = 2, // User
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                AccountId = 2, // admin
                RoleId = 4, // SuperAdmin
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            });
    }

    public override void Down()
    {
        Delete.Table($"{DbTableNames.ACCOUNT_ROLE_TABLE}");
    }
}