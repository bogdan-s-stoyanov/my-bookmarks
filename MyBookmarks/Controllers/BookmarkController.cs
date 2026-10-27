using Humanizer;
using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Common.Enums;
using MyBookmarks.Data.Models;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Category;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using MyBookmarks.ViewModels.Bookmark;
using MyBookmarks.ViewModels.Category;
using System.Security.Claims;

namespace MyBookmarks.Controllers
{
    /// <summary>
    /// Handles bookmark pages, form submissions and favorite updates for the current user.
    /// </summary>
    public class BookmarkController : Controller
    {
        private readonly IBookmarkService bookmarkService;
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="bookmarkService">The service providing bookmark operations.</param>
        /// <param name="categoryService">The service providing the user's category options.</param>
        public BookmarkController(IBookmarkService bookmarkService,
             ICategoryService categoryService)
        {
            this.bookmarkService = bookmarkService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Displays a page of the current user's bookmarks with search, filters and sorting.
        /// </summary>
        /// <param name="pageNumber">The requested page number, starting at 1.</param>
        /// <param name="searchTerm">Text to search for within bookmark titles.</param>
        /// <param name="categoryId">The category ID to filter by, or null to include all categories.</param>
        /// <param name="onlyFavorites">Whether to show only favorite bookmarks.</param>
        /// <param name="sortOrder">The requested bookmark ordering.</param>
        /// <returns>
        /// The bookmark list; a bad request for an invalid page number, category ID or sort order;
        /// not found for a missing or foreign category; or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Index(
            int pageNumber = 1, string? searchTerm = null, 
            int? categoryId = null, bool onlyFavorites = false,
            BookmarkSortOrder sortOrder = BookmarkSortOrder.TitleAscending)
        {
            if (!Enum.IsDefined(sortOrder))
            {
                return BadRequest("Invalid sort order.");
            }

            int maxPageNumber = int.MaxValue / PageConstants.BookmarksPageSize;

            if (pageNumber < 1 || pageNumber > maxPageNumber)
            {
                return BadRequest("Invalid page number.");
            }

            if (categoryId.HasValue && categoryId.Value <= 0)
            {
                return BadRequest("Invalid category id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            List<CategoryListItemDto> categories = await categoryService
                .GetOptionsAsync(userId);

            if (categoryId.HasValue && !categories.Any(c => c.Id == categoryId.Value))
            {
                return NotFound();
            }

            searchTerm = searchTerm?.Trim();

            PagedResultDto<BookmarkListItemDto> page = await bookmarkService
                .GetUserBookmarksAsync(userId, pageNumber, searchTerm, 
                categoryId, onlyFavorites, sortOrder);

            BookmarkIndexViewModel model = new()
            {
                Bookmarks = page.Items
                    .Select(b => new BookmarkListItemViewModel
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Url = b.Url,
                        Description = b.Description,
                        CategoryName = b.CategoryName,
                        IsFavorite = b.IsFavorite
                    })
                    .ToList(),

                Categories = categories
                    .Select(c => new CategoryListItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToList(),

                PageNumber = page.PageNumber,
                TotalCount = page.TotalCount,
                PageSize = page.PageSize,
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                OnlyFavorites = onlyFavorites,
                SortOrder = sortOrder
            };

            return View(model);
        }

        /// <summary>
        /// Displays the bookmark creation form with the current user's category options.
        /// </summary>
        /// <returns>
        /// The creation form, or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            List<CategoryListItemDto> categories = await categoryService
                .GetOptionsAsync(userId);

            BookmarkCreateViewModel model = new()
            {
                Categories = categories
                    .Select(c => new CategoryListItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToList()
            };

            return View(model);
        }


        /// <summary>
        /// Processes the bookmark creation form for the current user.
        /// </summary>
        /// <param name="bookCreateViewModel">The submitted bookmark data.</param>
        /// <returns>
        /// A redirect to the bookmark list on success; the form with errors for invalid input, duplicate title or URL,
        /// or an invalid category; or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create(BookmarkCreateViewModel bookCreateViewModel)
        {
            string? userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                await this.PopulateCategoriesAsync(userId, bookCreateViewModel);
                return View(bookCreateViewModel);
            }

            BookmarkCreateDto bookmarkCreateDto = new()
            {
                Title = bookCreateViewModel.Title,
                Url = bookCreateViewModel.Url,
                Description = bookCreateViewModel.Description,
                CategoryId = bookCreateViewModel.CategoryId
            };

            BookmarkCreateResult result =
                    await bookmarkService.CreateAsync(userId, bookmarkCreateDto);

            if (result == BookmarkCreateResult.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            switch (result)
            {
                case BookmarkCreateResult.InvalidUrl:
                    ModelState.AddModelError(
                        nameof(bookCreateViewModel.Url),
                        "Enter a valid HTTP or HTTPS URL within the allowed length.");
                    break;

                case BookmarkCreateResult.DuplicateTitle:
                    ModelState.AddModelError(
                        nameof(bookCreateViewModel.Title),
                        "You already have a bookmark with this title.");
                    break;

                case BookmarkCreateResult.DuplicateUrl:
                    ModelState.AddModelError(
                        nameof(bookCreateViewModel.Url),
                        "You have already saved this URL.");
                    break;

                case BookmarkCreateResult.InvalidCategory:
                    ModelState.AddModelError(
                        nameof(bookCreateViewModel.CategoryId),
                        "Select one of your categories.");
                    break;

                default:
                    ModelState.AddModelError(
                        string.Empty,
                        "The bookmark could not be created.");
                    break;
            }

            await this.PopulateCategoriesAsync(userId, bookCreateViewModel);
            return View(bookCreateViewModel);
        }

        /// <summary>
        /// Displays the edit form for a bookmark owned by the current user.
        /// </summary>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// The edit form with the user's category options, a bad request for a non-positive ID,
        /// not found for a missing or foreign bookmark, or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid bookmark id.");
            }

            string? userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            BookmarkEditDto? bookmark = await bookmarkService
                .GetForEditAsync(userId, id);

            if (bookmark == null)
            {
                return NotFound();
            }

            List<CategoryListItemDto> categories = await categoryService
                .GetOptionsAsync(userId);

            BookmarkEditViewModel model = new BookmarkEditViewModel
            {
                Id = bookmark.Id,
                Title = bookmark.Title,
                Url = bookmark.Url,
                Description = bookmark.Description,
                CategoryId = bookmark.CategoryId,

                Categories = categories
            .Select(c => new CategoryListItemViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList()
            };

            return View(model);
        }

        /// <summary>
        /// Processes changes to a bookmark owned by the current user.
        /// </summary>
        /// <param name="bookmarkEditViewModel">The bookmark ID and submitted data.</param>
        /// <returns>
        /// A redirect to the bookmark list on success; the form with validation, duplicate or category errors;
        /// not found for a missing or foreign bookmark; or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Edit(BookmarkEditViewModel bookmarkEditViewModel)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(userId, bookmarkEditViewModel);
                return View(bookmarkEditViewModel);
            }

