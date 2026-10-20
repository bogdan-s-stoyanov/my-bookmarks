using MyBookmarks.ViewModels.Category;

namespace MyBookmarks.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryListItemViewModel>> GetUserCategoriesAsync(string userId, int pageNumber);
    }
}
