namespace Persistence.Models
{
    public abstract class AbstractBaseModel : IBaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}