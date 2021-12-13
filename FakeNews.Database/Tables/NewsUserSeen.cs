﻿using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Tables.Identity;

namespace FakeNews.Database.Tables
{
    public class NewsUserSeen : IDbTable
    {
        public int Count { get; set; }

        public User Viewer { get; set; } = new User();
        public int ViewerId { get; set; }

        public News News { get; set; } = new News();
        public int NewsId { get; set; }
    }
}
