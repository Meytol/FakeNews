using FakeNews.Database.Tables;
using FakeNews.Database.Tables.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FakeNews.Database.Config
{
    internal sealed class FullDbSet
    {
        public FullDbSet()
        {

        }

        public FullDbSet(ApplicationDbContext applicationDbContext)
        {
            News = applicationDbContext.News.ToListAsync().Result ?? new List<News>();
            Categories = applicationDbContext.Categories.ToListAsync().Result ?? new List<Category>();
            Comments = applicationDbContext.Comments.ToListAsync().Result ?? new List<Comment>();
            Users = applicationDbContext.Users.ToListAsync().Result ?? new List<User>();
            Roles = applicationDbContext.Roles.ToListAsync().Result ?? new List<Role>();
            Roles = applicationDbContext.Roles.ToListAsync().Result ?? new List<Role>();
            UserRoles = applicationDbContext.UserRoles.ToListAsync().Result ?? new List<IdentityUserRole<int>>();
            NewsUserSeens = applicationDbContext.NewsUserSeens.ToListAsync().Result ?? new List<NewsUserSeen>();

            foreach (var comment in Comments)
            {
                comment.News = null;
            }
            foreach (var newsUserSeen in NewsUserSeens)
            {
                newsUserSeen.News = null;
                newsUserSeen.Viewer = null;
            }
        }

        public IList<News> News { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<NewsUserSeen> NewsUserSeens { get; set; }
        public IList<User> Users { get; set; }
        public IList<Role> Roles { get; set; }
        public IList<IdentityUserRole<int>> UserRoles { get; set; }
    }
}
