#pragma warning disable 1591 // Missing XML comment for publicly visible type or member
using Azure.Identity;

using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503270001)]
    public class AccountTable : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.ACCOUNT_TABLE}")
                .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbAccountTable.USERNAME}").AsString(50).NotNullable().Unique()
                .WithColumn($"{DbAccountTable.PASSWORD}").AsString(255).NotNullable()
                .WithColumn($"{DbAccountTable.LAST_LOGIN}").AsDateTime().Nullable()
                .WithColumn($"{DbAccountTable.IS_ACTIVE}").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn($"{DbAccountTable.IS_LOCKED}").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();

            Insert.IntoTable($"{DbTableNames.ACCOUNT_TABLE}")
                .Row(new
                {
                    Username = "cippio",
                    Password = "koffee71", // Replace with actual hashed password
                    LastLogin = DateTime.UtcNow,
                    IsActive = true,
                    IsLocked = false,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                })
                .Row(new
                {
                    Username = "admin",
                    Password = "admin123", // Replace with actual hashed password
                    LastLogin = DateTime.UtcNow,
                    IsActive = true,
                    IsLocked = false,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                });
        }


        public override void Down()
        {
            Delete.Table($"{DbTableNames.ACCOUNT_TABLE}");
        }
    }
}

#pragma warning restore 1591 // Missing XML comment for publicly visible type or member