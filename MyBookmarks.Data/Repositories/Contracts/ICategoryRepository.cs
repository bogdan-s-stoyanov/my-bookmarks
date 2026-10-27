using MyBookmarks.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Data.Repositories.Contracts
{
    /// <summary>
    /// Defines database operations for managing categories.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Retrieves a page of the user's categories, ordered by name and then by ID.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="take">The maximum number of categories to return.</param>
        /// <param name="skip">The number of categories to skip.</param>
        /// <returns>The requested page of categories, or an empty list if none match.</returns>
        Task<List<Category>> GetCategoriesByUserIdAsync(string userId, int take, int skip);

        /// <summary>
        /// Checks whether the user has a category with the specified name.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="name">The category name to check.</param>
        /// <returns>
        /// True if a matching category exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByNameAsync(string userId, string name);

        /// <summary>
        /// Adds a category and saves the changes to the database.
        /// </summary>
        /// <param name="category">The category to add.</param>
        Task AddAsync(Category category);

        /// <summary>
        /// Retrieves a category by ID that belongs to the specified user.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="id">The category ID.</param>
        /// <returns>
        /// The category, or null if it does not exist or belongs to another user.
        /// </returns>
        Task<Category?> GetById(string userId, int id);

        /// <summary>
        /// Checks whether the user has another category with the specified name.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <param name="name">The category name to check.</param>
        /// <param name="excludeId">The ID of the category to exclude from the check.</param>
        /// <returns>
        /// True if another matching category exists; otherwise, false.
        /// </returns>
        Task<bool> ExistsByNameExceptId(string userId, string name, int excludeId);

        /// <summary>
        /// Updates a category and saves the changes to the database.
        /// </summary>
        /// <param name="category">The category containing the updated data.</param>
        Task UpdateAsync(Category category);

        /// <summary>
        /// Deletes a category and saves the changes to the database.
        /// </summary>
        /// <param name="category">The category to delete.</param>
        Task DeleteAsync(Category category);

        /// <summary>
        /// Counts the categories owned by the specified user.
        /// </summary>
        /// <param name="userId">The category owner's ID.</param>
        /// <returns>
        /// The number of categories owned by the user.
        /// </returns>
        Task<int> CountCategoriesForUserAsync(string userId);
    }
}
