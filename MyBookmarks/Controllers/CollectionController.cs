using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.ViewModels.Collection;
using System.Security.Claims;

namespace MyBookmarks.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICollectionService collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            this.collectionService = collectionService;
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
    }
}
