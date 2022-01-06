using FakeNews.Common.Database.Enums;
using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Tables.Identity;
using System.Collections.Generic;

namespace FakeNews.Database.Tables
{
    public class Comment : IDbTable
    {
        public string Text { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Accepted;

        public string SenderName { get; set; }
        public string SenderMail { get; set; }

        public News News { get; set; }
        public int NewsId { get; set; }

    }
}
