using MyBookmarks.Services.DTOs.Collection;
using MyBookmarks.Services.DTOs.Pagination;
using MyBookmarks.Services.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.Contracts
{
    public interface ICollectionService
    {
        Task<PagedResultDto<CollectionListItemDto>>
            GetUserCollectionsAsync(string userId, int pageNumber);

        Task<bool> TryCreateAsync(string userId, CollectionCreateDto dto);

        Task<CollectionEditDto?> GetForEditAsync(string userId, int id);

        Task<CollectionEditResult> EditAsync(string userId, CollectionEditDto dto);
    }
}