            BookmarkEditDto dto = new BookmarkEditDto
            {
                Id = bookmarkEditViewModel.Id,
                Title = bookmarkEditViewModel.Title,
                Url = bookmarkEditViewModel.Url,
                Description = bookmarkEditViewModel.Description,
                CategoryId = bookmarkEditViewModel.CategoryId
            };

            BookmarkEditResult result = await bookmarkService
                .EditAsync(userId, dto);

            if (result == BookmarkEditResult.NotFound)
            {
                return NotFound();
            }

            if (result == BookmarkEditResult.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            switch (result)
            {
                case BookmarkEditResult.InvalidUrl:
                    {
                        ModelState.AddModelError(nameof(bookmarkEditViewModel.Url), "Enter a valid HTTP or HTTPS URL within the allowed length.");
                        break;
                    }
                case BookmarkEditResult.DuplicateTitle:
                    {
                        ModelState.AddModelError(nameof(bookmarkEditViewModel.Title), "You already have a bookmark with this title.");
                        break;
                    }
                case BookmarkEditResult.DuplicateUrl:
                    {
                        ModelState.AddModelError(nameof(bookmarkEditViewModel.Url), "You have already saved this URL.");
                        break;
                    }
                case BookmarkEditResult.InvalidCategory:
                    {
                        ModelState.AddModelError(nameof(bookmarkEditViewModel.CategoryId), "Select one of your categories.");
                        break;
                    }
                default:
                    {
                        ModelState.AddModelError(string.Empty, "The bookmark could not be updated.");
                        break;
                    }
            }

            await PopulateCategoriesAsync(userId, bookmarkEditViewModel);
            return View(bookmarkEditViewModel);
        }

