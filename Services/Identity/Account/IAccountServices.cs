
using ErrorHandling;
using Persistence.DTOs.Identity;

namespace Persistence.Services.Identity;
public interface IAccountServices
{
    ReturnValue CreateAccount(RegisterDTO registerDTO);
    Task<ReturnValue> CreateAccountAsync(RegisterDTO registerDTO);
    ReturnValue<AccountLoggedInDTO> Login(LoginDTO loginDTO);
    Task<ReturnValue<AccountLoggedInDTO>> LoginAsync(LoginDTO loginDTO);
    ReturnValue UpdateAccount(UpdateAccountDTO account);
    Task<ReturnValue> UpdateAccountAsync(UpdateAccountDTO account);
    ReturnValue DeleteAccount(int id);
    Task<ReturnValue> DeleteAccountAsync(int id);
}