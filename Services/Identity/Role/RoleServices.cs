using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Identity;
using Persistence.Repositories;

namespace Persistence.Services.Identity;
public class RoleServices : IRoleServices
{
    private readonly ILogger<RoleServices> _logger;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;
    private readonly RoleValidation _roleValidation;

    public RoleServices(ILogger<RoleServices> logger, RepositoryFactory repositoryFactory, ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
        _roleValidation = _modelValidationFactory.CreateRoleValidation();
    }

    public ReturnValue<List<Role>> GetAllRoles()
    {
        using (var unitOfWork = UnitOfWork.Create())
        {
            var roleRepository = _repositoryFactory.CreateRoleRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                return roleRepository.GetFirstXRows(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving roles.");
                return new ReturnValue<List<Role>>
                {
                    Success = false,
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "roleservice", new List<string> { "An error occurred while retrieving roles." } }
                    },
                    Data = null
                };
            }
        }
    }

    public async Task<ReturnValue<List<Role>>> GetAllRolesAsync()
    {
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var roleRepository = _repositoryFactory.CreateRoleRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                return await roleRepository.GetFirstXRowsAsync(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving roles asynchronously.");
                return new ReturnValue<List<Role>>
                {
                    Success = false,
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "roleservice", new List<string> { "An error occurred while retrieving roles asynchronously." } }
                    },
                    Data = null
                };
            }
        }
    }
}
