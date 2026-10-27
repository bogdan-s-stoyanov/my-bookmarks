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
    /// <summary>
    /// Handles collection pages, form submissions and bookmark membership for the current user.
    /// </summary>
    public class CollectionController : Controller
    {
        private readonly ICollectionService collectionService;
        private readonly IBookmarkService bookmarkService;
        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="collectionService">The service providing collection and membership operations.</param>
        /// <param name="bookmarkService">The service providing the user's bookmarks for selection.</param>
        public CollectionController(ICollectionService collectionService, IBookmarkService bookmarkService)
        {
            this.collectionService = collectionService;
            this.bookmarkService = bookmarkService;
        }

        /// <summary>
        /// Displays a page of the current user's collections.
        /// </summary>
        /// <param name="pageNumber">The requested page number, starting at 1.</param>
        /// <returns>
        /// The collection list, a bad request for an invalid page number, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Displays the collection creation form.
        /// </summary>
        /// <returns>
        /// The creation view with an empty input model.
        /// </returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CollectionCreateViewModel());
        }

        /// <summary>
        /// Processes the collection creation form for the current user.
        /// </summary>
        /// <param name="model">The submitted collection data.</param>
        /// <returns>
        /// A redirect to the collection list on success, the form with errors for invalid or duplicate input,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Displays the edit form for a collection owned by the current user.
        /// </summary>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// The edit form, a bad request for a non-positive ID, not found for a missing or foreign collection,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Processes changes to a collection owned by the current user.
        /// </summary>
        /// <param name="model">The collection ID and submitted data.</param>
        /// <returns>
        /// A redirect to the collection list on success, the form with validation or duplicate-name errors,
        /// not found for a missing or foreign collection, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Displays deletion confirmation for a collection owned by the current user.
        /// </summary>
        /// <param name="id">The collection ID.</param>
        /// <returns>
        /// The confirmation view, a bad request for a non-positive ID, not found for a missing or foreign collection,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Deletes the collection after confirmation by its owner.
        /// </summary>
        /// <param name="model">The submitted deletion model containing the collection ID.</param>
        /// <returns>
        /// A redirect to the collection list on success, a bad request for invalid input,
        /// not found for a missing or foreign collection, or an authentication challenge if the user ID is unavailable.
        /// </returns>
        /// <remarks>
        /// The bookmarks themselves are not deleted.
        /// </remarks>
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

        /// <summary>
        /// Displays the user's collection details and a page of its bookmarks.
        /// </summary>
        /// <param name="id">The collection ID.</param>
        /// <param name="pageNumber">The requested bookmark page number, starting at 1.</param>
        /// <returns>
        /// The details view, a bad request for an invalid ID or page number,
        /// not found for a missing or foreign collection, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Displays a page of the user's bookmarks for selection into a collection.
        /// </summary>
        /// <param name="id">The collection ID.</param>
        /// <param name="pageNumber">The requested bookmark selection page number, starting at 1.</param>
        /// <returns>
        /// The selection view with existing memberships indicated, a bad request for an invalid ID or page number,
        /// not found for a missing or foreign collection, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Adds one of the user's bookmarks to one of their collections.
        /// </summary>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkId">The bookmark ID.</param>
        /// <param name="pageNumber">The bookmark page number to preserve in the redirect.</param>
        /// <returns>
        /// A redirect to the bookmark selection page if added or already included;
        /// a bad request for non-positive resource IDs; not found if either resource is missing or foreign;
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Removes a bookmark from a collection owned by the same user.
        /// </summary>
        /// <param name="collectionId">The collection ID.</param>
        /// <param name="bookmarkId">The bookmark ID.</param>
        /// <param name="pageNumber">The bookmark page number to preserve in the redirect.</param>
        /// <returns>
        /// A redirect to the collection details if the link is removed or already absent;
        /// a bad request for non-positive resource IDs; not found if either resource is missing or foreign;
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
        /// <remarks>
        /// Only the link is removed; the bookmark itself is not deleted.
        /// </remarks>
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
