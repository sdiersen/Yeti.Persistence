using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.DTOs.Identity;
using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.Repositories;

namespace Persistence.Services.Identity;
public class AccountServices
{
    private readonly ILogger<AccountServices> _logger;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;

    public AccountServices(ILogger<AccountServices> logger, RepositoryFactory repositoryFactory, ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
    }

    public ReturnValue CreateAccount(RegisterDTO registerDTO)
    {
        //validate the DTO
        var validation = _modelValidationFactory.CreateAccountValidation();
        var account = new Account()
        {
            UserName = registerDTO.UserName,
            Password = registerDTO.Password,
            LastLogin = DateTime.UtcNow
        };
        var validationResult = validation.ValidateModel(account);
        if (!validationResult.Success)
        {
            return validationResult;
        }
        //Create the account and the role for the account in a transaction
        using (var unitOfWork = UnitOfWork.Create(true))
        {
            var returnValue = new ReturnValue();
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!); //Transaction is not null as Create was called with true
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!); //Transaction is not null as Create was called with true
            try
            {   
                var accountCreateValue = accountRepository.InsertRowAndGetId(account);
                if (!accountCreateValue.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.AddErrorRange(accountCreateValue.Errors);
                    returnValue.AddMessageRange(accountCreateValue.Messages);
                    return returnValue;
                }
                int id = accountCreateValue.Data;
                var accountRoleCreateValue = accountRoleRepository.InsertRow(new AccountRole()
                {
                    AccountId = id,
                    RoleId = 2 // 2 is the default for new accounts, which is the User role
                });
                if (!accountRoleCreateValue.Success)
                {
                    unitOfWork.Rollback();
                    return accountRoleCreateValue;
                }
                unitOfWork.Commit();
                returnValue.AddMessageRange(accountCreateValue.Messages);
                returnValue.AddMessageRange(accountRoleCreateValue.Messages);
                returnValue.AddErrorRange(accountCreateValue.Errors);
                returnValue.AddErrorRange(accountRoleCreateValue.Errors);
                returnValue.Success = true;
                return returnValue;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                returnValue.AddError("CreateAccount", "An error occurred while creating the account.");
                returnValue.AddError("CreateAccount", ex.Message);
                return returnValue;
            }
        }
    }
}
