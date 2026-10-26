using Microsoft.Data.SqlClient;
using MyBookmarks.Common;
using MyBookmarks.Common.EntityValidation;
using MyBookmarks.Common.Enums;
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

        public async Task<PagedResultDto<BookmarkListItemDto>> GetUserBookmarksAsync(
             string userId, int pageNumber,
             string? searchTerm = null, int? categoryId = null,
             bool onlyFavorites = false,
             BookmarkSortOrder sortOrder = BookmarkSortOrder.TitleAscending)
        {
            searchTerm = searchTerm?.Trim();

            int pageSize = PageConstants.BookmarksPageSize;

            int totalBookmarksCount = await bookmarkRepository
                .CountBookmarksForUserAsync(userId, searchTerm, categoryId, onlyFavorites);

            int totalPagesCount = Math.Max(1, (int)Math.Ceiling((double)totalBookmarksCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPagesCount);

            int skip = (pageNumber - 1) * pageSize;

            List<Bookmark> bookmarks = await bookmarkRepository
                .GetBookmarksByUserIdAsync(userId, pageSize, skip, 
                    searchTerm, categoryId, onlyFavorites, sortOrder);

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
                        IsFavorite = b.IsFavorite,
                    })
                    .ToList(),

                TotalCount = totalBookmarksCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
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
                IsFavorite = false,
            };

            await bookmarkRepository.AddAsync(bookmark);

            return BookmarkCreateResult.Success;
        }

        public async Task<BookmarkEditDto?> GetForEditAsync(string userId, int id)
        {
            Bookmark? bookmark = await bookmarkRepository
                .GetByIdAsync(userId, id);

            if (bookmark == null)
            {
                return null;
            }

            return new BookmarkEditDto
            {
                Id = bookmark.Id,
                Title = bookmark.Title,
                Url = bookmark.Url,
                Description = bookmark.Description,
                CategoryId = bookmark.CategoryId
            };
        }

        public async Task<BookmarkEditResult> EditAsync(string userId, BookmarkEditDto bookmarkEditDto)
        {
            Bookmark? bookmark = await bookmarkRepository
                .GetByIdAsync(userId, bookmarkEditDto.Id);

            if (bookmark == null)
            {
                return BookmarkEditResult.NotFound;
            }

            if (!UrlHelper.TryNormalize(bookmarkEditDto.Url, out string normalizedUrl))
            {
                return BookmarkEditResult.InvalidUrl;
            }

            if (normalizedUrl.Length > BookmarkValidationConstants.UrlMaxLength)
            {
                return BookmarkEditResult.InvalidUrl;
            }

            string title = bookmarkEditDto.Title.Trim();

            if (await bookmarkRepository
                .ExistsByTitleExceptIdAsync(userId, title, bookmarkEditDto.Id))
            {
                return BookmarkEditResult.DuplicateTitle;
            }

            if (await bookmarkRepository
                .ExistsByUrlExceptIdAsync(userId, normalizedUrl, bookmarkEditDto.Id))
            {
                return BookmarkEditResult.DuplicateUrl;
            }

            if (bookmarkEditDto.CategoryId.HasValue)
            {
                Category? category = await categoryRepository
                    .GetById(userId, bookmarkEditDto.CategoryId.Value);

                if (category == null)
                {
                    return BookmarkEditResult.InvalidCategory;
                }
            }

            bookmark.Title = title;
            bookmark.Url = normalizedUrl;
            bookmark.Description = bookmarkEditDto.Description?.Trim();
            bookmark.CategoryId = bookmarkEditDto.CategoryId;

            await bookmarkRepository.UpdateAsync(bookmark);

            return BookmarkEditResult.Success;
        }

        public async Task<BookmarkDeleteDto?> GetForDeleteAsync(string userId, int id)
        {
            Bookmark? bookmark = await bookmarkRepository.GetByIdAsync(userId, id);

            if (bookmark == null)
            {
                return null;
            }

            return new BookmarkDeleteDto
            {
                Id = bookmark.Id,
                Title = bookmark.Title
            };
        }

        public async Task<bool> TryDeleteAsync(string userId, int id)
        {
            Bookmark? bookmark = await bookmarkRepository
                .GetByIdAsync(userId, id);

            if (bookmark == null)
            {
                return false;
            }

            await bookmarkRepository.DeleteAsync(bookmark);

            return true;
        }

        public async Task<bool> ToggleFavoriteAsync(string userId, int id)
        {
            Bookmark? bookmark = await bookmarkRepository
                .GetByIdAsync(userId, id);

            if (bookmark == null)
            {
                return false;
            }

            bookmark.IsFavorite = !bookmark.IsFavorite;

            await bookmarkRepository.UpdateAsync(bookmark);

            return true;
        }
    }
}
