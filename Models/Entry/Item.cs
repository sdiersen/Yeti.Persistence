namespace Persistence.Models.Entry
{
    public class Item : AbstractBaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0.0m;
    }
}