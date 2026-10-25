using MyBookmarks.Services.DTOs.Category;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services.Contracts
{
    public interface ICategoryService
    {
        Task<PagedResultDto<CategoryListItemDto>> GetUserCategoriesAsync(string userId, int pageNumber);

        Task<bool> TryCreateAsync(string userId, CategoryCreateDto dto);

        Task<CategoryEditDto?> GetForEditAsync(string userId, int id);

        Task<CategoryEditResult> CategoryEditAsync(string userId, CategoryEditDto editDto);

        Task<bool> TryDeleteAsync(string userId, int id);

        Task<CategoryDeleteDto?> GetForDeleteAsync(string userId, int id);

        Task<List<CategoryListItemDto>> GetOptionsAsync(string userId);
    }
}
