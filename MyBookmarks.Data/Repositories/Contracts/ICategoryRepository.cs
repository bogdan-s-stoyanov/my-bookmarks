using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoryByUserIdAsync(string userId, int take, int skip);

        Task<bool> ExistsByNameAsync(string userId, string name);

        Task AddCategoryAsync(Category category);
    }
}
