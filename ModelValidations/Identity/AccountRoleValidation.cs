using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.Models.Identity;

namespace Persistence.ModelValidations.Identity
{
    /// <summary>
    /// Validates the AccountRole model.
    /// </summary>
    public class AccountRoleValidation : IModelValidation<AccountRole>
    {
        private readonly ILogger<AccountRoleValidation> _logger;

        public AccountRoleValidation(ILogger<AccountRoleValidation> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Validates the AccountRole model. Only tests that the Id's provided are in a valid format.
        /// Does not test the Id for the AccountRole object itself.
        /// </summary>
        /// <param name="model">AccountRole object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(AccountRole model)
        {
            ReturnValue returnValue = new ReturnValue();
            if (model.AccountId < 1)
            {
                returnValue.AddMessage("accountid", "AccountId must be greater than 0.");
            }
            if (model.RoleId < 1)
            {
                returnValue.AddMessage("roleid","RoleId must be greater than 0.");
            }
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }
        /// <summary>
        /// Validates the AccountRole model asynchronously. Only tests that the Id's provided are in a valid format.
        /// Does not test the Id for the AccountRole object itself.
        /// </summary>
        /// <param name="model">AccountRole object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(AccountRole model)
        {
            await Task.Yield();

            ReturnValue returnValue = new ReturnValue();
            if (model.AccountId < 1)
            {
                returnValue.AddMessage("accountid", "AccountId must be greater than 0.");
            }
            if (model.RoleId < 1)
            {
                returnValue.AddMessage("roleid", "RoleId must be greater than 0.");
            }
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }
    }
}