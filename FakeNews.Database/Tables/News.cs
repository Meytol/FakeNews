using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Tables.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeNews.Database.Tables
{
    public class News : IDbTable
    {
        [Display(Name = "آدرس عکس اصلی")]
        public string MainImageUri { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "تیتر")]
        public string HeadLine { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        [Display(Name = "تاریخ انتشار")]
        public DateTime PublishDate { get; set; } = new DateTime();

        [Display(Name = "کلمات کلیدی")]
        public string Keywords { get; set; }

        [Display(Name = "وضعیت انتشار")]
        public bool IsPublished => PublishDate > DateTime.Now && IsDeleted == false;

        [Display(Name = "تعداد بازدید")]
        public int SeenCount { get; set; }

        [Display(Name = "شناسه نویسنده")]
        public int AuthorId { get; set; }

        [Display(Name = "نویسنده")]
        public User Author { get; set; }


        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
