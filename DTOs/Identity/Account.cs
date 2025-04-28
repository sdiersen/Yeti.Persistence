using Persistence.Models.Identity;

namespace Persistence.DTOs.Identity;
public class LoginDTO
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class RegisterDefaultDTO
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateAccountDTO
{
    public Account Account { get; set; } = new Account();
    public List<int> Roles { get; set; } = [];
}

public class AccountLoggedInDTO
{
    public Account Account { get; set; } = new Account();
    public List<string> RoleNames { get; set; } = [];
}

public class AccountWithRolesDTO
{
    public Account Account { get; set; } = new Account();
    public List<SingleAccountRoleDTO> Roles { get; set; } = [];
}
