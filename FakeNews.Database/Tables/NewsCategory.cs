using FakeNews.Common.Database.Interfaces;

namespace FakeNews.Database.Tables
{
    public class NewsCategory : IDbTable
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public News News { get; set; }
        public int NewsId { get; set; }
    }
}
