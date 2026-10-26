using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using MyBookmarks.ViewModels.Bookmark;
using MyBookmarks.ViewModels.Collection;
using System.Security.Claims;

namespace MyBookmarks.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICollectionService collectionService;
        private readonly IBookmarkService bookmarkService;
        public CollectionController(ICollectionService collectionService, IBookmarkService bookmarkService)
        {
            this.collectionService = collectionService;
            this.bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int maxPageNumber = int.MaxValue / PageConstants.CollectionsPageSize;

            if (pageNumber < 1 || pageNumber > maxPageNumber)
            {
                return BadRequest("Invalid page number");
            }

            string? userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            PagedResultDto<CollectionListItemDto> page = await collectionService
                    .GetUserCollectionsAsync(userId, pageNumber);

            CollectionIndexViewModel model = new()
            {
                Collections = page.Items
                    .Select(c => new CollectionListItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToList(),

                TotalCount = page.TotalCount,
                PageNumber = page.PageNumber,
                PageSize = page.PageSize
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CollectionCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CollectionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CollectionCreateDto dto = new CollectionCreateDto
            {
                Name = model.Name,
                Description = model.Description
            };

            bool created = await this.collectionService
                .TryCreateAsync(userId, dto);

            if (!created)
            {
                ModelState.AddModelError(nameof(model.Name), "You already have a collection with this name.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid collection id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CollectionEditDto? collection = await collectionService
                .GetForEditAsync(userId, id);

            if (collection == null)
            {
                return NotFound();
            }

            CollectionEditViewModel model = new CollectionEditViewModel
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CollectionEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CollectionEditDto dto = new CollectionEditDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            CollectionEditResult result = await collectionService
                .EditAsync(userId, dto);

            if (result == CollectionEditResult.NotFound)
            {
                return NotFound();
            }

            if (result == CollectionEditResult.DuplicateName)
            {
                ModelState.AddModelError(nameof(model.Name), "You already have a collection with this name.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid collection id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CollectionDeleteDto? collection = await collectionService
                .GetForDeleteAsync(userId, id);

            if (collection == null)
            {
                return NotFound();
            }

            CollectionDeleteViewModel model = new CollectionDeleteViewModel
            {
                Id = collection.Id,
                Name = collection.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CollectionDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid collection id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            bool deleted = await collectionService
                .TryDeleteAsync(userId, model.Id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, int pageNumber = 1)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid collection id.");
            }

            int maxPageNumber = int.MaxValue / PageConstants.BookmarksPageSize;

            if (pageNumber < 1 || pageNumber > maxPageNumber)
            {
                return BadRequest("Invalid page number.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CollectionDetailsDto? collection = await collectionService
                .GetDetailsAsync(userId, id, pageNumber);

            if (collection == null)
            {
                return NotFound();
            }

            CollectionDetailsViewModel model = new CollectionDetailsViewModel
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description,

                BookmarksPage = new BookmarkIndexViewModel
                {
                    Bookmarks = collection.BookmarksPage.Items
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

                    TotalCount = collection.BookmarksPage.TotalCount,
                    PageNumber = collection.BookmarksPage.PageNumber,
                    PageSize = collection.BookmarksPage.PageSize
                }
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddBookmarks(int id, int pageNumber = 1)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid collection id.");
            }

            int maxPageNumber = int.MaxValue / PageConstants.BookmarksPageSize;

            if (pageNumber < 1 || pageNumber > maxPageNumber)
            {
                return BadRequest("Invalid page number.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CollectionListItemDto? collection = await collectionService
                .GetByIdAsync(userId, id);

            if (collection == null)
            {
                return NotFound();
            }

            PagedResultDto<BookmarkListItemDto> page =
                await bookmarkService
                    .GetUserBookmarksAsync(userId, pageNumber);

            List<int> bookmarkIds = page.Items
                .Select(b => b.Id)
                .ToList();

            List<int> includedBookmarkIds = await this.collectionService
                .GetIncludedBookmarkIdsAsync(userId, id, bookmarkIds);

            CollectionAddBookmarksViewModel model = new()
            {
                CollectionId = collection.Id,
                CollectionName = collection.Name,
                IncludedBookmarkIds = includedBookmarkIds,

                BookmarksPage = new BookmarkIndexViewModel
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

                    TotalCount = page.TotalCount,
                    PageNumber = page.PageNumber,
                    PageSize = page.PageSize,
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBookmark(int collectionId, int bookmarkId, int pageNumber = 1)
        {
            if (collectionId <= 0 || bookmarkId <= 0)
            {
                return BadRequest("Invalid collection or bookmark id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            bool added = await this.collectionService
                .TryAddBookmarkAsync(userId, collectionId, bookmarkId);

            if (!added)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(AddBookmarks), new { id = collectionId, pageNumber });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBookmark
            (int collectionId, int bookmarkId, int pageNumber = 1)
        {
            if (collectionId <= 0 || bookmarkId <= 0)
            {
                return BadRequest("Invalid collection or bookmark id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            bool removed = await collectionService
                .TryRemoveBookmarkAsync(userId, collectionId, bookmarkId);

            if (!removed)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = collectionId, pageNumber });
        }
    }
}
