
using FakeNews.Common.Database.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeNews.Database.Tables
{
    public class Category : IDbTable
    {
        [Display(Name = "عنوان انگلیسی")]
        public string TitleEn { get; set; }

        [Display(Name = "هنوان فارسی")]
        public string TitleFa { get; set; }
        [Display(Name = "شناسه دسته بندی والد")]
        public int? ParentCategoryId { get; set; }
        [Display(Name = "دسته بندی والد")]
        public Category? ParentCategory { get; set; }

        public ICollection<News> News { get; set; } = new HashSet<News>();
        public ICollection<Category>? ChildCategories { get; set; } = new HashSet<Category>();

    }
}
