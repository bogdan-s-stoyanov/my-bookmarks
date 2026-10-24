using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services.Contracts
{
    public interface IBookmarkService
    {
        Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(string userId, int pageNumber);

        Task<BookmarkCreateResult> CreateAsync(string userId, BookmarkCreateDto bookmarkDto);

        Task<BookmarkEditDto?> GetForEditAsync(string userId, int id);

        Task<BookmarkEditResult> EditAsync(string userId, BookmarkEditDto bookmarkEditDto);
    }
}
