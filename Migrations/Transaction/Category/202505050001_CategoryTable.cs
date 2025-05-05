
using FluentMigrator;

using Persistence.Migrations.Constants;

namespace Persistence.Migrations.Transaction;
public class MakeCategoryNameUnique : Migration
{
    public override void Up()
    {
        Create.UniqueConstraint("UC_Category_Name")
            .OnTable($"{DbTableNames.CATEGORY_TABLE}")
            .Column($"{DbCategoryTable.NAME}");
    }
    public override void Down()
    {
        Delete.UniqueConstraint("UC_Category_Name")
            .FromTable($"{DbTableNames.CATEGORY_TABLE}");
    }
}