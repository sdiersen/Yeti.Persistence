namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Constant column names for the CategoryItem table.
    /// CategoryItem is a one-to-many relationship between Category and Item
    /// </summary>
    public class DbCategoryItemTable : DbCommonColumns
    {
        /// <summary>
        /// This is a foreign key to the Item table.
        /// </summary>
        public const string ITEM_ID = "ItemId";
        /// <summary>
        /// This is a foreign key to the Category table.
        /// </summary>
        public const string CATEGORY_ID = "CategoryId";
    }
}