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
    public async Task<ReturnValue<Entry>> CreateAndReturnEntryAsync(EntryDTO entryDTO)
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
        var validationResults = await _entryValidation.ValidateModelAsync(entry);
        var returnValue = new ReturnValue<Entry>();
        if (!validationResults.Success)
        {
            return returnValue.Consume(validationResults);
        }
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
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
                var repoResult = await entryRepository.InsertRowAndGetIdAsync(entry);
                returnValue.Consume(repoResult);
                if (!repoResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }
                var getResult = await entryRepository.GetRowAsync(repoResult.Data);
                returnValue.Consume(getResult);
                if (!getResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }
                returnValue.Data = getResult.Data;
                returnValue.Success = true;
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
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

    public ReturnValue UpdateEntry(Entry entry)
    {
        var returnValue = _entryValidation.ValidateModel(entry);
        if (!returnValue.Success)
        {
            return returnValue;
        }

        using (var unitOfWork = UnitOfWork.Create(true))
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try
            {
                var categoryResult = categoryRepository.IsValidId(entry.CategoryId);
                var itemResult = itemRepository.IsValidId(entry.ItemId);
                if (!categoryResult.Success || !itemResult.Success)
                {
                    returnValue.Consume(categoryResult);
                    returnValue.Consume(itemResult);
                    returnValue.AddError("updateentry", "Invalid category or item id");
                    return returnValue;
                }
                returnValue = entryRepository.UpdateRow(entry);
                if (!returnValue.Success)
                {
                    unitOfWork.Rollback();
                    return returnValue;
                }
                unitOfWork.Commit();
                returnValue.Success = true;
                return returnValue;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                returnValue.AddError("updateentry", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue> UpdateEntryAsync(Entry entry)
    {
        var returnValue = await _entryValidation.ValidateModelAsync(entry);
        if (!returnValue.Success)
        {
            return returnValue;
        }

        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);

            try
            {
                var categoryResult = await categoryRepository.IsValidIdAsync(entry.CategoryId);
                var itemResult = await itemRepository.IsValidIdAsync(entry.ItemId);
                if (!categoryResult.Success || !itemResult.Success)
                {
                    returnValue.Consume(categoryResult);
                    returnValue.Consume(itemResult);
                    returnValue.AddError("updateentry", "Invalid category or item id");
                    return returnValue;
                }
                returnValue = await entryRepository.UpdateRowAsync(entry);
                if (!returnValue.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }
                await unitOfWork.CommitAsync();
                returnValue.Success = true;
                return returnValue;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                returnValue.AddError("updateentry", ex.Message);
                return returnValue;
            }
        }
    }
    public async Task<ReturnValue<Entry>> UpdateAndReturnEntryAsync(Entry entry)
    {
        var returnValue = new ReturnValue<Entry>();
        var validationResult = await _entryValidation.ValidateModelAsync(entry);
        if (!validationResult.Success)
        {
            return returnValue.Consume(validationResult);
        }
        await using (var unitOfWork = await UnitOfWorkAsync.CreateAsync(true))
        {
            var categoryRepository = _repositoryFactory.CreateCategoryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var itemRepository = _repositoryFactory.CreateItemRepository(unitOfWork.Connection, unitOfWork.Transaction);
            var entryRepository = _repositoryFactory.CreateEntryRepository(unitOfWork.Connection, unitOfWork.Transaction);
            try // TODO do I really need try blocks here, each call handles its own exceptions. Shouldn't this be around the using?
            {
                var categoryResult = await categoryRepository.IsValidIdAsync(entry.CategoryId);
                var itemResult = await itemRepository.IsValidIdAsync(entry.ItemId);
                if (!categoryResult.Success || !itemResult.Success)
                {
                    returnValue.Consume(categoryResult);
                    returnValue.Consume(itemResult);
                    returnValue.AddError("updateentry", "Invalid category or item id");
                    return returnValue;
                }
                var updateResult = await entryRepository.UpdateRowAsync(entry); // TODO should be UpdateRowAndGetIdAsync
                if (!updateResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue.Consume(updateResult);
                }
                var getResult = await entryRepository.GetRowAsync(entry.Id); // TODO should be updateResult.Data
                returnValue.Consume(getResult);
                if (!getResult.Success)
                {
                    await unitOfWork.RollbackAsync();
                    return returnValue;
                }
                returnValue.Data = getResult.Data;
                returnValue.Success = true;
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                returnValue.AddError("updateentry", ex.Message);
            }
            return returnValue;
        }
    }
}
