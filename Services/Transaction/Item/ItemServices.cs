
using ErrorHandling;

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.Repositories;

namespace Persistence.Services.Transaction;
public class ItemServices : IItemServices
{
    private readonly ILogger<ItemServices> _logger;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;
    private readonly ItemValidation _itemValidation;

    public ItemServices(ILogger<ItemServices> logger, RepositoryFactory repositoryFactory, ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
        _itemValidation = _modelValidationFactory.CreateItemValidation();
    }

    public ReturnValue CreateItem(ItemDTO itemDTO)
    {
        var item = new Item
        {
            Name = itemDTO.Name,
            Note = itemDTO.Note,
            IsExpense = itemDTO.IsExpense,
            BudgetAmount = itemDTO.BudgetAmount > 0 ? itemDTO.BudgetAmount : 0.0m,
            CurrentAmount = itemDTO.CurrentAmount > 0 ? itemDTO.CurrentAmount : 0.0m,
            CategoryId = itemDTO.CategoryId > 0 ? itemDTO.CategoryId : -1, // Ensure CategoryId is valid
        };
        var returnValue = _itemValidation.ValidateModel(item);
        if (!returnValue.Success)
        {
            return returnValue;
        }
        using (var unitOfWork = UnitOfWork.Create())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var repoResult = itemRepository.InsertRow(item);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("createitem", ex.Message);
            }
            return returnValue;
        }
    }
    public async Task<ReturnValue> CreateItemAsync(ItemDTO itemDTO)
    {
        var item = new Item
        {
            Name = itemDTO.Name,
            Note = itemDTO.Note,
            IsExpense = itemDTO.IsExpense,
            BudgetAmount = itemDTO.BudgetAmount > 0 ? itemDTO.BudgetAmount : 0.0m,
            CurrentAmount = itemDTO.CurrentAmount > 0 ? itemDTO.CurrentAmount : 0.0m,
            CategoryId = itemDTO.CategoryId > 0 ? itemDTO.CategoryId : -1, // Ensure CategoryId is valid
        };
        var returnValue = await _itemValidation.ValidateModelAsync(item);
        if (!returnValue.Success)
        {
            return returnValue;
        }
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var repoResult = await itemRepository.InsertRowAsync(item);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("createitem", ex.Message);
            }
            return returnValue;
        }
    }

    public ReturnValue<List<Item>> GetAllItems()
    {
        var returnValue = new ReturnValue<List<Item>>();
        using (var unitOfWork = UnitOfWork.Create())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = itemRepository.GetFirstXRows(0);
                if (!returnValue.Success)
                {
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                returnValue.AddError("getitem", ex.Message);
            }
            return returnValue;
        }
    }
    public async Task<ReturnValue<List<Item>>> GetAllItemsAsync()
    {
        var returnValue = new ReturnValue<List<Item>>();
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = await itemRepository.GetFirstXRowsAsync(0);
                if (!returnValue.Success)
                {
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                returnValue.AddError("getitem", ex.Message);
            }
            return returnValue;
        }
    }

    public ReturnValue<List<Item>> GetAllItemsByCategoryId(int categoryId)
    {
        var returnValue = new ReturnValue<List<Item>>();
        using (var unitOfWork = UnitOfWork.Create())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = itemRepository.GetAllItemsByCategoryId(categoryId);
                if (!returnValue.Success)
                {
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                returnValue.AddError("getitembycategoryid", ex.Message);
            }
            return returnValue;
        }
    }
    public async Task<ReturnValue<List<Item>>> GetAllItemsByCategoryIdAsync(int categoryId)
    {
        var returnValue = new ReturnValue<List<Item>>();
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = await itemRepository.GetAllItemsByCategoryIdAsync(categoryId);
                if (!returnValue.Success)
                {
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                returnValue.AddError("getitembycategoryid", ex.Message);
            }
            return returnValue;
        }
    }

    public ReturnValue UpdateCategory(Item item)
    {
        var returnValue = _itemValidation.ValidateModel(item);
        if (!returnValue.Success)
        {
            return returnValue;
        }

        using (var unitOfWork = UnitOfWork.Create())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var repoResult = itemRepository.UpdateRow(item);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("updateitem", ex.Message);
            }
            return returnValue;
        }
    }
    public async Task<ReturnValue> UpdateCategoryAsync(Item item)
    {
        var returnValue = await _itemValidation.ValidateModelAsync(item);
        if (!returnValue.Success)
        {
            return returnValue;
        }
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var repoResult = await itemRepository.UpdateRowAsync(item);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("updateitem", ex.Message);
            }
            return returnValue;
        }
    }

    public ReturnValue DeleteItem(Item item)
    {
        var returnValue = new ReturnValue();
        using (var unitOfWork = UnitOfWork.Create())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = itemRepository.DeleteRow(item);
                if (!returnValue.Success)
                {
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                returnValue.AddError("deleteitem", ex.Message);
            }
            return returnValue;
        }
    }
    public async Task<ReturnValue> DeleteItemAsync(Item item)
    {
        var returnValue = new ReturnValue();
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                returnValue = await itemRepository.DeleteRowAsync(item);
                if (!returnValue.Success)
                {
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                returnValue.AddError("deleteitem", ex.Message);
            }
            return returnValue;
        }
    }
}
