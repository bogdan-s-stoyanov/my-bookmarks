using MyBookmarks.Common;
using MyBookmarks.Common.EntityValidation;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Helpers;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository bookmarkRepository;
        private readonly ICategoryRepository categoryRepository;

        public BookmarkService(IBookmarkRepository bookmarkRepository, 
            ICategoryRepository categoryRepository)
        {
            this.bookmarkRepository = bookmarkRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(string userId, int pageNumber)
        {
            int pageSize = PageConstants.BookmarksPageSize;

            int totalBookmarksCount = await bookmarkRepository
                .CountBookmarksForUserAsync(userId);

            int totalPagesCount = Math.Max(1, (int)Math.Ceiling((double)totalBookmarksCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPagesCount);

            int skip = (pageNumber - 1) * pageSize;

            List<Bookmark> bookmarks = await bookmarkRepository.GetBookmarksByUserIdAsync(userId, pageSize, skip);

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


        public async Task<BookmarkCreateResult> CreateAsync(string userId, BookmarkCreateDto bookmarkDto)
        {
            if (!UrlHelper.TryNormalize(bookmarkDto.Url, out string normalizedUrl))
            {
                return BookmarkCreateResult.InvalidUrl;
            }

            // Normalization could make the URL longer.
            // Example: https://example.com → https://example.com/
            if (normalizedUrl.Length > BookmarkValidationConstants.UrlMaxLength)
            {
                return BookmarkCreateResult.InvalidUrl;
            }

            string title = bookmarkDto.Title.Trim();

            if (await bookmarkRepository.ExistsByTitleAsync(userId, title))
            {
                return BookmarkCreateResult.DuplicateTitle;
            }

            if (await bookmarkRepository.ExistsByUrlAsync(userId, normalizedUrl))
            {
                return BookmarkCreateResult.DuplicateUrl;
            }

            if (bookmarkDto.CategoryId.HasValue)
            {
                Category? category = await categoryRepository
                    .GetById(userId, bookmarkDto.CategoryId.Value);

                if (category == null)
                {
                    return BookmarkCreateResult.InvalidCategory;
                }
            }

            Bookmark bookmark = new()
            {
                Title = title,
                Url = normalizedUrl,
                Description = bookmarkDto.Description?.Trim(),
                CategoryId = bookmarkDto.CategoryId,
                UserId = userId,
                DateAdded = DateTime.UtcNow,
                IsFavorite = false
            };

            await bookmarkRepository.AddAsync(bookmark);

            return BookmarkCreateResult.Success;
        }
    }
}
