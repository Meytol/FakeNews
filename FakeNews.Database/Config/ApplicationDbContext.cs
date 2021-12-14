using FakeNews.Database.Tables;
using FakeNews.Database.Tables.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FakeNews.Database.Config
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
#if DEBUG
        private const string ConnectionString = @"Data Source=185.55.224.120;Initial Catalog=FakeNewsDb;Persist Security Info=True;User ID=mmhamze1_DbAdmin;Password=Leomleom19(&;";
#else
        private const string ConnectionString = @"Data Source=127.0.0.1;Initial Catalog=FakeNewsDb;Persist Security Info=True;User ID=mmhamze1_DbAdmin;Password=Leomleom19(&;";
#endif

        private const string SeedDataFileName = "ConnectionString";

        public ApplicationDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.UseInMemoryDatabase(databaseName: "FakeNewsDb");

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

            builder.Entity<Comment>()
                .HasMany(ugc => ugc.ChildComments)
                .WithOne(ugc => ugc.ParentComment)
                .HasForeignKey(pc => pc.ParentCommentId)
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
            FeedSeedData();
            base.SaveChanges();
        }

        #region override Seed changes

        public override int SaveChanges()
        {
            var rowsAffected = base.SaveChanges();
            GenerateSeedDataFile();
            return rowsAffected;
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var rowsAffected = base.SaveChanges(acceptAllChangesOnSuccess);
            GenerateSeedDataFile();
            return rowsAffected;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var rowsAffected = await base.SaveChangesAsync(cancellationToken);
            await GenerateSeedDataFileAsync();
            return rowsAffected;
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess
            , CancellationToken cancellationToken = default)
        {
            var rowsAffected = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await GenerateSeedDataFileAsync();
            return rowsAffected;
        }

        #endregion

        private void FeedSeedData()
        {
            var isDataFound = false;
            if (System.IO.File.Exists("SeedData.Json") is true)
            {
                var json = System.IO.File.ReadAllText("SeedData.Json");

                if (string.IsNullOrWhiteSpace(json))
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<FullDbSet>(json);
                    json = null;

                    Users.AddRange(data.Users);
                    Roles.AddRange(data.Roles);
                    UserRoles.AddRange(data.UserRoles);
                    Categories.AddRange(data.Categories);
                    News.AddRange(data.News);

                    data = null;
                    System.GC.Collect();
                    isDataFound = true;
                }
            }

            if (isDataFound is false)
            {
                Users.Add(new User()
                {
                    Email = "mohammadmahdi.hamzeh@yahoo.com",
                    UserName = "MM_Hamzeh",
                    CreatedOn = System.DateTime.Now,
                    IsDeleted = false,
                    LockoutEnabled = false,
                    PublicId = System.Guid.Parse("57D8F436-99E8-43A3-8751-8EFCD0B6B3AB"),
                    NormalizedEmail = "mohammadmahdi.hamzeh@yahoo.com".Normalize(),
                    NormalizedUserName = "MM_Hamzeh".Normalize(),
                    PasswordHash = "hash",
                    PhoneNumber = "09386114201",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    Id = 1,
                    ConcurrencyStamp = System.Guid.NewGuid().ToString(),
                    CreatorId = 1
                });
                Users.Add(new User()
                {
                    Email = "m.hamzeh2@test.com",
                    UserName = "meytol2",
                    CreatedOn = System.DateTime.Now,
                    IsDeleted = false,
                    LockoutEnabled = false,
                    PublicId = System.Guid.Parse("A0107FFA-FCB5-4F81-81C1-7709C34B9DA1"),
                    NormalizedEmail = "m.hamzeh2@test.com".Normalize(),
                    NormalizedUserName = "meytol2".Normalize(),
                    PasswordHash = "hash2",
                    PhoneNumber = "09386114202",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    Id = 2,
                    ConcurrencyStamp = System.Guid.NewGuid().ToString(),
                    CreatorId = 1
                });

                Roles.Add(new Role()
                {
                    Id = 1,
                    Name = "User",
                    PublicId = System.Guid.Parse("1E60F7C9-9825-4A82-B2A8-6D861864787D"),
                    CreatorId = 1,
                    IsDeleted = false,
                    NormalizedName = "User".Normalize()
                });
                Roles.Add(new Role()
                {
                    Id = 2,
                    Name = "Admin",
                    PublicId = System.Guid.Parse("1E60F7C9-9825-4A82-B2A8-6D861864787D"),
                    CreatorId = 1,
                    IsDeleted = false,
                    NormalizedName = "Admin".Normalize()
                });

                UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<int>()
                {
                    RoleId = 1,
                    UserId = 1
                });
                UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<int>()
                {
                    RoleId = 2,
                    UserId = 1
                });
                UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<int>()
                {
                    RoleId = 1,
                    UserId = 2
                });

                Categories.Add(new Category()
                {
                    Id = 1,
                    TitleEn = "AllArticles",
                    TitleFa = "همه نوشته  ها",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1
                });
                Categories.Add(new Category()
                {
                    Id = 2,
                    TitleEn = "Articles",
                    TitleFa = "اخبار و مقالات",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 1
                });
                Categories.Add(new Category()
                {
                    Id = 3,
                    TitleEn = "MobilePhone",
                    TitleFa = "گوشی موبایل",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });
                Categories.Add(new Category()
                {
                    Id = 4,
                    TitleEn = "MobilePhoneAccessory",
                    TitleFa = "لوازم جانبی گوشی موبایل",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 3
                });
                Categories.Add(new Category()
                {
                    Id = 5,
                    TitleEn = "Tablet",
                    TitleFa = "تبلت",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });
                Categories.Add(new Category()
                {
                    Id = 6,
                    TitleEn = "Laptop",
                    TitleFa = "لپ تاپ",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });
                Categories.Add(new Category()
                {
                    Id = 7,
                    TitleEn = "Desktop",
                    TitleFa = "دسک تاپ",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });
                Categories.Add(new Category()
                {
                    Id = 8,
                    TitleEn = "Motherboard",
                    TitleFa = "مادر بورد",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 7
                });
                Categories.Add(new Category()
                {
                    Id = 9,
                    TitleEn = "Cpu",
                    TitleFa = "پردازنده مرکزی",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 7
                });
                Categories.Add(new Category()
                {
                    Id = 10,
                    TitleEn = "Gpu",
                    TitleFa = "کارت گرافیک",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 7
                });
                Categories.Add(new Category()
                {
                    Id = 11,
                    TitleEn = "StorageDrive",
                    TitleFa = "هارد",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 7
                });
                Categories.Add(new Category()
                {
                    Id = 12,
                    TitleEn = "Review",
                    TitleFa = "برسی تخصصی",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 1
                });
                Categories.Add(new Category()
                {
                    Id = 13,
                    TitleEn = "MobilePhone",
                    TitleFa = "گوشی موبایل",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 12
                });
                Categories.Add(new Category()
                {
                    Id = 14,
                    TitleEn = "Tablet",
                    TitleFa = "تلبت",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 12
                });
                Categories.Add(new Category()
                {
                    Id = 15,
                    TitleEn = "laptop",
                    TitleFa = "لپ تاپ",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 12
                });
                Categories.Add(new Category()
                {
                    Id = 16,
                    TitleEn = "Desktop",
                    TitleFa = "دسک تاپ",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 12
                });
                Categories.Add(new Category()
                {
                    Id = 17,
                    TitleEn = "Other",
                    TitleFa = "متفرقه",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 1
                });
                Categories.Add(new Category()
                {
                    Id = 18,
                    TitleEn = "Advertisement",
                    TitleFa = "تبلیغات",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 17
                });
                Categories.Add(new Category()
                {
                    Id = 19,
                    TitleEn = "competition",
                    TitleFa = "مسابقه",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 17
                });

                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/s22noteteaser-960x540.png.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "گلکسی اس 22 نوت نام جدید اس 22 الترا خواهد بود؟",
                    Id = 1,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">


