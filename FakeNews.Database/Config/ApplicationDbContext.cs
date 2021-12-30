using FakeNews.Database.Tables;
using FakeNews.Database.Tables.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace FakeNews.Database.Config
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
#if DEBUG
        private const string ConnectionString = @"Data Source=185.55.224.117;Initial Catalog=mmhamze2_uniProjectDatabase;Persist Security Info=True;User ID=mmhamze2_uniProjectDbOwner;Password=Leomleom19(&";
#else
        private const string ConnectionString = @"Data Source=127.0.0.1;Initial Catalog=mmhamze2_uniProjectDatabase;Persist Security Info=True;User ID=mmhamze2_uniProjectDbOwner;Password=Leomleom19(&";
#endif

        public ApplicationDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            //optionsBuilder.UseInMemoryDatabase(databaseName: "FakeNewsDb");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>()
                .HasMany(ugc => ugc.ChildCategories)
                .WithOne(ugc => ugc.ParentCategory)
                .HasForeignKey(pc => pc.ParentCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }

        #region DbSet

        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<NewsUserSeen> NewsUserSeens { get; set; }

        #endregion

        public void Seed()
        {
            new SeedDataHelper().FeedSeedData(this);
            this.SaveChanges();
        }

    }
}
