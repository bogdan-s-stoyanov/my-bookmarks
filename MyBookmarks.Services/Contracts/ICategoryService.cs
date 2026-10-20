using MyBookmarks.Services.DTOs;

namespace MyBookmarks.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryListItemDto>> GetUserCategoriesAsync(string userId, int pageNumber);

        Task<bool> TryCreateAsync(string userId, CategoryCreateDto dto);
    }
}
