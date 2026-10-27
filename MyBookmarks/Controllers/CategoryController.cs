using Microsoft.AspNetCore.Mvc;
using MyBookmarks.Common;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Category;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using MyBookmarks.ViewModels.Category;
using System.Security.Claims;

namespace MyBookmarks.Controllers
{
    /// <summary>
    /// Handles category pages and form submissions for the current user.
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes the instance with its required dependencies.
        /// </summary>
        /// <param name="categoryService">The service providing category operations.</param>
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Displays a page of the current user's categories.
        /// </summary>
        /// <param name="pageNumber">The requested page number, starting at 1.</param>
        /// <returns>
        /// The category list, a bad request for an invalid page number, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

            PagedResultDto<CategoryListItemDto> page = await categoryService.
                GetUserCategoriesAsync(userId, pageNumber);

            CategoryIndexViewModel model = new()
            {
                Categories = page.Items
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

        /// <summary>
        /// Displays the category creation form.
        /// </summary>
        /// <returns>
        /// The creation view with an empty input model.
        /// </returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryCreateViewModel());
        }

        /// <summary>
        /// Processes the category creation form for the current user.
        /// </summary>
        /// <param name="model">The submitted category data.</param>
        /// <returns>
        /// A redirect to the category list on success, the form with errors for invalid or duplicate input,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Displays the edit form for a category owned by the current user.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>
        /// The edit form, a bad request for a non-positive ID, not found for a missing or foreign category,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Processes changes to a category owned by the current user.
        /// </summary>
        /// <param name="categoryEditViewModel">The category ID and submitted name.</param>
        /// <returns>
        /// A redirect to the category list on success, the form with validation or duplicate-name errors,
        /// not found for a missing or foreign category, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Displays deletion confirmation for a category owned by the current user.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>
        /// The confirmation view, a bad request for a non-positive ID, not found for a missing or foreign category,
        /// or an authentication challenge if the user ID is unavailable.
        /// </returns>
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

        /// <summary>
        /// Deletes the category after confirmation by its owner.
        /// </summary>
        /// <param name="model">The submitted deletion model containing the category ID.</param>
        /// <returns>
        /// A redirect to the category list on success, a bad request for invalid input,
        /// not found for a missing or foreign category, or an authentication challenge if the user ID is unavailable.
        /// </returns>
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
