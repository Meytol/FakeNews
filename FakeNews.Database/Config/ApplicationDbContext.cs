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

        private const string ConnectionString = @"Data Source=localhost;Initial Catalog=mmhamze2_uniProjectDatabase;Integrated Security=True;Pooling=False";

        //private const string ConnectionString = @"Data Source=185.55.224.117;Initial Catalog=mmhamze2_uniProjectDatabase;Persist Security Info=True;User ID=mmhamze2_uniProjectDbOwner;Password=Leomleom19(&";
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
            builder.Entity<Comment>()
                .HasOne(e => e.News)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.NewsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<News>()
                .HasOne(e => e.Category)
                .WithMany(e => e.News)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Seed();

            base.OnModelCreating(builder);

        }

        #region DbSet

        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        #endregion

    }
}
