namespace Persistence.Models.Entry
{
    // EntryCategory is a category for an expense entry. 
    // Both prepopulated and user-created categories are stored in the Category table.
    public class EntryCategory : AbstractBaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public static EntryCategory DefaultCategory()
        {
            return new EntryCategory { Id = -1 };
        }
    }
}