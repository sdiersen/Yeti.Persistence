
using ErrorHandling;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;

namespace Persistence.Services.Transaction;
public interface IItemServices
{
    ReturnValue CreateItem(ItemDTO itemDTO);
    Task<ReturnValue> CreateItemAsync(ItemDTO itemDTO);
    Task<ReturnValue<Item>> CreateAndReturnItemAsync(ItemDTO itemDTO);

    ReturnValue DeleteItem(Item item);
    Task<ReturnValue> DeleteItemAsync(Item item);

    ReturnValue<List<Item>> GetAllItems();
    Task<ReturnValue<List<Item>>> GetAllItemsAsync();
    ReturnValue<List<Item>> GetAllItemsByCategoryId(int categoryId);
    Task<ReturnValue<List<Item>>> GetAllItemsByCategoryIdAsync(int categoryId);

    ReturnValue UpdateItem(Item item);
    Task<ReturnValue> UpdateItemAsync(Item item);
    Task<ReturnValue<Item>> UpdateAndReturnItemAsync(Item item);
}