        /// <summary>
        /// Displays deletion confirmation for a bookmark owned by the current user.
        /// </summary>
        /// <param name="id">The bookmark ID.</param>
        /// <returns>
        /// The confirmation view, a bad request for a zero ID, not found for a missing or foreign bookmark,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            BookmarkDeleteDto? bookmark = await bookmarkService
                .GetForDeleteAsync(userId, id);

            if (bookmark == null)
            {
                return NotFound();
            }

            BookmarkDeleteViewModel model = new BookmarkDeleteViewModel
            {
                Id = bookmark.Id,
                Title = bookmark.Title
            };

            return View(model);
        }

        /// <summary>
        /// Deletes the bookmark after confirmation by its owner.
        /// </summary>
        /// <param name="model">The submitted deletion model containing the bookmark ID.</param>
        /// <returns>
        /// A redirect to the bookmark list on success, a bad request for invalid input,
        /// not found for a missing or foreign bookmark, or an authentication challenge if the user ID is unavailable.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Delete(BookmarkDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid bookmark id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            bool deleted = await this.bookmarkService
                .TryDeleteAsync(userId, model.Id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Reverses a bookmark's favorite status and returns to the bookmark list.
        /// </summary>
        /// <param name="id">The bookmark ID.</param>
        /// <param name="pageNumber">The page number to preserve when returning to the list.</param>
        /// <param name="searchTerm">Text to search for within bookmark titles.</param>
        /// <param name="categoryId">The category ID to filter by, or null to include all categories.</param>
        /// <param name="onlyFavorites">Whether to show only favorite bookmarks.</param>
        /// <param name="sortOrder">The requested bookmark ordering.</param>
        /// <returns>
        /// A redirect preserving the page, search, filters and sort order on success;
        /// a bad request for a non-positive bookmark ID; not found for a missing or foreign bookmark;
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
        /// <remarks>
        /// When the favorites filter is active, removing a bookmark from favorites also removes it from the displayed results.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(
            int id, int pageNumber = 1, string? searchTerm = null, 
            int? categoryId = null, bool onlyFavorites = false, 
            BookmarkSortOrder sortOrder = BookmarkSortOrder.TitleAscending)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid bookmark id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            bool updated = await bookmarkService
                .ToggleFavoriteAsync(userId, id);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(
                nameof(Index), new { pageNumber, searchTerm, categoryId, 
                    onlyFavorites, sortOrder });
        }

        /// <summary>
        /// Populates the creation form's category options, including when redisplaying invalid input.
        /// </summary>
        /// <param name="userId">The owner whose categories are available for selection.</param>
        /// <param name="bookmarkCreateViewModel">The creation model to populate.</param>
        private async Task PopulateCategoriesAsync(string userId,
            BookmarkCreateViewModel bookmarkCreateViewModel)
        {
            List<CategoryListItemDto> categories =
                await categoryService.GetOptionsAsync(userId);

            bookmarkCreateViewModel.Categories = categories
                .Select(c => new CategoryListItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        /// <summary>
        /// Populates the edit form's category options, including when redisplaying invalid input.
        /// </summary>
        /// <param name="userId">The owner whose categories are available for selection.</param>
        /// <param name="model">The edit model to populate.</param>
        private async Task PopulateCategoriesAsync(string userId, BookmarkEditViewModel model)
        {
            List<CategoryListItemDto> categories = await categoryService
                .GetOptionsAsync(userId);

            model.Categories = categories
                .Select(c => new CategoryListItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
