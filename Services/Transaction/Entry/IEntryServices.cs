
using ErrorHandling;

using Persistence.DTOs.Transaction;

namespace Persistence.Services.Transaction;
public interface IEntryServices
{
    ReturnValue CreateEntry(EntryDTO entryDTO);
    Task<ReturnValue> CreateEntryAsync(EntryDTO entryDTO);
}