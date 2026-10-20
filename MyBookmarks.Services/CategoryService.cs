using MyBookmarks.Common;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs;

namespace MyBookmarks.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryListItemDto>> GetUserCategoriesAsync(string userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.CategoriesPageSize;
            int skipped = (pageNumber - 1) * pageSize;

            List<Category> categories = await categoryRepository.GetCategoryByUserIdAsync(userId, pageSize, skipped);

            return categories
                .Select(c => new CategoryListItemDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
