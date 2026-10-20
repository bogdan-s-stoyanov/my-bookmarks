using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs;
using MyBookmarks.ViewModels.Category;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace MyBookmarks.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int maxPageNumber = int.MaxValue / PageConstants.CategoriesPageSize;

            if (pageNumber < 1 || pageNumber > maxPageNumber)
            {
                return BadRequest("Invalid page number");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            List<CategoryListItemDto> categories = await categoryService
                .GetUserCategoriesAsync(userId, pageNumber);

            List<CategoryListItemViewModel> model = categories.Select(c => new CategoryListItemViewModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
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

            CategoryCreateDto categoryCreateDto = new()
            {
                Name = model.Name
            };

            bool created = await categoryService
                .TryCreateAsync(userId, categoryCreateDto);

            if (!created)
            {
                ModelState.AddModelError(nameof(model.Name), "You have already created a category with this name.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
