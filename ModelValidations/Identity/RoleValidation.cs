using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.Models.Identity;

namespace Persistence.ModelValidations.Identity
{
    /// <summary>
    /// Validates the Role model.
    /// </summary>
    public class RoleValidation : IModelValidation<Role>
    {
        private readonly ILogger<RoleValidation> _logger;

        public RoleValidation(ILogger<RoleValidation> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Validates the Role model.
        /// </summary>
        /// <param name="model">the Role object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(Role model)
        {
            ReturnValue returnValue = new ReturnValue();

            returnValue.AddMessageRangeToKey("rolename", ValidateRoleName(model.RoleName));
            returnValue.AddMessageRangeToKey("description", ValidateDescription(model.Description));
            returnValue.AddMessageRangeToKey("rolenumber", ValidateRoleNumber(model.RoleNumber));

            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        /// <summary>
        /// Validates the Role model asynchronously.
        /// </summary>
        /// <param name="model">the Role object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(Role model)
        {
            await Task.Yield();

            ReturnValue returnValue = new ReturnValue();

            returnValue.AddMessageRangeToKey("rolename", ValidateRoleName(model.RoleName));
            returnValue.AddMessageRangeToKey("description", ValidateDescription(model.Description));
            returnValue.AddMessageRangeToKey("rolenumber", ValidateRoleNumber(model.RoleNumber));

            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        private static List<string> ValidateRoleName(string roleName)
        {
            List<string> messages = new List<string>();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                messages.Add("Role name cannot be null or empty.");
            }
            else if (roleName.Length > 50)
            {
                messages.Add("Role name must be less than or equal to 50 characters.");
            }
            return messages;
        }

        private static List<string> ValidateDescription(string description)
        {
            List<string> messages = new List<string>();
            if (string.IsNullOrWhiteSpace(description))
            {
                messages.Add("Description cannot be null or empty.");
            }
            else if (description.Length > 255)
            {
                messages.Add("Description must be less than or equal to 255 characters.");
            }
            return messages;
        }

        private static List<string> ValidateRoleNumber(int roleNumber)
        {
            List<string> messages = new List<string>();
            if (roleNumber < 0)
            {
                messages.Add("Role number must be greater than or equal to 0.");
            }
            return messages;
        }
    }
}