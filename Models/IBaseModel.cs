namespace Persistence.Models
{
    public interface IBaseModel
    {
        int Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
    }
}