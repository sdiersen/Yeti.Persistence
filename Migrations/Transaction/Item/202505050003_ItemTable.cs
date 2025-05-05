
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Transaction;

[Migration(202505050003)]
public class MakeItemNameUnAttchedUnique : Migration
{
    public override void Up()
    {
        Execute.Sql(@$"
            CREATE UNIQUE INDEX IX_Item_Name_Unattached
            ON {DbTableNames.ITEM_TABLE} ({DbItemTable.NAME})
            WHERE {DbItemTable.NAME} = 'Unattached';");
    }
    public override void Down()
    {
        Execute.Sql(@$"
            DROP INDEX IX_Item_Name_Unattached
            ON {DbTableNames.ITEM_TABLE};");
    }
}
