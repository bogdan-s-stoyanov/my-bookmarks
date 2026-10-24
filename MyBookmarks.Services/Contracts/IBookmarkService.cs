using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services.Contracts
{
    public interface IBookmarkService
    {
        Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(string userId, int pageNumber);

        Task<BookmarkCreateResult> CreateAsync(string userId, BookmarkCreateDto bookmarkDto);
    }
}
