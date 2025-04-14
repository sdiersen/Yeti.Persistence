using ErrorHandling;

using Persistence.Models.Identity;

namespace Persistence.ModelValidations.Identity;

/// <summary>
/// Validates the Account model.
/// </summary>
public class AccountValidation : IModelValidation<Account>
{
    /// <summary>
    /// Validates the Account model.
    /// This is used to validate the Account object before it is saved to the database.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public ReturnValue ValidateModel(Account model)
    {
        var returnValue = new ReturnValue();

        returnValue.AddMessageRangeToKey("username", ValidateUsername(model.Username));
        
        returnValue.AddMessageRangeToKey("password", ValidatePassword(model.Password));
    
        returnValue.AddMessageRangeToKey("lastLogin", ValidateLastLogin(model.LastLogin));

        // if there are no errors
        returnValue.Success = returnValue.Messages.Count == 0;
        return returnValue;
    }

    /// <summary>
    /// Validates the Account model asynchronously.
    /// This is used to validate the Account object before it is saved to the database.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<ReturnValue> ValidateModelAsync(Account model)
    {
        await Task.Yield();

        var returnValue = new ReturnValue();

        returnValue.AddMessageRangeToKey("username", ValidateUsername(model.Username));

        returnValue.AddMessageRangeToKey("password", ValidatePassword(model.Password));

        returnValue.AddMessageRangeToKey("lastLogin", ValidateLastLogin(model.LastLogin));

        returnValue.Success = returnValue.Messages.Count == 0;
        return returnValue;
    }

    private static List<string> ValidateUsername(string username)
    {
        var messages = new List<string>();
        if (string.IsNullOrWhiteSpace(username))
        {
            messages.Add("Username cannot be empty.");
        }
        else if (username.Length < 3 || username.Length > 20)
        {
            messages.Add("Username must be between 3 and 20 characters long.");
        }
        // this might be done here or wait for an insert and let the 
        // database say there is a problem
        // else if (UsernameNotUnique(username))
        // {
        //     messages.Add("Username already exists.");
        // }

        return messages;
    }

    private static List<string> ValidatePassword(string password)
    {
        var messages = new List<string>();
        if (string.IsNullOrWhiteSpace(password))
        {
            messages.Add("Password cannot be empty.");
        }
        else if (password.Length < 8 || password.Length > 20)
        {
            messages.Add("Password must be between 8 and 20 characters long.");
        }

        return messages;
    }
    private static List<string> ValidateLastLogin(DateTime lastLogin)
    {
        var messages = new List<string>();
        if (lastLogin > DateTime.Now)
        {
            messages.Add("Last login date cannot be in the future.");
        }

        return messages;
    }
}