<p > در ابتدا تصور می‌شد نام پرچمدار آینده کمپانی سامسونگ Galaxy S22 Ultra باشد، حتی با وجود قلم S Pen همراه آن اما حالا یکی از افشاگران ادعا می‌کند که گلکسی اس 22 نوت(Galaxy S22 Note) نام جدید اس 22 الترا خواهد بود و این غول کُره‌ای برندی را انتخاب کرده که به شکل دقیق‌تری نمایانگر اسمارت فون آینده این کمپانی است.</ p >



<ul ><li ><a href = ""https://sakhtafzarmag.com/%d8%b2%d8%af-%d9%81%d9%84%db%8c%d9%be-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af-%d8%af%d8%b1-100-%d8%a7%d8%ae%d8%aa%d8%b1%d8%a7%d8%b9-%d8%a8%d8%b1%d8%aa%d8%b1-%d8%aa%d8%a7%db%8c%d9%85/"" > گلکسی زد فلیپ سامسونگ در میان 100 اختراع برتر مجله تایم </ a ></ li ><li ><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%af%d9%88%d8%b4%db%8c-%da%af%d9%84%da%a9%d8%b3%db%8c-a22-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"" > تماشا کنید: بررسی گوشی گلکسی A22 سامسونگ </ a ></ li ><li ><a href = ""https://sakhtafzarmag.com/%d8%a7%d9%86%d8%aa%d9%82%d8%a7%d9%84-%d8%b3%d9%81%d8%a7%d8%b1%d8%b4%d8%a7%d8%aa-%da%a9%d9%88%d8%a7%d9%84%da%a9%d8%a7%d9%85-%d9%88-amd-%d8%a8%d9%87-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"" > انتقال سفارشات کوالکام و AMD به سمت سامسونگ – کاهش وابستگی به TSMC </ a ></ li ></ ul >



