using ErrorHandling;

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.Repositories;

namespace Persistence.Services.Transaction;
public class CategoryServices : ICategoryServices
{
    private readonly ILogger<CategoryServices> _logger;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;
    private readonly CategoryValidation _categoryValidation;

    public CategoryServices(ILogger<CategoryServices> logger, RepositoryFactory repositoryFactory, ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
        _categoryValidation = _modelValidationFactory.CreateCategoryValidation();
    }

    public ReturnValue CreateCategory(CategoryDTO categoryDTO)
    {
        var cat = new Category();
        cat.Name = categoryDTO.Name;
        cat.Description = categoryDTO.Description;

        var returnValue = new ReturnValue();

        var validationResult = _categoryValidation.ValidateModel(cat);
        if (!validationResult.Success)
        {
            return validationResult;
        }
        returnValue.Consume(validationResult);

        using (var unitOfWork = UnitOfWork.Create())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var repoResult = categoryRepository.InsertRow(cat);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("createcategory", ex.Message);
            }

            return returnValue;
        }
    }
    public async Task<ReturnValue> CreateCategoryAsync(CategoryDTO categoryDTO)
    {
        var cat = new Category();
        cat.Name = categoryDTO.Name;
        cat.Description = categoryDTO.Description;

        var returnValue = new ReturnValue();

        var validationResult = _categoryValidation.ValidateModel(cat);
        if (!validationResult.Success)
        {
            return validationResult;
        }
        returnValue.Consume(validationResult);

        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var repoResult = await categoryRepository.InsertRowAsync(cat);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("createcategory", ex.Message);
            }
            return returnValue;
        }
    }

    public ReturnValue<List<Category>> GetAllCategories()
    {
        var returnValue = new ReturnValue<List<Category>>();
        using (var unitOfWork = UnitOfWork.Create())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = categoryRepository.GetFirstXRows(0);
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("getallcategories", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue<List<Category>>> GetAllCategoriesAsync()
    {
        var returnValue = new ReturnValue<List<Category>>();
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = await categoryRepository.GetFirstXRowsAsync(0);
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("getallcategories", ex.Message);
                return returnValue;
            }
        }
    }

    public ReturnValue UpdateCategory(Category category)
    {
        var returnValue = new ReturnValue();
        using (var unitOfWork = UnitOfWork.Create())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = categoryRepository.UpdateRow(category);
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("updatecategory", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue> UpdateCategoryAsync(Category category)
    {
        var returnValue = new ReturnValue();
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = await categoryRepository.UpdateRowAsync(category);
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("updatecategory", ex.Message);
                return returnValue;
            }
        }
    }

    public ReturnValue DeleteCategory(int id)
    {
        var returnValue = new ReturnValue();
        using (var unitOfWork = UnitOfWork.Create())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = categoryRepository.DeleteRow(id);
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("deletecategory", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue> DeleteCategoryAsync(int id)
    {
        var returnValue = new ReturnValue();
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = await categoryRepository.DeleteRowAsync(id);
                return returnValue;
            }
            catch (Exception ex)
            {
                returnValue.AddError("deletecategory", ex.Message);
                return returnValue;
            }
        }
    }
}
