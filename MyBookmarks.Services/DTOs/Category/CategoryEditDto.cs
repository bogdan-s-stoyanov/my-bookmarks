using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookmarks.Services.DTOs.Category
{
    public class CategoryEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
