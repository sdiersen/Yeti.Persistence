namespace Persistence.Models
{
    /// <summary>
    /// Represents a base model for all models in the application.
    /// This class is abstract and cannot be instantiated.
    /// </summary>
    public abstract class AbstractBaseModel : IBaseModel
    {
        /// <summary>
        /// The unique identifier for the model.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The date and time the model was created.
        /// The default value is the current date and time.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        /// <summary>
        /// The date and time the model was last modified.
        /// The default value is the current date and time.
        /// </summary>
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}