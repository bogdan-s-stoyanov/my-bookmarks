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
    }
}
