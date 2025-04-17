
using Persistence.Models.Identity;

namespace Persistence.DTOs.Identity;
public class LoginDTO
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class RegisterDTO
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateAccountDTO
{
    public Account Account { get; set; } = new Account();
    public List<int> Roles { get; set; } = [];
}
