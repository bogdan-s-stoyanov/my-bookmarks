using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Contracts
{
    public interface IBookmarkService
    {
        Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(string userId, int pageNumber);
    }
}
