
namespace Persistence.DTOs.Identity;

public class AccountRoleDTO
{
    public int AccountId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
public class SingleAccountRoleDTO
{
    public int Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int RoleNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}