<p > این اسمارت فون بالا رده اولین نمونه در سری خود خواهد بود که از قلم داخلی S Pen بهره می‌برد تا به نوعی جایگزینی برای سری لغو شده Galaxy Note باشد.همانطور که اعلام کردیم ظاهرا سامسونگ پسوند Ultra را با Note جایگزین کرده که از نظر ما ایده بهتری خواهد بود.</ p >



<h2 id = ""h-22-22"" > گلکسی اس 22 نوت نام جدید اس 22 الترا خواهد بود؟</ h2 >



<p > افشاگر معروف <a href = ""https://twitter.com/FrontTron/status/1469119157124087808"" target = ""_blank"" rel = ""noreferrer noopener"" > Tron </ a > (یا FrontTron در توییتر) از این تغییر برند خبر داده است.با توجه به گفته او Galaxy S22 Ultra یا حداقل یکی از نمونه‌های آن با نام Galaxy S22 Note به بازار عرضه می‌شود.با توجه به طراحی افشا شده و استایل شبیه به اسمارت فون‌های نوت این شرکت به همراه به روز رسانی‌های مورد انتظار برای سری آینده اس، نام جدید منطقی به نظر می‌رسد. در واقع این دستگاه احتمالا برای کاربران حرفه‌ای یا تجاری جذاب خواهد بود اما باید صبر کنیم و ببینیم که آیا واقعا چنین شایعه‌ای حقیقت دارد یا خیر.</ p >
             



             <p > اسمارت فون مذکور(S22 Ultra یا S22 Note) در بنچمارک Geekbench با شماره مدل SM - S908B ماه گذشته با سیستم روی چیپ Exynos 2200 و 8 گیگابایت رم مشاهده شده بود اما حالا نسخه‌ای دیگر با چیپست Snapdragon 8 Gen 1 در دیتابیس همین بنچمارک با شماره مدل‌های SM-S908N و SM-S908U دیده شده است.شماره مدل اول احتمالا متعلق به کشور کُره جنوبی خواهد بود در حالی که شماره مدل دوم احتمالا نسخه‌‌ای برای ایالات متحده.</ p >
                



                <p > SM - S908N و SM-S908U از اندروید 12 بهره می‌برند، نسخه اول از 10 گیگابایت حافظه رم و نسخه دوم نیز از 8 گیگابایت حافظه رم استفاده می‌کنند.یکی از افشاگران اخیرا ادعا کرده بود که S22 Ultra در دو نسخه با حافظه‌های 12 و 16 گیگابایتی ارائه خواهد شد اما باید برای تایید این موضوع کمی صبر کنیم.</ p >
                   



                   <div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""956x900"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1.jpg.webp"" loading=""lazy"" width=""956"" height=""900"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1.jpg.webp"" alt=""بنچمارک S22 Ultra یا S22 Note"" class=""wp-image-252272 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1.jpg.webp 956w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1-400x377.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1-768x723.jpg.webp 768w"" data-sizes=""(max-width: 956px) 100vw, 956px"" sizes=""(max-width: 956px) 100vw, 956px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1.jpg.webp 956w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1-400x377.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1-768x723.jpg.webp 768w"" data-was-processed=""true""></figure></div>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""708x900"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2.jpg.webp"" loading=""lazy"" width=""708"" height=""900"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2.jpg.webp"" alt=""بنچمارک S22 Ultra یا S22 Note"" class=""wp-image-252273 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2.jpg.webp 708w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2-315x400.jpg.webp 315w"" data-sizes=""(max-width: 708px) 100vw, 708px"" sizes=""(max-width: 708px) 100vw, 708px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2.jpg.webp 708w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2-315x400.jpg.webp 315w"" data-was-processed=""true""></figure></div>



