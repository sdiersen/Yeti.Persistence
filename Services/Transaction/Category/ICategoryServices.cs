
using ErrorHandling;

using Persistence.DTOs.Transaction;
using Persistence.Models.Transaction;

namespace Persistence.Services.Transaction;
public interface ICategoryServices
{
    ReturnValue CreateCategory(CategoryDTO categoryDTO);
    Task<ReturnValue> CreateCategoryAsync(CategoryDTO categoryDTO);
    Task<ReturnValue<Category>> CreateAndReturnCategoryAsync(CategoryDTO categoryDTO);

    ReturnValue<List<Category>> GetAllCategories();
    Task<ReturnValue<List<Category>>> GetAllCategoriesAsync();

    ReturnValue UpdateCategory(Category category);
    Task<ReturnValue> UpdateCategoryAsync(Category category);
    Task<ReturnValue<Category>> UpdateAndReturnCategoryAsync(Category category);

    ReturnValue DeleteCategory(int id);
    Task<ReturnValue> DeleteCategoryAsync(int id);
}