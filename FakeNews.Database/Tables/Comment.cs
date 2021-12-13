using FakeNews.Common.Database.Enums;
using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Tables.Identity;
using System.Collections.Generic;

namespace FakeNews.Database.Tables
{
    public class Comment : IDbTable
    {
        public string Text { get; set; }
        public CommentStatus Status { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public News News { get; set; }
        public int NewsId { get; set; }

        public Comment ParentComment { get; set; }
        public int? ParentCommentId { get; set; }

        public ICollection<Comment> ChildComments { get; set; } = new List<Comment>();
    }
}
