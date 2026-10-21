using MyBookmarks.Services.DTOs;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryListItemDto>> GetUserCategoriesAsync(string userId, int pageNumber);

        Task<bool> TryCreateAsync(string userId, CategoryCreateDto dto);

        Task<CategoryEditDto?> GetForEditAsync(string userId, int id);

        Task<CategoryEditResult> CategoryEditAsync(string userId, CategoryEditDto editDto);

        Task<bool> TryDeleteAsync(string userId, int id);

        Task<CategoryDeleteDto?> GetForDeleteAsync(string userId, int id);
    }
}
