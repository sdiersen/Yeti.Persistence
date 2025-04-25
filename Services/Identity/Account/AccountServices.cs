using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.DTOs.Identity;
using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Identity;
using Persistence.Repositories;
using Persistence.Repositories.Identity;

namespace Persistence.Services.Identity;
public class AccountServices : IAccountServices
{
    private readonly ILogger<AccountServices> _logger;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;
    private readonly AccountValidation _accountValidation;

    public AccountServices(ILogger<AccountServices> logger, RepositoryFactory repositoryFactory, ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
        _accountValidation = _modelValidationFactory.CreateAccountValidation();
    }

    public ReturnValue CreateAccount(RegisterDTO registerDTO)
    {
        //validate the DTO
        var account = new Account()
        {
            Username = registerDTO.UserName,
            Password = registerDTO.Password,
            LastLogin = DateTime.UtcNow
        };
        var validationResult = _accountValidation.ValidateModel(account);
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
                    returnValue.Consume(accountCreateValue);
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
                returnValue.Consume(accountCreateValue);
                returnValue.Consume(accountRoleCreateValue);
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
    public async Task<ReturnValue> CreateAccountAsync(RegisterDTO registerDTO)
    {
        //validate the DTO
        var validation = _modelValidationFactory.CreateAccountValidation();
        var account = new Account()
        {
            Username = registerDTO.UserName,
            Password = registerDTO.Password,
            LastLogin = DateTime.UtcNow
        };
        var validationResult = validation.ValidateModel(account);
        if (!validationResult.Success)
        {
            return validationResult;
        }
        //Create the account and the role for the account in a transaction
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
        {
            var returnValue = new ReturnValue();
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!); //Transaction is not null as Create was called with true
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!); //Transaction is not null as Create was called with true
            try
            {
                var accountCreateValue = await accountRepository.InsertRowAndGetIdAsync(account);
                if (!accountCreateValue.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.AddErrorRange(accountCreateValue.Errors);
                    returnValue.AddMessageRange(accountCreateValue.Messages);
                    return returnValue;
                }
                int id = accountCreateValue.Data;
                var accountRoleCreateValue = await accountRoleRepository.InsertRowAsync(new AccountRole()
                {
                    AccountId = id,
                    RoleId = 2 // 2 is the default for new accountsResult, which is the User role
                });
                if (!accountRoleCreateValue.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return accountRoleCreateValue;
                }
                await unitOfWork.CommitAsync();
                returnValue.AddMessageRange(accountCreateValue.Messages);
                returnValue.AddMessageRange(accountRoleCreateValue.Messages);
                returnValue.AddErrorRange(accountCreateValue.Errors);
                returnValue.AddErrorRange(accountRoleCreateValue.Errors);
                returnValue.Success = true;
                return returnValue;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                returnValue.AddError("CreateAccount", "An error occurred while creating the account.");
                returnValue.AddError("CreateAccount", ex.Message);
                return returnValue;
            }
        }
    }

    public ReturnValue<AccountLoggedInDTO> Login(LoginDTO loginDTO)
    {
        var account = new Account()
        {
            Username = loginDTO.UserName,
            Password = loginDTO.Password
        };
        //Login the account
        using (var unitOfWork = new UnitOfWork(true))
        {
            var returnValue = new ReturnValue<AccountLoggedInDTO>();
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var roleRepository = _repositoryFactory.CreateRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            try
            {
                var accountLoginValue = accountRepository.GetRow(account);
                returnValue.Consume(accountLoginValue);
                if (!accountLoginValue.Success)
                {
                    unitOfWork.Rollback();
                    return returnValue;
                }
                //Update the last login time
                account = accountLoginValue.Data!;
                account.LastLogin = DateTime.UtcNow;
                
                var accountUpdateValue = accountRepository.UpdateRow(account);
                returnValue.Consume(accountUpdateValue);
                if (!accountUpdateValue.Success)
                {
                    unitOfWork.Rollback();
                    return returnValue;
                }

                var accountRoleValue = accountRoleRepository.GetRoleIdsForAccountId(account.Id);
                returnValue.Consume(accountRoleValue);
                if (!accountRoleValue.Success)
                {
                    unitOfWork.Rollback();
                    return returnValue;
                }

                var roleIds = accountRoleValue.Data ?? [];
                var roleNamesValue = roleRepository.GetRoleNamesForRoleIds(roleIds);
                returnValue.Consume(roleNamesValue);
                if (!roleNamesValue.Success)
                {
                    unitOfWork.Rollback();
                    return returnValue;
                }

                var accountLoggedInDTO = new AccountLoggedInDTO
                {
                    Account = account,
                    RoleNames = roleNamesValue.Data ?? []
                };
                returnValue.Data = accountLoggedInDTO;
                returnValue.Success = true;

                unitOfWork.Commit();
                return returnValue;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                returnValue.AddError("Login", "An error occurred while logging in.");
                returnValue.AddError("Login", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue<AccountLoggedInDTO>> LoginAsync(LoginDTO loginDTO)
    {
        var account = new Account()
        {
            Username = loginDTO.UserName,
            Password = loginDTO.Password
        };
        //Login the account
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
        {
            var returnValue = new ReturnValue<AccountLoggedInDTO>();
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var roleRepository = _repositoryFactory.CreateRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);

            try
            {
                var accountLoginValue = await accountRepository.GetRowAsync(account);
                returnValue.Consume(accountLoginValue);
                if (!accountLoginValue.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }

                //Update the last login time
                account.LastLogin = DateTime.UtcNow;
                var accountUpdateValue = await accountRepository.UpdateRowAsync(account);
                returnValue.Consume(accountUpdateValue);
                if (!accountUpdateValue.Success)
                {                    
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }

                var accountRoleValue = await accountRoleRepository.GetRoleIdsForAccountIdAsync(account.Id);
                returnValue.Consume(accountRoleValue);
                if (!accountRoleValue.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }
                var roleIds = accountRoleValue.Data ?? [];

                var roleNamesValue = await roleRepository.GetRoleNamesForRoleIdsAsync(roleIds);
                returnValue.Consume(roleNamesValue);
                if (!roleNamesValue.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }

                var accountLoggedInDTO = new AccountLoggedInDTO
                {
                    Account = account,
                    RoleNames = roleNamesValue.Data ?? []
                };

                returnValue.Data = accountLoggedInDTO;
                returnValue.Success = true;
                await unitOfWork.CommitAsync();
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("Login", "An error occurred while logging in.");
                returnValue.AddError("Login", ex.Message);
                return returnValue;
            }
        }
    }

    public ReturnValue<AccountLoggedInDTO> UpdateAccount(UpdateAccountDTO updateAccount)
    {
        var returnValue = new ReturnValue<AccountLoggedInDTO>();

        // Validate the account
        var validationResult = _accountValidation.ValidateModel(updateAccount.Account);
        if (!validationResult.Success)
        {
            return returnValue.Consume(validationResult);
        }
        returnValue.Consume(validationResult);

        // Begin a transaction
        using (var unitOfWork = UnitOfWork.Create(true))
        {
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var roleRepository = _repositoryFactory.CreateRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);

            try
            {
                // Step 1: Validate roles
                var roleValidationResult = roleRepository.CheckRolesExist(updateAccount.Roles);
                if (!roleValidationResult.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(roleValidationResult);
                    return returnValue;
                }
                returnValue.Consume(roleValidationResult);

                // Step 2: Update the Account table
                var accountUpdateResult = accountRepository.UpdateRow(updateAccount.Account);
                if (!accountUpdateResult.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(accountUpdateResult);
                    return returnValue;
                }
                returnValue.Consume(accountUpdateResult);

                // Step 3: Update the AccountRoles table
                var updateAccountRoleResults = accountRoleRepository.UpdateRolesForAccountId(updateAccount.Account.Id, updateAccount.Roles);
                if (!updateAccountRoleResults.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(updateAccountRoleResults);
                    return returnValue;
                }
                returnValue.Consume(updateAccountRoleResults);

                // Step 4: Retrieve the updated account for the return value
                var updatedAccount = accountRepository.GetRow(updateAccount.Account);
                if (!updatedAccount.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(updatedAccount);
                    return returnValue;
                }
                returnValue.Consume(updatedAccount);

                // Step 5: Retrieve the role names for the updated account
                var roleNamesResult = roleRepository.GetRoleNamesForRoleIds(updateAccount.Roles);
                if (!roleNamesResult.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(roleNamesResult);
                    return returnValue;
                }
                returnValue.Consume(roleNamesResult);

                var accountLoggedInDTO = new AccountLoggedInDTO
                {
                    Account = updatedAccount.Data!,
                    RoleNames = roleNamesResult.Data ?? []
                };

                returnValue.Data = accountLoggedInDTO;

                // if it get this far the, we know this is a success
                returnValue.Success = true;

                // Commit the transaction
                unitOfWork.Commit();
                returnValue.Success = true;
                returnValue.AddMessage("updateaccount", "Account and roles updated successfully.");
                return returnValue;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                returnValue.AddError("updateaccount", "An error occurred while updating the account.");
                returnValue.AddError("updateaccount", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue<AccountLoggedInDTO>> UpdateAccountAsync(UpdateAccountDTO updateAccount)
    {
        var returnValue = new ReturnValue<AccountLoggedInDTO>();

        // Validate the account
        var validationResult = _accountValidation.ValidateModel(updateAccount.Account);
        if (!validationResult.Success)
        {
            return returnValue.Consume(validationResult);
        }
        returnValue.Consume(validationResult);

        // Begin a transaction
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
        {
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var roleRepository = _repositoryFactory.CreateRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);

            try
            {
                // Step 1: Validate roles
                var roleValidationResult = await roleRepository.CheckRolesExistAsync(updateAccount.Roles);
                if (!roleValidationResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(roleValidationResult);
                    return returnValue;
                }
                returnValue.Consume(roleValidationResult);

                // Step 2: Update the Account table
                var accountUpdateResult = await accountRepository.UpdateRowAsync(updateAccount.Account);
                if (!accountUpdateResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(accountUpdateResult);
                    return returnValue;
                }
                returnValue.Consume(accountUpdateResult);

                // Step 3: Update the AccountRoles table
                var updateAccountRoleResults = await accountRoleRepository.UpdateRolesForAccountIdAsync(updateAccount.Account.Id, updateAccount.Roles);
                if (!updateAccountRoleResults.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(updateAccountRoleResults);
                    return returnValue;
                }
                returnValue.Consume(updateAccountRoleResults);

                // Step 4: Retrieve the updated account for the return value
                var updatedAccount = await accountRepository.GetRowAsync(updateAccount.Account);
                if (!updatedAccount.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(updatedAccount);
                    return returnValue;
                }
                returnValue.Consume(updatedAccount);

                // Step 5: Retrieve the role names for the updated account
                var roleNamesResult = await roleRepository.GetRoleNamesForRoleIdsAsync(updateAccount.Roles);
                if (!roleNamesResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(roleNamesResult);
                    return returnValue;
                }
                returnValue.Consume(roleNamesResult);

                var accountLoggedInDTO = new AccountLoggedInDTO
                {
                    Account = updatedAccount.Data!,
                    RoleNames = roleNamesResult.Data ?? []
                };

                returnValue.Data = accountLoggedInDTO;

                // if it get this far the, we know this is a success
                returnValue.Success = true;

                // Commit the transaction
                await unitOfWork.CommitAsync();
                returnValue.Success = true;
                returnValue.AddMessage("updateaccount", "Account and roles updated successfully.");
                return returnValue;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                returnValue.AddError("updateaccount", "An error occurred while updating the account.");
                returnValue.AddError("updateaccount", ex.Message);
                return returnValue;
            }
        }
    }

    public ReturnValue DeleteAccount(int id)
    {
        var returnValue = new ReturnValue();
        // Begin a transaction
        using (var unitOfWork = UnitOfWork.Create(true))
        {
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            try
            {
                // Step 1: Delete the AccountRoles for the account
                var deleteAccountRolesResult = accountRoleRepository.DeleteRolesForAccountId(id);
                if (!deleteAccountRolesResult.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(deleteAccountRolesResult);
                    return returnValue;
                }
                returnValue.Consume(deleteAccountRolesResult);
                // Step 2: Delete the Account
                var deleteAccountResult = accountRepository.DeleteRow(id);
                if (!deleteAccountResult.Success)
                {
                    unitOfWork.Rollback();
                    returnValue.Consume(deleteAccountResult);
                    return returnValue;
                }
                returnValue.Consume(deleteAccountResult);
                // Commit the transaction
                unitOfWork.Commit();
                returnValue.Success = true;
                returnValue.AddMessage("deleteaccount", "Account deleted successfully.");
                return returnValue;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                returnValue.AddError("deleteaccount", "An error occurred while deleting the account.");
                returnValue.AddError("deleteaccount", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue> DeleteAccountAsync(int id)
    {
        var returnValue = new ReturnValue();
        // Begin a transaction
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
        {
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction!);
            try
            {
                // Step 1: Delete the AccountRoles for the account
                var deleteAccountRolesResult = await accountRoleRepository.DeleteRolesForAccountIdAsync(id);
                if (!deleteAccountRolesResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(deleteAccountRolesResult);
                    return returnValue;
                }
                returnValue.Consume(deleteAccountRolesResult);
                // Step 2: Delete the Account
                var deleteAccountResult = await accountRepository.DeleteRowAsync(id);
                if (!deleteAccountResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    returnValue.Consume(deleteAccountResult);
                    return returnValue;
                }
                returnValue.Consume(deleteAccountResult);
                // Commit the transaction
                await unitOfWork.CommitAsync();
                returnValue.Success = true;
                returnValue.AddMessage("deleteaccount", "Account deleted successfully.");
                return returnValue;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                returnValue.AddError("deleteaccount", "An error occurred while deleting the account.");
                returnValue.AddError("deleteaccount", ex.Message);
                return returnValue;
            }
        }
    }

    public ReturnValue<List<AccountWithRolesDTO>> GetAllAccounts()
    {
        var returnValue = new ReturnValue<List<AccountWithRolesDTO>>();
        returnValue.Data = new List<AccountWithRolesDTO>();

        using (var unitOfWork = UnitOfWork.Create(true))
        {
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var accountsResult = accountRepository.GetFirstXRows(0);
                if (!accountsResult.Success)
                {
                    returnValue.Consume(accountsResult);
                    returnValue.AddError("accountservice", "An error occurred while retrieving accountsResult.");
                    unitOfWork.Rollback();
                    return returnValue;
                }
                var accounts = accountsResult.Data ?? new List<Account>();
                foreach (var account in accounts)
                {
                    var roles = accountRoleRepository.GetSingleAccountRoleDTOForAccountId(account.Id);
                    if (!roles.Success)
                    {
                        returnValue.Consume(roles);
                        returnValue.AddError("accountservice", $"An error occurred while retrieving roles for account id: {account.Id}.");
                        unitOfWork.Rollback();
                        return returnValue;
                    }
                    var accountRolesDTO = new AccountWithRolesDTO
                    {
                        Account = account,
                        Roles = roles.Data ?? []
                    };    
                    returnValue.Data.Add(accountRolesDTO);
                }
                returnValue.Success = true;
                unitOfWork.Commit();
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("accountservice", ex.Message);
                unitOfWork.Rollback();
                return returnValue;
            }
        }
    }   
    public async Task<ReturnValue<List<AccountWithRolesDTO>>> GetAllAccountsAsync()
    {
        var returnValue = new ReturnValue<List<AccountWithRolesDTO>>();
        returnValue.Data = new List<AccountWithRolesDTO>();

        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var accountRepository = _repositoryFactory.CreateAccountRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var accountRoleRepository = _repositoryFactory.CreateAccountRoleRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var accountsResult = await accountRepository.GetFirstXRowsAsync(0);
                if (!accountsResult.Success)
                {
                    returnValue.Consume(accountsResult);
                    returnValue.AddError("accountservice", "An error occurred while retrieving accountsResult.");
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }
                var accounts = accountsResult.Data ?? new List<Account>();
                foreach (var account in accounts)
                {
                    var roles = await accountRoleRepository.GetSingleAccountRoleDTOForAccountIdAsync(account.Id);
                    if (!roles.Success)
                    {
                        returnValue.Consume(roles);
                        returnValue.AddError("accountservice", $"An error occurred while retrieving roles for account id: {account.Id}.");
                        await unitOfWork.RollbackAsync();
                        return returnValue;
                    }
                    var accountRolesDTO = new AccountWithRolesDTO
                    {
                        Account = account,
                        Roles = roles.Data ?? []
                    };
                    returnValue.Data.Add(accountRolesDTO);
                }
                returnValue.Success = true;
                await unitOfWork.CommitAsync();
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("accountservice", ex.Message);
                await unitOfWork.RollbackAsync();
                return returnValue;
            }
        }
    }
}
