
using ErrorHandling;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;

namespace Persistence.Services.Transaction;
public interface IItemServices
{
    ReturnValue CreateItem(ItemDTO itemDTO);
    Task<ReturnValue> CreateItemAsync(ItemDTO itemDTO);
    ReturnValue DeleteItem(Item item);
    Task<ReturnValue> DeleteItemAsync(Item item);
    ReturnValue<List<Item>> GetAllItems();
    Task<ReturnValue<List<Item>>> GetAllItemsAsync();
    ReturnValue<List<Item>> GetAllItemsByCategoryId(int categoryId);
    Task<ReturnValue<List<Item>>> GetAllItemsByCategoryIdAsync(int categoryId);
    ReturnValue UpdateCategory(Item item);
    Task<ReturnValue> UpdateCategoryAsync(Item item);
}