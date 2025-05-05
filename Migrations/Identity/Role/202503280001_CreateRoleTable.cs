using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Identity;
[Migration(202503280001)]
public class RoleTable : Migration
{
    public override void Up()
    {
        Create.Table($"{DbTableNames.ROLE_TABLE}")
            .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
            .WithColumn($"{DbRoleTable.ROLE_NAME}").AsString(50).NotNullable().Unique()
            .WithColumn($"{DbRoleTable.DESCRIPTION}").AsString(255).NotNullable()
            .WithColumn($"{DbRoleTable.ROLE_NUMBER}").AsInt32().NotNullable().Unique()
            .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
            .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();

        Insert.IntoTable($"{DbTableNames.ROLE_TABLE}")
            .Row(new
            {
                RoleName = "Admin",
                Description = "Administrator role with full access",
                RoleNumber = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                RoleName = "User",
                Description = "Regular user role with limited access",
                RoleNumber = 2,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                RoleName = "Guest",
                Description = "Guest role with minimal access",
                RoleNumber = 3,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                RoleName = "SuperAdmin",
                Description = "Super administrator role with all access",
                RoleNumber = 4,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                RoleName = "Moderator",
                Description = "Moderator role with moderation access",
                RoleNumber = 5,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            })
            .Row(new
            {
                RoleName = "Creator",
                Description = "Creator role with content creation access",
                RoleNumber = 6,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            });
    }

    public override void Down()
    {
        Delete.Table($"{DbTableNames.ROLE_TABLE}");
    }
}