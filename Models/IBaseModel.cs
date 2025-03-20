namespace Persistence.Models
{
    /// <summary>
    /// This is the inteface for the base model for all models in the application.
    /// This interface cannot be instantiated.
    /// </summary>
    public interface IBaseModel
    {
        /// <summary>
        /// The unique identifier for the model.
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// The date and time the model was created.
        /// </summary>
        DateTime CreatedOn { get; set; }
        /// <summary>
        /// The date and time the model was last modified.
        /// </summary>
        DateTime ModifiedOn { get; set; }
    }
}