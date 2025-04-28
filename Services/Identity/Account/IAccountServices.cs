
using ErrorHandling;
using Persistence.DTOs.Identity;

namespace Persistence.Services.Identity;
public interface IAccountServices
{
    ReturnValue CreateAccount(RegisterDefaultDTO registerDTO);
    Task<ReturnValue> CreateAccountAsync(RegisterDefaultDTO registerDTO);
    ReturnValue AdminCreateAccount(UpdateAccountDTO updateAccountDTO);
    Task<ReturnValue> AdminCreateAccountAsync(UpdateAccountDTO updateAccountDTO);

    ReturnValue<AccountLoggedInDTO> Login(LoginDTO loginDTO);
    Task<ReturnValue<AccountLoggedInDTO>> LoginAsync(LoginDTO loginDTO);
    ReturnValue<AccountLoggedInDTO> UpdateAccount(UpdateAccountDTO account);
    Task<ReturnValue<AccountLoggedInDTO>> UpdateAccountAsync(UpdateAccountDTO account);
    ReturnValue DeleteAccount(int id);
    Task<ReturnValue> DeleteAccountAsync(int id);
    ReturnValue<List<AccountWithRolesDTO>> GetAllAccounts();
    Task<ReturnValue<List<AccountWithRolesDTO>>> GetAllAccountsAsync();
}