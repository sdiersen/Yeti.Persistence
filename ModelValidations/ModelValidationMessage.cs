using Persistence.Models;
using ErrorHandling;

namespace Persistence.ModelValidations
{
    /// <summary>
    /// Used to validate a model.
    /// ValidateModel should return a ReturnValue with Success = true if the model is valid.
    /// ValidateModel should return a ReturnValue with Success = false if the model is invalid.
    /// ValidateModel should return a ReturnValue with a list of errors if the model is invalid.
    /// It is up to the model to determine what is considered valid or invalid. For instance, UserData
    /// will require a username, email, and password. Each of these properties will be tested to see if
    /// meet the requirements of the model. A class like Category may require a name, that must fit 
    /// specific requirements, but a descritption could be optional. If descritption is present, then it
    /// must meet specific requirements. The lack of a description should not cause the model to be invalid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModelValidation<T> where T : IBaseModel
    {
        ReturnValue ValidateModel(T model);
        Task<ReturnValue> ValidateModelAsync(T model);
        // Not sold on the following methods. Should a validator be used only to make sure the data is ready for the database?
        // ReturnValue ValidateModelFields(Dictionary<string, object> fields);
        // Task<ReturnValue> ValidateModelFieldsAsync(Dictionary<string, object> fields);
    }

    // TODO make sure that this implementation is flexible enough. Should there be an interface or method
    // for the above interface that allows for validation of a single property or a list of properties?
    // This is probably something that needs to be done. In that vein, somewhere I should have constants 
    // that represent each field in a Model. These constants should be the exact spelling of the table column
    // name in the database. This would also allow for passing in a string of property names for validation.
}