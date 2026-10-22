using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs;
using MyBookmarks.Services.Results;
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

            CategoryPageDto page = await categoryService.
                GetUserCategoriesAsync(userId, pageNumber);

            CategoryIndexViewModel model = new()
            {
                Categories = page.Categories
                     .Select(c => new CategoryListItemViewModel
                     {
                         Id = c.Id,
                         Name = c.Name
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category id.");
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CategoryEditDto? category = await categoryService.GetForEditAsync(userId, id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryEditViewModel model = new()
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel categoryEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryEditViewModel);
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CategoryEditDto categoryEditDto = new()
            {
                Id = categoryEditViewModel.Id,
                Name = categoryEditViewModel.Name
            };

            CategoryEditResult result = await categoryService
                .CategoryEditAsync(userId, categoryEditDto);

            if (result == CategoryEditResult.NotFound)
            {
                return NotFound();
            }

            if (result == CategoryEditResult.DuplicateName)
            {
                ModelState.AddModelError(
                    nameof(categoryEditViewModel.Name),
                    "You already have a category with this name.");

                return View(categoryEditViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category id.");
            }

            string? userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            CategoryDeleteDto? category = await categoryService
                .GetForDeleteAsync(userId, id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryDeleteViewModel categoryDeleteViewModel = new()
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryDeleteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid category id.");
            }

            string? userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Challenge();
            }

            bool deleted = await this.categoryService
                .TryDeleteAsync(userId, model.Id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
