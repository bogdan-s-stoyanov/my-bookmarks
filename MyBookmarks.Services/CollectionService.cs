using MyBookmarks.Common;
using MyBookmarks.Data.Models;
using MyBookmarks.Data.Repositories.Contracts;
using MyBookmarks.Services.Contracts;
using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository collectionRepository;

        public CollectionService(ICollectionRepository collectionRepository)
        {
            this.collectionRepository = collectionRepository;
        }

        public async Task<PagedResultDto<CollectionListItemDto>> 
            GetUserCollectionsAsync(string userId, int pageNumber)
        {
            int pageSize = PageConstants.CollectionsPageSize;

            int totalCount = await collectionRepository
                .CountCollectionsForUserAsync(userId);

            int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            pageNumber = Math.Clamp(pageNumber, 1, totalPages);

            int skip = (pageNumber - 1) * pageSize;

            List<Collection> collections = await collectionRepository
                .GetCollectionsByUserIdAsync(userId, pageSize, skip);

            return new PagedResultDto<CollectionListItemDto>
            {
                Items = collections
                    .Select(c => new CollectionListItemDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToList(),

                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> TryCreateAsync(string userId, CollectionCreateDto dto)
        {
            string name = dto.Name.Trim();

            bool alreadyExists = await this.collectionRepository
                .ExistsByNameAsync(userId, name);

            if (alreadyExists)
            {
                return false;
            }

            Collection collection = new Collection
            {
                Name = name,
                Description = dto.Description?.Trim(),
                UserId = userId
            };

            await collectionRepository.AddAsync(collection);

            return true;
        }
    }
}
