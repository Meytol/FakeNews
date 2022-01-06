
using FakeNews.Common.Database.Interfaces;
using System.Collections.Generic;

namespace FakeNews.Database.Tables
{
    public class Category : IDbTable
    {
        public string TitleEn { get; set; }
        public string TitleFa { get; set; }

        public int? ParentCategoryId { get; set; }

        public ICollection<News> News { get; set; } = new HashSet<News>();
    }

    public class CategoryDto : Category
    {
        public CategoryDto ParentCategory { get; set; }


        public ICollection<CategoryDto> ChildCategories { get; set; } = new HashSet<CategoryDto>();

    }
}
