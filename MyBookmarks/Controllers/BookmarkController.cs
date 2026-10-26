using Humanizer;
using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
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
    public class BookmarkController : Controller
    {
        private readonly IBookmarkService bookmarkService;
        private readonly ICategoryService categoryService;

        public BookmarkController(IBookmarkService bookmarkService,
             ICategoryService categoryService)
        {
            this.bookmarkService = bookmarkService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int pageNumber = 1, string? searchTerm = null, 
            int? categoryId = null, bool onlyFavorites = false)
        {
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
                .GetUserBookmarksAsync(userId, pageNumber, searchTerm, categoryId, onlyFavorites);

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
                OnlyFavorites = onlyFavorites
            };

            return View(model);
        }

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

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(
            int id, int pageNumber = 1, 
            string? searchTerm = null, int? categoryId = null, bool onlyFavorites = false)
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
                nameof(Index), new { pageNumber, searchTerm, categoryId, onlyFavorites });
        }

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
