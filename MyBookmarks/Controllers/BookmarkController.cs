using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Data.Models;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Bookmark;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.ViewModels.Bookmark;
using System.Security.Claims;

namespace MyBookmarks.Controllers
{
    public class BookmarkController : Controller
    {
        private readonly IBookmarkService bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            this.bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int maxPageNumber = int.MaxValue / PageConstants.BookmarksPageSize;

            if (pageNumber < 1 || pageNumber > maxPageNumber)
            {
                return BadRequest("Invalid page number");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            PagedResultDto<BookmarkListItemDto> page =
                await bookmarkService.GetUserBookmarksAsync(userId, pageNumber);

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
                        IsFavorite = b.IsFavourite
                    })
                    .ToList(),

                PageNumber = page.PageNumber,
                TotalCount = page.TotalCount,
                PageSize = page.PageSize
            };

            return View(model);

        }
    }
}
