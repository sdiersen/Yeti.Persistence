
using ErrorHandling;

using Persistence.Models.Identity;

namespace Persistence.Services.Identity;
public interface IRoleServices
{
    ReturnValue<List<Role>> GetAllRoles();
    Task<ReturnValue<List<Role>>> GetAllRolesAsync();
}