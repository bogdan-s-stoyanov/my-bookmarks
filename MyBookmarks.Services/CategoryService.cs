using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBookmarks.Common;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs;
using MyBookmarks.Services.Results;

namespace MyBookmarks.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<CategoryPageDto> GetUserCategoriesAsync(string userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.CategoriesPageSize;

            int totalCount = await categoryRepository
                                .CountCategoriesForUserAsync(userId);

            int totalPages = Math.Max(
                1, (int)Math.Ceiling((double)totalCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPages);

            int skipped = (pageNumber - 1) * pageSize;

            List<Category> categories = await categoryRepository
                .GetCategoriesByUserIdAsync(userId, pageSize, skipped);

            return new CategoryPageDto
            {
                Categories = categories
                    .Select(c => new CategoryListItemDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToList(),

                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> TryCreateAsync(string userId, CategoryCreateDto dto)
        {
            string name = dto.Name.Trim();

            if (await categoryRepository.ExistsByNameAsync(userId, name))
            {
                return false;
            }

            Category category = new Category()
            {
                UserId = userId,
                Name = name
            };

            await categoryRepository.AddAsync(category);

            return true;
        }

        public async Task<CategoryEditDto?> GetForEditAsync(string userId, int id)
        {
            Category? category = await categoryRepository.GetById(userId, id);

            if (category == null)
            {
                return null;
            }

            return new CategoryEditDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryEditResult> CategoryEditAsync(string userId, CategoryEditDto editDto)
        {
            Category? category = await categoryRepository.GetById(userId, editDto.Id);

            if (category == null)
            {
                return CategoryEditResult.NotFound;
            }

            string name = editDto.Name.Trim();

            bool nameAlreadyExists = await categoryRepository.ExistsByNameExceptId(userId, name, editDto.Id);

            if (nameAlreadyExists)
            {
                return CategoryEditResult.DuplicateName;
            }

            category.Name = name;

            await categoryRepository.UpdateAsync(category);

            return CategoryEditResult.Success;
        }

        public async Task<bool> TryDeleteAsync(string userId, int id)
        {
            Category? category = await categoryRepository.GetById(userId, id);

            if (category == null)
            {
                return false;
            }

            await categoryRepository.DeleteAsync(category);

            return true;
        }

        public async Task<CategoryDeleteDto?> GetForDeleteAsync(string userId, int id)
        {
            Category? category = await categoryRepository.GetById(userId, id);

            if (category == null)
            {
                return null;
            }

            return new CategoryDeleteDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
