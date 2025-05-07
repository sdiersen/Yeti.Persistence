using ErrorHandling;

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.Repositories;

namespace Persistence.Services.Transaction;
public class EntryServices : IEntryServices
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly ILogger<EntryServices> _logger;
#pragma warning restore IDE0052 // Remove unread private members
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;
    private readonly EntryValidation _entryValidation;

    public EntryServices(
        ILogger<EntryServices> logger,
        RepositoryFactory repositoryFactory,
        ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
        _entryValidation = _modelValidationFactory.CreateEntryValidation();
    }

    public ReturnValue CreateEntry(EntryDTO entryDTO)
    {
        var isItemUnattached = entryDTO.ItemId == -1;
        var isCategoryUnattached = entryDTO.CategoryId == -1;
        var entry = new Entry
        {
            EntryDate = entryDTO.EntryDate,
            Amount = entryDTO.Amount,
            IsExpense = entryDTO.IsExpense,
            Note = entryDTO.Note,
            ItemId = isItemUnattached ? int.MaxValue : entryDTO.ItemId,
            CategoryId = isCategoryUnattached ? int.MaxValue : entryDTO.CategoryId
        };

        var returnValue = _entryValidation.ValidateModel(entry);
        if (!returnValue.Success)
        {
            return returnValue;
        }

        using (var unitOfWork = UnitOfWork.Create())
        {
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                // check for unattached items and categories, if so, get the correct id values
                if (isItemUnattached)
                {
                    var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
                    var result = itemRepository.GetUnattachedId();
                    if (result.Success)
                    {
                        entry.ItemId = result.Data; // use the unattached item id
                    }
                    else
                    {
                        returnValue.Consume(result);
                    }
                }
                if (isCategoryUnattached)
                {
                    var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
                    var result = categoryRepository.GetUnattachedId();
                    if (result.Success)
                    {
                        entry.CategoryId = result.Data; // use the unattached category id
                    }
                    else
                    {
                        returnValue.Consume(result);
                        return returnValue;
                    }
                }

                var repoResult = entryRepository.InsertRow(entry);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("createentry", ex.Message);
            }
            return returnValue;
        }
    }
    public async Task<ReturnValue> CreateEntryAsync(EntryDTO entryDTO)
    {
        var isItemUnattached = entryDTO.ItemId == -1;
        var isCategoryUnattached = entryDTO.CategoryId == -1;
        var entry = new Entry
        {
            EntryDate = entryDTO.EntryDate,
            Amount = entryDTO.Amount,
            IsExpense = entryDTO.IsExpense,
            Note = entryDTO.Note,
            ItemId = isItemUnattached ? int.MaxValue : entryDTO.ItemId,
            CategoryId = isCategoryUnattached ? int.MaxValue : entryDTO.CategoryId
        };
        var returnValue = await _entryValidation.ValidateModelAsync(entry);
        if (!returnValue.Success)
        {
            return returnValue;
        }
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                // check for unattached items and categories, if so, get the correct id values
                if (isItemUnattached)
                {
                    var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
                    var result = await itemRepository.GetUnattachedIdAsync();
                    if (result.Success)
                    {
                        entry.ItemId = result.Data; // use the unattached item id
                    }
                    else
                    {
                        returnValue.Consume(result);
                    }
                }
                if (isCategoryUnattached)
                {
                    var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
                    var result = await categoryRepository.GetUnattachedIdAsync();
                    if (result.Success)
                    {
                        entry.CategoryId = result.Data; // use the unattached category id
                    }
                    else
                    {
                        returnValue.Consume(result);
                        return returnValue;
                    }
                }
                var repoResult = await entryRepository.InsertRowAsync(entry);
                if (!repoResult.Success)
                {
                    return repoResult;
                }
                returnValue.Consume(repoResult);
                returnValue.Success = true;
            }
            catch (Exception ex)
            {
                returnValue.AddError("createentry", ex.Message);
            }
            return returnValue;
        }
    }

    public ReturnValue<List<Entry>> GetAllEntries()
    {
        try
        {
            using (var unitOfWork = UnitOfWork.Create())
            {
                var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
                return entryRepository.GetFirstXRows(0);
            }
        }
        catch (Exception ex)
        {
            var returnValue = new ReturnValue<List<Entry>>();
            returnValue.AddError("getallentries", ex.Message);
            return returnValue;
        }
    }
    public async Task<ReturnValue<List<Entry>>> GetAllEntriesAsync()
    {
        try
        {
            await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
            {
                var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
                return await entryRepository.GetFirstXRowsAsync(0);
            }
        }
        catch (Exception ex)
        {
            var returnValue = new ReturnValue<List<Entry>>();
            returnValue.AddError("getallentries", ex.Message);
            return returnValue;
        }
    }

    public ReturnValue<List<Entry>> GetAllEntriesByItemId(int itemId)
    {
        using (var unitOfWork = UnitOfWork.Create())
        {
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            return entryRepository.GetAllEntriesForItemId(itemId);
        }
    }   
    public async Task<ReturnValue<List<Entry>>> GetAllEntriesByItemIdAsync(int itemId)
    {
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync())
        {
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var result = await entryRepository.GetAllEntriesForItemIdAsync(itemId);
            return result;
        }
    }
}
