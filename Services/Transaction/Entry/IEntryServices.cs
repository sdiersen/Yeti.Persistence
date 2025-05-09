
using ErrorHandling;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;

namespace Persistence.Services.Transaction;
public interface IEntryServices
{
    ReturnValue CreateEntry(EntryDTO entryDTO);
    Task<ReturnValue> CreateEntryAsync(EntryDTO entryDTO);
    Task<ReturnValue<Entry>> CreateAndReturnEntryAsync(EntryDTO entryDTO);

    ReturnValue<List<Entry>> GetAllEntries();
    Task<ReturnValue<List<Entry>>> GetAllEntriesAsync();

    ReturnValue<List<Entry>> GetAllEntriesByItemId(int itemId);
    Task<ReturnValue<List<Entry>>> GetAllEntriesByItemIdAsync(int itemId);

    ReturnValue UpdateEntry(Entry entry);
    Task<ReturnValue> UpdateEntryAsync(Entry entry);
    Task<ReturnValue<Entry>> UpdateAndReturnEntryAsync(Entry entry);
}