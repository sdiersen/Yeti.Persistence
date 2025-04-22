#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

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
                .WithColumn($"{DbUserDataTable.FIRST_NAME}").AsString(50).NotNullable()
                .WithColumn($"{DbUserDataTable.LAST_NAME}").AsString(50).NotNullable()
                .WithColumn($"{DbUserDataTable.DATE_OF_BIRTH}").AsDate().Nullable()
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
            Insert.IntoTable($"{DbTableNames.USER_DATA_TABLE}")
                .Row(new
                {
                    FirstName = "Steve",
                    LastName = "Diersen",
                    DateOfBirth = new DateTime(1971, 4, 21),
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                });
        }

        public override void Down()
        {
            Delete.Table($"{DbTableNames.USER_DATA_TABLE}");
        }
    }
}


#pragma warning restore 1591 // Missing XML comment for publicly visible type or member