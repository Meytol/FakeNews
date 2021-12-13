using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Tables.Identity;
using System;
using System.Collections.Generic;

namespace FakeNews.Database.Tables
{
    public class News : IDbTable
    {
        public string MainImageUri { get; set; }
        public string Title { get; set; }
        public string HeadLine { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; } = new DateTime();
        public string Keywords { get; set; }
        public bool IsPublished => PublishDate > DateTime.Now && IsDeleted == false;
        public int SeenCount { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }


        public ICollection<NewsCategory> NewsCategories { get; set; } = new HashSet<NewsCategory>();
    }
}
