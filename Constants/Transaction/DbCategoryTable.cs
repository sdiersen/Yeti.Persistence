namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Constant column names for the EntryCategory table.
    /// </summary>
    public class DbCategoryTable : DbCommonColumns
    {
        /// <summary>
        /// The name of a category that entries can belong to.
        /// </summary>
        public const string NAME = "Name";
        /// <summary>
        /// What is this category about?
        /// </summary>
        public const string DESCRIPTION = "Description";
    }
}