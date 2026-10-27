using MyBookmarks.Services.DTOs.Category;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services.Contracts
{
    /// <summary>
    /// Defines operations for managing a user's categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a page of the user's categories, ordered by name and then by ID.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="pageNumber">
        /// The requested page number, starting at 1.
        /// Values outside the available range are adjusted to that range.
        /// </param>
        /// <returns>
        /// The categories and pagination information.
        /// The category list is empty if the user has no categories.
        /// </returns>
        Task<PagedResultDto<CategoryListItemDto>> GetUserCategoriesAsync(string userId, int pageNumber);

        /// <summary>
        /// Creates a category for the specified user.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="dto">The category data to create.</param>
        /// <returns>
        /// True if the category is created; false if the user already
        /// has a category with the same trimmed name.
        /// </returns>
        Task<bool> TryCreateAsync(string userId, CategoryCreateDto dto);

        /// <summary>
        /// Retrieves the user's category data for editing.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="id">The category ID.</param>
        /// <returns>
        /// The category data, or null if the category does not exist
        /// or belongs to another user.
        /// </returns>
        Task<CategoryEditDto?> GetForEditAsync(string userId, int id);

        /// <summary>
        /// Updates the name of a category owned by the specified user.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="editDto">The category ID and updated data.</param>
        /// <returns>
        /// Success if updated; NotFound if the category is missing or belongs
        /// to another user; DuplicateName if another category owned by the
        /// user has the same trimmed name.
        /// </returns>
        Task<CategoryEditResult> CategoryEditAsync(string userId, CategoryEditDto editDto);

        /// <summary>
        /// Deletes a category owned by the specified user.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="id">The category ID.</param>
        /// <returns>
        /// True if deleted; false if the category does not exist
        /// or belongs to another user.
        /// </returns>
        Task<bool> TryDeleteAsync(string userId, int id);

        /// <summary>
        /// Retrieves the user's category data for deletion confirmation.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="id">The category ID.</param>
        /// <returns>
        /// The category data, or null if the category does not exist
        /// or belongs to another user.
        /// </returns>
        Task<CategoryDeleteDto?> GetForDeleteAsync(string userId, int id);

        /// <summary>
        /// Retrieves all of the user's categories for category selection,
        /// ordered by name and then by ID.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <returns>
        /// All category options, or an empty list if the user has no categories.
        /// </returns>
        Task<List<CategoryListItemDto>> GetOptionsAsync(string userId);
    }
}
