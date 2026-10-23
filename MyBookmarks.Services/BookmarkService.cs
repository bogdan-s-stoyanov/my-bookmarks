using MyBookmarks.Common;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services
{
    public class BookmarkService : IBookmarkService
    {
        private IBookmarkRepository bookmarkRepository;

        public BookmarkService(IBookmarkRepository bookmarkRepository)
        {
            this.bookmarkRepository = bookmarkRepository;
        }

        public async Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(string userId, int pageNumber)
        {
            int pageSize = PageConstants.BookmarksPageSize;

            int totalBookmarksCount = await bookmarkRepository
                .CountBookmarksForUserAsync(userId);

            int totalPagesCount = Math.Max(1, (int)Math.Ceiling((double)totalBookmarksCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPagesCount);

            int skip = (pageNumber - 1) * pageSize;

            List<Bookmark> bookmarks = await bookmarkRepository.GetBookmarksByUserIdAsync(userId, totalPagesCount, skip);

            return new PagedResultDto<BookmarkListItemDto>
            {
                Items = bookmarks
                    .Select(b => new BookmarkListItemDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Url = b.Url,
                        Description = b.Description,
                        CategoryName = b.Category?.Name,
                        IsFavourite = b.IsFavorite
                    })
                    .ToList(),

                TotalCount = totalBookmarksCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
