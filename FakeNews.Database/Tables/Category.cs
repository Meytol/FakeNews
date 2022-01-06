
using FakeNews.Common.Database.Interfaces;
using System.Collections.Generic;

namespace FakeNews.Database.Tables
{
    public class Category : IDbTable
    {
        public string TitleEn { get; set; }
        public string TitleFa { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<News> News { get; set; } = new HashSet<News>();
        public ICollection<Category>? ChildCategories { get; set; } = new HashSet<Category>();

    }
}