<p>در مقایسه با نسخه Exynos 2200، مدل Snapdragon 8 Gen 1 عملکرد بهتری در تک هسته با امتیاز 1219 داشته در حالی که نسخه اگزینوس تنها امتیاز 691 را کسب کرده بود.با این حال در کارایی چند هسته تفاوت زیادی بین دو نسخه مشاهده نمی‌شود، 3167 امتیاز برای اگزینوس و 3154 امتیاز برای اسنپدراگون.همچنین باید اشاره کنیم که هر دو مدل در حال حاضر نمونه اولیه هستند و برای تست نسخه‌های نهایی باید تا زمان عرضه صبر کنیم.</p>



<p>هر چند که سامسونگ هنوز هیچ صحبتی از سری Galaxy S22 به میان نیاورده اما شایعات به رونمایی احتمالی در تاریخ 8 فوریه (19 بهمن) 2022 در رویداد Galaxy Unpacked اشاره دارند در حالی که 10 روز پس از آن عرضه رسمی اتفاق می‌افتد.البته پیش فروش این سری پس از مراسم مذکور شروع خواهد شد.</p>

												
												
												
											
										</div>",
                    Title = "گلکسی اس 22 نوت نام جدید اس 22 الترا – به علاوه بنچمارک Geekbench",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 3
                });
            }
        }

        private void GenerateSeedDataFile()
        {
            var tables = new FullDbSet(this);
            var jsonSerializerSetting = new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
            var dbContextJson = Newtonsoft.Json.JsonConvert.SerializeObject(tables, jsonSerializerSetting);

            if (System.IO.File.Exists("SeedData.Json"))
            {
                System.IO.File.Delete("SeedData.Json");
            }

            System.IO.File.WriteAllText("SeedData.Json", dbContextJson, System.Text.Encoding.UTF8);
        }
        private async Task GenerateSeedDataFileAsync()
        {
            var tables = new FullDbSet(this);
            var jsonSerializerSetting = new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
            var dbContextJson = Newtonsoft.Json.JsonConvert.SerializeObject(tables, jsonSerializerSetting);
            if (System.IO.File.Exists("SeedData.Json"))
            {
                System.IO.File.Delete("SeedData.Json");
            }

            await System.IO.File.WriteAllTextAsync("SeedData.Json", dbContextJson, System.Text.Encoding.UTF8);
        }

        

    }
}
