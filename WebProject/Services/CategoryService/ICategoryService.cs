using WebProject.Entites;

namespace WebProject.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();

        Task<List<Category>> GetMultiLevelMenuCategory();
        Task<Category> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);

        Task<IEnumerable<Category>> GetCategorysCacheAsync();
        Task DeleteCategoryAsync(string id);

    }
}
