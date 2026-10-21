using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesByUserIdAsync(string userId, int take, int skip);

        Task<bool> ExistsByNameAsync(string userId, string name);

        Task AddAsync(Category category);

        Task<Category?> GetById(string userId, int id);

        Task<bool> ExistsByNameExceptId(string userId, string name, int excludeId);

        Task UpdateAsync(Category category);

        Task DeleteAsync(Category category);
    }
}
