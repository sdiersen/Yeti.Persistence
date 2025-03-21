#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations
{
    [Migration(202503200001)]
    public class EntryTable : Migration
    {
        public override void Up()
        {
            Create.Table($"{DbTableNames.ENTRY_TABLE}")
                .WithColumn($"{DbCommonColumns.ID}").AsInt32().PrimaryKey().Identity()
                .WithColumn($"{DbEntryTable.ENTRY_DATE}").AsDateTime().NotNullable()
                .WithColumn($"{DbEntryTable.AMOUNT}").AsDecimal().NotNullable()
                .WithColumn($"{DbEntryTable.IS_EXPENSE}").AsBoolean().NotNullable()
                .WithColumn($"{DbEntryTable.NOTE}").AsString().Nullable()
                .WithColumn($"{DbEntryTable.ITEM_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.ITEM_TABLE, DbItemTable.ID)
                .WithColumn($"{DbEntryTable.CATEGORY_ID}").AsInt32().NotNullable().ForeignKey(DbTableNames.CATEGORY_TABLE, DbCategoryTable.ID)
                .WithColumn($"{DbCommonColumns.CREATED_ON}").AsDateTime().NotNullable()
                .WithColumn($"{DbCommonColumns.MODIFIED_ON}").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table($"{DbTableNames.ENTRY_TABLE}");
        }
    }
}

#pragma warning restore 1591 // Missing XML comment for publicly visible type or member