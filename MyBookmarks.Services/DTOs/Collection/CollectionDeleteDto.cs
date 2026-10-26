using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Collection
{
    public class CollectionDeleteDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}