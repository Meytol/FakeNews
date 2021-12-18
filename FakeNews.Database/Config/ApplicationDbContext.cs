using FakeNews.Database.Tables;
using FakeNews.Database.Tables.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
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
            //if (System.IO.File.Exists("SeedData.Json") is true)
            //{
            //    var json = System.IO.File.ReadAllText("SeedData.Json");

            //    if (string.IsNullOrWhiteSpace(json))
            //    {
            //        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<FullDbSet>(json);
            //        json = null;

            //        Users.AddRange(data.Users);
            //        Roles.AddRange(data.Roles);
            //        UserRoles.AddRange(data.UserRoles);
            //        Categories.AddRange(data.Categories);
            //        News.AddRange(data.News);

            //        data = null;
            //        System.GC.Collect();
            //        isDataFound = true;
            //    }
            //}

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

                //اخبار و مقالات
                News.Add(new Tables.News()
                {
                    Id = 1,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "سامسونگ,تلفن هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/s22noteteaser-960x540.png.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "گلکسی اس 22 نوت نام جدید اس 22 الترا خواهد بود؟",
                    Title = "گلکسی اس 22 نوت نام جدید اس 22 الترا – به علاوه بنچمارک Geekbench",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 3,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>در ابتدا تصور می‌شد نام پرچمدار آینده کمپانی سامسونگ Galaxy S22 Ultra باشد، حتی با وجود قلم S Pen همراه آن اما حالا یکی از افشاگران ادعا می‌کند که گلکسی اس 22 نوت (Galaxy S22 Note) نام جدید اس 22 الترا خواهد بود و این غول کُره‌ای برندی را انتخاب کرده که به شکل دقیق‌تری نمایانگر اسمارت فون آینده این کمپانی است.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%b2%d8%af-%d9%81%d9%84%db%8c%d9%be-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af-%d8%af%d8%b1-100-%d8%a7%d8%ae%d8%aa%d8%b1%d8%a7%d8%b9-%d8%a8%d8%b1%d8%aa%d8%b1-%d8%aa%d8%a7%db%8c%d9%85/"">گلکسی زد فلیپ سامسونگ در میان 100 اختراع برتر مجله تایم</a></li><li><a href=""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%af%d9%88%d8%b4%db%8c-%da%af%d9%84%da%a9%d8%b3%db%8c-a22-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"">تماشا کنید: بررسی گوشی گلکسی A22 سامسونگ</a></li><li><a href=""https://sakhtafzarmag.com/%d8%a7%d9%86%d8%aa%d9%82%d8%a7%d9%84-%d8%b3%d9%81%d8%a7%d8%b1%d8%b4%d8%a7%d8%aa-%da%a9%d9%88%d8%a7%d9%84%da%a9%d8%a7%d9%85-%d9%88-amd-%d8%a8%d9%87-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"">انتقال سفارشات کوالکام و AMD به سمت سامسونگ – کاهش وابستگی به TSMC</a></li></ul>



<p>این اسمارت فون بالا رده اولین نمونه در سری خود خواهد بود که از قلم داخلی S Pen بهره می‌برد تا به نوعی جایگزینی برای سری لغو شده Galaxy Note باشد. همانطور که اعلام کردیم ظاهرا سامسونگ پسوند Ultra را با Note جایگزین کرده که از نظر ما ایده بهتری خواهد بود.</p>



<h2 id=""h-22-22"">گلکسی اس 22 نوت نام جدید اس 22 الترا خواهد بود؟</h2>



<p>افشاگر معروف <a href=""https://twitter.com/FrontTron/status/1469119157124087808"" target=""_blank"" rel=""noreferrer noopener"">Tron</a> (یا FrontTron در توییتر) از این تغییر برند خبر داده است. با توجه به گفته او Galaxy S22 Ultra یا حداقل یکی از نمونه‌های آن با نام Galaxy S22 Note به بازار عرضه می‌شود. با توجه به طراحی افشا شده و استایل شبیه به اسمارت فون‌های نوت این شرکت به همراه به روز رسانی‌های مورد انتظار برای سری آینده اس، نام جدید منطقی به نظر می‌رسد. در واقع این دستگاه احتمالا برای کاربران حرفه‌ای یا تجاری جذاب خواهد بود اما باید صبر کنیم و ببینیم که آیا واقعا چنین شایعه‌ای حقیقت دارد یا خیر.</p>



<p>اسمارت فون مذکور (S22 Ultra یا S22 Note) در بنچمارک Geekbench با شماره مدل SM-S908B ماه گذشته با سیستم روی چیپ Exynos 2200 و 8 گیگابایت رم مشاهده شده بود اما حالا نسخه‌ای دیگر با چیپست Snapdragon 8 Gen 1 در دیتابیس همین بنچمارک با شماره مدل‌های SM-S908N و SM-S908U دیده شده است. شماره مدل اول احتمالا متعلق به کشور کُره جنوبی خواهد بود در حالی که شماره مدل دوم احتمالا نسخه‌‌ای برای ایالات متحده.</p>



<p> SM-S908N و SM-S908U از اندروید 12 بهره می‌برند، نسخه اول از 10 گیگابایت حافظه رم و نسخه دوم نیز از 8 گیگابایت حافظه رم استفاده می‌کنند. یکی از افشاگران اخیرا ادعا کرده بود که S22 Ultra در دو نسخه با حافظه‌های 12 و 16 گیگابایتی ارائه خواهد شد اما باید برای تایید این موضوع کمی صبر کنیم.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""956x900"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI5NTYiIGhlaWdodD0iOTAwIiB2aWV3Qm94PSIwIDAgOTU2IDkwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""956"" height=""900"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1.jpg.webp"" alt=""بنچمارک S22 Ultra یا S22 Note"" class=""wp-image-252272"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1.jpg.webp 956w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1-400x377.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-1-768x723.jpg.webp 768w"" data-sizes=""(max-width: 956px) 100vw, 956px""></figure></div>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""708x900"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI3MDgiIGhlaWdodD0iOTAwIiB2aWV3Qm94PSIwIDAgNzA4IDkwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""708"" height=""900"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2.jpg.webp"" alt=""بنچمارک S22 Ultra یا S22 Note"" class=""wp-image-252273"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2.jpg.webp 708w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/S22-Ultra-or-Note-Geekbench-Test-2-315x400.jpg.webp 315w"" data-sizes=""(max-width: 708px) 100vw, 708px""></figure></div>



<p>در مقایسه با نسخه Exynos 2200، مدل  Snapdragon 8 Gen 1 عملکرد بهتری در تک هسته با امتیاز 1219 داشته در حالی که نسخه اگزینوس تنها امتیاز 691 را کسب کرده بود. با این حال در کارایی چند هسته تفاوت زیادی بین دو نسخه مشاهده نمی‌شود، 3167 امتیاز برای اگزینوس و 3154 امتیاز برای اسنپدراگون. همچنین باید اشاره کنیم که هر دو مدل در حال حاضر نمونه اولیه هستند و برای تست نسخه‌های نهایی باید تا زمان عرضه صبر کنیم.</p>



<p>هر چند که سامسونگ هنوز هیچ صحبتی از سری Galaxy S22 به میان نیاورده اما شایعات به رونمایی احتمالی در تاریخ 8 فوریه (19 بهمن) 2022 در رویداد Galaxy Unpacked اشاره دارند در حالی که 10 روز پس از آن عرضه رسمی اتفاق می‌افتد. البته پیش فروش این سری پس از مراسم مذکور شروع خواهد شد.</p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 2,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "اپو,تلفن هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/OPPO-shows-the-unique-camera-feature-you-want-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "فناوری دوربین جمع شونده اوپو چگونه عمل می‌کند؟",
                    Title = "تماشا کنید: با دوربین جمع شونده اوپو مشکل برآمدگی دوربین‌های گوشی رفع می‌شود",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 3,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>باید قبول کنیم که یکی از ایرادات طراحی چند سال اخیر گوشی‌های هوشمند طراحی برآمدگی دوربین‌ها در قاب پشتی آنها است. اما ظاهرا این مشکل قرار است با فناوری جدید شرکت Oppo برطرف شود. اوپو در اولین روز از رویداد Inno Day 2021 در کنار رونمایی از عینک هوشمند ایر گلس از نمونه اولیه دوربین جمع شونده خود نیز رونمایی کرد.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%b9%db%8c%d9%86%da%a9-oppo-air-glass/"">عینک هوشمند اوپو با نام Oppo Air Glass رونمایی شد – آینده بر روی چشمانتان</a></li></ul>



<h2 id=""h-"">فناوری دوربین جمع شونده اوپو چگونه عمل می‌کند؟</h2>



<p>در ویدئویی که در ادامه مطلب منتشر شده، مشاهده خواهید کرد که اوپو به پیشرفت بزرگی در این زمینه دست پیدا کرده و می‎تواند سنسور بزرگ 1/1.56 اینچی 50 مگاپیکسلی IMX766 سونی را در گوشی به ضخامت 8.26 میلیمتر جای دهد. یکی از نکات منفی این فناوری اما اینست که تنها زمانی که دوربین جمع شود در دسترس است. بنابراین در موقعیت ثابت کاربردی ندارد.</p>



<figure class=""wp-block-video""><video controls="""" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Oppo-retractable-camera-prototype-showcased-at-Inno-Day-2021-GSMArena.com-news.mp4"" __idm_id__=""1875533825""></video></figure>



<p>در رابطه با استفاده از سنسور متحرک، این امکان برای فاصله بیشتر میان سنسور و لنز واقعی فراهم می‌شود. این ماژول معادل 50 میلیمتری، زوم اپتیکال 2x را ارائه می‌کند که در مقایسه با یک ماژول تله فوتو معمولی، طبیعتا جزئیات بیشتری را توسط یک سنسور پرچمدار ثبت می‌کند. یکی دیگر از مزایای این دوربین جمع شوند اینست که عکس‌های پرتره ظاهر طبیعی‌تر داشته و جداسازی سوژه و مو را بهتر ارائه می‌کند.</p>



<figure class=""wp-block-video""><video controls="""" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Oppo-retractable-camera-prototype-showcased-at-Inno-Day-2021-GSMArena.com-news_2.mp4"" __idm_id__=""1875533826""></video></figure>



<p>اوپو همچنین بر روی نسبت سیگنال به نویز برتر خود در عکس‌های زوم شده نیز مانور خوبی می‌دهد و ادعا می‌کند که به خصوص در تصاویر شب عملکرد موثرتری دارد. کلیه تجهیزات و قطعات متحرک در این نمونه اولیه دوربین در برابر پاشش آب و گرد و غبار مقاوم هستند و در صورتیکه خطر افتادن حس شود در 0.6 ثانیه جمع خواهد شد.</p>



<p>اوپو اعلام کرده که قصد دارد فناوری جدید خود را به محصولات آینده‌اش اضافه کند، با این حال هنوز هیچ چارچوب زمانی برای آن تعیین نکرده و نمی‌دانیم که این اتفاق چه زمانی رخ خواهد داد.</p>



<p>نظر شما در رابطه با فناوری جدید و نوآورانه اوپو چیست؟ آیا از دیدگاه شما هم این طراحی کاربردی است و یا مانند دوربین سلفی‌های بالا جهنده به جایگاه خاصی در صنعت موبایل نخواهند رسید؟</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%b9%db%8c%d9%86%da%a9-%d9%87%d9%88%d8%b4%d9%85%d9%86%d8%af-anker/""></a><a href=""https://sakhtafzarmag.com/%d9%85%d8%b9%d8%b1%d9%81%db%8c-%d9%86%d8%b3%d8%ae%d9%87-%d9%85%d8%b1%db%8c%d8%ae%db%8c-find-x3-pro-%d8%a7%d9%88%d9%be%d9%88/"">نسخه مریخی Find X3 Pro همزمان با فرود کاوشگر چینی بر مریخ معرفی شد</a></li><li><a href=""https://sakhtafzarmag.com/%d9%be%d8%b1%d9%88%da%98%d9%87-%d8%ae%d9%88%d8%a7%d9%86%d8%af%d9%86-%d9%85%d8%ba%d8%b2-%d8%a8%d8%a7-%d8%b9%db%8c%d9%86%da%a9-%d9%87%d8%a7%db%8c-%d9%88%d8%a7%d9%82%d8%b9%db%8c%d8%aa-%d8%a7%d9%81%d8%b2%d9%88%d8%af%d9%87/""></a><a href=""https://sakhtafzarmag.com/%d8%b2%d9%85%d8%a7%d9%86-%d9%85%d8%b9%d8%b1%d9%81%db%8c-%d8%a7%d9%88%d9%84%db%8c%d9%86-%da%af%d9%88%d8%b4%db%8c-%d8%aa%d8%a7%d8%b4%d9%88-%d8%a7%d9%88%d9%be%d9%88/"">Find N؛ اولین گوشی تاشو اوپو 24 آذرماه از راه می‌رسد</a></li><li><a href=""https://sakhtafzarmag.com/%d8%af%d8%b3%d8%aa%d8%a7%d9%88%d8%b1%d8%af%d9%87%d8%a7%db%8c-%d8%a7%d9%be%d8%aa%db%8c%da%a9%db%8c-%d8%b4%d8%b1%da%a9%d8%aa-%d8%a7%d9%88%d9%be%d9%88/"">دستاوردهای اپتیکی شرکت اوپو برای گوشی‌های هوشمند معرفی شدند</a></li></ul>



<p></p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 3,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "شیائومی,تلفن هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/gsmarena_001-960x540.jpg",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "جزئیات تکنولوژی جدید باتری شیائومی",
                    Title = "تکنولوژی جدید باتری شیائومی رونمایی شد – 10 درصد ظرفیت بیشتر",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 4,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>شاید بتوان گفت که بزرگترین معضل حال حاضر دنیا در زمینه‌ی دستگاه‌های الکتریکی باتری‌هایشان است. تمامی شرکت‌های تکنولوژيک هم به نوعی در این مسیر قدم نهاده تا شاید نتیجه‌ای حاصل شود. تکنولوژی جدید باتری شیائومی هم حاصل همین تلاش‌هاست که ما را به آینده‌ امیدوارتر می‌کند.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%da%af%d9%88%d8%b4%db%8c-%d9%87%d8%a7%db%8c-%d8%b4%db%8c%d8%a7%d8%a6%d9%88%d9%85%db%8c-%d8%a2%d9%be%d8%af%db%8c%d8%aa-miui-13/"">کدام گوشی های شیائومی آپدیت MIUI 13 را دریافت می‌کنند؟ </a></li><li><a href=""https://sakhtafzarmag.com/%d8%a7%d8%b3%d8%aa%d9%81%d8%a7%d8%af%d9%87-%da%a9%d8%a7%d8%b1%d9%85%d9%86%d8%af%d8%a7%d9%86-%d8%b4%db%8c%d8%a7%d8%a6%d9%88%d9%85%db%8c-%d8%a7%d8%b2-%d8%a2%db%8c%d9%81%d9%88%d9%86/"">آیا واقعا بیشتر کارمندان شیائومی از آیفون استفاده می‌کنند؟ شیائومی پاسخ می‌دهد </a></li></ul>



<h2 id=""h-"">جزئیات تکنولوژی جدید باتری شیائومی </h2>



<p>آنطور که از گزارش وبسایت Weibo پیداست، مقدمات تکنولوژی باتری جدید شرکت چینی شیائومی از چند سال پیش مهیا شده و حاصل تلاش صدها مهندس سخت افزار و شیمی است که برای استفاده در گوشی‌های سال آینده‌ی این شرکت آماده شده است. هنوز از ترکیبات شیمیایی به کار رفته در این باتری خبری منتشر نشده اما آنطور که پیداست محتوای سیلیکونی درون این باتری 3 برابر بیشتر از دیگر باتری‌های بازار است. شاید دلیل راندمان بهتر و عمر طولانی‌تر این باتری به نسبت نسل قبلی خود نیز همین موضوع باشد.</p>



<p>شرکت چینی شیائومی ادعا کرده که اگر این باتری در اندازه‌های باتری نسل قبل خود قرار بگیرد، 10 درصد ظرفیت بیشتری خواهد داشت که همین موضوع می‌تواند عمر دستگاه شما را 100 دقیقه اضافه کند. از طرف دیگر اندازه‌ی این باتری جدید نیز به نسبت مدل قبلی کوچک‌تر شده که همین موضوع باعث می‌شود که فضای کمتری در درون گوشی‌های این شرکت اشغال کند. ماژول PCM این باتری نیز با چرخشی 90 درجه‌ای همراه بوده و دیگر مسطح نیست که همین موضوع نیز به اندازه‌ی کمتر این باتری منجر شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x580"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-1160x580.jpg.webp"" loading=""lazy"" width=""1160"" height=""580"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-1160x580.jpg.webp"" alt=""تکنولوژی جدید باتری شیائومی رونمایی شد - 10 درصد ظرفیت بیشتر  ۲"" class=""wp-image-252211 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-1160x580.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-400x200.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-768x384.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging.jpg 1400w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-1160x580.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-400x200.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging-768x384.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Battery-Charging.jpg 1400w"" data-was-processed=""true""></figure></div>



<p>شیائومی یک چیپست ظرفیت نیز به باتری‌های خود اضافه کرده تا روند شارژ شدنش بهبود پیدا کند و از طرفی قابلیت تحت نظر گرفتن عملکرد این باتری بیش از پیش آسان گردند. برای مثال وقتی گوشی شما بیش از اندازه به کابل شارژ متصل مانده باشد، اخطاری برای کندن از کابل شارژ به شما ارسال خواهد شد و یا زمانی که باتری بیش از حد گرم شده باشد (که اکثرا دلیل این موضوع به خاطر همان اتصال بیش از اندازه به کابل شارژ است) میزان ولتاژ ورودی و توان آن تغییر پیدا خواهد کرد تا به باتری آسیبی نرسد.</p>



<p>همانطور که در ابتدای مطلب نیز به آن اشاره کردیم، شیائومی از نیمه‌ی اول سال 2022 تولید انبوه این باتری را آغاز خواهد کرد.</p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 4,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "اپل,ساعت هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/gsmarena_003-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "ادعای شاکیان: باتری اپل واچ می‌تواند باعث شکستن صفحه نمایش شود!",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 4,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>در جدیدترین شکایت گروهی از مشتریان ساعت‌های مچی هوشمند Apple به نام Apple Watch ادعا شده است، باتری اپل واچ سری 6 می‌تواند باعث شکستن صفحه نمایش و حتی آسیب به کاربران باشد. در این شکایت دسته جمعی، دلیل چنین مشکلی، ایراد در طراحی ساعت مچی اپل واچ سری 6 ذکر شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1.jpg"" data-slb-active=""1"" data-slb-asset=""1370418854"" data-slb-internal=""0"" data-slb-group=""252070""><img data-lazyloaded=""1"" data-placeholder-resp=""590x332"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-1160x653.jpg.webp"" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-1160x653.jpg.webp"" alt=""باتری Apple Watch"" class=""wp-image-252077 litespeed-loaded"" width=""590"" height=""332"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1.jpg.webp 1600w"" data-sizes=""(max-width: 590px) 100vw, 590px"" sizes=""(max-width: 590px) 100vw, 590px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw1.jpg.webp 1600w"" data-was-processed=""true""></a></figure></div>



<p>از قدیم گفته‌اند هر که بامش بیش، برفش بیشتر! می‌خواهم بگویم هر چه شرکت بزرگتری داشته باشید چالش‌های بزرگتری هم خواهید داشت. یکی از بزرگترین چالش‌های برای Apple شاید همین موضوع شکایت‌های پی در پی بابت ریزترین ایرادات باشد. از طرفی معروفیت شرکت‌ Apple هم می‌تواند باعث گسترش اخبار در مورد آن شود.</p>



<p>شاکیان جدید اپل، گفته‌اند به دلیل در نظر نگرفتن فضای کافی برای تورم باتری اپل واچ ، باد کردن باتری می‌تواند با وارد کردن فشار به صفحه نمایش، موجب شکستن یا خرابی صفحه و آسیب جسمی به کاربر شود. طراحی اپل واچ برای باریک کردن و ضدآب کردن آن، به گونه‌ای است که محفظه باتری، دقیقاً به اندازه خود باتری طراحی شده است و با متورم شدن باتری به صفحه نمایش فشار وارد می‌شود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2.jpg"" data-slb-active=""1"" data-slb-asset=""155994303"" data-slb-internal=""0"" data-slb-group=""252070""><img data-lazyloaded=""1"" data-placeholder-resp=""589x331"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI1ODkiIGhlaWdodD0iMzMxIiB2aWV3Qm94PSIwIDAgNTg5IDMzMSI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-1160x653.jpg.webp"" alt=""باتری ساعت مچی اپل"" class=""wp-image-252075"" width=""589"" height=""331"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/aw2.jpg.webp 1600w"" data-sizes=""(max-width: 589px) 100vw, 589px""></a></figure></div>



<p>این طرح شکایت از سوی چهار نفر از مشتریان پیشنهاد شده است که در دادگاهی در کالیفرنیا طرح شده است. شاکیان گفته‌اند :” صفحه‌ جدا شده و شکسته اپل واچ یک خطر مادی و جسمی است.”</p>



<p>در مستندات داخل دادگاه، عکسی از بریدگی عمیق روی دست یکی از کاربران ساعت مچی اپل واچ &nbsp;سری 3 ارائه شده است که ادعا می‌شود دلیل آن، تورم باتری اپل واچ بوده است. بنابراین در ادامه آمده است که این ایراد در طراحی فقط مختص به سری 6 نبوده و نسل‌های قبلی نیز چنین مشکلی در طراحی داشته‌اند. البته در طرح شکایت، نامی از ایراد در طراحی محفظه باتری اپل واچ در آخرین سری یعنی سری 7 مطرح نشده است.</p>



<p>به نظر می‌رسد رسیدگی بسیار مناسب در جلسات دادخواهی‌ توسط قاضیان در بعضی کشورها، باعث دردسرهایی برای تولید کنندگان می‌شود که البته اتفاق خوبی است.</p>



<p>به نظر شما این ادعا می‌تواند درست باشد؟ یا اینکه این گونه شکایات تنها برای دریافت امتیازات مادی است؟</p>



<p></p>



<p>مطالب مرتبط:<br><a href=""https://sakhtafzarmag.com/%d8%b3%d8%b1%db%8c-7-%d8%a7%d9%be%d9%84-%d9%88%d8%a7%da%86/"">سری 7 اپل واچ معرفی شد – صفحه نمایش بزرگ‌تر و پورت USB-C</a><br><a href=""https://sakhtafzarmag.com/%d8%aa%d9%85%d8%a7%d8%b3-%d8%a7%d8%b4%d8%aa%d8%a8%d8%a7%d9%87%db%8c-%d8%a8%d9%87-%d9%be%d9%84%db%8c%d8%b3-%d8%aa%d9%88%d8%b3%d8%b7-%d8%a7%d9%be%d9%84-%d9%88%d8%a7%da%86/"">تماس اشتباهی به پلیس توسط اپل واچ – قابلیت اپل تبدیل به یک مشکل شد!</a><br><a href=""https://sakhtafzarmag.com/%d8%b5%d9%81%d8%ad%d9%87-%d9%86%d9%85%d8%a7%db%8c%d8%b4-wrap-around-%d8%a8%d8%b1%d8%a7%db%8c-%d8%a7%d9%be%d9%84-%d9%88%d8%a7%da%86/"">صفحه نمایش Wrap-Around برای اپل واچ – نسل دوم ساعت اپل در راه است؟</a></p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 5,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "سامسونگ,تبلت هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8_0001-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "امتیاز اولین تبلت با اسنپدراگون 8 نسل1 در Geekbench",
                    Title = "گلکسی Tab S8+، اولین تبلت با اسنپدراگون 8 نسل1 در گیک بنچ رویت شد",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 5,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>انتظار می‌رود که تبلت‌های سری گلکسی تب S8 در ماه ژانویه سال جدید میلادی و در کنار گوشی‌های سری گلکسی S22 رونمایی شوند. تا امروز اطلاعات نسبتا زیادی در رابطه با مشخصات و طراحی احتمالی آنها لو رفته و تقریبا می‌دانیم که چه محصولاتی در انتظار ما هست. جدیدترین اطلاعات در رابطه با نسل آینده تبلت‌های پرچمدار سامسونگ از Geekbench&nbsp; <a href=""https://twitter.com/evleaks/status/1467761746609938433"">به بیرون درز شده</a> که در آن گلکسی Tab S8+ به عنوان اولین تبلت با چیپست اسنپدراگون 8 نسل1 در این بنچمارک لیست شده است.</p>



<h2 id=""h-8-1-geekbench"">امتیاز اولین تبلت با اسنپدراگون 8 نسل1 در Geekbench&nbsp;</h2>



<p>همانطور که در تصویر مشاهده می‌کنید، دستگاه مورد نظر دارای شماره مدل <strong>SM-X808U</strong><strong> </strong>بوده و نام سامسونگ در آن قید شده است. این شماره مدل متعلق به گلکسی تب S8+ بوده، تبلتی که توانسته در تست تک هسته امتیاز 1223 و در بخش تک هسته امتیاز 3195 را کسب کند.</p>



<figure class=""wp-block-image size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""720x900"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-001.jpg.webp"" loading=""lazy"" width=""720"" height=""900"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-001.jpg.webp"" alt=""امتیاز اولین تبلت با اسنپدراگون 8 نسل1 در Geekbench&nbsp;"" class=""wp-image-252552 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-001.jpg.webp 720w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-001-320x400.jpg.webp 320w"" data-sizes=""(max-width: 720px) 100vw, 720px"" sizes=""(max-width: 720px) 100vw, 720px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-001.jpg.webp 720w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-001-320x400.jpg.webp 320w"" data-was-processed=""true""></figure>



<p>نکته دیگری که در در این تصویر قابل مشاهده بوده اینست که این تبلت از 8 گیگابایت رم بهره می‌برد و دارای سیستم عامل اندروید 12 با رابط کاربری One UI 4 است. به غیر از این مشخصات انتظار می‌رود که گلکسی تب اس 8 پلاس با نمایشگر 12.4 اینچی AMOLED با نرخ نوسازی 120 هرتز با باتری 10090 میلی آمپرساعتی راهی بازار شود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x681"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY4MSIgdmlld0JveD0iMCAwIDExNjAgNjgxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""681"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8_000-1160x681.jpg.webp"" alt=""Samsung Galaxy Tab S8+"" class=""wp-image-252553"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8_000-1160x681.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8_000-400x235.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8_000-768x451.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8_000.jpg.webp 1200w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>Samsung Galaxy Tab S8+ عضو میانی خانواده خود خواهد بود، جاییکه نسخه استاندارد تب S8 با نمایشگر 11 اینچی عضو کوچک خانواده است و تبلت تماما بالا رده &nbsp;Tab S8 Ultra از نمایشگر 14.6 اینچی بهره می‎برد. نکته جالب در رابطه با نسخه اولترا این سری تبلت اینست که ظاهرا سامسونگ تصمیم دارد برای اولین بار این محصول از ناچ بالای نمایشگر استفاده کند و با هدف کم کردن حاشیه‌های اطراف آن، دوربین سلفی را درون این ناچ قرار دهد. رندرهایی که تا امروز از این سری منتشر شده تأیید می‌کند که تنها نسخه اولترا دارای این ناچ بوده و دو نسخه دیگر طراحی استانداردی دارند.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%aa%d8%b5%d8%a7%d9%88%db%8c%d8%b1-%d8%aa%d8%a8%d9%84%d8%aa-%da%af%d9%84%da%a9%d8%b3%db%8c-%d8%aa%d8%a8-s8-%d8%a7%d9%88%d9%84%d8%aa%d8%b1%d8%a7/"">اولین تصاویر گلکسی تب S8 اولترا با نمایشگر مجهز به ناچ منتشر شد</a></li></ul>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x752"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc1MiIgdmlld0JveD0iMCAwIDExNjAgNzUyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""752"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-ultra_003-1160x752.jpg.webp"" alt=""Tab S8 Ultra "" class=""wp-image-252554"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-ultra_003-1160x752.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-ultra_003-400x259.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-ultra_003-768x498.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Samsung-Galaxy-Tab-S8-ultra_003.jpg.webp 1200w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>اگر زمان احتمالی معرفی سری تب S8 (ژانویه 2022) صحیح باشد، چیزی به رونمایی از این محصولات نمانده و به زودی با اخبار تکمیلی در خصوص این تبلت‌ها در خدمت شما خواهیم بود.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%da%af%d9%84%da%a9%d8%b3%db%8c-%d8%a7%d8%b3-22-%d9%86%d9%88%d8%aa-%d9%86%d8%a7%d9%85-%d8%ac%d8%af%db%8c%d8%af-%d8%a7%d8%b3-22-%d8%a7%d9%84%d8%aa%d8%b1%d8%a7/"">گلکسی اس 22 نوت نام جدید اس 22 الترا – به علاوه بنچمارک Geekbench</a></li><li><a href=""https://sakhtafzarmag.com/%d8%a7%d9%86%d8%aa%d9%82%d8%a7%d9%84-%d8%b3%d9%81%d8%a7%d8%b1%d8%b4%d8%a7%d8%aa-%da%a9%d9%88%d8%a7%d9%84%da%a9%d8%a7%d9%85-%d9%88-amd-%d8%a8%d9%87-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"">انتقال سفارشات کوالکام و AMD به سمت سامسونگ – کاهش وابستگی به TSMC</a></li><li><a href=""https://sakhtafzarmag.com/%d8%b3%d8%a7%d8%b9%d8%aa-%d9%87%d9%88%d8%b4%d9%85%d9%86%d8%af-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af-%d8%a8%d8%a7-%d8%af%d9%88%d8%b1%d8%a8%db%8c%d9%86/"">ساعت هوشمند سامسونگ با دوربین ثبت اختراع شد</a></li><li><a href=""https://sakhtafzarmag.com/%d9%85%d8%b4%d8%ae%d8%b5%d8%a7%d8%aa-%da%af%d9%84%da%a9%d8%b3%db%8c-s21-fe-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"">لیست کامل مشخصات گلکسی S21 FE سامسونگ افشا شد</a></li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 6,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "مایکروسافت,تبلت هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-12-j-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "همکاری مایکروسافت با iFixit و معرفی ابزارهای جدید",
                    Title = "همکاری مایکروسافت با iFixit – تعمیرپذیری سرفیس ها بیشتر می شود",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 5,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>تبلت‌ها و لپ تاپ‌های هیبریدی اگر چه انقلابی در دسته بندی خود رقم زدند اما به واسطه طراحی خاص خود، تعمیر آنها کار دشواریست و اغلب این دستگاه ها امتیاز تعمیرپذیری کمی دریافت می‌کنند. طبیعتا هر چقدر دستگاه قابل تعمیرتر باشد، کاربر احتمالا می‌تواند مدت زمان بیشتری آن را نگه دارد که این موضوع هم از لحاظ اقتصادی و هم از لحاظ محیط زیستی مفیدتر است. سرفیس‌های مایکروسافت از جمله دستگاه‌هایی هستند که با وجود برخورداری از محاسن زیاد و محبوبیت بسیار بالا، تعمیرپذیری بسیار کمی دارند. همکاری مایکروسافت با <a href=""https://www.ifixit.com/News/56078/ifixit-works-with-microsoft-to-manufacture-service-tools-for-repair-techs"" target=""_blank"" rel=""noreferrer noopener"">iFixit </a>که یک کمپانی به نام در زمینه کالبدشکافی دستگاه‌های الکترونیکی است قرار است منجر به ساخت و فروش رسمی ابزار تعمیر سرفیس‌ها به فروشگاه‌های تعمیراتی شود.</p>



<h2 id=""h-ifixit"">همکاری مایکروسافت با iFixit و معرفی ابزارهای جدید</h2>



<p>iFixit در یک مراسم معارفه، از سه ابزار طراحی شده توسط مایکروسافت جهت تعمیر سرفیس‌های جدیدتر رونمایی کرد. اولین مورد یک اتصال دهنده نمایشگر به بدنه می‌باشد که در آن از لاستیک فوم با ضخامت بالا استفاده شده که تعمیرکاران توسط آن می‌توانند صفحه نمایش سرفیس پرو 7، پرو 8 و پرو X را تعویض کنند. دومین ابزار، یک کاور باتری برای سری سرفیس لپ تاپ است که از قطعات داخلی محافظت می‌کند و در نهایت سومین ابزار معرفی شده، یک ابزارجداسازی می‌باشد که به طور اختصاصی جهت برداشتن نمایشگرهای سرفیس پرو 7 پلاس، پرو 8 و Pro X طراحی شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1000x750"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j.jpg.webp"" loading=""lazy"" width=""1000"" height=""750"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j.jpg.webp"" alt=""همکاری مایکروسافت با iFixit"" class=""wp-image-252509 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j.jpg.webp 1000w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1000px) 100vw, 1000px"" sizes=""(max-width: 1000px) 100vw, 1000px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j.jpg.webp 1000w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/2021-12-14-image-9-j-768x576.jpg.webp 768w"" data-was-processed=""true""></figure></div>



<p>iFixit ضمن قدردانی از مایکروسافت بابت رفتن به سمت آسان‌تر کردن تعمیر سخت افزارها می‌گوید با ابزار جدید تکنیسین‌ها می‌توانند دقیق‌تر عمل کنند، تعمیرات را در تعداد بالا انجام دهند و نمایشگرها را با کیفیتی در حد خود کمپانی تعویض و نصب کنند. مایکروسافت راهنمای تعمیر این دستگاه‌ها را منتشر کرده و نکته مهم‌تر اینکه همکاری بین مایکروسافت و iFixit در امتیازدهی به تعمیرپذیری سرفیس‌های نسل بعد، تاثیرگذار نخواهد بود.</p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 7,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "AMD,لپ تاپ",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000S-RDNA-2-6nm-GPU-Refresh-For-Laptops-960x540.png.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "گرافیک های لپ تاپی AMD Radeon RX 6000S با فناوری 6 نانومتری",
                    Title = "گرافیک های لپ تاپی RX 6000S با فناوری 6 نانومتری برای فصل اول 2022",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 6,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>وب سایت <a href=""https://go.skimresources.com/?id=31026X886738&amp;isjs=1&amp;jv=15.2.1-stackpath&amp;sref=https%3A%2F%2Fwccftech.com%2Famd-allegedly-preps-6nm-radeon-rx-6000s-rdna-2-laptop-gpu-refresh-launching-in-q1-2022%2F&amp;url=https%3A%2F%2Fvideocardz.com%2Fnewz%2Famd-6nm-rdna2-mobile-gpu-refresh-to-be-called-radeon-rx-6000s&amp;xs=1&amp;xtz=-210&amp;xuuid=1bee76dafb410281b41174b861a7c893&amp;xjsf=other_click__auxclick%20%5B2%5D"" target=""_blank"" rel=""noreferrer noopener"">Videocardz</a> گزارش کرده که کمپانی AMD قصد دارد گرافیک های لپ تاپی Radeon RX 6000S با فناوری 6 نانومتری را به زودی به بازار عرضه نماید. با توجه به گزارش منتشر شده تیم سرخ چیپ‌های موبایلی RDNA 2 خود را تحت یک سری رفرش شاید با نام  Radeon RX 6000S عرضه کند. در واقع پسوند S تنها نشانگر این خواهد بود که پردازنده گرافیکی این خانواده رفرش می‌باشد یا خیر.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%b9%d9%85%d9%84%da%a9%d8%b1%d8%af-%d8%a7%d8%ad%d8%aa%d9%85%d8%a7%d9%84%db%8c-%da%af%d8%b1%d8%a7%d9%81%db%8c%da%a9-%d9%85%d8%ac%d8%aa%d9%85%d8%b9-%d8%b3%d8%b1%db%8c-ryzen-6000-rembrandt/"">عملکرد احتمالی گرافیک مجتمع سری Ryzen 6000 Rembrandt – در سطح GTX 1650</a></li><li><a href=""https://sakhtafzarmag.com/%da%af%d8%b1%d8%a7%d9%81%db%8c%da%a9-rtx-3080-12gb-%d8%aa%d8%a7%db%8c%db%8c%d8%af-%d8%b4%d8%af/"">گرافیک RTX 3080 12GB تایید شد – به همراه RTX 3070 Ti 16GB و RX 6500 XT</a></li><li><a href=""https://sakhtafzarmag.com/%d8%b2%d9%85%d8%a7%d9%86-%d8%b9%d8%b1%d8%b6%d9%87-rx-6500-xt-%d9%88-rx-6400/"">زمان عرضه RX 6500 XT و RX 6400 – پایین‌ترین نمونه‌های RDNA 2</a></li></ul>



<p>قبلا هم شایعاتی در مورد احتمال ارائه مدل‌های 6 نانومتری RDNA 2 وجود داشت و به نظر می‌رسد که سرانجام در فصل اول 2022 یا در CES 2022 اطلاعاتی از آنها منتشر خواهد شد. گزارش شده که گرافیک‌های رفرش جدید Radeon RX 6000S نام خواهند داشت و از لیتوگرافی 6 نانومتری TSMC استفاده می‌کنند که 18 درصد تراکم بالاتری را در مصرف انرژی یکسان فراهم می‌کند. همین موضوع در کنار بهینه سازی‌های دیگر بدون شک به سرعت کلاک و کارایی بر مصرف بالاتر ختم خواهد شد.</p>



<h2 id=""h-amd-radeon-rx-6000s-6"">گرافیک های لپ تاپی AMD Radeon RX 6000S با فناوری 6 نانومتری</h2>



<p>البته هنوز مدل‌های دقیق مشخص نشده‌اند. در واقع AMD حتی چیپ‌های پایین رده لپ تاپی مانند Radeon RX 6500M و Radeon RX 6300M را به بازار عرضه نکرده و بعید است شاهد رفرش شدن آنها باشیم. با این حال احتمال دارد که تیم سرخ به سراغ گرافیک‌های بر پایه Navi 22 و Navi 23 برود. در حال حاضر روی کاغذ می‌توانیم تصور کنیم که پرچمدار موبایلی با چیپ Navi 22 رفرش Radeon RX 6800S (جانشین Radeon RX 6800M) نام داشته باشد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x637"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYzNyIgdmlld0JveD0iMCAwIDExNjAgNjM3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""637"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000-RDNA-2-GPU-_2-1160x637.png.webp"" alt=""گرافیک لپ تاپی AMD Radeon RX 6000"" class=""wp-image-252003"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000-RDNA-2-GPU-_2-1160x637.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000-RDNA-2-GPU-_2-400x220.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000-RDNA-2-GPU-_2-768x422.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000-RDNA-2-GPU-_2-1536x844.png.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/AMD-Radeon-RX-6000-RDNA-2-GPU-_2-2048x1125.png.webp 2048w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>فارغ از آن لاین آپ رفرش Radeon RX 6000S RDNA 2 شرکت AMD به رقابت با لاین آپ ARC Alchemist اینتل می‌پردازد که اتفاقا از فناوری ساخت 6 نانومتری TSMC بهره می‌برد. سری GeForce RTX 30 نیز بدون شک در این رقابت جای خواهد داشت و البته که با حضور APUهای جدید Rembrandt اوضاع بسیار جذاب‌تر می‌شود.</p>



<figure class=""wp-block-table""><table><tbody><tr><td><strong>نام گرافیک</strong></td><td><strong>معماری</strong></td><td><strong>ابعاد die</strong></td><td><strong>تعداد هسته</strong></td><td><strong>فرکانس (حداکثر)</strong></td><td><strong>گذرگاه حافظه</strong></td><td><strong>حجم حافظه</strong></td><td><strong>توان حرارتی</strong></td></tr><tr class=""alt""><td>AMD Radeon RX 6800M</td><td>Navi 22</td><td>334.5 میلی متر مربع</td><td>2560</td><td>2300 مگاهرتز</td><td>192 بیت</td><td>12 گیگابایت<br>GDDR6<br>96 مگابایت Infinity Cache</td><td>145 وات</td></tr><tr><td>AMD Radeon RX 6700M</td><td>Navi 22</td><td>334.5 میلی متر مربع</td><td>2304</td><td>2300 مگاهرتز</td><td>160 بیت</td><td>10 گیگابایت<br>GDDR6<br>80 مگابایت Infinity Cache</td><td>135 وات</td></tr><tr class=""alt""><td>AMD Radeon RX 6600M</td><td>Navi 23</td><td>نامشخص</td><td>1792</td><td>2177 مگاهرتز</td><td>128 بیت</td><td>8 گیگابایت<br>GDDR6<br>32 مگابایت Infinity Cache</td><td>100 وات</td></tr><tr><td>AMD Radeon RX 6500M</td><td>Navi 23</td><td>نامشخص</td><td>1536</td><td>نامشخص</td><td>128 بیت</td><td>8 گیگابایت<br>GDDR6<br>32 مگابایت Infinity Cache</td><td>75 وات</td></tr><tr class=""alt""><td>AMD Radeon RX 6400M</td><td>Navi 24</td><td>نامشخص</td><td>1280</td><td>نامشخص</td><td>64 بیت</td><td>4 گیگابایت<br>GDDR6</td><td>نامشخص</td></tr><tr><td>AMD Radeon RX 6300M</td><td>Navi 24</td><td>نامشخص</td><td>1024</td><td>نامشخص</td><td>64 بیت</td><td>نامشخص</td><td>نامشخص</td></tr></tbody></table></figure>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 8,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "مک بوک,لپ تاپ",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/macbook-pro-14-and-16_overview__fz0lron5xyuu_og-960x540.png.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "عرضه مک بوک پرو پایین رده در سال 2022",
                    Title = "عرضه مک بوک پرو پایین رده در سال 2022 به همراه محصولات بیشتر",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 6,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>اپل در پروسه فاصله گرفتن از اینتل جلوتر از زمان بندی قرار دارد و برنامه‌های آتی این شرکت عرضه محصولات بیشتر می‌باشد. حالا اخباری از عرضه مک بوک پرو پایین رده در سال 2022 به گوش رسیده که البته در کنار مک‌های جدید دیگری قرار می‌گیرد.</p>



<p>Mark Gurman از <a href=""https://go.skimresources.com/?id=31026X886738&amp;isjs=1&amp;jv=15.2.1-stackpath&amp;sref=https%3A%2F%2Fwccftech.com%2Fapple-planning-to-launch-entry-level-macbook-pro-in-2022-along-with-new-mac-pro-imac-more%2F&amp;url=https%3A%2F%2Fwww.bloomberg.com%2Faccount%2Fnewsletters%2Fpower-on&amp;xs=1&amp;xtz=-210&amp;xuuid=1725962d6fb6a7bfe6b6ea64d2c4b783&amp;xjsf=other_click__auxclick%20%5B2%5D"" target=""_blank"" rel=""noreferrer noopener"">Bloomberg</a> در جدیدترین مطلب خود ادعا کرده که Apple قصد دارد تا پنج مک جدید را برای سال 2022 آماده کند. این شامل یک iMac بالا رده با سیلیکون اپل، مک بوک ایر باز طراحی شده با چیپ M2، مک مینی بهبود یافته، یک مک پرو با سیلیکون اپل و یک مک بوک پرو پایین رده می‌شود.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%a7%d9%81%d8%b2%d8%a7%db%8c%d8%b4-30-%d8%af%d8%b1%d8%b5%d8%af%db%8c-%d8%aa%d9%88%d9%84%db%8c%d8%af-%d8%a2%db%8c%d9%81%d9%88%d9%86/"">افزایش 30 درصدی تولید آیفون در نیمه اول 2022 – هدف عرضه 300 میلیون دستگاه</a></li><li><a href=""https://sakhtafzarmag.com/%d9%87%d8%af%d8%b3%d8%aa-%d9%88%d8%a7%d9%82%d8%b9%db%8c%d8%aa-%d9%85%d8%ac%d8%a7%d8%b2%db%8c-%d8%a7%d9%be%d9%84/"">هدست واقعیت مجازی اپل در سال 2022 عرضه خواهد شد</a></li><li><a href=""https://sakhtafzarmag.com/%d8%a7%d8%b3%d8%aa%d9%81%d8%a7%d8%af%d9%87-%d8%a7%d8%b2-%d8%af%d9%88-%d9%88-%da%86%d9%87%d8%a7%d8%b1-%d8%aa%d8%b1%d8%a7%d8%b4%d9%87-m1-%d8%a7%d9%be%d9%84/"">استفاده هم زمان از دو و چهار تراشه M1 – آخرین راهکار اپل برای رهایی از اینتل</a></li></ul>



<h2 id=""h-2022"">عرضه مک بوک پرو پایین رده در سال 2022</h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""740x476"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-Air-740x476-1.jpg.webp"" loading=""lazy"" width=""740"" height=""476"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-Air-740x476-1.jpg.webp"" alt=""مک بوک پرو"" class=""wp-image-251567 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-Air-740x476-1.jpg.webp 740w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-Air-740x476-1-400x257.jpg.webp 400w"" data-sizes=""(max-width: 740px) 100vw, 740px"" sizes=""(max-width: 740px) 100vw, 740px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-Air-740x476-1.jpg.webp 740w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-Air-740x476-1-400x257.jpg.webp 400w"" data-was-processed=""true""></figure></div>



<p>برنامه‌های اپل به این کمپانی اجازه می‌دهد که کاملا از اینتل فاصله بگیرد. در حالی که به چهار مک قبلی در گذشته اشاره شده بود اما شایعات در مورد مک بوک پرو پایین رده کاملا جدید هستند. با توجه به گزارشات قبلی چنین دستگاهی احتمالا به چیپ M2 مجهز خواهد شد که از تعداد هسته CPU مشابه با M1 بهره می‌برد اما تا 10 هسته گرافیکی را در خود جای خواهد داد.</p>



<p>در عین حال با عرضه مک بوک پرو‌های 14 و 16 اینچی جای چندانی برای یک مک بوک پرو پایین رده نیست. شاید اپل روی دو سناریو کار می‌کند، ارائه یک مک بوک پرو پایین رده در سال 2022 یا به روز رسانی مک بوک ایر از نظر سخت افزاری. گزارش جدید که به راهکار اول اشاره دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x645"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY0NSIgdmlld0JveD0iMCAwIDExNjAgNjQ1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""645"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-2060x1146-1-1160x645.jpeg.webp"" alt=""مک بوک پرو"" class=""wp-image-251566"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-2060x1146-1-1160x645.jpeg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-2060x1146-1-400x223.jpeg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-2060x1146-1-768x427.jpeg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-2060x1146-1-1536x854.jpeg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/MacBook-2060x1146-1-2048x1139.jpeg.webp 2048w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در حال حاضر جزئیات بسیار کم هستند و ما باید اطلاعات بیشتری را برای ترسیم یک تصویر مناسب در اختیار داشته باشیم. Gurman همچنین اطلاعاتی در مورد مدل‌های باز طراحی شده iPad Pro، هدست AR گیمینگ و یک iPhone SE با فناوری 5G نیز اشاره کرده است.</p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 9,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,مادربورد,ایسوس",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/image-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "ایسوس مادربرد ROG Strix Z690 - G Gaming WiFi را در فرم فاکتور mATX عرضه کرد",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 8,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>مادربردهای کوچک و جمع و جور در فرم فاکتور MicroATX امروزه از محبوبیت بسیار بالایی نزد کاربران، حتی کاربران حرفه‌ای و گیمرها برخوردارند. از همین رو سازندگان توجه ویژه‌ای به این محصولات دارند و در تنوع و رده‌های مختلف آنها را وارد بازار می‌کنند. حال ایسوس مادربرد <a href=""https://rog.asus.com/motherboards/rog-strix/rog-strix-z690-g-gaming-wifi-model/"" target=""_blank"" rel=""noreferrer noopener"">ROG Strix Z690-G Gaming WiF</a>i را برای طرفداران نسل دوازدهم پردازنده‌های اینتل آماده کرده است. کسانی که قصد دارند سیستمی بر پایه پلتفرم Alder Lake در فرم SFF جمع کنند می‌توانند بر روی این مادربرد کوچک و توانمند حساب باز کنند.</p>



<p>این مادربرد از چیپست پرچمدار Z690 بهره می‌برد و دارای مدار تغذیه 14+1 فاز می باشد و از فناوری‌های DrMOS و ProCool II برای اتصال پردازنده به مادربرد استفاده می‌کند. در قسمت حافظه، ایسوس 4 اسلات را بر روی این مادربرد تعبیه کرده که قادرند از حداکثر 128 گیگابایت رم DDR5 با فرکانس 6000 مگاهرتز پشتیبانی کنند. همچنین این مادربرد مجهز به 4 پورت SATA III، 3 اسلات M.2 سازگار با PCI-Express 4.0 x4 و 4 اسلات توسعه دهنده PCI-Express 5.0 x16 می‌باشد. در بخش صدا، ایسوس از تراشه ROG SupremeFX 7.1.1 با کدک ALC4080، Savitech SV3H712 و دیگر تراشه‌های صوتی استفاده کرده است. همچنین ROG Strix Z690-G Gaming WiFi از پورت شبکه 2.5 گیگابیتی، Wi-Fi 6E و بلوتوث 5.2 نیز برخوردار است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""800x806"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI4MDAiIGhlaWdodD0iODA2IiB2aWV3Qm94PSIwIDAgODAwIDgwNiI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""800"" height=""806"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/ROG-Strix-Z690-G-Gaming.jpg.webp"" alt="""" class=""wp-image-251967"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/ROG-Strix-Z690-G-Gaming.jpg.webp 800w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/ROG-Strix-Z690-G-Gaming-397x400.jpg.webp 397w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/ROG-Strix-Z690-G-Gaming-150x150.jpg.webp 150w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/ROG-Strix-Z690-G-Gaming-768x774.jpg.webp 768w"" data-sizes=""(max-width: 800px) 100vw, 800px""></figure></div>



<p>در مورد این مادربرد جمع و جور هم باید گفت که عرض آن 244 میلی متر می‌باشد و دارای طولی به اندازه 244 میلی متر است پس با یک مادربرد مربعی شکل روبرو هستیم. برای قسمت خروجی‌های تصویر ایسوس پورت های HDMI 2.1 و DisplayPort 1.4a را در نظر گرفته است. قیمت این مادربرد از سوی سازنده 389 دلار اعلام شده است.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d9%85%d8%b4%d8%ae%d8%b5%d8%a7%d8%aa-%da%86%db%8c%d9%be%d8%b3%d8%aa-%d9%87%d8%a7%db%8c-h670%d8%8c-b660-%d9%88-h610/"">مشخصات فنی چیپست های H670، B660 و H610 اینتل لو رفت</a></li><li><a href=""https://sakhtafzarmag.com/%d8%aa%d8%b5%d8%a7%d9%88%db%8c%d8%b1-%d9%85%d8%a7%d8%af%d8%b1%d8%a8%d8%b1%d8%af-msi-meg-z690-godlike/"">تصاویر مادربرد MSI MEG Z690 GODLIKE – باندل شده با حافظه DDR5 و خنک کننده مایع</a></li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 10,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,مادربورد,intel",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intel-Alder-Lake-is-all-of-the-three-kinds-of-chips-set-up-in-the-upcoming-Afgan-Alder-lake-H670-B660-and-H610-allegationally-exposed-are-only-sold-in-each-single-hole-960x540.jpeg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "مشخصات فنی چیپست های H670، B660 و H610",
                    Title = "مشخصات فنی چیپست های H670، B660 و H610 اینتل لو رفت",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 8,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>بنا به شایعات، اینتل چیپست های H670، B660 و H610 را در ژانویه سال 2022 عرضه خواهد کرد اما به لطف افشاگری momomo-us، هم اکنون جزئیاتی در مورد مشخصات فنی این چیپست‌ها در اختیارمان قرار گرفته است. همچون چیپست پرچمدار Z690، سه چیپست H670، B660 و H610 نیز در مدل های DDR4 و DDR5 از لحاظ پشتیبانی رم از راه خواهند رسید. کاربرانی که قصد اورکلاک CPUهای Alder Lake اینتل را داشته باشند، صرفا باید از بین مادربردهای چیپست Z690 انتخاب کنند. بنابراین چیپست‌های رده پائین‌تر بیشتر مناسب پردازنده‌های ضریب بسته هستند. با این حال محدودیتی برای اورکلاک رم بر روی چیپست‌های Z690، H670 و B660 نخواهید داشت، در حالی که H610 از این قابلیت بی بهره است و کاربران باید به فرکانس پیشفرض رم‌های خود اکتفا کنند.</p>



<h2 id=""h-h670-b660-h610"">مشخصات فنی چیپست های H670، B660 و H610</h2>



<p>اما PCIe 5.0، یکی دیگر از مهم‌ترین قابلیت‌هایی که با پردازنده‌های Alder Lake در دسترس کاربران قرار گرفتند، بر روی تمام چیپست‌های سری 600 اینتل قابل دسترس خواهند بود. با این حال لازم به ذکر است که همه سازندگان این قابلیت را بر روی مادربردهای خود پیاده سازی نمی‌کنند. به واسطه هزینه بسیار بالای این فناوری، سازندگان از آن در مادربردهای رده بالا استفاده می‌کنند. معمولا مادربردهای چیپست Z690 یا H670 از یک یا نهایتا دو اسلات PCIe 5.0 برخوردارند. اسلات صورت تکی بر روی 1×16 اجرا می‌شود در حالی که فعالیت اسلات‌های دوگانه به پیکربندی 2×8 کاهش خواهد یافت. در مقابل مادربردهای چیپست B660 و H610 به صرفا یک اسلات PCIe 5.0 محدود شده‌اند. Alder Lakeها قابلیت پشتیبانی از 4 مسیر PCIe 4.0 برای حافظه‌های M.2 را دارا هستند و فقط چیپست H610 فاقد این قابلیت است.</p>



<p>اینتل اتصالات DMI را نیز در پلتفرم Alder Lake به دو برابر افزایش داده است. در حالی که نسل‌های قبل پردازنده های اینتل دارای یک مسیر x8 DMI 3.0 با پهنای باند 7.88 گیگابایت بر ثانیه بودند، Alder Lake مجهز به یک مسیر x8 DMI 4.0 با پهنای باند 15.66 گیگابایت بر ثانیه می‌باشد. طبق ادعای <a href=""https://twitter.com/momomo_us/status/1467181907259670530"" target=""_blank"" rel=""noreferrer noopener"">momomo-us</a>، این موضوع در مورد چیپست‌های Z690 و H670 صدق می‌کند. اما چیپست‌های B660 و H610 به برخورداری از یک مسیر x4 DMI 4.0 محدود شده‌اند. به همین دلیل تعداد گزینه‌های اتصالاتی بر روی مادربردهای B660 و H610 نسبت به چیپست‌های رده بالاتر Alder Lake محدودتر است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1000x562"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDAwIiBoZWlnaHQ9IjU2MiIgdmlld0JveD0iMCAwIDEwMDAgNTYyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1000"" height=""562"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy.jpg"" alt="""" class=""wp-image-251679"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy.jpg 1000w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy-400x225.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy-384x216.jpg 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy-576x324.jpg 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/intel-chip-Copy-960x540.jpg.webp 960w"" data-sizes=""(max-width: 1000px) 100vw, 1000px""></figure></div>



<p></p>



<h2>قابلیت های ارتباطاتی و اتصالاتی چیپست‌های سری 600 الدرلیک</h2>



<p>در بخش ارتباطات با سرعت بسیار بالا یا اصطلاحا HSIO، چیپست Z690 از 12 مسیر PCIe 4.0 و 16 مسیر PCIe 3.0 پشتیبانی می‌کند. در مقابل چیپست H670 به پیکربندی 12+12 مجهز شده و در مورد B660 این چینش به صورت 6+8 است. اما چیپست H610 فقط 8 مسیر PCIe 3.0 را می‌تواند ساپورت کند. اما سازندگان مادربردها می‌توانند با تعداد گزینه‌های اتصالاتی و تعداد اسلات‌های PCIe بازی کنند.</p>



<p>به بخش اتصالات می‌رسیم جایی که طبیعتا Z690 بیشتر از سایر چیپست‌های Alder Lake گزینه های متعدد و متنوع تری را در اختیار کاربران قرار می‌دهد. این چیپست می‌تواند حداکثر 4 پورت USB 3.2 Gen 2×2 Type-C با پهنای باند 20 گیگابیت بر ثانیه، 10 پورت USB 3.2 Gen 2 با پهنای باند 10 گیگابیت بر ثانیه، 10 پورت USB 3.2 Gen 1 با پهنای باند 5 گیگابیت بر ثانیه و 14 پورت USB 2.0 را هندل کند. H610 تنها چیپست خانواده است که از پورت‌های USB 3.2 Gen 2×2 Type-C پشتیبانی نمی‌کند. از لحاظ پشتیبانی از حافظه‌های ذخیره سازی، چیپست‌ها با یکدیگر متفاوت هستند. Z690 و H670 از حداکثر 8 پورت SATA III پشتیبانی می‌کنند در حالی که این تعداد در چیپست‌های B660 و H610 به 4 پورت کاهش پیدا می‌کند که برای کاربران عادی کافی به نظر می‌رسد.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d9%84%db%8c%d8%b3%d8%aa-%da%a9%d8%a7%d9%85%d9%84-%d9%85%d8%a7%d8%af%d8%b1%d8%a8%d8%b1%d8%af%d9%87%d8%a7%db%8c-%da%86%db%8c%d9%be%d8%b3%d8%aa-msi-b660/"">لیست کامل مادربردهای چیپست MSI B660 به همراه قیمت لو رفت</a></li><li><a href=""https://sakhtafzarmag.com/%d9%85%d8%a7%d8%af%d8%b1%d8%a8%d8%b1%d8%af-z690-%d8%a7%d8%b1%d8%b2%d8%a7%d9%86-%d9%82%db%8c%d9%85%d8%aa-%da%af%db%8c%da%af%d8%a7%d8%a8%d8%a7%db%8c%d8%aa/"">مادربرد Z690 ارزان قیمت گیگابایت با عدم پشتیبانی از PCIe 5.0 معرفی شد</a></li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 11,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,پردازنده مرکزی,Intel",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_00-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "وجود باکس تقلبی پردازنده های Intel در بازار ایران",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 9,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>شرکت معظم اینتل پردازنده های خود را به دو صورت Box و Tray در دنیا عرضه می کند. لغت باکس به معنای جعبه است و به پردازنده هایی گفته می شود که دارای بسته بندی کارخانه جات اینتل هستند و به همان صورت با بسته بندی اورجینال اینتل عرضه می شوند. کلمه Tray به معنای (طَبَق) سینی است و این نوع پردازنده ها بدون هیچ بسته بندی در سینی های چند تایی به دست مصرف کننده می رسند. به دلیل ابعاد کوچک سینی ها، تعداد پردازنده بیشتری را می توان در یک فضا جا داد و آن را حمل کرد. عموما خریداران پردازنده های تِرِی شرکت ها، موسسات، و همچنین سازندگان سیستم های پیش ساخته Pre-Built در دنیا هستند. نکته حائز اهمیت در مورد پردازنده های تِرِی آسیب پذیر بودن آنها نسبت به پردازنده ها باکس است، بنابراین به هنگام خرید این نوع از پردازنده ها، حتما می بایستی تست شوند تا از سلامت آنها اطمینان حاصل شود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-1160x870.jpg.webp"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-1160x870.jpg.webp"" alt="""" class=""wp-image-252417 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-1536x1152.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01-1536x1152.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_01.jpg.webp 1600w"" data-was-processed=""true""><figcaption>پردازنده های Tray اینتل</figcaption></figure></div>



<p>در این میان، وضعیت پردازنده های ضریب باز Tray بدتر است. چرا که علاقمندان به اورکلاک در سراسر دنیا دائما مشغول تست هستند و احتمال اینکه پردازنده های ضریب باز حتی قبل از ورود به ایران زیر تست سنگین اورکلاک رفته باشند، زیاد است. بنابراین ما همیشه به تمام کاربران خانگی توصیه می کنیم، به هیچ وجه پردازنده ضریب باز Tray را تهیه نکنند. اما خرید مدلهای ضریب بسته به صورت Tray، به دلیل عدم امکان اورکلاک، به نسبت امنیت بیشتری دارد و نگرانی از بابت خرابی آنها کمتر است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1100x446"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1.jpg.webp"" loading=""lazy"" width=""1100"" height=""446"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1.jpg.webp"" alt="""" class=""wp-image-252432 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1.jpg.webp 1100w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1-400x162.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1-768x311.jpg.webp 768w"" data-sizes=""(max-width: 1100px) 100vw, 1100px"" sizes=""(max-width: 1100px) 100vw, 1100px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1.jpg.webp 1100w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1-400x162.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_02-1-768x311.jpg.webp 768w"" data-was-processed=""true""><figcaption>نمونه اورجینال</figcaption></figure></div>



<p>از نظر کیفی، پردازنده های تِرِی معمولا بی کیفیت ترین سیلیکن ها را دارند و نسبت به پردازنده های هم کلاس باکس، اغلب عملکرد داغتری دارند. البته استثنا هم پیدا می شود و ممکن است یک در صد، پردازنده تری خنک هم پیدا شود. در مورد کارایی و پرفورمنس، در مقام مقایسه در حالت پیش فرض، پردازنده تری با نمونه باکس برابر است. بنابراین تفاوتی در پردازش ندارند. برتری پردازنده های باکس در این است که به هنگام حمل از کارخانه اینتل تا منزل خریدار، کاملا در باکس خود محفوظ بوده و توسط اینتل تست شده و مطمئن به دست خریدار می رسد. بنابراین پردازنده باکس اینتل نیاز به تست سلامت ندارد.</p>



<p>در ایران اصولا خریدارن سیستم های خانگی سراغ پردازنده های Tray نمی روند. یکی به دلیل عدم اطمینان از سلامت آنهاست، و دلیل دوم آن است که پردازنده های Tray فاقد کولر استوک اینتل هستند. وجود کولر استوک در باکس مدلهای ضریب بسته، باعث کاهش هزینه اولیه کاربر در خرید سیستم می شود. هرچند کولر استوک اینتل همچین آش دهان سوزی هم نیست و کارایی آن در خنک سازی پردازنده افتضاح است، اما با این همه، وجود آن از هیچی بهتر است و به همین جهت خریدارن عموما سراغ مدلهای باکس پردازنده می روند.</p>



<p>ظاهرا در ایران، تعداد زیادی از پردازنده های Tray بر روی دست تامین کنندگان مانده است، از این رو آستین ها را بالا زده و دست به کار شده اند تا با ساخت جعبه برای این پردازنده ها، و اضافه کردن یک کولر چینی استوک اینتل، آنها را به صورت “باکس اینتل” به فروش رسانند. صد البته ما اطلاع دقیق نداریم این جعبه ها در چاپخانه در ایران تولید شده و یا به همین شکل با جعبه Fake وارد ایران شده اند؛ اما به هر صورتی که باشد، وجود آنها در بازار با هدف فریب کاربر و غالب کردن آن به عنوان باکس اورجینال به خریدار است. امیدواریم که شما جزو قربانیان خرید پردازنده های باکس فیک اینتل در ایران نباشید.</p>



<h2><strong>نمونه باکس فیک </strong><strong>i5 10400</strong><strong>:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1144x1024"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-1144x1024.jpg.webp"" loading=""lazy"" width=""1144"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-1144x1024.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252419 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-1144x1024.jpg.webp 1144w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-400x358.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-768x687.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03.jpg.webp 1246w"" data-sizes=""(max-width: 1144px) 100vw, 1144px"" sizes=""(max-width: 1144px) 100vw, 1144px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-1144x1024.jpg.webp 1144w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-400x358.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03-768x687.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_03.jpg.webp 1246w"" data-was-processed=""true""><figcaption>تصاویر باکس تقلبی</figcaption></figure></div>



<p>کیفیت باکس اینتل ایران، کمابیش مطلوب است؛ رنگ آمیزی عالی و حتی نسبت به مدلهای اورجینال اینتل، جعبه کمی براق تر است؛ مشخصه عزیزانی که زحمت ساخت باکس پردازنده اینتل را قبول کرده اند، واقعا سنگ تمام گذاشته و باکس را بهتر از مدل اورجینال اینتل درآوردند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x587"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-1160x587.jpg.webp"" loading=""lazy"" width=""1160"" height=""587"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-1160x587.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252420 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-1160x587.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-400x203.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-768x389.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04.jpg.webp 1483w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-1160x587.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-400x203.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04-768x389.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_04.jpg.webp 1483w"" data-was-processed=""true""></figure></div>



<p>انصافا به جزئیات ساخت جعبه توجه ویژه ای شده است. هاشور های هفت و هشتی جعبه اورجینال شبیه سازی شده اند اما این هاشور ها در مدل باکس ایرانی کم عمق تر است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1099x569"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05.jpg.webp"" loading=""lazy"" width=""1099"" height=""569"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252421 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05.jpg.webp 1099w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05-400x207.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05-768x398.jpg.webp 768w"" data-sizes=""(max-width: 1099px) 100vw, 1099px"" sizes=""(max-width: 1099px) 100vw, 1099px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05.jpg.webp 1099w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05-400x207.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_05-768x398.jpg.webp 768w"" data-was-processed=""true""></figure></div>



<p>در مورد فونت، حتی الامکان تلاش خود را کرده اند تا نزدیک ترین فونت را پیدا کنند و در این زمینه نمره قبولی دریافت می کنند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""688x475"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_06.jpg.webp"" loading=""lazy"" width=""688"" height=""475"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_06.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252422 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_06.jpg.webp 688w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_06-400x276.jpg.webp 400w"" data-sizes=""(max-width: 688px) 100vw, 688px"" sizes=""(max-width: 688px) 100vw, 688px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_06.jpg.webp 688w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_06-400x276.jpg.webp 400w"" data-was-processed=""true""></figure></div>



<p>مبحث بعدی، تاریخ کپی رایت طراحی جعبه اینتل است. درحالی که با جعبه پردازنده 10400 روبه رو هستیم که در سال 2020 تولید شده است، اما تاریخ حق کپی رایت چاپ شده بر روی جعبه تقلبی موجود در ایران، به سال 2016 میلادی باز می گردد. احتمالا اینتل از سال 2016 این جعبه را برای پردازنده های چهار سال بعد خود از قبل طراحی کرده و در شیشه خیار شور نگهداری می کرده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1000x605"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07.jpg.webp"" loading=""lazy"" width=""1000"" height=""605"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252423 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07.jpg.webp 1000w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07-400x242.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07-768x465.jpg.webp 768w"" data-sizes=""(max-width: 1000px) 100vw, 1000px"" sizes=""(max-width: 1000px) 100vw, 1000px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07.jpg.webp 1000w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07-400x242.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_07-768x465.jpg.webp 768w"" data-was-processed=""true""></figure></div>



<p>و اما به سراغ برچسب مشخصات می رویم. در این بخش خوشبختانه تپه سالم باقی نگذاشته اند. یکی از مهمترین خصوصیات Core i5 های نسل 10 اینتل آن بود که پس از سالها، برای اولین بار، میزان حافظه کش از 6 مگابایت به 12 مگابایت افزایش یافت. متاسفانه خبر این تغییر عظیم به گوش دوستان کارتن ساز اینتل در ایران نرسیده است. از این رو با بی توجهی به پیشرفت های اینتل در نسل 10؛ برچسب روی باکس پردازنده 10400 را، به جای 12 مگابایت، همچنان با 6 مگابایت حافظه کش چاپ کرده اند. البته جای شکرش باقی است که سوکت را LGA1200 اعلام کرده اند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x346"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-1160x346.jpg.webp"" loading=""lazy"" width=""1160"" height=""346"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-1160x346.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252424 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-1160x346.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-400x119.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-768x229.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-1536x458.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-1160x346.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-400x119.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-768x229.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08-1536x458.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_08.jpg.webp 1600w"" data-was-processed=""true""></figure></div>



<p>به سراغ Product Code می رویم. برای مقایسه از جعبه اورجینال پردازنده 10400F استفاده می کنیم. در این بخش نیز سازندگان باکس اینتل در نهایت علم و دانش رفتار کرده اند. کد محصول یا Product Code های اینتل از دو بخش تشکیل شده است. بخش اول کد مرجع اینتل برای نسل پردازنده است، و&nbsp; بخش دوم به مدل محصول اشاره می کند. تولید کنندگان کارتن اینتل در ایران، متوجه بخش دوم کد محصول شده اند و با دقت فراوان، i510400 را در بخش دوم آورده اند. اما این دوستان متوجه معنای بخش اول کد نشده اند. &nbsp;</p>



<p>در پردازنده های نسل 10 اینتل، بخش اول کد مرجع با BX80701 شروع می شود. اگر این کد را در گوگل سرچ کنید، به پردازنده های نسل 10 اینتل می رسید. اما تولید کنندگان کارتن فیک اینتل از کد BX80677 استفاده کرده اند که مربوط به پردازنده های Kaby Lake نسل 7 اینتل است. این موضوع نشان میدهد نمونه اولیه جعبه های Fake مربوط به نسل 7 اینتل بوده و کارتن سازان، بدون تغییر این کد، آن را برای نسل 10 هم استفاده کرده اند. کی به کی است، نه اینتل یقه آنها را خواهد گرفت و نه کسی متوجه این موضوع خواهد شد.</p>



<p>نکته قابل توجه دیگر در این بخش، کم کاری اینتل است! در حالی که اینتل بر روی جعبه اورجینال پردازنده 10400 خود، در بخش دوم کد محصول، نامی از i5 بودن آن نبرده است، اما کارتن سازان، با اضافه کاری تمام، این کم کاری اینتل را جبران کرده، و نام i5 را به کد محصول اینتل اضافه کرده اند. باشد که اینتل یاد بگیرد با مانند نسل های پیشین، نام محصول را در این بخش کامل ذکر کند!</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x643"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-1160x643.jpg.webp"" loading=""lazy"" width=""1160"" height=""643"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-1160x643.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252425 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-1160x643.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-400x222.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-768x426.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-1536x852.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-1160x643.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-400x222.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-768x426.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09-1536x852.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_09.jpg.webp 1600w"" data-was-processed=""true""></figure></div>



<p>به بخش UPC کد می رسیم. Universal Product Code یک سیستم کد گذاری جهانی است که برای بسیاری از کالاها در کشور های پیشرفته از آن استفاده می شود. بسیاری از نرم افزار های QR Scanner موبایل به راحتی با اسکن کردن این کد می توانند محصول مورد نظر را به سرعت در اینترنت ردیابی کنند. لازم است دوستان کارتن ساز به این موضوع هم در آینده دقت داشته باشند تا UPC کد محصول را طوری چاپ کنند که در نرم افزار های QR Code آن محصول را نشان دهد، نه اینکه محصول دیگری را نمایان سازد!</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x443"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ0MyIgdmlld0JveD0iMCAwIDExNjAgNDQzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""443"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_10-1160x443.jpg.webp"" alt=""باکس تقلبی پردازنده های Intel در ایران"" class=""wp-image-252426"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_10-1160x443.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_10-400x153.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_10-768x293.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_10.jpg.webp 1536w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>به بخش آخر آموزش امروز می رسیم. جایی که باید با استفاده از DNS شکن، وارد <a href=""https://supporttickets.intel.com/warrantyinfo"">وبسایت اینتل</a> شویم و از باکس بودن محصول اطمینان حاصل کنیم. در وبسایت شرکت اینتل می توان با وارد کردن بچ نامبر و سریال نامبر، از اصالت محصول (باکس بودن آن)، تاریخ تولید و مدت زمان گارانتی آن اطمینان حاصل کرد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x524"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjUyNCIgdmlld0JveD0iMCAwIDExNjAgNTI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""524"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_11-1160x524.jpg.webp"" alt=""تشخیص تقلبی بودن پردازنده های Intel در ایران"" class=""wp-image-252434"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_11-1160x524.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_11-400x181.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_11-768x347.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_11-1536x694.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Intelfakebox_11.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>و اگر به مانند تصویر فوق پاسخ اینتل نیافتن محصول مورد نظر بود، به معنای آن است که آن پردازنده فاقد باکس اورجینال است.</p>



<h2 id=""h-"">سخن پایانی</h2>



<p>اگرچه این پردازنده 10400 موجود در این باکس فیک، یک پردازنده واقعی 10400 و ساخت اینتل است، اما چون Tray بودن آن باعث عدم اقبال خریداران است، تامین کننده را بر آن داشته تا آستین بالا بزند و خودش برای آن باکس تولید کند. ممکن است این پردازنده ها به همین شکل از کشور ثالثی وارد شده باشند، اما با توجه به اشتباهات متعدد فنی در بخش مشخصات، بسیار بعید است که هیچ کجای دنیا چنین اشتباهات تابلو و اظهر و من الاشمسی را در کپی کردن مرتکب شوند.</p>



<p>به هر شکل احتمالا باکس فیک پردازنده های نسل 7 تا 10 اینتل در بازار ایران وجود دارد. هنوز خوشبختانه باکس فیک برای پردازنده های نسل 11 اینتل در بازار تاکنون مشاهده نشده است. به هر جهت مطلع باشید که این پردازنده ها Tray هستند و باید با قیمت کمتری فروخته شوند. همچنین به دلیل ذات تری بودن آنها، بهتر است حتما تست شده و یا از فروشنده مهلت تست دریافت کنید. اگر قیمت آنها مناسب باشد و مهلت تست داده شود، منعی برای خرید و فروش آنها نیست، اما واقعا آیا بازار ما نیاز به چنین حرکت های فریبنده ای دارد؟</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x261"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjI2MSIgdmlld0JveD0iMCAwIDExNjAgMjYxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""261"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/365854574354-1160x261.png.webp"" alt="""" class=""wp-image-252428"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/365854574354-1160x261.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/365854574354-400x90.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/365854574354-768x173.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/365854574354-1536x346.png.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/365854574354.png.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>بسیاری از خریداران قطعات کامپیوتر، دائما در ترس و وحشت این موضوع هستند که ممکن است محصولی که می خرند، اورجینال و اصل نباشد. چنین حرکت های چیپ و بی معنا، فقط و فقط استرس و شک از اصالت کالا ها در بازار بیشتر می کند. درست است که این یک محصول واقعی است، اما چرا باید در بسته بندی فیک و تقلبی به فروش رسانده شود؟</p>



<p>امیدوارم فروشندگانی که پردازنده های باکس اینتل را در بازار عرضه می کنند، در صورت مشاهده باکس فیک آن، صادقانه Tray بودن آنها را به خریداران گوشزد کنند و با علم و دانش اینکه پردازنده های تری نیاز به تست دارند، به قیمت واقعی، آنها را به خریداران بفروشند.</p>



<p>بدون شک، در هر کسب و کاری، هر چه از فریب کاری و دروغ فاصله بگیریم، خیر و برکت بیشتر خواهد شد، ترس از خرید محصولات تقلبی کاهش پیدا خواهد کرد و شک و بی اعتمادی از بین خواهد رفت.</p>



<ul><li>پی نوشت: تشکر از همکار خوبم آقای <a href=""https://sakhtafzarmag.com/author/isteve/"" target=""_blank"" rel=""noreferrer noopener"">ناصر مغانی</a> عزیز.</li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 12,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,پردازنده مرکزی,Ryzen",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/ULvANzAirbtZr3zjNU3jHG-1200-80-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "اولین نسل از پردازنده های رایزن سازگار با DDR5",
                    Title = "اولین نسل از پردازنده های رایزن با قابلیت پشتیبانی از رم های DDR5 ظاهر شدند",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 9,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>در حال حاضر یکی از مهم ترین برتری‌های پردازنده‌های نسل دوازدهم اینتل، پشتیبانی از نسل بعد حافظه‌های رم می باشد که به غول آبی در مقابل تیم قرمز برتری محسوسی بخشیده اما مسلما قرار نیست این برتری تا مدت زیادی در انحصار اینتل باقی بماند چرا که اولین نسل از پردازنده های رایزن که با رم های DDR5 سازگارند در راه هستند. نمونه آزمایشی پردازنده رایزن AMD با کد شناسایی 100-000000560-40-Y که گفته می‌شود مجهز به 8 هسته و 16 رشته پردازشی می‌باشد توسط فردی ناشناس، با 16 گیگابایت رم که از نوع DDR5 یا LPDDR5 بوده و فرکانس آن 4800 مگاهرتز بوده به ثبت رسیده است. بر اساس شایعات، پردازنده‌های Raphael با معماری Zen 4 و سری Rembrandt پردازنده‌های APU Mobile با معماری Zen 3+ از رم‌های DDR5 پشتیبانی می‌کنند.</p>



<h2 id=""h-ddr5"">اولین نسل از پردازنده های رایزن سازگار با DDR5</h2>



<p>دیگر مشخصات پردازنده آزمایشی رایزن AMD در حال حاضر مشخص نیست. بر اساس کدرمز ذکر شده، فرکانس پایه در این پردازنده رایزن 4 گیگاهرتز بوده است. همچنین این پردازنده در دستگاه معرفی شده M3402RA ایسوس قرار داشته که احتمالا یک لپ تاپ است چرا که لپ تاپ‌های 14 و 15 اینچی ایسوس از سری VivoBook Pro OLED دارای شماره مدل‌های M3401 و 3500 هستند و M3402RA شاید نسخه ای جدید از مدل VivoBook Pro 14 باشد. همچنین تست پردازنده بر روی رزولوشن 2560 * 1600 صورت گرفته که وضوح بسیار بالایی برای یک لپ تاپ محسوب می‌شود. در صورتی که اطلاعات دقیق باشند، این پردازنده از هشت هسته‌ای از خانواده Rembrandt می‌باشد که به عنوان جایگزین خانواده Cezanne راهی بازار خواهند شد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x652"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-1160x652.jpg.webp"" loading=""lazy"" width=""1160"" height=""652"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-1160x652.jpg.webp"" alt=""اولین نسل از پردازنده های رایزن سازگار با DDR5"" class=""wp-image-252494 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-1160x652.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-1536x863.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-1160x652.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-1536x863.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/LTXQty6PE7bS7N3a8cXnuf.jpg.webp 1600w"" data-was-processed=""true""></figure></div>



<p>Rembrandt احتمالا کد رمز پردازنده‌های رایزن 6000 می‌باشد که در صورت دقیق بودن شایعات، ارتقای بزرگی را از لحاظ توان پردازشی در این سری شاهد نخواهیم بود چرا که این APUها به هسته های بهبود یافته با معماری Zen 3+ مجهز هستند. اما در بخش گرافیک، با برخورداری احتمالی از تراشه‌های مبتنی بر RDNA 2 انتظار می‌رود در این بخش پیشرفت قابل توجهی را شاهد باشیم. پیش از این بنچمارک‌های اولیه از عملکرد پردازنده گرافیکی داخلی RDNA 2 پردازنده‌های رایزن 6000 منتشر شده است. با این حال عملکرد پردازنده چندان تاثیرگذار نبود اما با توجه به اینکه مدل آزمایشی بوده، نمی‌توانیم به این نتایج استناد کنیم. ضمن اینکه به طور کلی بنچمارک CrossMark رابط خوبی با پردازنده‌های AMD ندارد و بیشتر هوای پردازنده‌های اینتل را دارد.</p>



<p>تست CrossMark حاکی از این است که پردازنده AMD موفق به کسب امتیاز 1426 شده است. این پردازنده تست‌های بهره وری، تولید و پاسخگویی را به ترتیب با امتیاز 1442، 1492 و 1269 به پایان رسانده است. از نظر عملکرد، پردازنده AMD پرفورمنسی در سطح Ryzen 7 5800H و Core i7-10870H اینتل داشته است. در حالی که Core i7-11800H سریع‌تر بوده، باید دید نسخه نهایی این پردازنده در چه حدی ظاهر خواهد شد. AMD به زودی در <a href=""https://www.tomshardware.com/news/amd-ces-2022-ryzen-radeon-keynote"" target=""_blank"" rel=""noreferrer noopener"">رویداد CES 2022</a> که در لاس وگاس برگزار خواهد شد، اطلاعات بیشتری در مورد پردازنده‌ها و گرافیک‌های نسل بعد خود با کاربران به اشتراک خواهد گذاشت.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%d9%be%d8%b4%d8%aa%db%8c%d8%a8%d8%a7%d9%86%db%8c-%d8%a7%d8%b2-%d8%b1%d9%85-%d9%87%d8%a7%db%8c-12-%da%a9%d8%a7%d9%86%d8%a7%d9%84%d9%87-ddr5/"" target=""_blank"" rel=""noreferrer noopener"">پچ AMD پشتیبانی پردازنده های Zen 4 Epyc از رم های 12 کاناله DDR5 را تائید کرد</a></li><li><a href=""https://sakhtafzarmag.com/%d8%a8%d9%86%da%86%d9%85%d8%a7%d8%b1%da%a9-%d9%be%d8%b1%d8%af%d8%a7%d8%b2%d9%86%d8%af%d9%87-%d9%87%d8%a7%db%8c-core-i3-12100-%d9%88-i3-12300/"">جدیدترین بنچمارک از پردازنده های Core i3-12100 و i3-12300 اینتل منتشر شد</a></li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 13,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,کارت گرافیک,MSI",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/HFjRrvXfDiATKdaFyWvo7k-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "کارت گرافیک های گیمینگ MSI با گارانتی آونگ – خرید خوب با خیال راحت",
                    Title = "کارت گرافیک های گیمینگ MSI با گارانتی آونگ – تنوع بالا برای سلایق و بودجه های مختلف",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 10,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>امروزه تاکید ما بر خرید سخت افزار کامپیوتر با یک گارانتی معتبر بیشتر از هر زمان دیگریست. برای تاکید بر این موضوع نیز دلیل قدرتمندی داریم آن هم این است که امروزه با توجه به وضعیت بازار، بسیاری از محصولات با قیمت‌های نامتعارف به فروش می‌رسند و از طرف دیگر، واقعیت مسئله این است که کمپانی‌ها با نهایت سرعت و ظرفیت خود در حال فعالیت هستند و در طول دو سه سال اخیر کم راجع به خطاهای فنی و انسانی در تولید سخت افزارهای مختلف نشنیده‌ایم. به همین دلیل خرید یک محصول با گارانتی معتبر می‌تواند در این شرایط خیال کاربر را تا حد زیادی راحت کند. در مورد کارت گرافیک‌ها مسائل مذکور بیشتر از سایر قطعات قابل درک است. خوشبختانه آونگ، یکی از شرکت‌های خوشنام وارد کننده سخت افزار به تازگی <a href=""https://www.msi.com/Graphics-Cards"" target=""_blank"" rel=""noreferrer noopener"">کارت گرافیک های گیمینگ MSI</a> را وارد بازار کشورمان کرده که اتفاق بسیار خوبی برای جامعه گیمرهای داخل ایران محسوب می‌شود.</p>



<h2 id=""h-msi"">کارت گرافیک های گیمینگ MSI با گارانتی آونگ – خرید خوب با خیال راحت</h2>



<p>MSI از سازندگان برتر سخت افزار در کلاس جهانی، همواره بهترین کارت گرافیک‌های گیمینگ را در هر نسل روانه بازار کرده و می‌توانیم بگوئیم بهترین محصولات این سازنده تایوانی، در واقع همین کارت گرافیک‌های MSI می‌باشد که از محبوبیت بالایی نزد کاربران، خصوصا گیمرهای حرفه‌ای برخوردارند. ترکیب این سخت افزارهای باکیفیت با یک گارانتی معتبر و اصیل تبدیل به یک گزینه ارزشمند برای خرید می‌شوند. آونگ کارت گرافیک های گیمینگ MSI از سری RTX 30 انویدیا و نسل‌های قبل تر را با تنوع بالا راهی بازار سخت افزار کشور کرده که باعث می‌شود همه کاربران با سطح بودجه‌های مختلف بتوانند کارت گرافیک مورد نظر خود را با یک گارانتی قابل اعتماد خریداری کنند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1024x455"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjQ1NSIgdmlld0JveD0iMCAwIDEwMjQgNDU1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1024"" height=""455"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/atrinkala-GTX-1660-VENTUS-XS-6G-OC-t1.jpg.webp"" alt=""کارت گرافیک های گیمینگ MSI با گارانتی آونگ"" class=""wp-image-252526"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/atrinkala-GTX-1660-VENTUS-XS-6G-OC-t1.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/atrinkala-GTX-1660-VENTUS-XS-6G-OC-t1-400x178.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/atrinkala-GTX-1660-VENTUS-XS-6G-OC-t1-768x341.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></figure></div>



<p>از ارزان‌ترین کارت گرافیک گیمینگ MSI که با گارانتی آونگ روانه بازار شده کار را آغاز می‌کنیم. کارت گرافیک GTX 1660 Super Ventus XS OC کمپانی MSI که به 6 گیگابایت حافظه ویدئویی اختصاصی مجهز است. این کارت گرافیک از سری Turing انویدیا می‌باشد. انویدیا برای پاسخگویی به نیاز بازار، تولید کارت گرافیک‌های نسل قبل تر را ادامه داد که استراتژی خوبی بود و GTX 1660 اکنون جز محبوب‌ترین گرافیک‌ها بین گیمرها، بر اساس نظرسنجی سخت افزاری استیم محسوب می‌شود. یک کارت گرافیک ایده آل جهت گیمینگ در رزولوشن Full HD با نرخ فریم مناسب که در این وضعیت بازار گرافیک، ارزش خرید بالایی هم دارد. مدل مذکور از MSI با گارانتی آونگ به قیمت حدودا 22 میلیون تومان به فروش می‌رسد که البته قیمت سخت افزار به لطف نوسان دلار در این اواخر دستخوش تغییرات زیادی شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1000x564"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDAwIiBoZWlnaHQ9IjU2NCIgdmlld0JveD0iMCAwIDEwMDAgNTY0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1000"" height=""564"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d.jpg.webp"" alt=""msi rtx 3070 ti"" class=""wp-image-252527"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d.jpg.webp 1000w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d-400x226.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d-768x433.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/6467497cv14d-960x540.jpg.webp 960w"" data-sizes=""(max-width: 1000px) 100vw, 1000px""></figure></div>



<p></p>



<h2>از اقتصادی‌ترین مدل‌ها تا پرچمداران فوق العاده قدرتمند انویدیا</h2>



<p>برای گیمرهایی که می‌خواهند جدیدترین تکنولوژی‌های گرافیکی انویدیا را تجربه کنند، کمپانی آونگ کارت گرافیک‌های سفارشی MSI از سری RTX 30 انویدیا را آماده فروش کرده است. عضو رده پائین خانواده، کارت گرافیک MSI GeForce RTX 3060 Gaming X می‌باشد که به 12 گیگابایت حافظه ویدئویی پرسرعت و خنک کننده دو فنه مجهز است. یک کارت گرافیک قدرتمند در رده Full HD Gaming که قادر است بازی‌های روز را با بالاترین تنظیمات گرافیکی و نرخ فریم ایده آل، بدون کندی و تاخیر اجرا کند. MSI این کارت را برای سیستم‌های گیمینگ میان رده آماده کرده و در مقایسه با وضعیت فعلی، با قیمت حدودا 35 میلیون تومان با گارانتی آونگ از ارزش خرید مناسبی برخوردار است. نسخه Ti کارت گرافیک Gaming X Trio شرکت MSI نیز که از سه فن خنک کننده و 8 گیگابایت حافظه بهره می‌برد، برای سیستم‌های گیمینگ رده بالاتر با قیمت تقریبی 55 میلیون تومان در دسترس کاربران قرار گرفته است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""829x553"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI4MjkiIGhlaWdodD0iNTUzIiB2aWV3Qm94PSIwIDAgODI5IDU1MyI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""829"" height=""553"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1889-MSI-GeForce-RTX-3090-Gaming-X-Trio-24G-cabecera.jpg.webp"" alt=""msi rtx 3070"" class=""wp-image-252528"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1889-MSI-GeForce-RTX-3090-Gaming-X-Trio-24G-cabecera.jpg.webp 829w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1889-MSI-GeForce-RTX-3090-Gaming-X-Trio-24G-cabecera-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1889-MSI-GeForce-RTX-3090-Gaming-X-Trio-24G-cabecera-768x512.jpg.webp 768w"" data-sizes=""(max-width: 829px) 100vw, 829px""></figure></div>



<p>اما می‌رسیم به قدرتمندترین کارت گرافیک گیمینگ حال حاضر در بازار، یعنی RTX 3090 انویدیا که آونگ مدل Gaming X Trio 24GB از کمپانی MSI را برای گیمرها و کاربران بسیار حرفه‌ای تدارک دیده است. انویدیا این کارت گرافیک را به لطف بهره مندی از 24 گیگابایت حافظه فوق العاده سریع GDDR6X، برای گیمینگ در رزولوشن‌های بسیار بالا مانند 4K بهینه سازی کرده که حتی با تنظیمات گرافیکی حداکثری نیز قادر است نرخ فریم ایده آلی در بازی‌های سنگین امروزی به شما دهد. مشخصا قیمت این کارت گرافیک، پرچمدار بودن آن را نیز کاملا توجیه می‌کند. این محصول با گارانتی معتبر آونگ با قیمتی نزدیک به 113 میلیون تومان در این به فروش می‌رسد. مدل SUPRIM این کارت گرافیک نیز که از لحاظ ظاهری کمی متفاوت‌تر و زیباتر به نظر می‌رسد، با قیمت تقریبی 118 میلیون تومان آماده فروش شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1024x820"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjgyMCIgdmlld0JveD0iMCAwIDEwMjQgODIwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1024"" height=""820"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/product_1605863560f0f4ea5d1a8d9e2e534c77c608665bcf.jpg.webp"" alt=""rtx 3090 suprim"" class=""wp-image-252529"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/product_1605863560f0f4ea5d1a8d9e2e534c77c608665bcf.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/product_1605863560f0f4ea5d1a8d9e2e534c77c608665bcf-400x320.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/product_1605863560f0f4ea5d1a8d9e2e534c77c608665bcf-768x615.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></figure></div>



<p><strong>لازم به ذکر است شرکت آونگ تمامی کارت گرافیک‌های MSI را به مدت 36 ماه ضمانت می‌کند.</strong></p>



<ul><li><a href=""https://sakhtafzarmag.com/msi-%d9%87%d9%85-%d8%a2%d9%88%d9%86%da%af%db%8c-%d8%b4%d8%af/"">MSI هم آونگی شد</a></li><li><a href=""https://sakhtafzarmag.com/%da%a9%d8%a7%d8%b1%d8%aa-%da%af%d8%b1%d8%a7%d9%81%db%8c%da%a9-geforce-rtx-2060-ventus-12gb/"">MSI از کارت گرافیک GeForce RTX 2060 Ventus 12GB رونمایی کرد</a></li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 14,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,کارت گرافیک,AMD Radeon",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/…lCWaokvC3og1AjIC39PE11i35rmC-zvE-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "کارت گرافیک Radeon RX 6500XT و 6400 پاورکالر از طریق EEC تائید شد",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 10,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>اتحادیه اقتصادی اروپا موسوم به EEC لیستی که شامل 10 کارت گرافیک Radeon RX 6500XT و همچنین 6 مدل از گرافیک Radeon RX 6400 را منتشر کرده که طبق شایعات هر دو این مدل از گرافیک‌های AMD مجهز به واحد پردازنده گرافیکی Navi 24 هستند. همچنین این لیست تائید می‌کند که کارت گرافیک‌های مذکور مجهز به 4 گیگابایت حافظه GDDR6 هستند و خبری هم از مدل‌های 2 یا 8 گیگابایتی نیست. نکته قابل توجه این لیست، تائید کارت گرافیک Radeon RX 6400 می‌باشد که طبق اطلاعات قبلی، این کارت نیازی به کانکتورهای تغذیه اکسترنال ندارد و به عنوان یک مدل رده پائین وارد بازار خواهد شد و اولین کارت گرافیک رده پائین از سری گرافیک‌های Radeon RX6000/RDNA تیم قرمز محسوب می‌شود.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""552x151"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/BC160.jpg.webp"" loading=""lazy"" width=""552"" height=""151"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/BC160.jpg.webp"" alt="""" class=""wp-image-252465 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/BC160.jpg.webp 552w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/BC160-400x109.jpg.webp 400w"" data-sizes=""(max-width: 552px) 100vw, 552px"" sizes=""(max-width: 552px) 100vw, 552px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/BC160.jpg.webp 552w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/BC160-400x109.jpg.webp 400w"" data-was-processed=""true""></figure></div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""959x326"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400.jpg.webp"" loading=""lazy"" width=""959"" height=""326"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400.jpg.webp"" alt="""" class=""wp-image-252464 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400.jpg.webp 959w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400-400x136.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400-768x261.jpg.webp 768w"" data-sizes=""(max-width: 959px) 100vw, 959px"" sizes=""(max-width: 959px) 100vw, 959px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400.jpg.webp 959w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400-400x136.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PowerColor-RX6500XT-RX6400-768x261.jpg.webp 768w"" data-was-processed=""true""></figure></div>
</div>
</div>



<p>علاوه بر این، لیست لو رفته توسط EEC نام جدید مدل ماینینگ گرافیک‌های AMD را نیز لو داده است. مدل BC-2235 که مجهز به 10 گیگابایت حافظه GDDR6 می‌باشد اما مشخص نیست که از چه مدل GPU در این محصول استفاده شده است. طبق گزارش از قبل منتشر شده، نسخه قبل این کارت گرافیک که مبتنی بر معماری RDNA نسل اول بود، <a href=""https://videocardz.com/newz/amd-xfx-bc-160-cryptomining-card-with-navi-12-gpu-8gb-hbm2-memory-is-now-available-for-2000-usd"" target=""_blank"" rel=""noreferrer noopener"">به تازگی در چین عرضه شده</a> است. BC-2235 احتمالا به Blockchain Compute اشاره دارد که احتمالا دومین نسل مجهز به تراشه Navi 22 با هش ریت 35 MH/s خواهد بود. ممکن است BC-2235 نام رسمی کارت گرافیک لو رفته Sapphire GPRO X080 باشد. این گرافیک مجهز به 10 گیگابایت حافظه GDDR6 می‌باشد از رابط 160 بیتی بهره می‌برد.</p>



<ul><li><a href=""https://sakhtafzarmag.com/%da%a9%d8%a7%d8%b1%d8%aa-%d9%88%db%8c%da%98%d9%87-%d8%a7%d8%b3%d8%aa%d8%ae%d8%b1%d8%a7%d8%ac-amd/"">کارت گرافیک ویژه استخراج AMD با فناوری قدیمی ولی قیمت به روز</a></li><li><a href=""https://sakhtafzarmag.com/%da%af%d8%b1%d8%a7%d9%81%db%8c%da%a9-rx-6500-xt-%d8%aa%d9%88%d8%b3%d8%b7-%d9%84%d9%86%d9%88%d9%88-%d8%aa%d8%a7%db%8c%db%8c%d8%af-%d8%b4%d8%af/"">گرافیک RX 6500 XT توسط لنوو تایید شد – 4 گیگابایت حافظه ویدیویی</a></li></ul>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 15,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,هارد,خرابی",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/…ound-broken-hd-wallpaper-preview-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "علائم خرابی هارد دیسک لپ تاپ و کامپیوتر رومیزی",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 11,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<p>از دست رفتن اطلاعات یکی از ناگوارترین اتفاقاتی است که برای کاربران کامپیوتر می‌افتد. دانستن علائم خرابی هارد می‌تواند کمک کند تا پیش از آنکه هارد به کلی از کار بیوفتد، نسبت به تهیه فایل پشتیبان اقدام کنیم. قبل از هر چیز همواره توصیه می‌شود با توجه به اهمیت اطلاعات کامپیوتر باید به همان نسبت برای حفظ اطلاعات هزینه کنیم.</p>



<p>برای حفظ کامل اطلاعات همواره هاردی با ظرفیت پایین‌تر برای نگهداری اطلاعات با ارزش داشته باشید چرا که احتمال بازگردانی یا ریکاوری اطلاعات از دست رفته، زیاد نیست و در بهترین حالت ممکن، بخشی از اطلاعات ریکاوری می‌شوند اما بقیه اطلاعات، دیگر قابل بازگردانی نخواهند بود.</p>



<p>در ادامه نشانه‌ها، راه حل‌ها و پیشگیری از خرابی هارد دیسک را بررسی می‌کنیم.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1.jpg"" data-slb-active=""1"" data-slb-asset=""1966983862"" data-slb-internal=""0"" data-slb-group=""252297""><img data-lazyloaded=""1"" data-placeholder-resp=""-189x-89"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-1160x551.jpg.webp"" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-1160x551.jpg.webp"" alt=""خرابی هارد"" class=""wp-image-252304 litespeed-loaded"" width=""-189"" height=""-89"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-1160x551.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-400x190.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-768x365.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-1536x730.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1.jpg.webp 1920w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-1160x551.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-400x190.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-768x365.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1-1536x730.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/Unraid_pci-e_sata_expansion_cards_00-1.jpg.webp 1920w"" data-was-processed=""true""></a></figure></div>



<h2 id=""h-""><strong>علائم خرابی هارد دیسک</strong></h2>



<p><strong>سر و صدای اضافه:</strong> هارد دیسک به کلی دارای سر و صداست. به علت آنکه بر خلاف SSD، در هارد دیسک قطعات مکانیکی به کار رفته است. چرخش پلاتر صدایی شبیه به فن کامپیوتر ایجاد می‌کند و حرکت بازوی هِد نیز گاهی اوقات صدایی شبیه تیک تیک ساعت می‌دهد. اما صدای اضافه و بلند مانند خِرت خِرت کردن‌های بلند و پی در پی یا صدایی شبیه افتادن جسم فلزی که همواره تکرار شود، نشانه‌هایی از احتمال به وجود آمدن ایرادات فیزیکی در هارد است.</p>



<p><strong>کند شدن سیستم:</strong> اگر در هنگام روشن کردن کامپیوتر، سیستم عامل، خیلی دیرتر از قبل بالا آمده و در محیط ویندوز هنگام انتقال یا باز کردن فایل‌ها این کار به کندی انجام می‌شود، احتمالاً هارد شما آسیب دیده است. البته این علائم می‌تواند دلایل دیگری داشته باشد اما اول از همه باید سلامت هارد بررسی شود.</p>



<p><strong>چک کردن دیسک در حین روشن شدن:</strong> پس از روشن کردن کامپیوتر گاهی اوقات به دلیل انباشه شدن اطلاعات طبقه بندی نشده، پیش از ورود به سیستم عامل، درایوها توسط کامپیوتر چک می‌شوند. اگر در هربار روشن کردن کامپیوتر ابتدا پیام Disk Check نمایش داده می‌شود و درخواست چک کردن درایوها داده می‌شود بنابراین ممکن است ایرادی در هارد به وجود آمده باشد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/wcd_top.png"" data-slb-active=""1"" data-slb-asset=""1488586600"" data-slb-internal=""0"" data-slb-group=""252297""><img data-lazyloaded=""1"" data-placeholder-resp=""741x342"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI3NDEiIGhlaWdodD0iMzQyIiB2aWV3Qm94PSIwIDAgNzQxIDM0MiI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/wcd_top.png"" alt=""چک دیسک هارد"" class=""wp-image-252305"" width=""741"" height=""342"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/wcd_top.png 650w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/wcd_top-400x185.png.webp 400w"" data-sizes=""(max-width: 741px) 100vw, 741px""></a></figure></div>



<p><strong>مشاهده بلواسکرین:</strong> دیدن صفحه آبی مرگ یا بلواسکرین (BSOD) اگر در بازه‌های زمانی خیلی بلندی رخ دهد، اتفاق بدی نیست. زمانی که رشته پردازش از دست سیستم عامل خارج شود، سیستم عامل برای پیدا کردن مسیر تازه و پردازش دوباره، نیاز به ری‌استارت پیدا می‌کند و در این زمان، صفحه BSOD نمایش داده شده و سیستم ری‌استارت می‌شود. اما اگر این اتفاق به صورت پی در پی با فاصله زمانی بسیار کوتاه اتفاق افتاد، می‌توان حدس زد که ممکن است هارد آسیب دیده باشد. البته در بیشتر مواقع چنین ایراداتی نرم افزاری است و با تعویض ویندوز حل خواهد شد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK.jpg"" data-slb-active=""1"" data-slb-asset=""674277680"" data-slb-internal=""0"" data-slb-group=""252297""><img data-lazyloaded=""1"" data-placeholder-resp=""744x419"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI3NDQiIGhlaWdodD0iNDE5IiB2aWV3Qm94PSIwIDAgNzQ0IDQxOSI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-1160x653.jpg.webp"" alt=""بلواسکرین هارد"" class=""wp-image-252306"" width=""744"" height=""419"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/VmSUXc6MrUABpm5a4tH8KK.jpg.webp 1600w"" data-sizes=""(max-width: 744px) 100vw, 744px""></a></figure></div>



<p><strong>عدم شناسایی هارد:</strong> معمولاً هاردهایی که آسیب دیده‌اند گاهی اوقات با روشن شدن سیستم، شناسایی نمی‌شوند و پیامی مبنی بر شناسایی نشدن درایو بوت توسط کامپیوتر نمایش داده خواهد شد. چنین اتفاقی اگر به دفعات تکرار شود، می‌تواند از علائم خرابی هارد دیسک باشد. احتمالات دیگری مانند خرابی کابل، مادربرد یا پاور و… نیز باید در نظر گرفته شود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1579048749_Solutions-quotNo-Boot-Devicequot-error.webp.jpeg"" data-slb-active=""1"" data-slb-asset=""1376323387"" data-slb-internal=""0"" data-slb-group=""252297""><img data-lazyloaded=""1"" data-placeholder-resp=""747x392"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI3NDciIGhlaWdodD0iMzkyIiB2aWV3Qm94PSIwIDAgNzQ3IDM5MiI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1579048749_Solutions-quotNo-Boot-Devicequot-error.webp-1160x609.jpeg.webp"" alt=""شناسایی هارد"" class=""wp-image-252307"" width=""747"" height=""392"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1579048749_Solutions-quotNo-Boot-Devicequot-error.webp-1160x609.jpeg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1579048749_Solutions-quotNo-Boot-Devicequot-error.webp-400x210.jpeg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1579048749_Solutions-quotNo-Boot-Devicequot-error.webp-768x403.jpeg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1579048749_Solutions-quotNo-Boot-Devicequot-error.webp.jpeg.webp 1200w"" data-sizes=""(max-width: 747px) 100vw, 747px""></a></figure></div>



<h3><strong>اگر با علائم خرابی هارد رو به رو شدیم چه باید کرد؟</strong></h3>



<p>علائم خرابی هارد که در این مطلب از آن‌ها یاد شد، یک احتمال هستند و به طور یقین نمی‌توان گفت که صد در صد مشکل از کجاست. پیشنهاد می‌کنیم در صورت مقابله با چنین نشانه‌هایی، ابتدا نسبت به کپی اطلاعات مهم اقدام نمایید، سپس با تعویض یا جابه جایی کابل‌های هارد و تست کامپیوتر با یک پاور سالم و استاندارد، از سلامت کابل‌ها و پاور اطمینان حاصل نمایید، سپس در صورت ادامه مشکل، سیستم عامل را مجدداً نصب کرده و با نصب یک آنتی ویروس معتبر لایسنس شده و آپدیت، از عدم وجود ویروس روی هارد مطمئن شوید چر که تمام نشانه‌های بالا به احتمال زیاد از وجود ویروس حکایت دارند.</p>



<p>گاهی اوقات محل قرارگیری کیس کامپیوتر در یک محل پر تردد و یا روی یک میز نامناسب قرار گرفته است و در شرایطی قرار دارد که در معرض تکان‌های مداوم یا لرزش است. لرزش و تکان خوردن باعث بروز خطاهایی که در بالا به عنوان علائم خرابی هارد یاد شد، می‌گردد. بنابراین کیس را یک محل آرام و بدون لرزش قرار دهید.</p>



<p>کاربران لپ تاپ نیز می‌توانند در مرحله اول، با تعویض ویندوز و نصب آنتی ویروس لایسنس شده و به روز شده، نیمی از مراحل عیب یابی را طی کنند. اما به دلیل نیاز به تخصص برای باز کردن لپ تاپ، پیشنهاد می‌کنیم در صورت عدم رفع مشکل، لپ تاپ را به یک تعمیرکار بسپارید تا با چک کردن کابل‌های اتصال و تست با هارد دوم، سلامت هارد بررسی شود.</p>



<p>اگر با امتحان تمام این موارد همچنان مشکل شما پابرجا بود می‌بایست از یک متخصص کمک بگیرید.</p>



<h4><strong>کمک گرفتن از نرم افزار تست سلامت هارد</strong></h4>



<p>نرم افزارهای بسیار خوبی مانند HD Sentinel می‌توانند تا حد نسبتاً دقیقی، میزان سلامت هارد را نمایش دهند. همچنین نرم افزار HD Tune هم در این زمینه مورد استفاده قرار می‌گیرد. اگر چه این نرم افزارها تا حد زیادی قابل اطمینان هستند اما به طور صد در صد نمی‌توانند سلامت یک هارد را تضمین کنند. پس اگر با وجود نشان دادن سلامت بالای هارد در تست نرم‌افزاری، همچنان علائم خرابی هارد را داشتید، از اطلاعات مهم خود نسخه پشتیبان یا کپی تهیه کنید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/hdsentinel-main-window.png"" data-slb-active=""1"" data-slb-asset=""703729772"" data-slb-internal=""0"" data-slb-group=""252297""><img data-lazyloaded=""1"" data-placeholder-resp=""744x478"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI3NDQiIGhlaWdodD0iNDc4IiB2aWV3Qm94PSIwIDAgNzQ0IDQ3OCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/hdsentinel-main-window.png"" alt=""سلامت هارد"" class=""wp-image-252308"" width=""744"" height=""478"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/hdsentinel-main-window.png 954w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/hdsentinel-main-window-400x257.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/hdsentinel-main-window-768x493.png.webp 768w"" data-sizes=""(max-width: 744px) 100vw, 744px""></a></figure></div>



<h3><strong>چگونه از هارد خود در برابر آسیب محافظت کنیم؟</strong></h3>



<p>هارد دیسک یک فضای ذخیره سازی سنتی است که دارای قطعات متحرک و حساسی است. هاردها حتی در هنگام خاموش بودن هم به لرزش شدید و تکان خوردن حساسیت خیلی زیادی دارند اما در حین روشن بودن به شدت حساس می‌شوند. بنابراین هارد را جا به جا نکنید و کیس کامپیوتر یا لپ تاپ را در جایی آرام و بدون تکان و لرزش استفاده کنید.</p>



<p>همواره سعی کنید هارد را ترجیحاً در فضای افقی داخل کیس محکم کنید. اگر فضایی برای نصب افقی هارد نداشتید در نهایت به شکل کاملاً عمودی هم می‌توان نصب کرد اما باید الویت اول نصب، به شکل افقی باشد. سعی کنید هر چهار طرف هارد پیچ ببندید تا هارد کج قرار نگیرد. کج بودن هارد به دلیل تعداد دور بالای پلاتر، می‌تواند با برهم زدن بالانس، باعث آسیب در دراز مدت شود.</p>



<p>استفاده از یک پاور استاندارد و با کیفیت علاوه بر افزایش راندمان و طول عمر مفید تمام قطعات کامپیوتری، به طور مستقیم روی طول عمر هارد، تأثیر فوق العاده زیادی دارد. بر حسب تجربه می‌توان به جرأت گفت حداقل بیش از 60 درصد خرابی‌های هارد به دلیل وجود یک پاور غیر استاندارد و بی کیفیت است.</p>



<p>اگر در پایان کار به این نتیجه رسیدید که عمر زیادی از هارد شما باقی نمانده است پیشنهاد می‌کنیم برای جلوگیری از بروز دوباره چنین مشکلاتی در آینده نزدیک، با تهیه یک SSD، خود و کامپیوترتان را راحت کنید!</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/damaged-drive-failure.jpg"" data-slb-active=""1"" data-slb-asset=""700881802"" data-slb-internal=""0"" data-slb-group=""252297""><img data-lazyloaded=""1"" data-placeholder-resp=""734x436"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI3MzQiIGhlaWdodD0iNDM2IiB2aWV3Qm94PSIwIDAgNzM0IDQzNiI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/damaged-drive-failure-1160x690.jpg.webp"" alt=""نشانه خرابی هارد"" class=""wp-image-252309"" width=""734"" height=""436"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/damaged-drive-failure-1160x690.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/damaged-drive-failure-400x238.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/damaged-drive-failure-768x457.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/damaged-drive-failure.jpg.webp 1500w"" data-sizes=""(max-width: 734px) 100vw, 734px""></a></figure></div>



<p><strong>مطالب مرتبط:</strong></p>



<ul><li><a href=""https://sakhtafzarmag.com/%d8%aa%d9%81%d8%a7%d9%88%d8%aa-%d9%87%d8%a7%d8%b1%d8%af-%d8%af%db%8c%d8%b3%da%a9%d9%87%d8%a7%db%8c-smr-%d9%88-cmr-%d8%a8%d8%b1%d8%a7%db%8c-%d8%b3%db%8c%d8%b3%d8%aa%d9%85%d9%87%d8%a7%db%8c-%d8%ae%d8%a7%d9%86%da%af%db%8c/"">برای سیستم‌های خانگی هارد دیسک‌های SMR نخرید! به دنبال مدلهای CMR باشید!</a></li><li><a href=""https://sakhtafzarmag.com/%d8%b1%d8%a7%d9%87%d9%86%d9%85%d8%a7%db%8c-%d8%ae%d8%b1%db%8c%d8%af-%d9%87%d8%a7%d8%b1%d8%af%d8%af%db%8c%d8%b3%da%a9/"">راهنمای خرید هارد دیسک اینترنال</a> </li><li><a href=""https://sakhtafzarmag.com/%d9%87%d8%a7%d8%b1%d8%af-%d8%af%db%8c%d8%b3%da%a9-%d8%a7%d8%ae%d8%aa%d8%b1%d8%a7%d8%b9-%da%a9%d8%af%d8%a7%d9%85-%d8%b4%d8%b1%da%a9%d8%aa-%d8%a7%d8%b3%d8%aa/"">هارد دیسک اختراع کدام شرکت است ؟</a></li></ul>



<p></p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 16,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "قطعات داخلی کامپیوتر,هارد,ttt",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/12/35435435-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "CMR در برابر SMR",
                    Title = "برای سیستم‌های خانگی هارد دیسک‌های SMR نخرید! به دنبال مدلهای CMR باشید!",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 29),
                    CategoryId = 11,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">

											
<h2 id=""h-""><strong>مقدمه</strong></h2>



<p>برای افزایش کارایی و سرعت سیستم، کاربران حرفه ای حافظه‌های غیر متحرک یا به اصطلاح SSD را جایگزین هارد دیسک کرده اند. اما همچنان &nbsp;هارد دیسک به دلیل حجم بالای ارزان قیمتی که در اختیار کاربران قرار می‌دهد، اهیمت خود را حفظ کرده است. بسیاری از کاربران حرفه ای به دلیل شغل و نیاز خود، همچنان از هارد دیسک به عنوان حافظه دائمی‌در سیستم‌ها استفاده میکنند.</p>



<p>طراحان سه بعدی و دو بعدی که پروژه‌های خود را در هارد دیسک ذخیره می‌کنند، و یا گیمر‌هایی که بازی‌های قدیمی‌تر خود را به هارد دیسک منتقل می‌کنند. بسیاری از کاربران نیز، تعداد زیادی عکس و فیلم برای آرشیو دارند. به دلیل وجود نیاز به حجم بالا و ارزان قیمت، تمام این کاربران از هارد دیسک برای سیستم‌های خود استفاده می‌کنند.</p>



<p>شرکت‌های سازنده هارد دیسک نیز بر اساس نیاز کاربران، دسته بندی‌های متفاوتی را برای محصولات خود برگزیده اند. مثلا برای کاربرانی که نیاز به هارد دیسک‌‌های کلاس سروری دارند، یک گروه مشخصی از هارد دیسک‌ها را برای آنها اختصاص داده اند. اما، این همه ی موضوع نیست.</p>



<p>علاوه بر طبقه بندی هارد دیسک‌ها بر اساس رنگ و دسته بندی سازنده، ساخت هارد دیسک از نظر فن آوری نیز می‌تواند متفاوت باشد. هارد دیسک اطلاعات را بر روی شیار‌های دایره شکل که بر روی پلات (دیسک) درست می‌کند، نوشته و میخواند.</p>



<h2><strong>CMR در برابر SMR</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x607"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-1160x607.jpg.webp"" loading=""lazy"" width=""1160"" height=""607"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-1160x607.jpg.webp"" alt="""" class=""wp-image-252197 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-1160x607.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-400x209.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-768x402.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR.jpg.webp 1200w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-1160x607.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-400x209.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR-768x402.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/تفاوت-CMR-با-SMR.jpg.webp 1200w"" data-was-processed=""true""></figure></div>



<p>هارد دیسک‌های متداول بازار، از شیوه فن آوری Conventional Magnetic Recording برای نوشتار استفاده می‌کنند. بدین صورت که ترک‌های اطلاعات مستقل از هم و با فاصله ای نوشته می‌شود و هد هارد دیسک اطلاعات هر ترک را به آسانی نوشته و یا می‌خواند. به این شیوه از نوشتار در هارد دیسک‌ها، CMR می‌گویند. این نوع از هارد دیسک‌ها لزوما سریع تر نیستند، اما به عنوان هارد دیسک از سرعت معقولی برخوردارند. به این نوع هارد دیسک‌ها PMR هم گفته می‌شود. در عمل PMR = CMR است بنابراین این دو تقریبا به یک نوع فن آوری اشاره دارند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""600x316"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/3654534.png.webp"" loading=""lazy"" width=""600"" height=""316"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/3654534.png.webp"" alt="""" class=""wp-image-252198 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/3654534.png.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/3654534-400x211.png.webp 400w"" data-sizes=""(max-width: 600px) 100vw, 600px"" sizes=""(max-width: 600px) 100vw, 600px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/3654534.png.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/3654534-400x211.png.webp 400w"" data-was-processed=""true""></figure></div>



<p>اما نوع دیگری از فن آوری نوشتار در هارد دیسک، Shingled Magnetic Recording نام دارد که به اصطلاح آن را SMR می‌نامند و اتفاقا جدید تر از شیوه CMR/PMR است. به لطف این فن آوری، با باریک تر کردن فاصله هر ترک، امکان نوشتار نزدیکتر به هم (نوشتار توهم توهم) برای هد فراهم می‌شود.</p>



<p>دوستان دهه شصتی به خوبی به خاطر دارند در زمان‌های قدیم، به دلیل گرانی کاغذ، موقع امتحانات نهایی معلم از ما میخواست که توهم توهم بنویسیم تا پاسخ به سوالات کاغذ کمتری مصرف کند. فن آوری SMR دقیقا همین کار را با هارد دیسک انجام می‌دهد. آنقدر ترک‌ها توهم توهم نوشته می‌شوند تا سازنده هارد دیسک بتواند با صرفه جویی در سطح پلاتر، حجم بیشتری از اطلاعات را بر روی آن جای دهد. این نوع از هارد دیسک‌ها ارزان قیمت تر هستند و نسبت به CMR‌ها، به هنگام نوشتار کندتر عمل می‌کنند.</p>



<h2><strong>چرا </strong><strong>SMR</strong><strong> کند است؟</strong></h2>



<p>در هارد دیسک‌های CMR، هد هارد صرفا بر روی هر ترک به صورت جدا ازهم اطلاعات را نوشته و یا می‌خواند. اما در هارد دیسک‌های SMR، به دلیلی نزدیکی بیش از حد ترک‌ها، و توهم توهم بودنشان، هد هارد مجبور است به هنگام نوشتن در هر ترک، اطلاعات ترک بقل دستی را نیز مجددا” پاکسازی و باز نویسی کند. به دلیل این عملیات اضافی پاکسازی و دوباره نویسی ترک‌های بقل دستی، در عمل SMR در نوشتار کندتر عمل می‌کند و نتیجه این عملیات اضافی می‌شود کندتر بودن و کندتر عمل کردن و سرعت پایینتر از حد عادی برای هارد دیسک سیستم.</p>



<h2><strong>چرا </strong><strong>SMR</strong><strong> بوجود آمد؟</strong></h2>



<p>دلیل آن اقتصادی است. سازنده هارد دیسک در نوع SMR می‌تواند با تعداد پلاتر کمتر، به حجم بیشتر برسد. مثلا اگر قرار است یک هارد دیسک 8 ترابایتی تولید شود، با فن آوری SMR می‌توان تنها با 6 پلات به این حجم از هارد دیسک رسید. اما با فن آوری CMR تولید کننده مجبور است تعداد 8 پلات را در هارد دیسک استفاده کند. بنابراین SMR باعث می‌شود تولید کننده در هزینه‌های خود صرفه جویی می‌کند. شرکت‌های بزرگ که صرفا تنها نیاز به نگهداری اطلاعات خود دارند و دیر به دیر به آرشیو‌ها رجوع می‌کنند، مشکلی از بابت استفاده از هارد دیسک نوع SMR ندارند. اما کاربران خانگی و عادی کامپیوتر که دائم به هارد دیسک سر می‌زنند، مجبور به تحمل کندی غیر عادی سرعت هارد خود هستند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x684"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY4NCIgdmlld0JveD0iMCAwIDExNjAgNjg0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""684"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/22222-1160x684.png.webp"" alt="""" class=""wp-image-252199"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/22222-1160x684.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/22222-400x236.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/22222-768x453.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/22222-1536x905.png.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/22222.png.webp 1676w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>یک نمونه از تفاوت در این نوع از هارد دیسک‌ها در مدل Western Digital Blue 4TB است. شرکت وسترن دیجیتال 2 نوع هارد دیسک آبی 4 ترابایتی را در بازار عرضه کرده است. یک نوع آن پارت نامبر WD40EZRZ است که این مدل از نوع CMR و دارای 64 مگابایت کش می‌باشد. قیمت این هارد دیسک در آمازون 180 دلار است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x587"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU4NyIgdmlld0JveD0iMCAwIDExNjAgNTg3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""587"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1111-1160x587.png.webp"" alt="""" class=""wp-image-252200"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/1111-1160x587.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1111-400x202.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1111-768x389.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1111-1536x777.png.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/1111.png.webp 1678w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>نوع دیگری نیز از هارد وسترن آبی 4 ترابایتی در بازار مدلی با پارت نامبر WD40EZAZ است، که این مدل از نوع SMR بوده و اتفاقا 256 مگابایت کش دارد! قیمت این هارد دیسک در آمازون 75 دلار است.</p>



<p>هر دوی این هارد دیسک‌ها از خانواده وسترن آبی و هر دو یک حجم دارند، اما این کجا و آن کجا. بسیاری از کاربران ممکن است در نگاه اول حتی گول ظاهر فریبنده کش 256 مگابایتی هارد دیسک WD40EZAZ را خورده و آن را خریداری کنند!</p>



<p>اما واقعیت آن است که WD40EZRZ هارد دیسک به مراتب بهتری است. مدل WD40EZAZ با 256 مگابایت کش، در دنیا تقریبا نصف قیمت WD40EZRZ با 64 مگابایت کش را دارد! پس داستان بر سر برتری ذاتی WD40EZRZ به دلیل CMR بودن آن است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x488"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ4OCIgdmlld0JveD0iMCAwIDExNjAgNDg4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""488"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/PPPP-1160x488.png.webp"" alt="""" class=""wp-image-252201"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/12/PPPP-1160x488.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PPPP-400x168.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PPPP-768x323.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PPPP-1536x647.png.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/12/PPPP-2048x862.png.webp 2048w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>به هنگام خرید هارد دیسک، حتما نوع CMR و یا SMR بودن آن را تحقیق کنید. هارد دیسک‌های SMR به دلیل ضعف ذاتی و ساختاری، صرفا به درد سازمان‌ها و مراکزی که تنها استفاده آرشیوی از هارد دیسک دارند می‌خورند.</p>



<p>اما مدلهای CMR برای کاربران خانگی و حرفه‌ای مناسب تر هستند. هارد دیسک‌های CMR لزوما ممکن است سریعترین نباشند اما به عنوان یک هارد دیسک، سرعت مطلوبی دارند و قطعا از هارد دیسک‌های SMR سریعتر هستند و در نوشتار تاخیر اضافی و کندی سرعت ندارند.</p>



<h2>نتیجه گیری</h2>



<p>متاسفانه در ایران، به دلیل کم اطلاع بودن کاربران، هارد دیسک‌های نوع SMR مانند WD40EZAZ را هم قیمت و بعضا حتی گرانتر از هارد دیسک‌های CMR مانند WD40EZRZ به فروش می‌رسانند. از این رو اطلاع رسانی در مورد تفاوت فن آوری در شیوه نوشتار و تفاوت ساختاری این دو نوع از هارد دیسک‌ها، مرا بر آن داشت که این مقاله را برای تاکید بر اهمیت خرید هارد دیسک‌های CMR بنویسم و کاربران حرفه ای و خانگی را از تهیه هارد دیسک‌های SMR بر حذر دارم.</p>



<p>پیش از خرید پارت نامبر هارد را با دیتا شیت تولید کننده چک کنید و حتی الامکان هارد دیسک SMR نخرید.</p>

												
												
												
											
										</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 17,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "هوآوی,تلفن هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00278-1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "بررسی تخصصی گوشی نوا 8 هواوی",
                    Title = "بررسی گوشی نوا 8 هواوی – شبه پرچمدار جدید چینی",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 13,
                    Body = @"<div itemprop=""articleBody"" class=""entry-inner"">
											
<p>گوشی‌های سری نوا Nova &nbsp;هواوی همواره از محصولات جذاب این شرکت هستند که تحت عنوان “شبه پرچمدار” با مشخصات خوب و ظاهری جذاب راهی بازار می‌شوند. گوشی نوا 8 هواوی (Huawei Nova 8) یکی از جدیدترین محصولات این خانواده است که امروز تصمیم داریم بررسی آن را با شما به اشتراک بگذاریم.</p>



<p>باوجود آنکه هواوی ماه گذشته از گوشی‌های سری نوا 9 رونمایی کرد، پرونده نسل قبل این خانواده هنوز به طور کامل بسته نشده است. Nova 8 اواسط تابستان امسال برای بازار جهانی رونمایی شد. مانند دیگر اعضای این سری، نوا 8 از طراحی زیبا و مشخصات ارزشمندی بهره می‌برد. ترکیب این دو مشخصه آن را به گزینه جذابی برای خریداران تبدیل کرده است. از همین رو در بررسی امروز تصمیم داریم نگاه دقیق‌تری داشته باشیم به این گوشی و نقاط قوت و ضعیت آن</p>



<h2 id=""h-8"">بررسی تخصصی گوشی نوا 8 هواوی</h2>



<p>این بررسی حاصل جمع بندی تجربیات 7 روزه تیم سخت افزارمگ از گوشی Huawei Nova 8 است. نوا 8 توسط نمایندگی رسمی هواوی برای بررسی در اختیار وبسایت سخت‌افزار قرار گرفته است.</p>



<h3 id=""h-"">طراحی</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-1160x653.jpg.webp"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-1160x653.jpg.webp"" alt="""" class=""wp-image-246787 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00141.jpg.webp 1919w"" data-was-processed=""true""></figure>



<p>ظاهر گوشی و مشخصه‌هایی که در طراحی آن لحاظ شده، اولین نقطه‌ای است که توجه کاربر را به خود جلب می‌کند. بنابراین فرقی ندارد که یک گوشی ارزان قیمت باشد یا میان‌رده و پرچمدار، شرکت‌ها تلاش می‌کنند تا برای محصولات خود ظاهری جذاب طراحی کنند. گوشی نوا 8 هواوی در نگاه اول احساس یک گوشی باکیفیت و گران قیمت را منتقل می‌کند. دلیل آن هم خوش ساخت بودن آن، بالا بودن کیفیت و باریک و خودش دست بودن این گوشی است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-1160x653.jpg.webp"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-1160x653.jpg.webp"" alt="""" class=""wp-image-246788 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00271.jpg.webp 1919w"" data-was-processed=""true""></figure>



<p>باوجود آنکه در <a href=""https://consumer.huawei.com/en/phones/nova8/specs/"" target=""_blank"" rel=""noreferrer noopener"">صفحه مشخصات نوا 8 در وبسایت هواوی</a> اشاره‌ای به جنس بدنه آن نشده، به نظر می‌رسد که جنس قاب پشتی این گوشی از شیشه است. این شیشه ترکیبی از طراحی مات و براق دارد. بخش بزرگی از آن مات طراحی شده، اما در لبه‌های خمیده که به فریم دور دستگاه متصل می‌شوند، این بدنه به شدت براق است. در قسمت وسط پایین این قاب هم لوگوی nova و Huawei طراحی براقی دارند. رنگ مات و پیروی از ترند روز، بر زیبایی‌هایی این گوشی اضافه کرده. هرچند که این بدنه مستعد جذب اثر انگشت و لک است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-1160x653.jpg.webp"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-1160x653.jpg.webp"" alt="""" class=""wp-image-246789 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00142.jpg.webp 1919w"" data-was-processed=""true""></figure>



<p>نوا 8 هواوی تنها در رنگ Blush Gold راهی بازار خواهد شد. این رنگ معادل فارسی ندارد و به یک رنگ خاص اشاره ندارد. در یک شرایط نوری ثابت رنگ این گوشی نقره‌ای طلایی است. اما با تغییر زاویه نور رنگ این گوشی به آبی، زرد، صورتی، بنفش و … نیز تغییر خواهد کرد.</p>



<p>در سمت چپ بالای قاب پشتی محفظه‌ بزرگ بیضی شکلی برای دوربین‌های تعبیه شده است. 4 سنسور و یک فلش در این قسمت قرار گرفته‌اند. این محفظه برآمدگی نسبتا زیادی دارد و زمانیکه بر روی سطح صاف با گوشی کار کنید، در این ناحیه لق می‌زند. این برآمدگی دوربین‌ها را در معرض آسیب پذیری بیشتر قرار می‌دهد. بنابراین بهتر است که حتما در زمان کار با گوشی از قاب محافظی ژله‌ای که هواوی درون جعبه این گوشی قرار داده استفاده کنید و یا قاب‌های مستحکم‌تری را از بازار خریداری کنید.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-1160x653.jpg.webp"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-1160x653.jpg.webp"" alt="""" class=""wp-image-246790 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00138.jpg.webp 1919w"" data-was-processed=""true""></figure>



<p>فریم فلزی دور دستگاه در سمت راست و چپ خمیده و در بالا و پایین صاف طراحی شده است. خمیدگی دو طرف با وزن ایده‌ال 169 گرمی دستگاه باعث شده تا در دست گرفتن نوا 8 حس خوبی داشته باشد. با این حال، بدنه دستگاه کمی سُر است و بهتر است که با احتیاط و ترجیحا با قاب آن را حمل کنید. </p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143.jpg"" data-slb-active=""1"" data-slb-asset=""1073101196"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-1160x653.jpg.webp"" alt="""" class=""wp-image-246791"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00143.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-1160x653.jpg"" data-slb-active=""1"" data-slb-asset=""192220211"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-1160x653.jpg.webp"" alt="""" class=""wp-image-246792"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00144.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>
</div>



<p>در لبه سمت راست کلیدهای پاور و تنظیم صدا دستگاه قرار گرفته‌اند. لبه پایین جایگاه بلندگوی اصلی، پورت USB-C و میکروفون مکالمه است. لبه سمت چپ هیچ المان خاصی ندارد، اما در لبه بالای میکروفون حذف نویز و شیار سیم کارت‌ها قرار گرفته‌اند.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-1160x653.jpg"" data-slb-active=""1"" data-slb-asset=""308230228"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-1160x653.jpg.webp"" alt="""" class=""wp-image-246793"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00146.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147.jpg"" data-slb-active=""1"" data-slb-asset=""166147148"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-1160x653.jpg.webp"" alt="""" class=""wp-image-246794"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00147.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>
</div>



<p> در فاصله میان پنل جلو گوشی و فریم بالای دستگاه هم بلندگوی مکالمه قرار گرفته است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-1160x653.jpg.webp"" alt="""" class=""wp-image-246796"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00153.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>چیزی در حدود 90 درصد از پنل جلوی گوشی به نمایشگر دستگاه اختصاصی یافته است. این نمایشگر که از دو طرف خمیده است، حاشیه‌های باریکی در اطراف خود دارد. این حاشیه‌ها از آن جهت که متقارن طراحی شده‌اند، ظاهر زیبایی به نمای جلویی گوشی بخشیده‌اند. در قسمت وسط بالای نمایشگر حفره‌ای تعبیه شده که دوربین سلفی درون آن قرار گرفته است. این حفره کمی بزرگتر از آن چیزی است که در گوشی‌های بالارده و پرچمدار دیده‌ایم. با این حال، جایگاه قرارگیری آن (در وسط) برای ثبت تصاویر سلفی ایده‌ال است و مزاحمت خاصی در زمان تماشای محتوا ایجاد نمی‌کند.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00148.jpg.webp');""></div>



<p>نوا 8 هواوی دارای ابعاد 160.12×74.1×7.64 است. ضخامت کم با لبه‌های خمیده و نسبت ابعاد بلند، در زمان دست گرفتن نوا 8 به شما حس یک گوشی پریمیوم را القا خواهد کرد. به خصوص آنکه دو فاکتور مهم گوشی‌های بالارده یعنی حذف جک 3.5 میلیمتری هدفون و شیار کارت حافظه نیز در این گوشی رعایت شده است. فارغ از طراحی جذاب، این گوشی فاقد گواهینامه مقاومت در برابر آب و غبار است و بدنه شیشه‌ای و سر آن مستعد جذب اثر انگشت و آسیب پذیری. خوشبختانه با وجود قاب محافظ درون جعبه این گوشی می‌توان هر دو مورد مذکور را تا حد بسیار زیادی مرتفع کرد.</p>



<p></p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-1160x653.jpg.webp"" alt="""" class=""wp-image-246815"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00162.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h2 id=""h--1"">نمایشگر</h2>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-1160x653.jpg.webp"" alt="""" class=""wp-image-246798"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-ReviewDSC00155.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>هواوی برای گوشی نوا 8 از یک نمایشگر 6.57 اینچی OLED با رزولوشن 2340×1080 پیکسل (FHD+) و تراکم پیکسلی 392 ppi استفاده کرده است. در بخش تنظیمات نمایشگر به غیر از دقت تصویر مذکور که بالاترین حالت نمایش محتوا است، یک حالت Low (رزولوشن 1560×720) و یک حالت Smart در نظر گرفته شده که در حالت دوم رزولوشن نمایشگر به صورت پویا برای مصرف انرژی بهینه تنظیم می‌شود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-1160x653.jpg.webp"" alt="""" class=""wp-image-246814"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00152.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>یک نکته مثبت دیگر در خصوص این نمایشگر نرخ نوسازی بالاتر از حالت استاندارد، یعنی 90 هرتز است. فعال کردن این نرخ نوسازی بالاتر در تنظیمات باعث می‌شود تجربه کار با گوشی از گشتن در منوها تا وبگردی و … روان و سریع‌تر شود. حتی بازی‌هایی با فریم ریت بالاتر هم بر روی گوشی قابل اجرا خواهند بود. در تنظیمات نمایشگر بخشی تحت عنوان Screen Refresh Rate در نظر گرفته شده است که در آن می‌توانید نرخ نوسازی نمایشگر را بر روی حالت High یا همان 90 هرتز، استاندارد (60 هرتز) و یا داینامیک تنظیم کنید. در حالت داینامیک گوشی به صورت خودکار تعادلی میان روان بودن و مصرفه بهینه باتری برقرار خواهد کرد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-1160x653.jpg.webp"" alt="""" class=""wp-image-246799"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00160.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>به غیر از این در بخش تنظیمات نمایشگر امکانی برای تنظیم دستی و خودکار روشنایی، فعال کردن دارک مد، فیلتر نور آبی، حالت مطالعه و تنظیم رنگ و دمای نمایشگر &nbsp;در نظر گرفته شده است. در بخش Color Mode &amp; Temperature&nbsp; برای تنظیم رنگ امکان انتخاب میان دو حالت Normal و Vivid در نظر گرفته شده و برای تنظیم دما هم می‌توان به صورت دستی رنگ‌ها را تنظیم کرد و هم می‌تواند حالت گرم یا سرد را فعال کرد. علاوه‌بر این موارد امکان فعالسازی صفحه نمایش همیشه روشن نیز برای این نمایشگر در نظر گرفته شده است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"" style=""flex-basis:25%"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015321_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""1238049090"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015321_com.android.settings-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246847"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015321_com.android.settings-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015321_com.android.settings-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015321_com.android.settings-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015321_com.android.settings.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column is-vertically-aligned-center"" style=""flex-basis:50%"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015314_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""1974613452"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015314_com.android.settings-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246848"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015314_com.android.settings-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015314_com.android.settings-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015314_com.android.settings-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015314_com.android.settings.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""2100885121"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246849"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1.jpg"" data-slb-active=""1"" data-slb-asset=""1220402582"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246850"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015347_com.android.settings-1.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"" style=""flex-basis:25%"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""58812271"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246851"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_015415_com.android.settings.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>



<p>در مجموع نمایشگر خمیده OLED نوا 8 از کیفیت بالایی در مقایسه با هم سطحان خود برخوردار است. رنگ‌ها وضوح و دقت خوبی دارند و روشنایی دستگاه حتی در زیر نور مستقیم آفتاب هم رضایت بخش است و زاویه دید عالی دارد. نرخ نوسازی بالاتر تجربه نرم و روانی را در زمان استفاده از این نمایشگر رقم میزند و نکته مثبت نهایی ماجرا اینست که هواوی یک محفظ TPU به صورت پیش فرض بر روی آن قرار داده تا در حد توان خود از وارد شدن خط و خراش به نمایشگر جلوگیری کند.</p>



<h3 id=""h--2"">روش‌های احراز هویت</h3>



<p>هواوی دو روش احراز هویت بایومتریک برای این nova 8 در نظر گرفته است؛ اثر انگشت و تشخیص چهره. سنسور اثر انگشت این گوشی از نوع Under Display بوده و در زیر صفحه نمایش قرار گرفته است. برخلاف یکی دو نسل اول سنسورهای UD، این سنسور از دقت خوبی برخوردار است و به راحتی در اکثر شرایط انگشت شما را به درستی تشخیص داده و قفل دستگاه را باز می‌کند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-1160x653.jpg.webp"" alt="""" class=""wp-image-246800"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00191.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>عملیات تشخیص چهره با کمک دوربین سلفی دستگاه انجام می‌شود. این دوربین هم از سرعت بسیار خوب برای تشخیص چهره برخوردار است. هرچند که شاید بتوان نبود سنسور تشخیص عمق برای امنیت بالاتر تشخیص چهره را به عنوان یک ایراد در نظر گرفت. اما باید در نظر داشته باشید که این امکان حتی در بسیاری از گوشی‌های پرچمدار هم یافت نمی‌شود. بنابراین ایرادی به تشخیص چهره دو بعدی نوا 8 هواوی نمی‌توان وارد دانست.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-1160x653.jpg.webp"" alt="""" class=""wp-image-246801"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00186.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h3 id=""h--3"">صدا</h3>



<p>بلندگوی اصلی دستگاه که در لبه پایینی قرار گرفته وظیفه پخش صدا را بر عهده دارد. متأسفانه صدای ارائه شده در این گوشی از نوع استریو نیست. اما با این حال، از بلندی خوبی برخوردار است. مانند دیگر گوشی‌های هوشمند بازار، اسپیکر نوا 8 در پخش صدای ریز عملکرد خوبی دارد، اما پخش صدای بم نقطه قوت آن به حساب نمی‌آید. متأسفانه همانطور که در بخش طراحی اشاره کردیم، در این گوشی خبری از جک هدفون نیست. بنابراین برای استفاده از هدفون سیمی شخصی خودتان با این گوشی به یک تبدیل USB-C نیاز خواهید داشت.</p>



<h3 id=""h--4"">سخت افزار</h3>



<p>هواوی برای گوشی شبه پرچمدار خود از چیپست ساخت خود در شرکت HiSillicon یعنی Kirin 820E استفاده کرده است. این تراشه 7 نانومتری یکی از عجیب‌ترین معماری‌های موجود در بازار را دارد. در شرایطی که این روزها حتی گوشی‌های اقتصادی و ارزان قیمت از پردازنده‌های 8 هسته‌ای بهره می‌برند، کرین 820E هواوی 6 هسته است. سه هسته این تراشه Cortex-A76 با فرکانس 2.22 گیگاهرتز است و 3 هسته دیگر آن Cortex-A55 با فرکانس 1.84 گیگاهرتز. پردازنده گرافیکی این تراشه هم Mali-G57 شش هسته‌ای است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-1160x653.jpg.webp"" alt="""" class=""wp-image-246802"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00270.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>هواوی گوشی نوا 8 را تنها در یک نسخه با 8 گیگابایت حافظه رم و 128 گیگابایت حافظه ذخیره سازی راهی بازار می‌کند. بنابراین در زمان انتخاب این گوشی مانند رنگ، نیازی به این ندارید که انتخاب کنید کدام گزینه برای شما مناسب‌تر است. هم رم و هم حافظه برای یک گوشی میان‌رده اندازه به نظر می‌رسند. با این حال، اگر هواوی امکان پشتیبانی از کارت حافظه را برای این گوشی در نظر می‌گرفت، همه چیز می‌توانست زیباتر شود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-1160x653.jpg.webp"" alt=""Huawei nova 8 Review"" class=""wp-image-246853"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00294.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در بررسی گوشی نوا 8 هواوی عملکرد سخت‌افزاری در تست‌های بنچمارک بهترین نبود. در تست بنچمارک‌هایی مانند3DMark، &nbsp;GeekBench و Antutu پایین‌تر از رقیبان هم سطح خود بود. حتی در این تست‌ها گوشی‌هایی از شرکت‌های رقیب با قیمت‌های پایین‌تر نیز بودند که امتیازات بهتری را کسب کرده باشند. با این حال، عملکرد نوا 8 در تست بنچمارک حافظه، AndroBench کمی بهتر از رقبا بود.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122004_com.futuremark.dmandroid.application.jpg"" data-slb-active=""1"" data-slb-asset=""740057256"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122004_com.futuremark.dmandroid.application-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246839"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122004_com.futuremark.dmandroid.application-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122004_com.futuremark.dmandroid.application-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122004_com.futuremark.dmandroid.application-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122004_com.futuremark.dmandroid.application.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122726_com.primatelabs.geekbench5.jpg"" data-slb-active=""1"" data-slb-asset=""945345371"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122726_com.primatelabs.geekbench5-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246840"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122726_com.primatelabs.geekbench5-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122726_com.primatelabs.geekbench5-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122726_com.primatelabs.geekbench5-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_122726_com.primatelabs.geekbench5.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_130203_com.antutu.ABenchMark.jpg"" data-slb-active=""1"" data-slb-asset=""1917130585"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_130203_com.antutu.ABenchMark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246841"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_130203_com.antutu.ABenchMark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_130203_com.antutu.ABenchMark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_130203_com.antutu.ABenchMark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_130203_com.antutu.ABenchMark.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2.jpg"" data-slb-active=""1"" data-slb-asset=""1102088790"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246842"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_134222_com.andromeda.androbench2.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120156_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1372790938"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120156_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246835"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120156_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120156_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120156_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120156_com.futuremark.pcmark.android.benchmark.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120534_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1653781250"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120534_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246836"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120534_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120534_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120534_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120534_com.futuremark.pcmark.android.benchmark.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120612_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""865183952"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120612_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246837"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120612_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120612_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120612_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_120612_com.futuremark.pcmark.android.benchmark.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_121106_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1443347447"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_121106_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246838"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_121106_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_121106_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_121106_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_121106_com.futuremark.pcmark.android.benchmark.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_.jpg"" data-slb-active=""1"" data-slb-asset=""975033820"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246843"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_140845_com.basemark.basemarkgpu.free_.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_.jpg"" data-slb-active=""1"" data-slb-asset=""106242002"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246844"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_141135_com.basemark.basemarkgpu.free_.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>



<p>با این حال، برای درک بهتر عملکرد سخت‌افزاری گوشی نوا 8 به سراغ انجام تست‌های عملی مانند بازی، دوربین و … رفتیم. کالاف دیوتی موبایل که یکی از محبوب‌ترین بازی‌ها در گوشی‌های هوشمند به حساب می‌آید، به راحتی بر روی این گوشی نصب و اجرا شد. این بازی با کیفیت گرافیک متوسط و فریم ریت حداکثر در نوا 8 اجرا می‌شود. با وجود اجرا نرم و روان بازی، این نکته که حالت گرافیکی High برای آن در نظر گرفته نشده، جای تأمل داشت.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-1160x653.jpg.webp"" alt="""" class=""wp-image-246803"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00165.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>بازی پابجی موبایل نیز در گوشی نوا 8 با گرافیک HDR و فریم ریت اولترا قابل اجرا است. البته هنوز امکان اجرای 90 فریم PUBG بر روی این گوشی در نظر گرفته نشده است، اما می‌توان امیدوار بود که با وجود نمایشگر 90 هرتز، این امکان با بروزرسانی به nova 8 اضافه شود.</p>



<p></p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-1160x653.jpg.webp"" alt="""" class=""wp-image-246804"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00175.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<ul><li><a href=""https://sakhtafzarmag.com/%d9%be%d8%a7%d8%a8%d8%ac%db%8c-%d9%86%d8%b1%d8%ae-90-%d9%81%d8%b1%db%8c%d9%85-%d8%a8%d8%b1-%d8%ab%d8%a7%d9%86%db%8c%d9%87-%d8%b1%d9%88-%da%af%d9%88%d8%b4%db%8c-%d9%87%d9%88%d8%a7%d9%88%db%8c/"">پابجی را روی این گوشی های هواوی با نرخ 90 فریم بر ثانیه بازی کنید</a></li></ul>



<p>سخت‌افزاری که هواوی برای نوا 8 در نظر گرفته، برای اجرای بازی‌های جدید با کیفیتی خوب، مناسب است. البته از نظر قدرت این گوشی شاید در برابر رقیبان خود عملکردی پایین‌تری داشته باشد. با این حال، نوا 8 هواوی به خوبی از پس از اجرای تمامی عملیات روزانه مانند وبگردی، وقت گذارنی در شبکه‌های اجتماعی و پیام‎رسان‌ها، استفاده از دوربین، تماشای فیلم و ویدئو، برقراری تماس و ارسال مسیج بر می‌آید و در هیچکدام از آنها شاهد کندی، لگ و افت سرعت نخواهید بود. حتی در زمان انجام بازی هم گوشی از ناحیه پشت اندکی گرم می‌شود، اما این گرما نه آنقدر محسوس است و نه آزاردهنده.</p>



<p>نوا 8 از نسل جدید شبکه موبایلی یعنی 5G پشتیبانی نمی‌کند، اما نسخه 5G آن نیز با تراشه‌ای متفاوت در بازار به فروش می‌رسد. نسخه در دست بررسی دارای مودم LTE بوده و از Wi-Fi 802.11 a/b/g/n/ac، بلوتوث 5.0 و NFC پشتیبانی می‌کند. برای این گوشی همچنین سنسورهای &nbsp;مجاورتی، نور محیطی، قطب نمای دیجیتال، جاذبه، ژیروسکوپ و البته اثر انگشت در نظر گرفته شده است.</p>



<h3 id=""h--5"">نرم افزار</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-1160x653.jpg.webp"" alt="""" class=""wp-image-246806"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00150.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>نوا 8 هواوی دارای رابط کاربری EMUI 12.0.0 است. باوجود آنکه واضح مشخص است این نرم‌افزار بر پایه اندروید طراحی شده، در هیچ نقطه از گوشی به این موضوع اشاره نشده و مشخص نیست که با چه نسخه‌ای از اندروید سر و کار داریم. مانند تمام گوشی‌های جدید هواوی، نوا 8 هم فاقد سرویس‌های موبایلی گوگل (GMS) است. این یعنی برنامه‌هایی مانند گوگل پلی استور، گوگل پلی سرویسز، گوگل پلی گیم، کروم، یوتیوب، جیمیل، ترنسلیت و … به طور پیش‌فرض بر روی گوشی نصب نیستند.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00154.jpg.webp');""></div>



<p>هواوی اما حالا ترومای نداشتن سرویس‌های گوگل را پشت سر گذشته و توانسته با توسعه HMS Core و جایگزین کردن آن با سرویس گوگل خود را وارد رقابت جدی با بزرگان توسعه نرم‌افزار و سیستم عامل کرده است. البته جایگزینی GMS با HMS به معنای خداحافظی با تمامی برنامه‌های گوگل در گوشی‌های هواوی نیست. بسیاری از این برنامه‌ها از مارکت‌های دیگر به غیر از پلی استور قابل دریافت هستند و از برخی از آنها نیز می‌توانید در نسخه وب استفاده کنید. همچنین برای برخی از آنها نیز جانشینانی وجود دارد که با استفاده از آنها نیازی به نسخه گوگلی برنامه‌ها پیدا نخواهید کرد.</p>



<p>App Gallery، فروشگاه اصلی گوشی‌های هواوی است که به طور پیش‌فرض در این محصولات نصب شده و می‌توان برنامه‌های مورد نیاز را از طریق آن دانلود و نصب کرد. تعداد برنامه‌های موجود در این فروشگاه روزبه‌روز بالاتر می‌رود و رفته رفته تبدیل به یک مارکت کامل تبدیل می‎شود. با این وجود کماکان ممکن است برنامه‌ای در اپ گالری موجود نباشد. مارکت‌های دیگری مانند کافه بازار و APKPure هم در راستای همکاری با این شرکت، شرایطی را فراهم کردند تا گوشی‌های هواوی با سرویس HMS بتوانند برنامه‌های مورد نیاز خود را بدون مشکل نصب کنند.در کنار تمام این موارد موتور جستجوی اختصاصی هواوی، یعنی Petal Search نیز می‌تواند به کاربران در یافتن برنامه‌های مورد نیاز خود کمک کند. باید اعتراف کنیم که پتال از همه گزینه‌ها انتخاب بهتری است و به سادگی هر چه تمام برنامه‌های مورد نیاز شما را پیدا خواهد کرد.</p>



<p>در بررسی گوشی‌های قبلی هواوی این تجربه را داشتیم که برخی از برنامه‌های فارسی زبان که از APIهای گوگل برای موقعیت مکانی استفاده می‌کنند، به خوبی بر روی گوشی‌های هواوی اجرا نمی‌شوند. هرچند که به نظر می‌رسد با آپدیت‌هایی این مورد برای سرویس HMS برطرف شده، اما متأسفانه در بررسی گوشی نوا 8 هواوی دوباره به مشکل سابق برخوردیم و موفق نشدیم دو برنامه اسنپ و تپسی را بر روی آن اجرا کنیم. باوجود اجرای بدون مشکل برنامه‌های فارسی پرطرفدار بر روی این گوشی، عدم امکان استفاده از سرویس‌های تاکسی آنلاین کمی ناامیدکننده است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger.jpg"" data-slb-active=""1"" data-slb-asset=""1481649191"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger-473x1024.jpg.webp"" alt=""اسنپ گوشی نوا 8 هواوی"" class=""wp-image-246830"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224351_cab.snapp_.passenger.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger.jpg"" data-slb-active=""1"" data-slb-asset=""1108813233"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger-473x1024.jpg.webp"" alt=""تپسی گوشی نوا 8 هواوی"" class=""wp-image-246831"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211031_224441_taxi.tap30.passenger.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch.jpg"" data-slb-active=""1"" data-slb-asset=""2065362273"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch-473x1024.jpg.webp"" alt=""اسنپ گوشی نوا 8 هواوی"" class=""wp-image-246829"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_213442_com.huawei.hwsearch.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>



<p>به صورت کلی در مقایسه گوشی‌هایی که پیش از این از برند هواوی بررسی کرده بودیم، در نوا 8 و نرم افزار آن تکامل را به خوبی حس می‌کنیم. ایرادات موجود در نسخه‌های قبلی برطرف شده و همه چیزی آسان‌تر در دسترس قرار دارد. شاید یکی از ایراداتی که هنوز به قوت خود باقی است و هواوی احتمالا راهی برای آن ندارد، عدم دسترسی به اکانت بازی‌هایی است که کاربران با حساب گوگل خود سینک کرده‎اند. با این وجود در صورتیکه کاربران بتوانند اکانت‌های بازی خود را همزمان به دیگر حساب‌های خود مانند فیسبوک، توئیتر و یا شرکت‌های بازی‌ساز مانند سوپرسل متصل کنند، دسترسی به این اکانت‌ها بر روی گوشی‌‌های هواوی به هیچ عنوان چالشی نخواهد بود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-1160x653.jpg.webp"" alt="""" class=""wp-image-246854"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00291.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>ظاهر رابط کاربری EMUI در نسخه 12 چندین تغییر مثبت کرده است. یکی از آنها مجاز شدن منوهای Quick Setting بالای نمایشگر است که حالا با سوایپ به سمت پایین از سمت راست منوی تنظیمات و از سمت چپ به منوی اعلانات و نوتفیکیشن‌ها دسترسی خواهید داشت. این شبیه به آن چیزی است که پیش‌تر در گوشی‌های آیفون و اخیرا در گوشی‌های شیائومی دیده بودیم.</p>



<p>امکانات معمول دیگری که این روزهای در اکثر گوشی‌های هوشمند شاهد آنها هستیم مانند تم تاریک یکپارچه در محیط گوشی، اضافه شدن منوی کشویی به گوشی، ژست‌های حرکتی و … در نوا 8 هواوی نیز در نظر گرفته شده است. Digital Balance؛ جهت کنترل زمان استفاده از گوشی برای توسط شخص بزرگسال یا کودکان و App Assistant ؛ برای بالابردن عملکرد بازی، مسدود کردن اعلانات و … 2 امکان کاربردی دیگر در محیط نرم‌افزار این گوشی هستند.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher.jpg"" data-slb-active=""1"" data-slb-asset=""515321175"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246832"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143038_com.huawei.android.launcher.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher.jpg"" data-slb-active=""1"" data-slb-asset=""1665536560"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246833"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_143032_com.huawei.android.launcher.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol.jpg"" data-slb-active=""1"" data-slb-asset=""227606232"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol-473x1024.jpg.webp"" alt=""تنظیمات گوشی نوا 8 هواوی"" class=""wp-image-246834"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_142914_com.huawei.parentcontrol.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>



<p>به صورت کلی پیشرفت نرم‌افزاری هواوی در گوشی نوا 8 بیشتر از سطح انتظارات ما بود. سازگاری بیشتر برنامه‌ها و حذف ایرادات موجود در برنامه‌ها تجربه‌ نرم‌افزاری خوبی در این گوشی برای ما به ارمغان آورد و می‌توان امیدوار بود که با بروزرسانی‌های آینده، مشکلاتی که در برنامه‌های تاکسی آنلاین به آن برخوردیم نیز برطرف شود.</p>



<h3 id=""h--6"">دوربین</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-1160x653.jpg.webp"" alt="""" class=""wp-image-246807"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00277.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در محفظه بزرگ بیضی شکل قاب پشت گوشی، هواوی 4 سنسور به همراه یک فلش LED قرار داده است. در یک سمت این محفظه عبارت Ultra HD Sensor و در سمت دیگر آن AI Quad Camera نوشته است. سنسور اصلی این دوربین‌ها 64 مگاپیکسلی با گشادگی دیافراگم f/1.9 است. دوربین فوق عریض که&nbsp; بالاترین سنسور در این چیدمان بوده، 8 مگاپیکسلی با f/2.4 است. دو سنسور دو مگاپیکسلی دیگر با f/2.4 یکی از نوع ماکرو برای عکاسی از فواصل نزدیک و دیگری برای تشخیص عمق دوربین اصلی را همراهی می‌کنند.</p>



<p>نرم‌افزار دوربین به سبک همیشگی گوشی‌های هواوی طراحی شده است. در صفحه اصلی گزینه‌ای برای انتخاب میان دوربین فوق عریض، دوربین اصلی و زوم 2x، خاموش کردن قابلیت هوش مصنوعی، روشن و خاموش کردن فلش، گذاشتن فیلتر به صورت زنده بر روی دوربین و ورود به بخش تنظیمات در نظر گرفته شده است. در نوار پایینی صفحه هم می‌توان حالت مختلف عکاسی و فیلمبرداری را انتخاب کرد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-1160x653.jpg.webp"" alt="""" class=""wp-image-246808"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00272.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در بخش تنظیمات امکان تغییر نسبت تصاویر، فعال کردن فرمان صوتی، ژست‌های حرکتی، تشخیص لبخند، تنظیم کیفیت فیلمبرداری، تایمر و … در نظر گرفته شده است. هواوی حالت‌های متنوعی از عکاسی و فیلمبرداری را برای دوربین این گوشی در نظر گرفته است که از جمله آنها می‌توان به Aperture (تار کردن پس‌زمینه سوژه)، Night (عکاسی شب)، Pro (تنظیمات دستی)، Super Macro (عکاسی از فاصله نزدیک)، HIGH-RES (در حالت 64 مگاپیکسلی)، پانوراما و Dual View (فیلمبرداری همزمان 1x و 2x) اشاره کرد. &nbsp;</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""882724847"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera-473x1024.jpg.webp"" alt=""تنظیمات دوربین گوشی نوا 8 هواوی"" class=""wp-image-246828"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014201_com.huawei.camera.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""695315112"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera-473x1024.jpg.webp"" alt=""تنظیمات دوربین گوشی نوا 8 هواوی"" class=""wp-image-246827"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014153_com.huawei.camera.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""1828075553"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera-473x1024.jpg.webp"" alt=""تنظیمات دوربین گوشی نوا 8 هواوی"" class=""wp-image-246826"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014141_com.huawei.camera.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""1360736800"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera-473x1024.jpg.webp"" alt=""تنظیمات دوربین گوشی نوا 8 هواوی"" class=""wp-image-246825"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211101_014126_com.huawei.camera.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>
</div>
</div>



<p>دوربین اصلی 64 مگاپیکسلی دستگاه به صورت پیش‌فرض از فناوری ترکیب پیکسلی 4 به 1 بهره می‌برد و خروجی این دوربین 16 مگاپیکسلی است. البته امکان ثبت تصاویر در حالت 64 MP نیز وجود دارد، اما از آنجاییکه جزئیات بسیار خوبی به خصوص در شرایط کم‌نور در حالت 16 مگاپیکسلی به ثبت می‌رسد، نیاز چندانی حالت HIGH-RES پیدا نخواهید کرد. این دوربین تقریباً در تمام شرایط نوری تصاویر خوبی ثبت می‌کند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-1160x653.jpg.webp"" alt=""تنظیمات دوربین گوشی نوا 8 هواوی"" class=""wp-image-246809"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00266.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>برای &nbsp;محیط‌هایی که نور به اندازه کافی نیست هواوی حالت عکاسی Night را برای سنسور اصلی در نظر گرفته است. کسب نتیجه مطلوب با این سنسور باز هم به میزان نور بستگی دارد و در صورتیکه تاریکی مطلق باشد، عملکرد فوکوس چندان دقیق نیست و تصاویر خروجی تار خواهد شد. اما باوجود روشنایی هرچند کم، تصاویری که سنسور اصلی ثبت می‌کند از کیفیت قابل قبولی برخوردار است، به شرط آنکه در زمان ثبت عکس، گوشی به هیچ وجه تکان نخورد.</p>



<p>این دوربین در حالت عکاسی پرتره و &nbsp;Aperture عملکرد بسیار خوبی دارد و محیط اطراف سوژه در سطح خوب و با دقت قابل قبولی تار می‌کند. این موضوع تا حد زیادی مدیون دوربین 2 مگاپیکسلی تشخیص عمق و هوش مصنوعی دستگاه است. هواوی برای دوربین نوا 8 امکان فیلمبرداری 4K را نیز در نظر گرفته شده است.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00273.jpg.webp');""></div>



<p>دوربین 8 مگاپیکسلی فوق عریض دستگاه برای ثبت تصاویر با دامنه دید گسترده‌تر مورد استفاده قرار می‎گیرد. در هنگام ثبت تصاویر با این دوربین در گوشه‌های تصویر شاهد اعوجاج بسیار زیادی خواهید بود، اما پس از ثبت تصاویر هوش مصنوعی به کمک خواهد آمد و تصاویر را به حالت نرمال و بدون اعوجاج تبدیل خواهد کرد. این دوربین در شرایطی که نور کافی برای عکاسی محیا باشد، تصاویر خوبی به ثبت می‌رساند. جنس رنگ تصاویر این دوربین چندان نزدیک به دوربین اصلی نیست. از طرف دیگر از آنجاییکه حالت عکاسی Night برای آن در نظر گرفته نشده، نمی‌توانید بر روی کیفیت عکس شب آن حساب ویژه‌ باز کنید.</p>



<p>دوربین 2 مگاپیکسلی ماکرو این گوشی برای عکاسی در حالت Super Macro از فاصله نزدیک (فاصله بهینه 4 سانتیمتری) مورد استفاده قرار می‌گیرد. مانند دوربین فوق عریض، کسب نتیجه مطلوب در این دوربین ربط مستقیمی به شرایط نوری و البته میزان حوصله شما دارد. عکاسی با دوربین ماکرو از سوژه‌های نزدیک و یافتن نقطه فوکوس مانند دیگر دوربین‌های 2 مگاپیکسلی چندان ساده نیست. از همین رو، ثبت تصاویر با دوربین اصلی گوشی و کراپ آن به اندازه دلخواه نه تنها راه آسان‌تری است، بلکه عکس باکیفیت‎تری نیز ارائه خواهد کرد.</p>



<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_144920-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""1908495250"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_144920-rotated.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246867"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_144920-rotated.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_144920-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_144920-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>ماکرو</figcaption></figure>



<p>دوربین سلفی 32 مگاپیکسلی نوا 8 از کیفیت بسیار خوبی برخوردار است. این دوربین هم با کمک فناوری ترکیب پیکسلی تصاویر را به صورت پیش فرض به صورت 8 مگاپیکسلی به ثبت می‌رساند. تصاویر ثبت شده با این دور در شرایط نوری مناسب از کیفیت بسیار خوبی برخوردار است. حالت عکاسی پرتره به خوبی و با دقت مناسبی پس زمینه را تار خواهد کرد. در محیط تاریک و شب که نورهای محیطی کمتر از حالت روز هستند، حالت عکاسی شب دوربین سلفی نوا 8 تصاویر خوب با حداقل میزان نویز را به ثبت می‌رساند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-1160x653.jpg.webp"" alt="""" class=""wp-image-246811"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00268.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>هواوی برای گوشی شبه پرچمدار خود دوربین‌های متنوعی در نظر گرفته است. دوربین اصلی، فوق عریض و سلفی در شرایط نوری مناسب عکس‌های خوبی به ثبت می‌رسانند. عملکرد دوربین اصلی و سلفی در محیط‌های تاریک و شب نیز رضایت بخش است. اما دوربین فوق عریض تصاویر چندان باکیفیتی در محیط‌های تاریک به ثبت نمی‌رساند. در نظر گرفتن حالت عکاسی Night برای دوربین فوق عریض می‌توانست کیفیت عکاسی در این گوشی را بالاتر از آن چیزی که هست ببرد.</p>



<h4 id=""h-8-1"">نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی</h4>



<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_133423.jpg"" data-slb-active=""1"" data-slb-asset=""953143819"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_133423.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246856"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_133423.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_133423-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_133423-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a></figure>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002951.jpg"" data-slb-active=""1"" data-slb-asset=""543652685"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002951.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246857"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002951.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002951-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002951-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>پرتره شب</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135630.jpg"" data-slb-active=""1"" data-slb-asset=""721609168"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135630.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246858"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135630.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135630-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135630-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>پرتره روز</figcaption></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002913.jpg"" data-slb-active=""1"" data-slb-asset=""1999457499"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002913.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246859"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002913.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002913-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002913-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>حالت Pro</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002806.jpg"" data-slb-active=""1"" data-slb-asset=""1615186851"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002806.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246860"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002806.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002806-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_002806-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>حالت نرمال</figcaption></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150118.jpg"" data-slb-active=""1"" data-slb-asset=""913368558"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150118.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246861"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150118.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150118-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150118-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>64 مگاپیکسلی</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150109.jpg"" data-slb-active=""1"" data-slb-asset=""1169702898"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150109.jpg.webp"" alt="""" class=""wp-image-246862"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150109.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150109-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_150109-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>12 مگاپیکسلی</figcaption></figure>
</div>
</div>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140331.jpg"" data-slb-active=""1"" data-slb-asset=""167640429"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140331.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246863"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140331.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140331-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption>پرتره سلفی روز</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140305.jpg"" data-slb-active=""1"" data-slb-asset=""9309330"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140305.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246864"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140305.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_140305-300x400.jpg 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption>سلفی روز</figcaption></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211029_221851.jpg"" data-slb-active=""1"" data-slb-asset=""93617716"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211029_221851.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246865"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211029_221851.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211029_221851-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption>پرتره سلفی شب</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_003339.jpg"" data-slb-active=""1"" data-slb-asset=""68637109"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_003339.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گوشی نوا 8 هواوی"" class=""wp-image-246866"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_003339.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211028_003339-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption>سلفی شب – Night</figcaption></figure>
</div>
</div>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135506.jpg"" data-slb-active=""1"" data-slb-asset=""626112331"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135506.jpg.webp"" alt="""" class=""wp-image-246868"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135506.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135506-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135506-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>فوق عریض</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135502.jpg"" data-slb-active=""1"" data-slb-asset=""436966059"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135502.jpg.webp"" alt="""" class=""wp-image-246869"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135502.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135502-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135502-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>1x</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135512.jpg"" data-slb-active=""1"" data-slb-asset=""321045670"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135512.jpg.webp"" alt="""" class=""wp-image-246870"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135512.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135512-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/IMG_20211101_135512-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption>2x</figcaption></figure>
</div>
</div>



<h3 id=""h--7"">باتری</h3>



<p>قابل پیش بینی است که برای ضخامت کم این گوشی و وزن سبک آن، باتری دستگاه مجبور به فداکاری شده است. هواوی برای نوا 8 یک باتری 3800 میلی آمپرساعتی در نظر گرفته است. در مقایسه با گوشی‌های هم سطح که از باتری‌هایی با ظرفیت بالای 4000 بهره می‌برند، این رقم چندان چشمگیر نیست. اما خوشبختانه مدیریت مصرف انرژی در این گوشی به گونه‌ایست که باتری می‌تواند تا یک روز کامل شارژدهی داشته باشد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-1160x653.jpg.webp"" alt="""" class=""wp-image-246812"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00156.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>نتایج کسب شده در تست بنچمارک باتری PC Mark هم مانند استفاده روزمره، ربط مستقیمی به میزان رزولوشن نمایشگر و ریفرش ریت آن دارد. در رزولوشن High و نرخ نوسازی 90 هرتز نوا 8 زمان 13 ساعت و 9 دقیقه را ثبت کرد. در رزولوشن Low و نرخ نوسازی 60 هرتز زمان 15 ساعت و 27 دقیقه (2 ساعت و 14 دقیقه بیشتر) ثبت شد. در حالت Smart Resolution و نرخ نوسازی Dynamic هم زمانی میان دو حالت بالا و پایین، یعی 14 ساعت و 8 دقیقه به ثبت رسید.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1552985871"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246823"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211029_114829_com.futuremark.pcmark.android.benchmark.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1200221071"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246822"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark-768x1664.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark-945x2048.jpg.webp 945w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211028_141816_com.futuremark.pcmark.android.benchmark.jpg.webp 1080w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_115028_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""48791980"" data-slb-internal=""0"" data-slb-group=""246783""><img data-lazyloaded=""1"" data-placeholder-resp=""473x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ3MyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""473"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_115028_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp"" alt=""نتایج بنچمارک گوشی نوا 8 هواوی"" class=""wp-image-246824"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_115028_com.futuremark.pcmark.android.benchmark-473x1024.jpg.webp 473w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_115028_com.futuremark.pcmark.android.benchmark-185x400.jpg.webp 185w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_115028_com.futuremark.pcmark.android.benchmark-709x1536.jpg.webp 709w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot_20211030_115028_com.futuremark.pcmark.android.benchmark.jpg.webp 720w"" data-sizes=""(max-width: 473px) 100vw, 473px""></a></figure>
</div>
</div>



<p>نتایج کسب شده توسط نوا 8 در بنچمارک نشان از عملکرد قابل رقابت این گوشی دارد. با تمام این اوصاف، میزان پایداری و دوام باتری ربط مستقیمی به الگوی مصرف دارد. در صورت استفاده طولانی مدت از دوربین یا انجام بازی با گوشی با روشنایی حداکثر و نرخ نوسازی بالا می‌توان انتظار داشت که باتری دستگاه در کمتر از یک روز تخلیه شود. اما با یک استفاده متعادل این گوشی در پایان روز کماکان شارژ کافی خواهد داشت.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-1160x653.jpg.webp"" alt="""" class=""wp-image-246855"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00240.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>هواوی برای این دستگاه شارژر فوق سریع 66 واتی با عنوان SuperCharge&nbsp; در نظر گرفته که می‌تواند در مدت حدودا 35 دقیقه باتری را از 0 تا 100 درصد شارژ کند. وجود چنین شارژری در جعبه نوا 8 هواوی که در کوتاهترین زمان ممکن می‌تواند باتری گوشی را به طور کامل شارژ کند، از نکات مثبتی است که این روزها به ندرت در گوشی‌های هوشمند بازار آن را می‌توان مشاهده کرد.</p>



<h3 id=""h--8"">جمع بندی</h3>



<p>نوا 8 هواوی یک گوشی میان‌رده/شبه پرچمدار است که از طراحی زیبایی بهره می‌برد. عدم وجود جک هدفون و شیار کارت حافظه از ایراداتی است که در بخش طراحی به این گوشی وارد است. با این وجود بدنه مات شیشه‌ای آن با لبه‌های خمیده و ضخامت کم ظاهر قشنگی به گوشی بخشیده است. ابعاد و وزن این گوشی کاملا ایده‌ال هستند و استفاده طولانی مدت از گوشی باعث خستگی نمی‌شود. هواوی نمایشگر OLED این گوشی با نرخ نوسازی 90 هرتز از کیفیت و روشنایی بسیار خوبی برخوردار است. سخت‌افزار این گوشی هم با وجود امتیازات نه چندان عالی در تست‌های بنچمارک عملکرد خوبی در کاربری روزانه و اجرای بازی‌ها با کیفیت متوسط دارد. سنسور اثر انگشت و تشخیص چهره گوشی هم عملکرد رضایت بخشی دارند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-1160x653.jpg.webp"" alt="""" class=""wp-image-246813"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-400x225.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-768x432.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-384x216.jpg 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-576x324.jpg 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Huawei-nova-8-Review-DSC00282.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>نرم‌افزار این گوشی با وجود فقدان سرویس‌های گوگل، توانسته از پس نیاز کاربر تا حد ممکن برآید. با این وجود همچنان ایراداتی در HMS و سازگاری آن با برخی از برنامه‌ها وجود دارد که می‌توان امیدوار بود با آپدیت‌های آینده برطرف شود. دوربین‌های متنوعی که هواوی برای نوا 8 در نظر گرفته در مجموع عملکرد قابل قبولی دارند. هرچند که عملکرد دوربین‌های فوق عریض و ماکرو می‌توانست بهتر باشد، اما دوربین سلفی و اصلی گوشی تقریبا در اکثر شرایط تصاویر خوبی را به ثبت می‌رسانند. در نهایت عملکرد خوب باتری این گوشی با شارژر فوق العاده آن یک ترکیب لذت بخش از کاربری گوشی را رقم می‌زنند.</p>



<p>نوا 8 هواوی در بازار ایران با قیمتی در حدود 11 میلیون و 500 هزار تومان به فروش می‌رسد. در این بازه قیمتی شبه پرچمدار هواوی رقیبان پرقدرتی از برندهای شیائومی و سامسونگ را پیش رو خواهد داشت. با این وجود نوا 8 مناسب برای افرادی خواهد بود که به یک گوشی نه چندان گرانقیمت و زیبا برای اجرای کارهای رومزه و معمول نیاز دارند و نبود سرویس‌های گوگل برای آنها دغدغه‌ای به حساب نمی‌آید.</p>



<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span>



<ul><li>طراحی زیبا و چشمگیر</li><li>وزن سبک</li><li>نمایشگر باکیفیت 90 هرتز</li><li>نرم‌افزار تند و سریع</li><li>دوربین‌های متنوع</li><li>شارژر فوق العاده سریع</li></ul>



</div>



<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span>



<ul><li>نداشتن جک 3.5 میلیمتری هدفون</li><li>عدم پشتیبانی از کارت حافظه</li><li>ایرادات نرم‌افزاری ناشی از نبود سرویس‌های گوگل در برنامه‌ها</li><li>عملکرد معمولی دوربین‌های کمکی</li><li>ظرفیت نه چندان زیاد باتری</li></ul>



</div>



<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span>



<p>نوا 8 هواوی مناسب برای افرادی خواهد بود که به یک گوشی نه چندان گرانقیمت و زیبا برای اجرای کارهای رومزه و معمول نیاز دارند و نبود سرویس‌های گوگل برای آنها دغدغه‌ای به حساب نمی‌آید.</p>



</div>



<span data-sr=""enter"" class=""gk-review clearfix""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.9""><span>9</span></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.89""><span>8.9</span></span><span class=""gk-review-partial-label"">سخت‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.93""><span>9.3</span></span><span class=""gk-review-partial-label"">صفحه نمایش</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.89""><span>8.9</span></span><span class=""gk-review-partial-label"">دوربین</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.85""><span>8.5</span></span><span class=""gk-review-partial-label"">صدا</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-partial-label"">طراحی</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-partial-label"">باتری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.87""><span>8.7</span></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 18,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "سامسونگ,تلفن هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28301-1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "بررسی گلکسی A52s سامسونگ",
                    Title = "بررسی گوشی گلکسی A52s سامسونگ – بهترین میان‌رده سامسونگ",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 13,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

< p > فکر می‌کنم این جمله را تقریبا در اول تمام بررسی‌هایی گوشی‌های Galaxy A سامسونگ گفته‌ام که در چند سال اخیر، گوشی‎‌های این خانواده به شدت محبوب و پرطرفدار شده‌اند.طراحی زیبا از یک طرف و ارائه مشخصات خوب و مشتری پسند در قالب گوشی‌های غیرپرچمدار دلیل اصلی محبوبیت این سری به حساب می‌آید.گوشی‌های سری Galaxy A50 مانند A52s اعضای میانی این خانواده هستند که از نظر مشخصات و قیمت در سطح میانی(بالاتر از A1، A2 و … و پایین‌تر از A7) قرار دارند.به‌طورکلی شاید بتوان اعضای سری A50 را از نظر اندازه، مشخصات و قیمت ایده‌آل‌ترین اعضای خانواده Galaxy A نامید.گوشی گلکسی A52s سامسونگ یکی از جدیدترین اعضای این سری است که امروز قصد بررسی آن را داریم.</ p >



< p > سامسونگ روز 17 آگوست(26 مرداد) یعنی درست 5 ماه بعد از معرفی Galaxy A52 از نسخه بروزرسانی شده این گوشی، یعنی Galaxy A52s رونمایی کرد.بر روی کاغذ تفاوت این دو گوشی چندان زیاد نیست و دقیقا به همین دلیل است که سامسونگ ترجیح داده به جای انتخاب نام جدید، تنها یک پسوند S را به انتهای نام گوشی اضافه کند.اما سامسونگ & nbsp; با همان تفاوت ناچیز روی کاغذ در عمل تأثیر محسوسی گذاشته، چرا که نمایشگر و مغز متفکر گوشی یعنی پردازنده را هدف قرار داده است.</ p >
  



  < h2 id = ""h-a52s"" > بررسی گلکسی A52s سامسونگ </ h2 >
       



       < figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-1160x653.jpg.webp"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-1160x653.jpg.webp"" alt="""" class=""wp-image-247970 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-29001.jpg.webp 1919w"" data-was-processed=""true""></figure>



<p>برخلاف نسخه استاندارد A52، گوشی A52s تنها در یک نسخه 5G راهی بازار می‌شود که البته با در نظر گرفتن سرعت پیشرفت این شبکه در کشور، شاید این موضوع چندان اهمیتی نداشته باشد.در هر حال، &nbsp;این بررسی حاصل جمع‌بندی تجربه 5 روزه تیم سخت‌افزار از گوشی گلکسی A52s &nbsp;سامسونگ است.این گوشی توسط نمایندگی محترم سامسونگ در ایران در اختیار وبسایت سخت‌افزارمگ قرار گرفته است.</p>



<h3 id = ""h-"" > طراحی </ h3 >




< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-1160x653.jpg.webp"" alt="""" class=""wp-image-247958"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19400.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>گلکسی A52 سامسونگ یکی از زیباترین گوشی‌های میان‌رده بازار به حساب می‌آید، خوشبختانه سامسونگ هم به خوبی از این موضوع مطلع است و در طراحی A52s به ترکیب تیم برنده دست نزده است.سامسونگ این گوشی را در 4 رنگ خفن(Awesome)؛ یعنی مشکی(Awesome Black)، سفید(Awesome White)، بنفش(Awesome Purple) و سبز(Awesome Mint) راهی بازار کرده است.نسخه‌ای که برای بررسی در اختیار ما قرار گرفته رنگ سفید خفن این گوشی بود که شاید بهترین رنگ این خانواده باشد.البته این موضوع به شدت سلیقه ایست و ممکن است نظر شما متفاوت از نظر بنده باشد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-1160x653.jpg.webp"" alt="""" class=""wp-image-247957"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25101.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>طراحی قاب پشتی این گوشی مات است و به غیر از گردی‌های سیاه رنگ دوربین همه چیز، حتی محفظه دوربین‌ها هم سفید طراحی شده است.این بدنه تک رنگ و یکپارچه سفید است و دیگر خبری از جلوه‌های رنگی مختلف در زوایای نوری متفاوت نیست. </p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19401-1160x773.jpg.webp"" alt="""" class=""wp-image-247969"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19401-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19401-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19401-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19401-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19401.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>جنس قاب پشتی A52s از پلی کربنات مقاوم(همان پلاستیک خودمان) است.به غیر از محفظه دوربین‌ها که برآمدگی نسبتا کمی دارند، اما همچنان باعث لق زدن گوشی بر روی سطح صاف می‌شوند، تنها یک لوگوی سامسونگ در قسمت پایین قاب قرار گرفته است.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20101.jpg.webp');""></div>



<p>مانند A52، این گوشی هم خمیدگی خاصی در لبه‌های قاب ندارد.طراحی صاف قاب پشتی، فریم و نمایشگر به گوشی حالت مکعبی شکلی داده است.هرچند که در دست گرفتن این گوشی شاید به اندازه گوشی‌های با لبه خمیده راحت‌تر نباشد اما با این طراحی میزان سُر بودن A52s بسیار کمتر است و به عقیده من گوشی شکل زیباتری دارد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-1160x653.jpg.webp"" alt="""" class=""wp-image-247960"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19501.jpg.webp 1920w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>فریم دور گوشی هم مانند قاب پشت، پلاستیکی به نظر می‌رسد.در قسمت سمت راست آن کلیدهای تنظیم صدا و کمی پایین‌تر کلید پاور قرار گرفته‌اند. در لبه بالایی در کنار میکروفون حذف نویز، شیار سیم‌کارت‌ها و کارت حافظه جای گرفته است. لبه چپ کاملا خالی است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-1160x653.jpg"" data-slb-active=""1"" data-slb-asset=""1354288620"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-1160x653.jpg.webp"" alt="""" class=""wp-image-247961"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19601.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701.jpg"" data-slb-active=""1"" data-slb-asset=""82681414"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-1160x653.jpg.webp"" alt="""" class=""wp-image-247962"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19701.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>
</div>



<p>در لبه پایین بلندگوی اصلی، میکروفون مکالمه، پورت USB-C و جک 3.5 میلیمتری هدفون&nbsp;جا خوش کرده‎اند.بلندگوی مکالمه نیز در فاصله میان حاشیه بالای نمایشگر و لبه بالایی دستگاه قرار دارد.تقریباً همه آن چیزی که یک گوشی هوشمند به آن نیاز دارد، مانند جک هدفون، شیار کارت حافظه و پورت USB-C برای گوشی A52s سامسونگ در نظر گرفته شده است.شاید تنها جای خالی فرستنده مادون قرمز در این گوشی حس شود که آن هم برای بسیاری کاربرد خاصی ندارد.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801.jpg"" data-slb-active=""1"" data-slb-asset=""1432043787"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-1160x653.jpg.webp"" alt="""" class=""wp-image-247963"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19801.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901.jpg"" data-slb-active=""1"" data-slb-asset=""1891392494"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-1160x653.jpg.webp"" alt="""" class=""wp-image-247964"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-19901.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>
</div>



<p>84.9 درصد از پنل جلوی گوشی به نمایشگر اختصاص یافته است.سنسور اثر انگشت گوشی در زیر این نمایشگر قرار گرفته و به غیر از حفره‌ای که در قسمت وسط بالای آن برای دوربین سلفی در نظر گرفته شده، حاشیه‌های کمی در اطراف آن به چشم می‌خورد. </p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-1160x653.jpg.webp"" alt="""" class=""wp-image-247965"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20801.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>بر روی کاغذ نسبت نمایشگر به بدنه این گوشی در مقایسه با A52 کمی بیشتر شده(84.1 در مقابل 84.9 درصد)، اما این تفاوت آنقدرها محسوس نیست و نیاز به دقت بالایی برای تشخیص دارد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-1160x653.jpg.webp"" alt="""" class=""wp-image-247966"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20001.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>گوشی گلکسی A52s سامسونگ دارای ابعاد 159.9×75.1×8.4 میلیمتر است و 189 گرم وزن دارد.نمایشگر این دستگاه دارای گوریلا گلس 5 است و با گواهینامه IP67 در برابر آب و غبار دارای مقاومت است.در مجموع به نسبت به یک گوشی میان‌رده، A52s همه آنچه که باید در طراحی یک گوشی مد نظر قرار بگیرد را به همراه دارد.از جک هدفون و کارت حافظه گرفته تا ابعاد ایده‌ال و وزن مناسب و گواهینامه مقاومت در برابر آب. </p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-1160x653.jpg.webp"" alt="""" class=""wp-image-247967"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21001.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>شاید تنها ایراد سختگیرانه‎‌ای که بتوان به این گوشی وارد دانست بدنه پلاستیکی دستگاه باشد که آن هم با در نظر گرفتن سیاست‎‌های سامسونگ(استفاده از قاب پلاستیکی در گوشی‌های پرچمداری مانند S21)، احتمالا انتظار بی‌موردی است.</p>



<h3 id = ""h--1"" > صفحه نمایش</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-1160x653.jpg.webp"" alt="""" class=""wp-image-247954"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23801.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>نمایشگر یکی از آن مواردی است که سامسونگ در گوشی A52s آن را نسبت به نسل قبل ارتقا داده است.این گوشی از &nbsp; یک نمایشگر 6.5 اینچی Super AMOLED &nbsp;با نسبت ابعاد 20:9 و رزولوشن 1080×2400 پیکسل &nbsp;(با تراکم پیکسلی 405 ppi) بهره می‌برد.تا اینجای کار این همان مشخصاتی است که در A52 دیده بودیم، اما تفاوت در این نقطه است که نمایشگر جدید دارای نرخ نوسازی بالاتر، یعنی 120 هرتز (در مقایسه با 90 هرتز A52) است.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21601.jpg.webp');""></div>



<p>استفاده از نمایشگر با ریفرش ریت بالاتر تجربه کاربری و استفاده از گوشی را یک سطح بالاتر برده است.این موضوع را نه تنها در بازی‌هایی که از فریم ریت بالاتر پشتیبانی می‌کنند می‌توان دریافت، بلکه حتی در استفاده روزمره، گشت زدن در منوهای گوشی و وبگردی نیز محسوس است. &nbsp;</p>



<p>در بخش تنظیمات نمایشگر، در قسمت Motion Smoothness دو گزینه Standard و High در نظر گرفته شده‌اند که با فعال کردن گزینه High، نرخ نوسازی بر روی 120 هرتز تنظیم خواهد شد و در حالت استاندارد بر روی 60 هرتز.از آنجاییکه این گوشی برخلاف گوشی‌های پرچمدار نرخ نوسازی بالاتر حالت تطبیقی(Adaptive) نداشته و تنها در 120 هرتز فیکس می‌شود، مصرف باتری در این حالت بیشتر از زمانی است که نرخ نوسازی بر روی 60 هرتز تنظیم باشد.حالت 120 هرتز را در طول بررسی در بازی‌ها و برنامه‌هایی متفاوت تجربه کردیم و در تمامی آنها گوشی عملکرد نرم و روان‌تری در مقایسه با 60 هرتز داشت.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""476575708"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247876"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160757_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""1175533698"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247906"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160823_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""363096055"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247877"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160803_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>



<p>به غیر از نرخ نوسازی بالاتر، همه چیز حتی منوی تنظیمات نمایشگر این گوشی هم مشابه نسل قبل است.در این بخش امکان فعالسازی تم تاریک یا روشن، تنظیم دستی و خودکار روشنایی نمایشگر، فعال کردن فیلتر نور آبی و تنظیم حالت نمایشگر در نظر گرفته شده است.در بخش Screen Mode می‌توان حالت نمایشگر را Vivid یا Natural انتخاب کرد. در این قسمت همچنین امکان تنظیم گرما و سرمای رنگ و شدت رنگ‌های قرمز، سبز و آبی نیز نظر گرفته شده است. نمایشگر به طور پیش‌فرض بر روی حالت Vivid تنظیم شده که فضای رنگی DCI-P3 را ارائه کرده و رنگ‌ها را اشباع شده‌تر نمایش می‌دهد.در حالت Natural، رنگ‌ها در فضای sRGB و طبیعی‌تر ارائه می‌شوند و به واقعیت نزدیک‌تر هستند.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""1145949981"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247878"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160809_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""1053528299"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247907"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160813_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay.jpg"" data-slb-active=""1"" data-slb-asset=""1459654022"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay-461x1024.jpg.webp"" alt="""" class=""wp-image-247879"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160838_AlwaysOnDisplay.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>



<p>نمایشگر A52s با در نظر گرفتن اینکه این گوشی میان‌رده است، از کیفیت بسیار خوبی برخوردار است.فارغ از مشخصات برجسته‌ای مانند نرخ نوسازی بالاتر، روشنایی این گوشی کاملا ایده‌ال است و در حالت Auto حتی به 800 نیت هم می‌رسد.زاویه دید نمایشگر در محیط‌های روشن و زیر نور آفتاب مناسب است و از خوانایی خوبی برخوردار است. جنس رنگ‌ها و دقت تصویر نیز کیفیت بسیار مطلوبی دارد. با دانستن اینکه Samsung Display سازنده نمایشگر این گوشی است، نمی‌توان از آن چیزی به غیر از این سطح کیفیت را انتظار داشت.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-1160x653.jpg.webp"" alt="""" class=""wp-image-247956"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25001.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h3 id = ""h--2"" > سخت‌افزار </ h3 >



< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-1160x653.jpg.webp"" alt="""" class=""wp-image-247950"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22501.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>بهترین تغییری که در گوشی A52s در مقایسه با نسل قبلی آن حاصل شده، استفاده از چیپست جدید و قدرتمندتر است.سامسونگ برای این گوشی از چیپست اسنپدراگون 778G استفاده کرده است.این تراشه 6 نانومتری که در کارخانه TSMC تولید شده، دارای 4 هسته Kryo 670 با فرکانس 2.4 گیگاهرتز و 4 هسته Kryo 670 با فرکانس 1.9 گیگاهرتز است. پردازنده گرافیکی این تراشه &nbsp; Adreno 642L بوده و مودم X53 5G با حداکثر سرعت 3.3 گیگابیت بر ثانیه برای آن در نظر گرفته شده است.علاوه‌براین چیپست اسنپدراگون 778G دارای پردازنده سیگنال دیجیتال جدید Hexagon 770 و پردازنده سیگنال تصویر Spectra 570L نیز هست. این تراشه همچنین از مودم FastConnect 6700 بهره می‎برد و از بلوتوث 5.2، Wi-Fi ax در کنار NFC پشتیبان می‌کند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-1160x653.jpg.webp"" alt="""" class=""wp-image-247953"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22801.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p></p>



<p>گوشی گلکسی A52s سامسونگ در نسخه پایه با 6 گیگابایت رم و 128 گیگابایت حافظه داخلی راهی بازار می‌شود.دو نسخه دیگر از این گوشی با 8 گیگابایت رم و 128 یا 256 گیگابایت حافظه نیز در بازار موجود است.نسخه‌ای که برای بررسی در اختیار ما قرار گرفته 6/128 گیگابایت است. البته در تمامی این مدل‌ها امکان افزایش حافظه از طریق کارت حافظه (به جای سیم‌کارت دوم) در نظر گرفته شده است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-1160x653.jpg.webp"" alt="""" class=""wp-image-247951"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20901.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>همه چیز بر روی کاغذ نشان از برتری سخت‌افزاری این گوشی در مقایسه با نسل قبلی خود دارد، از پردازنده و گرافیک گرفته تا مودم و اتصالات دیگر.خوشبختانه نتایج عملی و تئوری هم چیزی غیر از این را ثابت نکرد. برای تست سخت‌افزاری به سراغ بنچمارک‌های معتبری مانند AnTuTu، گیک‎‌بنچ، PCMark، 3DMark و اندروبنچ رفتیم. نتایج تست‌های بنچمارک برتری قاطع A52s در مقایسه با A52 را نشان می‌دهد.در تمامی این تست‌ها امتیازات کسب شده توسط گوشی جدیدتر سامسونگ بهتر بود.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1057632214"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark-461x1024.jpg.webp"" alt="""" class=""wp-image-247880"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-154745_AnTuTu-Benchmark.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5.jpg"" data-slb-active=""1"" data-slb-asset=""1628246940"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5-461x1024.jpg.webp"" alt="""" class=""wp-image-247881"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211031-141930_Geekbench-5.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark.jpg"" data-slb-active=""1"" data-slb-asset=""454157254"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark-461x1024.jpg.webp"" alt="""" class=""wp-image-247882"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211030-121952_3DMark.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909.jpg"" data-slb-active=""1"" data-slb-asset=""1497556451"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909-461x1024.jpg.webp"" alt="""" class=""wp-image-247883"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211031-141909.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark.jpg"" data-slb-active=""1"" data-slb-asset=""1798864531"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark-461x1024.jpg.webp"" alt="""" class=""wp-image-247884"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120337_PCMark.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark.jpg"" data-slb-active=""1"" data-slb-asset=""603179068"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark-461x1024.jpg.webp"" alt="""" class=""wp-image-247885"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-120151_PCMark.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>



<p>پس از بنچمارک‌ها، نوبت به بازی‌های محبوب رسید که باید در آنها به بررسی عملکرد گوشی گلکسی A52s سامسونگ می‌پرداختیم.باوجود آنکه این گوشی با هدف گیمینگ طراحی نشده، اما به لطف تراشه‌ای که پسوند G را با خود به همراه دارد، می‌تواند همه بازی‌های جدید و محبوب را با کیفیت بسیار خوبی اجرا کند.به عنوان مثال، بازی کالاف دیوتی موبایل در A52s با کیفیت گرافیکی بسیار بالا(Max) و فریم ریت بسیار بالا اجرا می‌شود.خوشبختانه در طول اجراهای بازی‌های سنگین خبری از داغ شدن‌های آزاردهنده نیست و در طولانی مدت پشت دستگاه کمی گرمای محسوس خواهد داشت.</p>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty.jpg"" data-slb-active=""1"" data-slb-asset=""493864045"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x522"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjUyMiIgdmlld0JveD0iMCAwIDExNjAgNTIyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""522"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty-1160x522.jpg.webp"" alt="""" class=""wp-image-247905"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty-1160x522.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty-400x180.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty-768x346.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty-1536x691.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-030016_Call-of-Duty.jpg.webp 1920w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>



<p>Galaxy A52s یک گوشی میان‌رده با بهترین حالت از امکانات سخت‌افزاری است.این گوشی به خوبی از پس اجرای عملیات روزمره مانند برقرار تماس، ارسال پیام، استفاده از دوربین، وبگردی، تماشای فیلم، گوش دادن به موسیقی، استفاده از برنامه‌های پیام‎رسان و شبکه‌های اجتماعی و … بر می‌آید و حتی می‌توانید با کیفیت خوبی جدیدترین بازی‌ها را در آن نصب کنید.از نظر عملکرد سخت‌افزاری A52s عملکردی شبیه به گوشی Mi 11 Lite 5G شیائومی دارد که آن هم اتفاقا یکی از بهترین میان‌رده‌های بازار به حساب می‌آید.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-22001.jpg.webp');""></div>



<ul><li><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%af%d9%88%d8%b4%db%8c-mi-11-lite-5g-%d8%b4%db%8c%d8%a7%d8%a6%d9%88%d9%85%db%8c/"" > تماشا کنید: بررسی Mi 11 Lite 5G – جذاب‌ترین گوشی 10 میلیونی شیائومی؟</a></li></ul>



<h3 id = ""h--3"" > امنیت </ h3 >



< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-1160x653.jpg.webp"" alt="""" class=""wp-image-247949"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23401.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>سامسونگ دو روش احراز هویت بایومتریک برای گوشی A52s در نظر گرفته است؛ تشخیص چهره و اثر انگشت.تشخیص چهره از طریق دوربین سلفی دستگاه انجام می‌شود و عملکرد آن دقیقاً مشابه چیزیست که در دیگر گوشی‌های معمول در بازار مانند A52 دیده بودیم. </p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-1160x653.jpg.webp"" alt="""" class=""wp-image-247948"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23501.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>سنسور اثر انگشت هم که در زیر نمایشگر دستگاه جای گرفته عملکرد رضایت بخشی دارد.البته سرعت این سنسور به اندازه سنسور زیر نمایشگر گوشی‌های پرچمدار جدید سامسونگ نیست، اما کماکان در اندازه یک گوشی میان‌رده از کیفیت مطلوبی برخوردار است.به غیر از تشخیص چهره و اثر انگشت، سامسونگ روش‌های تأمین امنیت دیگری مانند رمز عبور، پین، الگو … را نیز برای A52s در نظر گرفته است.</p>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""412345826"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247904"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160847_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>



<p></p>



<h3 id = ""h--4"" > صدا </ h3 >



< p > خوشبختانه سامسونگ رسم خوبی را در گوشی‌های میان‎‌رده خود بنا نهاده و آن استفاده از بلندگوهای استریو است.گلکسی A52s هم مانند برادر بزرگتر خود از این امکان پخش صدای استریو توسط بلندگوی اصلی(لبه پایینی) و بلندگوی مکالمه(فاصله میان نمایشگر و لبه بالایی) بهره می‎برد.می‌توان گفت که قدرت بلندگوی پایینی دستگاه بیشتر است و صدایی که از این ناحیه خارج می‌شود سطح بالاتری دارد.مثل تمامی بلندگوهای گوشی‌های هوشمند، A52s در پخش صدام بم عملکرد چشمگیری ندارند، اما صدای زیر را با جزئیات مناسبی پخش کرده و به طور کلی بلندی خوبی دارند. نکته مثبت درخصوص صدای این گوشی، فناوری از Dolby Atmos است که جدا از امکان تنظیم اکولایزر، حالت‌های پیش‌فرضی برای موسیقی، صدا و فیلم نیز دارد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-1160x653.jpg.webp"" alt="""" class=""wp-image-247947"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-23701.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>هرچند که شاید با وجود بلندگوهای استریو و فناوری Dolby Atmos&nbsp; صدای ارائه شده توسط A52s آنقدر همه جانبه و فراگیر باشد که نیاز به هدفون پیدا نکنید، اما نکته مثبت دیگری در بخش صوت این گوشی وجود دارد که در قسمت طراحی نیز به آن اشاره کردیم؛ یعنی جک 3.5 میلیمتری هدفون.برخلاف ترندی که این روزها در گوشی‌های هوشمند بالارده بازار می‌بینیم، سری Galaxy A سامسونگ در سال 2021 کماکان دارای جک 3.5 میلیمتری هدفون هستند و این یعنی می‌توانید با هدفون مورد علاقه خود بدون نیاز به تبدیل‌های اضافی با این گوشی به آنچه که می‌خواهید گوش فرا دهید.در مجموع وجود جک هدفون و بلندگوهای استریو 2 مشخصه برجسته در A52s است که بدون شک آن را از گوشی‌های هم سطح خود که از این دو امکان بهره نمی‎برند، متمایز خواهد کرد.</p>



<h3 id = ""h--5"" > نرم‌افزار </ h3 >



< p > گوشی گلکسی A52s سامسونگ را با اندروید 11 و با رابط کاربری OneUI 3.1 به بازار عرضه شده است.اگر<a href=""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%af%d9%84%da%a9%d8%b3%db%8c-a52-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/""> بررسی گوشی Galaxy A52</a> را مطالعه کرده باشید، رابط کاربری این گوشی هیچ حرف تازه‌ای برای شما نخواهد داشت و هر چه بود پیش‌تر از اینها در بررسی‌های قبلی گفته شده بود. در جدیدترین نسخه از رابط کاربری سامسونگ شاهد بروز تغییرات مثبتی در منوها و صفحه‌های گوشی هستیم.البته این تغییرات هرچند مثبت، آنقدرها هم چشمگیر نیستند و اگر در چند سال اخیر با گوشی‌های سامسونگ کار کرده باشید، تجربه کار با این گوشی برای شما جدید نیست.اکثر تغییرهای اعمال شده در نسخه جدید، از نوع ظاهری و در راستای بهینه‌تر کردن رابط کاربری و محیط گوشی است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-1160x653.jpg.webp"" alt="""" class=""wp-image-247946"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20501.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در OneUI 3.0 منوی تنظیمات سریع که باکشیدن انگشت از بالای نمایشگر به سمت پایین ظاهر می‌شود، نسبت به نسل قبل کمی تغییر کرده و حالت تمام صفحه به خود گرفته است.در منوی سمت چپ گوشی که تا پیش از این تنها فید سامسونگ یا همان Samsung Free بود حالا یک گزینه دیگر نیز دارد و می‌توان Google Discover را در آن تنظیم کرد. &nbsp;همچنین امکان اضافه شدن ویجت‌هایی مانند Digital Wellbeing به منوی لاک اسکرین اضافه شده است و شما می‌توانید حتی بدون باز کردن قفل گوشی از زمان کار کردن با گوشی مطلع شوید.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing.jpg"" data-slb-active=""1"" data-slb-asset=""1846113143"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing-461x1024.jpg.webp"" alt="""" class=""wp-image-247887"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160930_Digital-Wellbeing.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing.jpg"" data-slb-active=""1"" data-slb-asset=""1522898713"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing-461x1024.jpg.webp"" alt="""" class=""wp-image-247888"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-160936_Digital-Wellbeing.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home.jpg"" data-slb-active=""1"" data-slb-asset=""1504462300"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home-461x1024.jpg.webp"" alt="""" class=""wp-image-247886"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161206_One-UI-Home.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers.jpg"" data-slb-active=""1"" data-slb-asset=""868833118"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers-461x1024.jpg.webp"" alt="""" class=""wp-image-247889"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161216_Wallpapers.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>
</div>



<p>در کنار این موارد، گزینه‌های همیشگی نرم‌فزار سامسونگ مانند منوی کناری صفحه(Edge Screen)، ایجاد پوشه مخصوص بازی‌ها برای اعمال تغییرات در زمان اجرای بازی(Game Launcher)، تم تاریک، ژست‌های حرکتی، ژست‌های هوایی و … در گوشی گلکسی A52s &nbsp;نیز در نظر گرفته شده است.ترکیب گزینه‌های کاربردی پیشین One UI در کنار امکانات جدیدی که در نسخه جدید به آن اضافه شده است، تجربه استفاده از این گوشی را برای کاربر لذت‎بخش خواهد کرد.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home.jpg"" data-slb-active=""1"" data-slb-asset=""1549292703"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home-461x1024.jpg.webp"" alt="""" class=""wp-image-247890"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160733_One-UI-Home.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home.jpg"" data-slb-active=""1"" data-slb-asset=""1075142949"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home-461x1024.jpg.webp"" alt="""" class=""wp-image-247891"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160740_One-UI-Home.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""873881409"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247892"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-160913_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""1419347150"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247893"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-160908_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>
</div>



<p>اما شاید جذاب‌ترین نکته در بخش نرم‌افزار این گوشی، این باشد که سامسونگ اعلام کرده تا سه سال آپدیت نرم‌افزاری و سیستم عامل را برای A52s ارائه خواهد کرد.با در نظر گرفتن اینکه در چند سال اخیر سامسونگ پیشرفت بسیار خوبی در زمینه ارائه آپدیت‌های امنیتی و سیستم عاملی داشته و این بروزرسانی‌ها را با سرعت بهتری در مقایسه با رقیبان اندرویدی خود برای محصولاتش ارائه می‌کند، این موضوع می‌تواند یک امتیاز مثبت برای A52s باشد.امتیاز مثبت دیگر در بخش نرم‌افزار این گوشی که بخشی از آن سخت‌افزاری نیز هست، سیستم امنیت Samsung Knox است که امنیت چندلایه می‌تواند از اطلاعات حساس شما در برابر خطرات و تهدیدهای امنیتی محافظت کند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x527"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjUyNyIgdmlld0JveD0iMCAwIDExNjAgNTI3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""527"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/in-feature-protect-what-matters-to-you-496614627-1160x527.jpg.webp"" alt=""سیستم امنیت Samsung Knox"" class=""wp-image-248003"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/in-feature-protect-what-matters-to-you-496614627-1160x527.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/in-feature-protect-what-matters-to-you-496614627-400x182.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/in-feature-protect-what-matters-to-you-496614627-768x349.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/in-feature-protect-what-matters-to-you-496614627.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h3 id = ""h--6"" > دوربین </ h3 >



< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-1160x653.jpg.webp"" alt="""" class=""wp-image-247938"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24701.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>گوشی A52s دارای چهار دوربین در قاب پشتی خود و یک دوربین سلفی در پنل جلو است.سنسور اصلی این دوربین‌ها 64 مگاپیکسلی و از نوع عریض با گشادگی دیافراگم f/1.8 است.این سنسور از لرزشگیر اپتیکال تصویر(OIS) و فوکوس خودکار با تشخیص فاز(PDAF) بهره می‌برد.در کنار این دوربین یک سنسور 12 مگاپیکسلی فوق عریض با زاویه دید 123 درجه و f/2.2 قرار گرفته است.دو سنسور 5 مگاپیکسلی با f/2.4 هم یکی از نوع ماکرو و دیگری تشخیص عمق این دوربین‌ها را همراهی می‌کنند. دوربین سلفی گوشی هم که در حفره دورن نمایشگر قرار گرفته، 32 مگاپیکسلی، از نوع عریض با f/2.2 است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-1160x653.jpg.webp"" alt="""" class=""wp-image-247939"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24601.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در نرم‌افزار دوربین، به غیر از حالت معمول عکاسی و فیلمبرداری، شاهد امکان عکاسی تا بزرگنمایی 10 برابر(0.5x, 1x, 2x, 4x, 10x) به صورت دیجیتالی هستیم.که طبیعتاً هر چه رقم زوم بالاتر می‌رود، کیفیت تصویر خروجی افت پیدا می‌کند. عکاسی در حالت پرتره (تار کردن تصویر پس‌زمینه) هم برای دوربین اصلی و هم سلفی در نظر گرفته شده است.عکاسی و فیلمبرداری در حالت Pro (تنظیم دستی)، Night Mode، صحنه آهسته، فوق آهسته، ماکرو، عکاسی غذا و … از دیگر حالت‌های دوربین A52s هستند. برای دوربین اصلی امکان فیلمبرداری 4K در نرخ 30 فریم بر ثانیه و FHD در نرخ 30 و 60 در نظر گرفته شده است که حالت Super Steady که لرزش‌های در حین فیلمبرداری را تا حد ممکن حذف می‌کند، در حالت FHD@30fps وجود دارد.دوربین سلفی نیز می‌تواند با کیفیت 4K در نرخ 30 فریم بر ثانیه فیلمبرداری کند.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera.jpg"" data-slb-active=""1"" data-slb-asset=""735955045"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera-461x1024.jpg.webp"" alt="""" class=""wp-image-247894"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161110_Camera.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera.jpg"" data-slb-active=""1"" data-slb-asset=""1838580138"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera-461x1024.jpg.webp"" alt="""" class=""wp-image-247895"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161132_Camera.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera.jpg"" data-slb-active=""1"" data-slb-asset=""1314715686"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera-461x1024.jpg.webp"" alt="""" class=""wp-image-247896"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-20211101-161118_Camera.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera.jpg"" data-slb-active=""1"" data-slb-asset=""888568596"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera-461x1024.jpg.webp"" alt="""" class=""wp-image-247897"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211101-161123_Camera.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>
</div>



<p>دوربین اصلی در حالت پیش فرض و با لطف فناوری ترکیب پیکسلی، تصاویر را به صورت 16 مگاپیکسلی به ثبت می‌رساند.اما در نرم‌افزار دوربین امکان ثبت تصاویر در حالت 64 مگاپیکسلی نیز وجود دارد. در بررسی گوشی گلکسی A52s سامسونگ به این نتیجه رسیدیم که نه تنها تصاویر 64 مگاپیکسلی برتری خاصی نسبت به تصاویر 16 مگاپیکسلی ندارند، بلکه به واسطه فضای بیشتری که در گوشی اشغال می‎کنند، می‌توانند برای کاربران مشکل ساز باشند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-1160x653.jpg.webp"" alt="""" class=""wp-image-247974"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25401.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p class=""has-text-align-center""><strong>نمونه عکس 64 مگاپیکسلی و 16 مگاپیکسلی</strong></p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150031.jpg"" data-slb-active=""1"" data-slb-asset=""202560524"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150031.jpg.webp"" alt="""" class=""wp-image-247909"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150031.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150031-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150031-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>عکس 64 مگاپیکسلی</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150015.jpg"" data-slb-active=""1"" data-slb-asset=""427820944"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150015.jpg.webp"" alt="""" class=""wp-image-247910"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150015.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150015-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_150015-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>عکس 16 مگاپیکسلی</strong></figcaption></figure>
</div>
</div>



<p>تصاویری که دوربین اصلی 64 مگاپیکسلی گوشی A52s به ثبت می‌رساند، در شرایط نوری مناسب از کیفیت بسیار خوبی برخوردار هستند.با این حال؛ &nbsp; با تاریک شدن محیط کیفیت تصاویر کمی افت پیدا کرده و نویز در تصاویر ظاهر می‌شود.هر چند که حالت عکاسی مخصوص شب (Night) تا حد زیادی می‌تواند از نویز تصاویر بکاهد، با این حال، اندکی نویز کماکان محسوس است و جزئیات در نقاط تاریک تا حدی از بین می‌رود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-1160x653.jpg.webp"" alt="""" class=""wp-image-247941"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24301.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>دوربین فوق عریض 12 مگاپیکسلی برای مناظری که دوربین اصلی نتواند همه آنها را به ثبت برساند، به کار خواهد آمد.جنس رنگی که دوربین فوق عریض در تصاویر ثبت می‌کند کمی متفاوت از دوربین اصلی و دوربین‌های دیگر است. با این حال، نمی‌توان این را به عنوان یک انتقاد در نظر گرفت، چراکه رنگ‌های ثبت شده با این دوربین شباهت بیشتری به واقعیت دارند. هرچند که انتقاد به وجود نویز در تصاویر شب و از دست رفتن جزئیات در این دوربین هم پا برجا است.نکته جالب ماجرا در عکاسی شب اینست مانند تصاویر روز، عکس‌هایی که با دوربین فوق عریض در Night Mode به ثبت می‌رسند جنس رنگ بهتر و نزدیکتری به واقعیت دارند.</p>



<p class=""has-text-align-center""><strong>نمونه عکس‌های دوربین اصلی و فوق عریض و تفاوت رنگی</strong></p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135855.jpg"" data-slb-active=""1"" data-slb-asset=""1770802209"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135855.jpg.webp"" alt="""" class=""wp-image-247921"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135855.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135855-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135855-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>دوربین فوق عریض</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135859.jpg"" data-slb-active=""1"" data-slb-asset=""642436903"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135859.jpg.webp"" alt="""" class=""wp-image-247922"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135859.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135859-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135859-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>دوربین اصلی</strong></figcaption></figure>
</div>
</div>



<p>دوربین 5 مگاپیکسلی ماکرو برای این گوشی هم مانند دیگر گوشی‌های بازار، برای ما دوربین محبوبی نیست.چراکه فوکوس بر روی سوژه و پیدا کردن فاصله ایده‌ال برای ثبت یک تصویر مناسب با آن یکی از سخت‌ترین شرایط عکاسی با دوربین‌های این گوشی است.حتی باوجود فراهم کردن این شرایط، در نهایت باز هم نتیجه چندان مطلوبی حاصل نمی‌شود و کماکان دوربین اصلی دستگاه تصویر بسیار بهتری در مقایسه با دوربین ماکرو به ثبت می‎رساند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-1160x653.jpg"" alt="""" class=""wp-image-247940"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-1160x653.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-400x225.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-768x432.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-384x216.jpg 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-576x324.jpg 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901-960x540.jpg 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28901.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p class=""has-text-align-center""><strong>نمونه عکس دوربین ماکرو</strong></p>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_145130.jpg"" data-slb-active=""1"" data-slb-asset=""1879801883"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1042x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDQyIiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNDIgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1042"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_145130.jpg.webp"" alt="""" class=""wp-image-247911"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_145130.jpg.webp 1042w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_145130-400x307.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_145130-768x590.jpg.webp 768w"" data-sizes=""(max-width: 1042px) 100vw, 1042px""></a></figure>



<p>دوربین 5 مگاپیکسلی دیگری که تحت عنوان تشخیص عمق شناخته می‌شود، برای ثبت تصاویر پرتره و تار کردن تصویر پس‌زمینه مورد استفاده قرار می‌گیرد.عملکرد این دوربین قابل قبول است و به خوبی مرز میان سوژه و پس‌زمینه را تشخیص می‌دهد.اما همانطور که در تصویر پایین مشاهده می‌کنید، گاهاً در نواحی حساس و زمانیکه رنگ‌های پس زمینه و سوژه نزدیک به هم باشد، این دوربین و هوش مصنوعی آنقدرها هم دقیق عمل نمی‌کنند.</p>



<p class=""has-text-align-center""><strong>نمونه عکس پرتره دوربین اصلی</strong></p>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135742.jpg"" data-slb-active=""1"" data-slb-asset=""1884217469"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135742.jpg.webp"" alt="""" class=""wp-image-247912"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135742.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135742-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_135742-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a></figure>



<p>گلکسی A52s همانطور که پیش‌تر اشاره شده از یک دوربین سلفی 32 مگاپیکسلی بهره می‌برد.این دوربین با فناوری ترکیب پیکسلی Quad Bayer تصاویر 8 مگاپیکسلی به ثبت می‎رساند.در نرم‌افزار این دوربین به صورت پیش‌فرض بر روی حالت سلفی تک نفره است، اما با آگاهی از محیط تشخیص اشخاص بیشتر سلفی را به حالت عریض تغییر می‌دهد.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25701.jpg.webp');""></div>



<p>به طور کلی تصاویری که این دوربین به ثبت می‌رساند از کیفیت خوبی برخوردار است.جنس رنگ‌ها نزدیک به واقعیت است و تار کردن پس‌زمینه طبیعی انجام می‌دهد.حتی می‌توان گفت که دقت تار کردن پس‌زمینه از دوربین از اصلی نیز بهتر است.در شرایطی که محیط نور مناسبی داشته باشد، نویز محیط حداقل و نزدیک به صفر است.اما اگر محیط به شدت تاریک باشد، حتی حالت عکاسی Night هم نمی‌تواند کار زیادی برای شما انجام دهد.</p>



<p class=""has-text-align-center""><strong>نمونه عکس‌های دوربین سلفی</strong></p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222031-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""1585685503"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222031-rotated.jpg.webp"" alt="""" class=""wp-image-247913"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222031-rotated.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222031-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Normal</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222037-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""234354500"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222037-rotated.jpg.webp"" alt="""" class=""wp-image-247914"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222037-rotated.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222037-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Portrait</strong></figcaption></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234746-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""947317960"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234746-rotated.jpg.webp"" alt="""" class=""wp-image-247915"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234746-rotated.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234746-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Normal</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234755-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""123735731"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234755-rotated.jpg.webp"" alt="""" class=""wp-image-247916"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234755-rotated.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234755-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Night</strong></figcaption></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234801-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""1483596787"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234801-rotated.jpg.webp"" alt="""" class=""wp-image-247917"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234801-rotated.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_234801-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Portrait</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235825-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""866464853"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235825-rotated.jpg.webp"" alt="""" class=""wp-image-247918"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235825-rotated.jpg.webp 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235825-300x400.jpg.webp 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Normal</strong>(در تاریکی مطلق)</figcaption></figure>
</div>
</div>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141634-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""313345102"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141634-rotated.jpg"" alt="""" class=""wp-image-247919"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141634-rotated.jpg 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141634-300x400.jpg 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Normal</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141640-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""1606594281"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141640-rotated.jpg"" alt="""" class=""wp-image-247920"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141640-rotated.jpg 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_141640-300x400.jpg 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a><figcaption><strong>Portrait</strong></figcaption></figure>
</div>
</div>



<p>در مجموع دوربین‌های گوشی گلکسی A52s در طول بررسی زمانیکه شرایط نوری ایده‌ال باشد تصاویر بسیار خوبی به ثبت می‌رساندند.در نور روز و محیط‌های روشن شاید تنها ایراد کار تفاوت جنس رنگ دوربین فوق عریض با دوربین اصلی گوشی بود. به جز این مورد کیفیت عکس‌ها کاملا مناسب بود.نکته مهم دیگر که شاید اصلی‌ترین تفاوت دوربین گوشی‌های میان‌رده و پرچمدار باشد، اینست که دوربین A52s هم مانند نوا 8، در شرایط تاریک مطلق و بدون نور حتی در حالت نایت هم عکس‌های خوبی ثبت نمی‌کند. &nbsp; با این حال، در محیط‌های نسبتا تاریک به لطف حالت عکاسی شب عکس‌ها نویز کمتر با جزئیات بیشتر دارند.البته تصاویر Night و حتی عادی در دوربین اصلی کمی زردتر و گرم‌تر از آن چیزی ثبت می‌شد که در واقعیت بود.</p>



<ul><li><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%af%d9%88%d8%b4%db%8c-%d9%86%d9%88%d8%a7-8-%d9%87%d9%88%d8%a7%d9%88%db%8c/"" > بررسی گوشی نوا 8 هواوی – شبه پرچمدار جدید چینی</a></li><li><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%d9%88%db%8c%d8%af%d8%a6%d9%88%db%8c%db%8c-%da%af%d9%88%d8%b4%db%8c-%d9%87%d9%88%d8%a7%d9%88%db%8c-%d9%86%d9%88%d8%a7-8/"" > تماشا کنید: بررسی ویدئویی گوشی هواوی نوا 8 – زیباتر از پرچمدارهای اندرویدی<br></a></li></ul>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-1160x653.jpg.webp"" alt="""" class=""wp-image-247945"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20301.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h4 id = ""h-a52s-1"" > نمونه تصاویر ثبت شده در بررسی گوشی گلکسی A52s سامسونگ</h4>



<p></p>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_221714-rotated.jpg"" data-slb-active=""1"" data-slb-asset=""822474823"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""600x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MDAiIGhlaWdodD0iODAwIiB2aWV3Qm94PSIwIDAgNjAwIDgwMCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""600"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_221714-rotated.jpg"" alt="""" class=""wp-image-247924"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_221714-rotated.jpg 600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_221714-300x400.jpg 300w"" data-sizes=""(max-width: 600px) 100vw, 600px""></a></figure>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222153.jpg"" data-slb-active=""1"" data-slb-asset=""1839051350"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222153.jpg.webp"" alt="""" class=""wp-image-247923"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222153.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222153-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211029_222153-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a></figure>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235104.jpg"" data-slb-active=""1"" data-slb-asset=""692004191"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235104.jpg.webp"" alt="""" class=""wp-image-247925"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235104.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235104-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235104-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>Normal – 0.5x</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235140.jpg"" data-slb-active=""1"" data-slb-asset=""214172347"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235140.jpg.webp"" alt="""" class=""wp-image-247926"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235140.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235140-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235140-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>Night – 0.5x</strong></figcaption></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235115.jpg"" data-slb-active=""1"" data-slb-asset=""1361545997"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235115.jpg.webp"" alt="""" class=""wp-image-247927"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235115.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235115-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235115-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong> Normal – 1x</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235150.jpg"" data-slb-active=""1"" data-slb-asset=""209269423"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235150.jpg.webp"" alt="""" class=""wp-image-247928"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235150.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235150-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211031_235150-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption> <strong>Night</strong> – <strong>1x</strong></figcaption></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140815.jpg"" data-slb-active=""1"" data-slb-asset=""912869134"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140815.jpg.webp"" alt="""" class=""wp-image-247931"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140815.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140815-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140815-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>0.5x</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140822.jpg"" data-slb-active=""1"" data-slb-asset=""1667050729"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140822.jpg.webp"" alt="""" class=""wp-image-247932"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140822.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140822-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140822-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>1x</strong></figcaption></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140828.jpg"" data-slb-active=""1"" data-slb-asset=""2035194824"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140828.jpg.webp"" alt="""" class=""wp-image-247933"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140828.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140828-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140828-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>2x</strong></figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140836.jpg"" data-slb-active=""1"" data-slb-asset=""273956680"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140836.jpg.webp"" alt="""" class=""wp-image-247934"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140836.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140836-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_140836-768x576.jpg 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>4x</strong></figcaption></figure>
</div>
</div>
</div>
</div>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_000039.jpg"" data-slb-active=""1"" data-slb-asset=""592319336"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_000039.jpg.webp"" alt="""" class=""wp-image-247929"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_000039.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_000039-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_000039-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a><figcaption><strong>Night</strong></figcaption></figure>



<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_133453.jpg"" data-slb-active=""1"" data-slb-asset=""842660262"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""1067x800"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY3IiBoZWlnaHQ9IjgwMCIgdmlld0JveD0iMCAwIDEwNjcgODAwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1067"" height=""800"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_133453.jpg.webp"" alt="""" class=""wp-image-247930"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_133453.jpg.webp 1067w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_133453-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/20211101_133453-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1067px) 100vw, 1067px""></a></figure>



<h3 id = ""h--7"" > باتری </ h3 >



< p > ظرفیت باتری A52s در مقایسه با نسل قبلی خود تغییری نکرده و این گوشی کماکان از باتری 4500 میلی آمپرساعتی بهره می‌برد.اما دو نکته تأثیرگذار در عملکرد باتری در A52s تغییر کرده‌اند؛ یکی نمایشگر که حالا به نرخ نوسازی بالاتر مجهز شده و انتظار می‌رود که مصرف باتری را بالاتر ببرد و دیگری چیپست دستگاه و تمامی متعلقات آن که در نگاه اول مشخص نبود چه تأثیری بر عملکرد باتری خواهد گذاشت.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-1160x653.jpg.webp"" alt="""" class=""wp-image-247943"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-20601.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در حالت 120 هرتز گوشی A52s در تست باتری PCMark امتیاز 10 ساعت و 50 دقیقه را کسب کرد که این چیزی در حدود 20 دقیقه کمتر از امتیاز A52 در حالت 90 هرتز است.در حالت 60 هرتز اما A52s زمان 14 ساعت و 18 دقیقه را به ثبت رساند که 1 ساعت بیشتر از A52 بود.نتایج این دو تست نشان می‌دهد که تراشه جدید میزان مصرف باتری و به طورکل عملکرد آن را بهتر کرده است، اما در حالت 120 هرتز کماکان شارژ سریع‌تر تخلیه می‌شود. احتمالا اگر سامسونگ حالت آداپتیو را برای پنل این نمایشگر در نظر می‌گرفت، نتایج ثبت شده بسیار بهتر از این می‌شد.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-100005_PCMark-461x1024.jpg.webp"" alt="""" class=""wp-image-247902"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-100005_PCMark-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-100005_PCMark-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-100005_PCMark-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-100005_PCMark-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211030-100005_PCMark.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211029-114938_PCMark-461x1024.jpg.webp"" alt="""" class=""wp-image-247903"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211029-114938_PCMark-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211029-114938_PCMark-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211029-114938_PCMark-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211029-114938_PCMark-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G_20211029-114938_PCMark.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></figure>
</div>
</div>



<p>فارغ از اعداد و ارقامی که در تست‌های بنچمارک کسب شدند، در طول بررسی گوشی گلکسی A52s سامسونگ نمایشگر تقریبا همیشه در حالت 120 هرتز تنظیم بود.باوجود استفاده سنگینی که در طول مدت روز از گوشی به عمل می‌آمد، در پایان روز تا حد مناسبی شارژ باقی مانده بود.البته همانطور که همیشه در تمامی بررسی‌ها به آن اشاره کردیم، عملکرد باتری گوشی ربط مستقیمی به الگوریتم مصرف کاربران دارد. اگر گوشی تمام مدت روز صرف بازی با نرخ 120 فریم و روشنایی مکس شود، طبیعتا می‌توان انتظار داشت که باتری کمتر از یک روز دوام بیارود. حالت عکس این موضوع نیز صادق است و در صورت استفاده کمتر از حد استاندارد، باتری این گوشی بیشتر از یک روز شارژدهی خواهد داشت.</p>



<p>در بخش تنظیمات باتری گزینه فعال کردن Saving Mode و ایجاد محدودیت در استفاده باتری در پس‌زمینه در نظر گرفته شده است. در بخش تنظیمات بیشتر نیز می‌توانید با فعال کردن گزینه Adaptive Battery میزان مصرف برنامه‌هایی که از آنها استفاده نمی‎‌کنید را محدود کنید.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care.jpg"" data-slb-active=""1"" data-slb-asset=""583703223"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care-461x1024.jpg.webp"" alt="""" class=""wp-image-247900"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161037_Device-care.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care.jpg"" data-slb-active=""1"" data-slb-asset=""968368847"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care-461x1024.jpg.webp"" alt="""" class=""wp-image-247899"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161053_Device-care.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care.jpg"" data-slb-active=""1"" data-slb-asset=""2138261722"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care-461x1024.jpg.webp"" alt="""" class=""wp-image-247898"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161100_Device-care.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings.jpg"" data-slb-active=""1"" data-slb-asset=""1805280627"" data-slb-internal=""0"" data-slb-group=""247871""><img data-lazyloaded=""1"" data-placeholder-resp=""461x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI0NjEiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDQ2MSAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""461"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings-461x1024.jpg.webp"" alt="""" class=""wp-image-247901"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings-461x1024.jpg.webp 461w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings-180x400.jpg.webp 180w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings-768x1707.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings-691x1536.jpg.webp 691w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Screenshot-Samsung-Galaxy-A52s-5G-_20211101-161019_Settings.jpg.webp 864w"" data-sizes=""(max-width: 461px) 100vw, 461px""></a></figure>
</div>
</div>
</div>
</div>



<p>سامسونگ برای این گوشی امکان پشتیبانی از شارژر 25 وات را در نظر گرفته است، اما از آنجاییکه محتویات جعبه این گوشی در اختیار ما قرار نداشت، مشخص نیست که آیا همین شارژر 25 وات درون بسته A52s قرار گرفته و یا مانند A52، سامسونگ آن را با شارژر کندتر 15 واتی راهی بازار کرده است.در هر حال، با در نظر گرفتن اینکه شرکت‌های رقیب مانند شیائومی و هواوی محصولات هم‌رده با A52s را با شارژرهای سریع در حد 66 وات راهی بازار می‎کنند، شاید تفاوت 15 و 25 وات آنقدر چشمگیر نباشد.با تمام این اوصاف، شارژر 25 واتی A52s برای شارژر کامل باتری 4500 میلی آمپرساعتی به چیزی در حدود 1 ساعت و 30 دقیقه زمان نیاز دارد و این چندان زمان چشمگیری در سال 2021 نیست. &nbsp;</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-1160x653.jpg.webp"" alt="""" class=""wp-image-247944"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-21301.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h3 id = ""h--8"" > جمع‌بندی </ h3 >



< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-1160x653.jpg.webp"" alt="""" class=""wp-image-247971"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-24901.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>Samsung Galaxy A52s 5G یکی از زیباترین و در عین حال خوش دست و جمع‌وجوترین گوشی‌های میان‌رده سامسونگ و بازار به حساب می‌آید که نکات مثبت فراوانی به غیر از طراحی با خود به همراه دارد.نمایشگر باکیفیت، روشن و 120 هرتزی این دستگاه در کنار نرم‌افزار به روز و سریع و سخت‌افزار قدرتمند تجربه کاربری خوبی برای کاربران به ارمغان خواهد آورد.بلندگوهای استریو، پشتیبانی از جک 3.5 میلیمتری هدفون و کارت حافظه نکات مثبت دیگر این گوشی هستند که سامسونگ در طراحی A52s به آن توجه کرده است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-1160x653.jpg.webp"" alt="""" class=""wp-image-247972"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-28501.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>برخلاف دوربین اصلی، دوربین‌های فوق عریض و ماکرو عملکرد خارق العاده‌ای ندارند.با این حال، با وجود یک دوربین 64 مگاپیکسلی باکیفیت و سلفی 32 مگاپیکسلی شاید چندان نیازی به دوربین‌های دیگر پیدا نکنید.پیشرفت باتری این گوشی در مقایسه با نسل‌های قبلی به خوبی در نتایج بنچمارک‌ها مشهود است.با این حال، سرعت شارژ کماکان یکی از نقاطی است که رقیبان سامسونگ در آن بهتر عمل کرده‌اند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-1160x653.jpg.webp"" alt="""" class=""wp-image-247973"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-400x225.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-384x216.jpg 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-576x324.jpg 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Samsung-Galaxy-A52s-5G-25301.jpg.webp 1919w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در لحظه نگارش این بررسی، Galaxy A52s در نسخه 8/128 گیگابایت با قیمت 9.700 میلیون تومان و در نسخه 8/256 با قیمت 10.800 میلیون تومان به فروش می‌رسد.این گوشی به نوعی جانشین A52 و A52 5G به حساب می‌آید که نسخه 8/256 گیگابایتی آنها به ترتیب 10.100 میلیون تومان و 11.400 میلیون تومان قیمت دارد.ارزان‌تر بودن این گوشی در مقایسه با A52 5G آن هم با در نظر گرفتن پیشرفت‌های سخت‌افزاری نشان می‌دهد که A52s تا چه اندازه ارزش خرید بالایی دارد. از همین رو، نشان پیشنهاد شده توسط سخت‌افزار و نشان منتخب سردبیر به این گوشی ارزشمند تعلق می‌گیرد.</p>



<figure class=""wp-block-gallery columns-2 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""340x454"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzNDAiIGhlaWdodD0iNDU0IiB2aWV3Qm94PSIwIDAgMzQwIDQ1NCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""340"" height=""454"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Highly-Recommended-Award.jpg.webp"" alt="""" data-id=""247873"" class=""wp-image-247873"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Highly-Recommended-Award.jpg.webp 340w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Highly-Recommended-Award-300x400.jpg.webp 300w"" data-sizes=""(max-width: 340px) 100vw, 340px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""340x454"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzNDAiIGhlaWdodD0iNDU0IiB2aWV3Qm94PSIwIDAgMzQwIDQ1NCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""340"" height=""454"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Editors-Choice-Award.jpg.webp"" alt="""" data-id=""247874"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Editors-Choice-Award.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=247874"" class=""wp-image-247874"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/Editors-Choice-Award.jpg.webp 340w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/Editors-Choice-Award-300x400.jpg.webp 300w"" data-sizes=""(max-width: 340px) 100vw, 340px""></figure></li></ul></figure>



<p></p>



<p>هرچند که در مقایسه با رقیبان چینی، A52s کمی گران‌تر است، اما رویه قیمت گذاری سامسونگ همواره به همین صورت بوده و آمارها در بازار موبایل نشان می‌دهد که کاربران با وجود قیمت بالاتر اعتماد بیشتری نسبت به این برند کره‌ای دارند.در این بازه قیمتی اصلی‌ترین رقیبان Galaxy A52s در بازار ایران، Galaxy M62 سامسونگ و Mi 11 Lite 5G و Poco F3 شیائومی هستند.</p>



<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span>



<ul><li>طراحی ساده، زیبا و جمع‌وجور</li><li>جک 3.5 میلیمتری هدفون</li><li>پشتیبانی از کارت حافظه</li><li>بلندگوهای استریو</li><li>نمایشگر 120 هرتزی بسیار باکیفیت</li><li>نرم‌افزار به روز با وعده 3 سال آپدیت</li><li>سخت‌افزار قدرتمند در کلاس قیمتی</li><li>دوربین اصلی 64 مگاپیکسلی با کیفیت</li><li>عملکرد خوب باتری</li></ul>



</div>



<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span>



<ul><li>جنس رنگی متفاوت دوربین اصلی و فوق عریض</li><li>بی استفاده بودن دوربین ماکرو</li><li>شارژر کندتر در برابر رقبا</li></ul>



</div>



<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span>



<p>گلکسی A52s سامسونگ یک گوشی &nbsp;در بازه قیمتی 10 میلیون، با طراحی زیبا، امکانات فراوان و سخت‌افزار قوی بوده و مناسب برای کسانی است که به یک گوشی میان‌رده با امکانات معمول برای استفاده طولانی مدت نیاز دارند.</p>



</div>



<span data-sr=""enter"" class=""gk-review clearfix""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.9""><span>9</span></span><span class=""gk-review-partial-label"">سخت‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.95""><span>9.5</span></span><span class=""gk-review-partial-label"">نرم‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-partial-label"">صفحه نمایش</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.88""><span>8.8</span></span><span class=""gk-review-partial-label"">دوربین</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.91""><span>9.1</span></span><span class=""gk-review-partial-label"">صدا</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.93""><span>9.3</span></span><span class=""gk-review-partial-label"">طراحی</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.94""><span>9.4</span></span><span class=""gk-review-partial-label"">باتری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.93""><span>9.3</span></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>



<p></p>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 19,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "سامسونگ,تبلت هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0292-1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "بررسی گلکسی تب A7 2020 سامسونگ",
                    Title = "بررسی گلکسی تب A7 2020 سامسونگ – میان‌رده جدید و خوش قیمت",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 14,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

< p > سامسونگ اواخر تابستان تبلت 10 اینچی اقتصادی جدید خود را معرفی و راهی بازار کرد.امروز در این مطلب با بررسی گلکسی تب A7 2020 سامسونگ در خدمت شما عزیزان هستیم.با ما همراه باشید.</ p >



< p > از زمان شیوع ویروس کرونا در کشور، بسیاری از مدارس و دانشگاه‌ها به صورت غیرحضوری برگزار می‌شوند.از همین رو دانش‌آموزان و دانشجویان به گجت‌هایی مانند گوشی، تبلت و حتی لپ‌تاپ برای جا نماندن از تکالیف خود بیشتر از همیشه نیاز دارند.به واسطه شرایط & nbsp; اقتصادی که این روزها بر جامعه حاکم قدرت خرید محصولات پرچمدار و بالارده از بخش بزرگی از مردم گرفته شده و تمایل مردم به خرید محصولات اقتصادی بیشتر از همیشه است.</ p >
   



   < figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-1160x773.jpg.webp"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207967 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0288.jpg.webp 1620w"" data-was-processed=""true""></figure>



<p>Samsung Galaxy Tab A7 10.4 در حال حاضر یکی از تبلت‌های اقتصادی بازار ایران به حساب می‌آید.از همین تصمیم داریم امروز با بررسی گلکسی تب A7 2020 سامسونگ ببینیم که این محصول میان‌رده جدید کره‌ای تا چه اندازه تبلت خوبی است و ارزش خرید دارد؟</p>



<h2>بررسی گلکسی تب A7 2020 سامسونگ</h2>



<p>سامسونگ 11 سپتامبر 2020، یعنی 20 شهریور گلکسی تب جدید خود را در دو نسخه LTE و Wi-Fi راهی بازار کرد. این دستگاه در بازار اروپا 240 یورو قیمت دارد و در بازار ایران با قیمتی در حدود 6 میلیون تومان به فروش می‌رسد.این تبلت در سه رنگ طلایی نقره‌ای و خاکستری تیره راهی بازار شده است که نسخه خاکستری تیره آن از طرف نمایندگی سامسونگ در اختیار ما قرار گرفته است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-1160x773.jpg.webp"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207943 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0285.jpg.webp 1620w"" data-was-processed=""true""></figure>



<h3>طراحی</h3>



<p>چندی پیش<a href= ""https://sakhtafzarmag.com/?p=200099"" > بررسی گلکسی تب A 10.1 سامسونگ</a> را با شما به اشتراک گذاشتیم.تبلت 10.4 اینچی جدید سامسونگ از نظر اندازه و ابعاد و حتی وزن تفاوت تفاوت چندانی با آن دستگاه ندارد. البته ضخامت آن اندکی کمتر شده است.</p>



<figure class=""wp-block-table""><table><tbody><tr><td>&nbsp;</td><td>گلکسی Tab A 10.1 2019</td><td><strong>گلکسی</strong><strong>Tab A7 10.4 2020</strong></td></tr><tr class=""alt""><td>ابعاد</td><td>245.2×149.4×7.5 میلیمتر</td><td><strong>247.6×157.4×7</strong><strong> میلیمتر</strong></td></tr><tr><td>وزن</td><td>469 گرم</td><td><strong>477 گرم</strong></td></tr></tbody></table></figure>



<p>تبلت جدید سامسونگ با بدنه فلزی خود ظاهری شبیه به دیگر تبلت‌های این شرکت دارد.قاب پشتی این دستگاه فلزی طراحی شده و درست در مرکزی آن لوگوی سامسونگ به صورت افقی حک شده است.پایین‌تر از لوگوی سامسونگ هم IMEI دستگاه نوشته شده است.در قسمت بالای سمت راست (در حالت Landscape) محفظه مربعی شکل دوربین اصلی قرار گرفته که تنها یک سنسور در آن جای دارد و این تنها عنصر تعبیه شده در قاب پشت بوده و خبری از فلش LED یا سنسور اثر انگشت در آن نیست.دوربین دستگاه برآمدگی نسبتاً محسوسی دارد، اما خوشبختانه قرار گرفتن تبلت بر روی سطح صاف موجب لق زدن به دلیل این برآمدگی نخواهد شد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0306-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207945"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0306-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0306-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0306-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0306-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0306.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>قسمت بالای دوربین با خطی از بدنه پشت جدا شده و با فریم لبه گوشی یکپارچه شده است.به نظر می‌رسد که جنس این بخش از لبه تبلت از دیگر لبه‌های و قاب پشت متفاوت و از پلاستیک باشد. &nbsp; در همین لبه(بالا در حالت افقی/ راست در حالت عمودی) کلید پاور/قفل، کلیدهای تنظیم صدا و یکی از میکروفون‌های دستگاه جای گرفته است.عوض شدن جایگاه کلیدهای پاور و تنظیم صدا در این تبلت هم کمی آزاردهند است.چرا که دسترسی به کلید پاور چندان آسان نیست و در موارد بسیاری شاهد قفل شدن گوشی به جای بلند کردن صدا و کم کردن صدا به جای قفل شدن خواهید بود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0309-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207947"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0309-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0309-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0309-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0309-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0309.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در لبه سمت راست دو بلندگو در دو طرف قرار گرفته‌اند و در وسط آنها پورت شارژ و انتقال داده USB-C و میکروفون مکالمه قرار گرفته است.در پایین‌ترین قسمت این لبه و در واقع کنج آن، درگاه جک 3.5 میلیمتری تعبیه شده است. &nbsp; در لبه پایینی این تبلت شیار کارت حافظه و سیم‌کارت قرار دارد که البته در نسخه Wi-Fi این شیار کوچکتر و مخصوص یک کارت حافظه است.در نهایت در لب سمت چپ هم دو بلندگوی دیگر جای گرفته‌اند.</p>



<figure class=""wp-block-gallery columns-3 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0313-1160x773.jpg.webp"" alt="""" data-id=""207948"" data-link=""https://sakhtafzarmag.com/?attachment_id=207948"" class=""wp-image-207948"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0313-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0313-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0313-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0313-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0313.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1-1160x773.jpg.webp"" alt="""" data-id=""207950"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207950"" class=""wp-image-207950"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0311-1.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0312-1160x773.jpg.webp"" alt="""" data-id=""207949"" data-link=""https://sakhtafzarmag.com/?attachment_id=207949"" class=""wp-image-207949"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0312-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0312-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0312-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0312-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0312.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li></ul></figure>



<p>فریم دو دستگاه از طریق یک لایه پلاستیکی مشکی رنگ به نمایشگر دستگاه متصل شده است. 79 درصد از سطح پنل جلو را این نمایشگر به خود اختصاص داده است و چهار طرف آن با حاشیه‌های یکسان و متقارنی پوشیده شده است.باوجود آنکه در سال 2021 داشتن این سطح از حاشیه حتی برای یک تبلت هم زیاد است، اما از نگاه مثبت این حاشیه‌ها مانعی برای تماس ناخواسته دستان شما با نمایشگر خواهد بود. در حاشیه بالایی نمایشگر (افقی) دوربین سلفی و سنسورهای دیگری مانند سنسور نور محیطی و ژیروسکوپ جای گرفته‌اند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0307-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207951"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0307-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0307-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0307-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0307-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0307.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>گلکسی Tab A7 10.4 &nbsp;از نظر طراحی همانگونه است که از یک تبلت معمول انتظار داریم.ظاهری شیک و ساده با ابعاد و وزنی ایده‌ال. پشتیبانی از کارت حافظه و وجود جک هدفون و 4 بلندگو از نکات مثبت این تبلت در طراحی است و نبود فلش LED را هم شاید بتواند در حالت سخت‌گیرانه ایرادی در این بخش دانست.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0291-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207968"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0291-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0291-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0291-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0291-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0291.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h2>نمایشگر</h2>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0328-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207952"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0328-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0328-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0328-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0328-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0328.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>همانطور که از نام آن مشخص است، این تبلت از یک نمایشگر 10.4 اینچی TFT LCD بهره می‌برد.این نمایشگر با نسبت ابعاد 5:3 در حدود 79.0 درصد از سطح جلو تبلت را به خود اختصاص داده است.دقت تصویر آن نیز 2000×1200 پیکسل(WUXGA+) با تراکم پیکسلی 224 ppi اعلام شده است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0320-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207953"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0320-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0320-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0320-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0320-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0320.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>از نظر مشخصات این نمایشگر بسیار نزدیک به آن چیزی است که در تبلت Tab A 10.1 دیده بودیم.با این حال، روشنایی این تبلت در بالاترین سطح 330 نیت مشخص شده که این رقم چندان بالایی نیست.استفاده از نمایشگرهای TFT که تکنولوژی قدیمی‌تری در مقایسه با نمایشگرهای IPS LCD و AMOLED به حساب می‌آید، در یک تبلت اقتصادی شاید چندان عجیب و غیرمعمول نباشد، اما زمانیکه سامسونگ به عنوان یکی از بزرگترین تولیدکنندگان نمایشگر از این فناوری در محصول جدید، هرچند اقتصادی خود استفاده می‌کند، شاید کمی ناامیدکننده باشد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207954"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>با این حال، این به آن معنا نیست که نمایشگر Tab A7 از کیفیت بدی برخوردار است.با در نظر گرفتن بازه قیمتی این تبلت، نمایشگر 60 هرتز آن از کیفیت رنگ و دقت تصویری خوبی برخوردار است.با این حال، از نظر میزان روشنایی و زاویه دید عملکرد آن چندان چشمگیر نیست. این یعنی کار کردن با تبلت در محیط‌های خیلی روشن مانند زیر نور آفتاب و یا تماشای صحنه‌های تاریک در فیلم و ویدیو آنگونه که باید رضایت‌بخش نیست.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0289-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207962"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0289-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0289-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0289-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0289-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0289.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در بخش تنظیمات Display گزینه‌ای برای انتخاب تم تاریک و روشن، تنظیمات مربوط به تم تارک، تنظیم دستی میزان روشنایی و فعال کردن حالت تنظیم روشنایی خودکار است.در این قسمت گزینه‎های دیگری برای تغییر اندازه فونت، بزرگنمایی گزینه‌های نمایشگر، زمان روشن بودن آن، تنظیمات مربوط به صفحه Home، کلیدهای ناوبری و … نیز قرار گرفته است.</p>



<figure class=""wp-block-gallery columns-3 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224044_One-UI-Home-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207926"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224044_One-UI-Home.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207926"" class=""wp-image-207926"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224044_One-UI-Home-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224044_One-UI-Home-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224044_One-UI-Home.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224029_Settings-1-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207927"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224029_Settings-1.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207927"" class=""wp-image-207927"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224029_Settings-1-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224029_Settings-1-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224029_Settings-1.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224037_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207928"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224037_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207928"" class=""wp-image-207928"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224037_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224037_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224037_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li></ul></figure>



<h3>سخت‌افزار</h3>



<p>تبلت میان‌رده جدید سامسونگ از چیپست اسنپدراگون 662 کوالکام راهی بازار کرده است.این تراشه میان‌رده که با معماری 11 نانومتری طراحی شده، اوایل سال 2020 معرفی شده و از 4 هسته پرقدرت Kryo Gold با فرکانس 2.0 گیگاهرتز و 4 هسته کم مصرف Kryo 260 Silver با فرکانس 1.8 گیگاهرتز بهره می‌برد.پردازنده گرافیکی این تراشه Adreno 610 بوده و از مودم X11 LTE پشتیبانی می‌کند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0308-2-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207970"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0308-2-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0308-2-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0308-2-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0308-2-1536x1024.jpg 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0308-2.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>سامسونگ برای این تبلت 3 گیگابایت حافظه رم و 32 یا 64 گیگابایت حافظه داخلی در نظر گرفته است.نسخه‌ای که برای بررسی در اختیار ما قرار گرفته و عموماً در بازار ایران موجود است، 32 گیگ حافظه دارد که از این رقم 19.5 گیگ آن در دسترس است.باوجود آنکه این رقم بالایی برای حافظه ذخیره‌سازی نیست، خوشبختانه پشتیبانی از کارت حافظه MicroSD&nbsp; تا سقف 1 ترابایت می‎تواند شرایط را برای کاربران تا حد زیادی مطلوب کند.</p>



<p>اسنپدراگون 662 همانطور که پیش‌تر اشاره شد که تراشه نسبتاً جدید میان‌رده است که در طول بررسی گلکسی تب A7 2020 سامسونگ مشاهده کردیم که وظیفه خود را به عنوان چنین پردازنده‌ای به خوبی انجام می‌دهد.نتایج کسب شده در تست‌های بنچمارک این تبلت در مقایسه با تبلت Tab A 10.1 تقریبا همه جا بهتر بود و این نشان از بهبود عملکرد نسل جدید در مقایسه با محصول سال قبل‌تر می‌دهد.</p>



<figure class=""wp-block-gallery columns-4 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-022519_AnTuTu-Benchmark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207913"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-022519_AnTuTu-Benchmark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207913"" class=""wp-image-207913"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-022519_AnTuTu-Benchmark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-022519_AnTuTu-Benchmark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-022519_AnTuTu-Benchmark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183219_Basemark-GPU-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207915"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183219_Basemark-GPU.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207915"" class=""wp-image-207915"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183219_Basemark-GPU-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183219_Basemark-GPU-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183219_Basemark-GPU.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183257_Basemark-GPU-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207916"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183257_Basemark-GPU.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207916"" class=""wp-image-207916"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183257_Basemark-GPU-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183257_Basemark-GPU-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-183257_Basemark-GPU.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-141423_Geekbench-5-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207917"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-141423_Geekbench-5.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207917"" class=""wp-image-207917"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-141423_Geekbench-5-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-141423_Geekbench-5-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-141423_Geekbench-5.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-142237_PCMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207918"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-142237_PCMark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207918"" class=""wp-image-207918"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-142237_PCMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-142237_PCMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210117-142237_PCMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-222511_PCMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207919"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-222511_PCMark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207919"" class=""wp-image-207919"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-222511_PCMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-222511_PCMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-222511_PCMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223648_PCMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207921"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223648_PCMark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207921"" class=""wp-image-207921"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223648_PCMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223648_PCMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223648_PCMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223044_PCMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207920"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223044_PCMark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207920"" class=""wp-image-207920"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223044_PCMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223044_PCMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223044_PCMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223925-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207922"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223925.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207922"" class=""wp-image-207922"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223925-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223925-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-223925.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-205145-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207925"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-205145.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207925"" class=""wp-image-207925"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-205145-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-205145-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-205145.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-203950_3DMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207924"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-203950_3DMark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207924"" class=""wp-image-207924"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-203950_3DMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-203950_3DMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-203950_3DMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-201209_3DMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207923"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-201209_3DMark.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207923"" class=""wp-image-207923"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-201209_3DMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-201209_3DMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210116-201209_3DMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li></ul></figure>



<p>&nbsp;باید قبول کنیم که 3 گیگابایت حافظه رم، حتی برای یک محصول اقتصادی هم رقم کمی است و این موضوع تأثیر خود را در زمان کار با گوشی نشان می‌دهد.به عنوان مثال در زمان کاربری عادی مانند استفاده از مرورگر، دوربین، اینستاگرام و … عملکرد دستگاه کاملا نرمال و روان است.اما زمانیکه قصد داشته باشید میان این برنامه‌ها سوئیچ کنید و یا در مرورگر از تبی به تب دیگر بروید شاهد کند شدن دستگاه و کمی لگ خواهید بود.</p>



<p>در اجرای بازی‌ها وضعیت کمی متفاوت است.به عنوان مثال بازی PUBG Mobile به صورت پیش‌فرض با گرافیک بالانس و فریم ریت متوسط اجرا می‌شود که در این تنظیمات بازی چندان روان نیست و برای بهتر شدن کیفیت اجرا باید گرافیک را بر روی Smooth و فریم ریت را بر روی High تنظیم کند.Call of Duty Mobile با تنظیمات پیش‌فرض بدون لگ و افت فریم اجرا می‌شود. اما بازی Asphalt 9 که گرافیک بالاتری نیازمند است، چندان نرم و روان در این دستگاه قابل بازی نیست. با این حال، اینکه جدیدترین و سنگین‌ترین بازی‌ها حتی در همین سطح کیفیت بر روی این تبلت قابل اجرا هستند، به خودی خود نکته مثبتی به حساب می‌آید.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-2-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207969"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-2-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-2-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-2-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-2-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0324-2.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>با در نظر گرفتن بازه قیمتی Tab A7 به این نتیجه خواهید رسید که سخت‌افزار این تبلت به خوبی با مشخصه‌های دیگر آن سازگار است و از پس اجرای کارهای روزمره مانند برقرار تماس، ارسال پیام، تماشای فیلم، وبگردی، استفاده از شبکه‌های اجتماعی، دوربین و مقاصد آموزش دانش‌آموزان برمی‌آید.با این حال، نمی‌توان انتظار عملکردی در سطح پرچمداران در زمان اجرای چند برنامه همزمان و بازی‌های سنگین را از این دستگاه داشت. با تمامی این اوصاف داشتن حداقل 4 گیگ حافظه رم می‌توانست این تبلت را به یکی از بهترین تبلت‌های بازه قیمتی خود تبدیل کند.</p>



<p>سامسونگ برای این تبلت تشخیص چهره دو بعدی را به عنوان تنها روش احراز هویت بایومتریک در نظر گرفته است. تشخیص چهره این دستگاه تنها قابلیت ذخیره یک چهره را دارد و از طریق دوربین سلفی چهره کاربر را شناسایی خواهد کرد. در حالت استفاده عمودی باوجود آنکه دوربین در حاشیه سمت راست قرار گرفته، باز هم قادر است با سرعت خوبی چهره را تشخیص داده و قفل صفحه را باز کند.متأسفانه سامسونگ برای کم کردن هزینه‌ها در این تبلت سنسور اثر انگشت در نظر نگرفته است. با این حال، به غیر از تشخیص چهره، امکان استفاده از روش‌هایی مانند رمز عبور، پین و الگو نیز برای تب A7 در نظر گرفته شده است.</p>



<figure class=""wp-block-gallery columns-3 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-210908_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207907"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-210908_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207907"" class=""wp-image-207907"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-210908_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-210908_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-210908_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224117_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207908"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224117_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207908"" class=""wp-image-207908"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224117_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224117_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224117_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224105_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207909"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224105_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207909"" class=""wp-image-207909"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224105_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224105_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224105_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li></ul></figure>



<h3>صدا</h3>



<p>سامسونگ در دو لبه چپ و راست این تبلت 4 محفظه برای بلندگو در نظر گرفته است که می‌توانند صدا را به صورت استریو ارائه کنند.با قرار گرفتن این چهار بلندگو در دو لبه، صدا با بلندی خوب و فراگیر پخش می‌شود و در زمان بازی و پخش ویدیو که ممکن است دست شما دو بلندگو را مسدود کند، صدا از خروجی‌های دیگر به خوبی پخش خواهند شد.تبلت میان‌رده سامسونگ از فناوری صوتی Dolby Atmos نیز بهره می‌برد که می‌تواند به صورت خودکار کیفیت صدا را بهینه کند.البته این قابلیت حالت‌های اختصاصی برای پخش صدا، موسیقی و فیلم نیز دارد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-211716_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207906"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-211716_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-211716_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-211716_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure>



<p>کیفیت صدای ارائه شده توسط این تبلت بهترین نیست، اما با در نظر گرفتن این که Tab A7 10.4 یک محصول میان‌رده به حساب می‌آید به این نتیجه خواهید رسید که صدای استریو، وجود جک 3.5 میلیمتری هدفون و فناوری صوتی Dolby Atmos در این تبلت از امکاناتی هستند که احتمالاً نمی‌توانید تمامی آنها را به طور همزمان در یک محصول هم‌رده پیدا کنید.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0316-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207955"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0316-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0316-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0316-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0316-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0316.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h3>نرم‌افزار</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0322-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207956"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0322-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0322-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0322-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0322-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0322.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>Galaxy Tab A7 سامسونگ نسخه 2.5 رابط کاربری One UI Core بر پایه اندروید 10 راهی بازار شده است. &nbsp; تفاوت نسخه Core با نسخه استاندارد One UI اینست که یک سری از امکانات گوشی‌های سامسونگ مانند Samsung Pay، Secure Folder، Samsung Pass، Private Mode و قابلیت‌هایی GoodLock و Edge Screen یا حتی Game Luncher را در اختیار ندارد.با این حال، این نسخه از نرم‌افزار دارای پلتفرم امنیتی چند لایه Knox&nbsp; برای محافظت از اطلاعات شخصی کاربران است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0321-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207957"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0321-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0321-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0321-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0321-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0321.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در محیط نرم‌افزاری این تبلت تقریباً تمامی امکانات لازم برای یک تبلت در نظر گرفته شده است.تم تاریک(Dark Mode) مهم‌ترین مشخصه این نرم‌افزار است که فعال کردن آن به طرز چشمگیری در میزان مصرف باتری تأثیرگذار بوده و میزان خستگی چشم را کمتر می‌کند. &nbsp;قابلیت Digital well-being و Parental Control نیز یکی دیگر از امکانات خوب این تبلت است که هم می‌تواند شما را از مدتی که در برنامه‌های مختلف می‌گذرانید مطلع می‌کند و هم محدودیت‌هایی برای استفاده شما یا فرزندنتان از دستگاه و برنامه‌های آن اعمال کند.</p>



<figure class=""wp-block-gallery columns-3 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-114355_Digital-Wellbeing-and-parental-controls-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207900"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-114355_Digital-Wellbeing-and-parental-controls.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207900"" class=""wp-image-207900"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-114355_Digital-Wellbeing-and-parental-controls-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-114355_Digital-Wellbeing-and-parental-controls-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-114355_Digital-Wellbeing-and-parental-controls.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235606_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207902"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235606_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207902"" class=""wp-image-207902"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235606_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235606_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235606_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235623_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207903"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235623_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207903"" class=""wp-image-207903"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235623_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235623_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210121-235623_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002442_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207901"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002442_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207901"" class=""wp-image-207901"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002442_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002442_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002442_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002427_Settings-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207904"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002427_Settings.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207904"" class=""wp-image-207904"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002427_Settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002427_Settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-002427_Settings.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224013_One-UI-Home-614x1024.jpg.webp"" alt=""تنظیمات تبلت گلکسی تب A7 2020 سامسونگ"" data-id=""207905"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224013_One-UI-Home.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207905"" class=""wp-image-207905"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224013_One-UI-Home-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224013_One-UI-Home-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210120-224013_One-UI-Home.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li></ul></figure>



<p>&nbsp;ژست‌های حرکتی برای این تبلت در نظر گرفته شده‌اند و به غیر از کلیدهای ناوبری چند عملیات محدود دیگر نیز انجام می‌دهند که از جمله آنها می‌تواند سوایپ کردن بر روی نام مخاطبین برای برقراری زنگ و یا تماس و دو بار زدن بر روی نمایشگر جهت روشن شدن آن اشاره کرد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0325-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207958"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0325-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0325-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0325-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0325-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0325.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>سامسونگ محیط نرم‌افزاری گلکسی Tab A7 را متناسب با سخت‌افزار آن ساده و سبک طراحی کرده است.وجود ژست‌های حرکتی و تم تاریک از امکانات خوب این نرم‌افزار هست.علاوه‌براین در منوی Quick Setting که با کشیدن انگشت از بالای صفحه به سمت پایین ظاهر می‌شود امکانات کاربردی مانند فیلتر نور آبی، Samrt View، Quick Share و Nearby Share در نظر گرفته شده است. نبود امکاناتی مانند Secure Folder، گیم لانچر و منوی Edge Screan را شاید بتوان تنها ایرادات نرم‌افزاری این تبلت قلمداد کرد.</p>



<h3>دوربین</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0295-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207959"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0295-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0295-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0295-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0295-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0295.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>به‌طورکلی افرادی که به دنبال خرید یک تبلت میان‌رده و اقتصادی هستند، اگر بخواهند در بخشی توقع خود را پایین بیاورند، آن بخش دوربین است.در واقع کسی از یک تبلت میان‌قیمت انتظار داشتن دوربین‌های فوق العاده باکیفیت را ندارد. سازندگان هم عموماً برای کنترل قیمت نهایی از دوربین‌های نه چندان جدید و به‌روز در این محصولات استفاده می‌کنند.گلکسی تب A7 از یک دوربین 8 مگاپیکسلی با گشادگی دیافراگم f/2.0 مجهز به فوکوس خودکار به عنوان دوربین اصلی بهره می‌برد. دوربین سلفی این گوشی نیز 5 مگاپیکسلی است. هر دو دوربین این دستگاه قادر به فیلمبرداری با کیفیت 1080p در نرخ 30 فریم بر ثانیه هستند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0315-1-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207960"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0315-1-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0315-1-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0315-1-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0315-1-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0315-1.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>دوربین‌هایی که در این تبلت استفاده شده بر روی کاغذ دقیقا همان دوربین‌هایی هستند که در نسخه سال 2019 تبلت A 10.1 دیده بودیم.در بررسی گلکسی تب A7 2020 سامسونگ هم مشاهده کردیم که عملکرد دوربین‌های آن تفاوت چندانی با تبلت مذکور ندارد. دوربین اصلی در شرایطی نور محیط کافی باشد، مانند نور روز عکس‌های بسیار خوبی به ثبت می‌رساند.اما در شرایطی وضعیت نور چالشی شود، یا سوژه کمی در حال حرکت باشه، نویز در تصاویر ثبت شده ظاهر شده و به طور قطع سوژه تصویر تار خواهد بود.</p>



<p>دوربین سلفی دستگاه هم بسته به شرایط می‌تواند عکس خوب یا بدی ثبت کند. سنسور 5 مگاپیکسلی زمانیکه محیط به اندازه کافی روشن باشد، تصاویری باکیفیت و بالاتر از حد انتظار ثبت می‌کند.اما کیفیت تصاویر ثبت شده با این دوربین در محیط‌های تاریک دلچسب نبوده و مناسب اشتراک گذاری در شبکه‌های اجتماعی نیست.با این وجود از آنجاییکه کاربری دوربین سلفی تبلت‌ها بیشتر برای برقراری تماس تصویری و استفاده از برنامه‌های ارتباطی برای تحصیل یا کاربردهای مشابه است، نمی‌توان به کیفیت تصاویر ثبت شده با این دوربین ایرادی وارد دانست.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0303-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207961"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0303-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0303-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0303-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0303-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0303.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>نرم‌افزار دوربین این تبلت بسیار ساده طراحی شده است و به غیر از دو حالت معمول عکاسی و فیلمبرداری امکان عکاسی در حالتLive Focus(برای تار کردن اطراف سوژه)، Pro و پانوراما در نظر گرفته شده است.در بخش تنظیمات گزینه‌هایی برای بهینه کردن صحنه، ذخیره کردن تصاویر و ویدئوها با فرمت بهینه با حفظ کیفیت و HDR خودکار در نظر گرفته شده است.تمامی تصاویر ارائه شده در ادامه مطلب با روشن بودن دو گزینه Scene Optimiser و Auto HDR به ثبت رسیده‌اند.</p>



<figure class=""wp-block-gallery columns-3 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003135_Camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین گلکسی تب A7 2020 سامسونگ"" data-id=""207893"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003135_Camera.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207893"" class=""wp-image-207893"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003135_Camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003135_Camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003135_Camera.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003116_Camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین گلکسی تب A7 2020 سامسونگ"" data-id=""207894"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003116_Camera.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207894"" class=""wp-image-207894"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003116_Camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003116_Camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003116_Camera.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003124_Camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین گلکسی تب A7 2020 سامسونگ"" data-id=""207895"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003124_Camera.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207895"" class=""wp-image-207895"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003124_Camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003124_Camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-003124_Camera.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li></ul></figure>



<h4>نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ</h4>



<figure class=""wp-block-gallery columns-2 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163357-1-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ"" data-id=""207930"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163357-1.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207930"" class=""wp-image-207930"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163357-1-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163357-1-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163357-1-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163357-1.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""><figcaption class=""blocks-gallery-item__caption"">Normal</figcaption></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163348-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ"" data-id=""207931"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163348.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207931"" class=""wp-image-207931"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163348-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163348-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163348-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_163348.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""><figcaption class=""blocks-gallery-item__caption"">Live Focus</figcaption></figure></li></ul></figure>



<figure class=""wp-block-gallery columns-3 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x866"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg2NiIgdmlld0JveD0iMCAwIDExNjAgODY2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""866"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165825-1160x866.jpg.webp"" alt="""" data-id=""207932"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165825.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207932"" class=""wp-image-207932"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165825-1160x866.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165825-400x299.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165825-768x574.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165825.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x866"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg2NiIgdmlld0JveD0iMCAwIDExNjAgODY2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""866"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195043-1160x866.jpg.webp"" alt="""" data-id=""207933"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195043.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207933"" class=""wp-image-207933"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195043-1160x866.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195043-400x299.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195043-768x574.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195043.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x866"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg2NiIgdmlld0JveD0iMCAwIDExNjAgODY2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""866"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195119-1160x866.jpg.webp"" alt="""" data-id=""207934"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195119.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207934"" class=""wp-image-207934"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195119-1160x866.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195119-400x299.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195119-768x574.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_195119.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li></ul></figure>



<figure class=""wp-block-gallery columns-2 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210125_024100-1160x870.jpg.webp"" alt="""" data-id=""207935"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210125_024100.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207935"" class=""wp-image-207935"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210125_024100-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210125_024100-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210125_024100-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210125_024100.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210121_122614-1160x870.jpg.webp"" alt="""" data-id=""207936"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210121_122614.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207936"" class=""wp-image-207936"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210121_122614-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210121_122614-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210121_122614-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210121_122614.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></li></ul></figure>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_171255-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207937"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_171255-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_171255-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_171255-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_171255.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165517-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207938"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165517-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165517-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165517-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_165517.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_170003-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207939"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_170003-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_170003-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_170003-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_170003.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_164825-1-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207941"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_164825-1-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_164825-1-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_164825-1-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/IMG_20210122_164825-1.jpg.webp 1280w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<h3>باتری</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0323-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207963"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0323-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0323-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0323-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0323-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0323.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>برخلاف دوربین، باتری یکی از آن بخش‌هایی است که طراحان تبلت‌های میان‌رده به آن توجه ویژه‌ای می‌کنند.سامسونگ در تبلت A7 جدید خود نیز این موضوع را به خوبی مد نظر داشته و برای این دستگاه یک باتری 7040 میلی آمپرساعتی در نظر گرفته است که در مقایسه با نسل قبل خبر از ارتقای خوبی می‌دهد.در تست بنچمارک PCMark این تبلت زمان 9 ساعت و 44 دقیقه را به ثبت رساند که با توجه به ابعاد و ظرفیت باتری رقم خوبی برای یک گوشی میان‌رده به حساب می‌آید. </p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-110324_PCMark-614x1024.jpg.webp"" alt=""بنچمارک باتری در بررسی گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207890"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-110324_PCMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-110324_PCMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210125-110324_PCMark.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure>



<p>تبلت A7 10.4 با شارژر 7.5 واتی راهی بازار می‌شود، اما از شارژر 15 وات سریع نیز پشتیبانی می‌کند.با شارژر سریع شارژ کامل باتری چیزی در حدود 3 ساعت و 30 دقیقه زمان می‌برد.اما اگر بخواهید این دستگاه را با شارژر درون جعبه شارژ کنید، باید زمان بیشتری را به این موضوع اختصاص دهد.</p>



<figure class=""wp-block-gallery columns-2 is-cropped""><ul class=""blocks-gallery-grid""><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141110_Device-care-614x1024.jpg.webp"" alt=""تنظیمات باتری گلکسی تب A7 2020 سامسونگ"" data-id=""207891"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141110_Device-care.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207891"" class=""wp-image-207891"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141110_Device-care-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141110_Device-care-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141110_Device-care.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li><li class=""blocks-gallery-item""><figure><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141114_Device-care-614x1024.jpg.webp"" alt=""تنظیمات باتری گلکسی تب A7 2020 سامسونگ"" data-id=""207892"" data-full-url=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141114_Device-care.jpg"" data-link=""https://sakhtafzarmag.com/?attachment_id=207892"" class=""wp-image-207892"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141114_Device-care-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141114_Device-care-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Screenshot_20210122-141114_Device-care.jpg.webp 720w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure></li></ul></figure>



<p>در بخش تنظیمات باتری کنار حالت مصرف بهینه(Optimized)، دو حالت Medium-power saving(سطح میانی ذخیره انرژی) و Maximum power saving(بالاترین سطح ذخیره انرژی) برای زمانیکه نیاز به مدیریت مصرف باتری است در نظر گرفته شده‌اند.تنظیمات باتری به صورت پیش‌فرض بر روی حالت بهینه تنظیم شده است.</p>



<p>با این شرایط و در صورت استفاده معمول، یعنی ترکیبی از وبگردی، بازی، شبکه‌های اجتماعی، دوربین، تماس و … باتری دستگاه حداقل یک روز کامل شارژدهی خواهد داشت و این شارژدهی بسته به میزان استفاده می‌تواند تا یک روز و نیم و حتی دو روز افزایش یابد. البته در صورتیکه بخواهید زمان بیشتری را صرف بازی یا فعالیت‌های سنگین دیگر کنید و به تبلت اجازه خاموش شدن نمایشگر را برای مدتی ندهید، میزان دوام باتری کمتر از آن چیزی خواهد شد که انتظار دارید. در مجموع عملکرد باتری این تبلت با وجود ابعاد بزرگ نمایشگر آن خوب است و در صورت مدیریت مصرف انرژی، شارژدهی بسیار خوبی خواهد داشت.</p>



<h3>جمع‌بندی</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0286-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207966"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0286-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0286-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0286-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0286-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0286.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در بازار تبلت‌های میان‌رده انتخاب‌های فراوانی پیش روی کاربران نیست.اصلی‌ترین رقیبان تبلت جدید سامسونگ عموماً دیگر محصولات ساخت همین شرکت هستند که با قیمتی پایین‌تر، امکانات به نسبت پایین‌تری ارائه می‌کنند. با بررسی گلکسی تب A7 2020 سامسونگ به این نتیجه رسیدیم که این تبلت بهترین انتخاب در بازه قیمتی 6 میلیون تومان در بازار ایران است. شاید بتوان دوربین‌های نه چندان توانمند و شارژر کند را در کنار حافظه رم پایین بزرگترین ایرادات این تبلت معرفی کرد.در مقابل اما نمایشگر بزرگ، طراحی زیبا، پردازنده جدید، بلندگوهای چهارگانه استریو در کنار مواردی مانند جک هدفون و باتری بزرگ از مهمترین نقاط قوت این تبلت هستند.به طورکلی این تبلت مناسب برای دو گروه از افراد خواهد بود؛ دانش‌آموزان که قصد انجام تکالیف مدرسه خود به صورت مجازی و از راه دور را دارند و افرادی که به دنبال دستگاهی با نمایشگر بزرگ برای تماشا فیلم و ویدیو و انجام کارهای روزمره هستند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0284-1160x773.jpg.webp"" alt="" تبلت گلکسی تب A7 2020 سامسونگ"" class=""wp-image-207965"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0284-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0284-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0284-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0284-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/DSC_0284.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>با بررسی گلکسی تب A7 2020 سامسونگ وبسایت سخت‌افزار نشان مقرون به‌صرفه‌ترین تبلت بازار در بازه قیمتی مورد نظر را به این محصول اعطاء می‌کند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""340x454"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzNDAiIGhlaWdodD0iNDU0IiB2aWV3Qm94PSIwIDAgMzQwIDQ1NCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" width=""340"" height=""454"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Best-Value-For-Money-Award.jpg.webp"" alt="""" class=""wp-image-207977"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/01/Best-Value-For-Money-Award.jpg.webp 340w, https://sakhtafzarmag.com/wp-content/uploads/2021/01/Best-Value-For-Money-Award-300x400.jpg.webp 300w"" data-sizes=""(max-width: 340px) 100vw, 340px""></figure>



<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span>



<ul><li>طراحی زیبا و متقارن</li><li>وزن ایده‌آل</li><li>جک هدفون 3.5 میلیمتری</li><li>پشتیبانی از کارت حافظه</li><li>بلندگوهای چهارگانه استریو</li><li>صدای استریو و باکیفیت</li><li>باتری بزرگ</li></ul>



</div>



<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span>



<ul><li>روشنایی کم نمایشگر</li><li>میزان کم حافظه رم و داخلی در نسخه پایه</li><li>نداشتن سنسور اثر انگشت</li><li>عملکرد نه چندان چشمگیر دوربین‌ها</li><li>سرعت پایین شارژ</li></ul>



</div>



<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span>



<p>گلکسی تب A7 2020 سامسونگ مناسب برای افرادی است که به دنبال یک تبلت با نمایشگر بزرگ برای تماشای ویدیو، انجام کارهای روزمره و بازی سبک با آن هستند یا دانش‌آموزانی که قصد انجام تکالیف مدرسه خود از طریق سامانه‌های آموزشی آنلاین و یا پیام‌رسان‌ها را دارند.</p>



</div>



<span data-sr=""enter"" class=""gk-review clearfix""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.87""><span>8.7</span></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.85""><span>8.5</span></span><span class=""gk-review-partial-label"">سخت‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.87""><span>8.7</span></span><span class=""gk-review-partial-label"">نرم‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.82""><span>8.2</span></span><span class=""gk-review-partial-label"">صفحه نمایش</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.8""><span>8</span></span><span class=""gk-review-partial-label"">دوربین</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-partial-label"">صدا</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.91""><span>9.1</span></span><span class=""gk-review-partial-label"">طراحی</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.89""><span>8.9</span></span><span class=""gk-review-partial-label"">باتری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 20,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "ششش,تبلت هوشمند",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-Matepad-10.4--1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "بررسی میت پد 10.4 هواوی",
                    Title = "بررسی میت پد 10.4 هواوی – یک تبلت اقتصادی و خوش قیمت",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 14,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

< p > در روزهایی که به واسطه ویروس کرونا بسیاری از مدارس در سراسر کشور به صورت مجازی کلاس‌های خود را دایر می‌کنند، استفاده از تبلت توسط دانش‌آموزان بسیار رایج شده است.امروز با بررسی میت پد 10.4 هواوی یکی از تبلت‌های اقتصادی بازار در خدمت شما هستیم.با ما همراه باشید.</ p >



< p > در دو سال گذشته که با شیوع ویروس کرونا در سراسر دنیا، مدارس در بسیاری از نقاط جهان به صورت غیرحضوری است، تبلت به ابزار کاربردی‌تری برای کودکان، نوجوانان و حتی بزرگسالان تبدیل شده است.تا پیش از کرونا تبلت‌ها اسباب‌بازی مدرن کودکان امروزی به حساب می‌آمدند، اما اگر بخواهیم برای لحظه‌ای مانند بلاگرهای سرخوش اینستاگرامی فراموش کنیم که این ویروس مرگبار روزانه چندصد نفر از هموطنانمان را به زیر خاک می‌فرستند، باید بگوییم که از انگشت شمار جنبه‌های مثبت کرونا این بود که تبلت‌ها را به ابزاری کاربردی برای همگان تبدیل کرد.</ p >



< p > دسترسی دانش‌آموزان به کلاس‌های آنلاین از طریق برنامه‌های آموزشی مانند شاد، برقراری تماس‌های ویدئویی با خانواده در زمان قرنطینه، برگزاری جلسات آنلاین در مواقع دورکاری و … از کاربردهایی هستند که در زمان کرونا، ارزش تبلت‌ها را بیشتر از همیشه به ما نشان دادند.با استقبالی که بازار جهانی از گجت‌های الکترونیکی مانند تبلت در دوران کرونا به عمل آورد، نیاز بازار به محصولات اقتصادی از همیشه بیشتر خود را نشان داد.با این حال، برای انتخاب تبلت خوش قیمتی که به خوبی از پس کارهای مورد انتظار برآید، کار چندان آسانی نیست.</ p >



< h2 id = ""h-10-4"" > بررسی میت پد 10.4 هواوی </ h2 >



< p >< a href = ""https://consumer.huawei.com/en/tablets/matepad/specs/"" target = ""_blank"" rel = ""noreferrer noopener"" > Huawei MatePad 10.4 </ a > یکی از تبلت‌های اقتصادی بازار به حساب می‌آید که امروز تصمیم به بررسی آن گرفته‌ایم.پس اگر به دنبال خرید یک تبلت اقتصادی نه چندان گرانقیمت هستید، به شما توصیه می‌کنیم با ادامه این مطلب همراه ما باشید.لازم به ذکر است که این تبلت از سمت نمایندگی رسمی هواوی در ایران و به مدت 2 هفته برای بررسی در اختیار وبسایت سخت‌افزارمگ قرار گرفته است.</ p >



< h3 id = ""h-"" > محتویات جعبه </ h3 >



< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1152x824"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2.jpg.webp"" loading=""lazy"" width=""1152"" height=""824"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234997 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2.jpg.webp 1152w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2-400x286.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2-768x549.jpg.webp 768w"" data-sizes=""(max-width: 1152px) 100vw, 1152px"" sizes=""(max-width: 1152px) 100vw, 1152px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2.jpg.webp 1152w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2-400x286.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/huawei-matepad2-768x549.jpg.webp 768w"" data-was-processed=""true""></figure>



<p>در جعبه تبلت که سفید رنگ طراحی شده و نقش خود تبلت بر روی آن حک شده، به غیر از خود دستگاه، دفترچه راهنما، کابل شارژر USB-C، کابل تبدیل USB-C به 3.5 میلیمتری، شارژ 10 وات و پین جداکننده شیار سیم کارت جای گرفته است.با در نظر گرفتن اینکه در بسته‌بندی دیگر تبلت‌های بازار هم خبری از هدفون نیست و شارژر هم قرار است در آینده حذف شود، محتویات جعبه این تبلت کامل به نظر می‌رسد.</p>



<h3 id = ""h--1"" > طراحی </ h3 >



< p > تبلت میت پد 10.4 هواوی از نظر طراحی تا حد زیادی شبیه میت پد T10ای است که چندی پیش آن را بررسی کرده بودیم. با این حال، در مقایسه با آن ظاهری شیک‌تر و گرانقیمت‌تر دارد.بدنه این دستگاه در قسمت قاب پشتی کاملا یکپارچه و صاف طراحی شده است و هیچگونه خمیدگی ندارد. این بدنه فلزی از چهار طرف به فریم فلزی دور دستگاه متصل شده است. قسمت پشت تبلت بدنه ماتی نسبت به فریم نسبتاً براق دستگاه دارد ولوگوی هواوی به صورت افقی در مرکز آن هک شده است.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/08/DSC_0338-2.jpg.webp');""></div>



<p>به غیر از لوگوی هواوی و اطلاعات سازنده دستگاه در قسمت پایین آن، محفظه دوربین‌ها و فلش در قسمت بالای سمت چپ(در حالت عمودی) تنها مشخصه‌هایی هستند که در قاب پشت دستگاه جای گرفته‌اند.محفظه دوربین برآمدگی چندان محسوسی ندارد و خوشبختانه قرار دادن آن بر روی سطح صاف و کار کردن باعث لق زدن دستگاه نمی‌شود.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0326-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234984"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0326-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0326-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0326-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0326-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0326.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>این تبلت تنها در یک رنگ خاکستری تیره(Midnight Grey) به بازار عرضه شده است.هرچند که تنوع رنگی می‌توانست برای این دستگاه بیشتر باشد، اما تنها رنگ در نظر گرفته شده برای آن زیبا است، البته اگر مشکل جذب اثر انگشت و لک در آن را نادیده بگیریم و یا دائما در حال تمیز کردن آن باشیم.</p>



<p>در لبه سمت راست (حالت عمودی) کلیدهای تنظیم صدای دستگاه جای گرفته‌اند.چهار میکروفون در راستای این کلیدها برای دریافت صدای همه جانبه در همین لبه فلزی قرار دارند.به سبک آیپدهای اپل، کلید پاور در این تبلت به لبه بالایی دستگاه منتقل شده است. دو بلندگو هم این کلید پاور را همراهی می‌کنند.در لبه سمت چپ تبلت تنها شیار سیم کارت و کارت حافظه قرار گرفته و لبه پایینی پذیرای پورت USB-C و دو بلندگوی دیگر است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342.jpg"" data-slb-active=""1"" data-slb-asset=""362940749"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234986"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0342.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1.jpg"" data-slb-active=""1"" data-slb-asset=""54087172"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234989"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0341-1.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2.jpg"" data-slb-active=""1"" data-slb-asset=""1229469437"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234992"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0340-2.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>
</div>



<p>وجود 4 بلندگوی در دو لبه بالا و پایین دستگاه می‌تواند نوید کیفیت صدای عالی را در این تبلت دهد.با این حال، متأسفانه جک 3.5 میلیمتری هدفون در این تبلت حذف شده است و این یعنی اگر از آن دسته از افرادی هستید که هندزفری از شما جدا نمی‌شود، همواره باید تبدیل 3.5 میلیمتری به USB-C که هواوی در جعبه این تبلت قرار داده را همواره با خود به همراه داشته باشید.</p>



<p>ابعاد این تبلت 245.2×154.96×7.35 میلیمتر است و چیزی در حدود 450 گرم وزن دارد.چیزی در حدود 81 درصد از سطح پنل جلوی میت پد 10.4 اینچی توسط نمایشگری با قطری به همین اندازه احاطه شده است.حاشیه‌های اطراف این نمایشگر از 4 طرف کاملا متقارن به نظر می‌رسند و به غیر از حاشیه بالایی (سمت راست در حالت ایستاده) که دوربین سلفی، سنسور نور محیطی و چراغ نوتیفیکیشن در آن قرار گرفته، هیچ مشخصه دیگری در قسمت جلو تبلت لحاظ نشده است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0348-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234985"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0348-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0348-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0348-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0348-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0348.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>با در نظر گرفتن نحوه حک شدن لوگوی هواوی در پشت تبلت و قرار گیری دوربین سلفی، هواوی تبلت میت پد خود را با هدف کار در حالت افقی طراحی کرده است.با این حال، به غیر از زمان برقراری تماس‌های تصویری و ثبت تصاویر سلفی، به نظر نمی‌رسد که این موضوع اهمیت چندانی داشته باشد.به طورکلی مشخصاتی که هواوی در طراحی این تبلت در نظر گرفته به نظر کامل می‌رسد.نبود جک 3.5 میلیمتری اتفاق جالبی برای میت پد 10.4 نیست، اما هواوی با قرار دادن تبدیل آن در جعبه توانسته این کمبود را جبران کند.شاید تنها ایرادی که بتوان در بخش طراحی به این تبلت وارد دانست، عدم وجود رنگبندی است. رنگ‌های شاد و متنوع برای این دستگاه می‌توانست آن را به گزینه جذاب‌تری&nbsp; برای قشر نوجوان و دانش‌آموز تبدیل کند.</p>



<h3 id = ""h--2"" > صفحه نمایش</h3>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0344-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234981"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0344-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0344-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0344-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0344-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0344.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>میت پد 10.4 اینچی همانطور که از نام آن نیز مشخص است، از یک نمایشگر 10.4 اینچی IPS با رزولوشن 2000×1200 پیکسل و تراکم پیکسلی 224 ppi بهره می‌برد. &nbsp;نسبت ابعاد این نمایشگر 5 به 3 است و روشنایی در سطح 470 نیت ارائه می‌کند.بر روی کاغذ نمایشگری که هواوی برای میت پد خود در نظر گرفته، کاملا در حد و اندازه یک تبلت میان‌رده است.یکی از نکات مثبت این نمایشگر وجود سنسور نور محیطی است که امکان تنظیم خودکار روشنایی نمایشگر را به بسته به نور محیط می‌دهد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0349-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234982"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0349-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0349-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0349-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0349-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0349.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در بخش تنظیمات این نمایشگر به سبک همیشگی محصولات هواوی، گزینه‌های کاربردی و مفیدی در نظر گرفته است.به غیر از تنظیم دستی و خودکار روشنایی صفحه، امکان فعال کردن Natural Tome (تنظیم رنگ نمایشگر براساس نور محیط)، تغییر دمای رنگ(سردی و گرمی)، فعال کردن حالت Eye Comfort(حذف نورهای آبی نمایشگر) در سطوح مختلف، فعال کردن حالت مخصوص مطالعه(eBook mode)، تم تاریک(Dark Mode) و Smart Resolution نیز در نظر گرفته شده است.به غیر از این موارد گزینه‌های معمول تنظیمات نمایشگر برای نمایشگر این دستگاه نیز لحاظ شده است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings-614x1024.jpg"" data-slb-active=""1"" data-slb-asset=""104503822"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234937"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012844_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings-614x1024.jpg"" data-slb-active=""1"" data-slb-asset=""134001091"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234938"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012850_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""1157082831"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234939"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012856_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""1147358712"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234940"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012909_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>
</div>
</div>



<p>نمایشگر میت پد هواوی از نظر کیفیت، دقت رنگ، روشنایی، زاویه دید و امکانات نرم‌افزاری درست همان چیزی است که از یک تبلت میان‌رده و اقتصادی انتظار می‌رود.هواوی طبیعتاً می‌توانست برای این تبلت از نمایشگرهای با تکنولوژی بهتر مانند AMOLED استفاده می‌کرد، اما در آن صورت قیمت نهایی دستگاه از ایده‌آل بودن فاصله می‌گرفت.</p>



<h3 id = ""h--3"" > سخت‌افزار </ h3 >



< figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0356-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234980"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0356-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0356-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0356-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0356-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0356.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>میت پد 10.4 به چیپست میان‌رده ساخت شرکت هواوی، یعنی Kirin 810 مجهز شده است.این پردازنده دارای دو هسته سریع Cortex-A76 با سرعت 2.27 گیگاهرتز و 6 هسته کم‌مصرف A55 با حداکثر کلاک اسپید 1.88 گیگاهرتز است.پردازنده گرافیکی این تراشه 7 نانومتری هم از نوع Mali-G52 MP6 و از نوع 6 هسته‌ای است.</p>



<p>نسخه‌ای که برای بررسی در اختیار ما قرار گرفته دارای 3 گیگابایت حافظه رم و 32 گیگابایت حافظه ذخیره است. این نسخه پایه تبلت میت پد 10.4 است و طبیعتاً میزان حافظه آن برای استفاده بلندمدت اصلا کافی نیست. اما خوشبختانه این دستگاه از کارت حافظه microSD پشتیبانی می‌کند و قابلیت افزایش حافظه از این طریق برای آن در نظر گرفته شده است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_163016_com.android.settings-614x1024.jpg.webp"" alt="" میت پد 10.4 هواوی"" class=""wp-image-234941"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_163016_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_163016_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_163016_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_163016_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_163016_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure>



<p>عملکردی که میت پد هواوی در بنچمارک‌ها از خود نشان داد، همان چیزی است که از یک محصول میان‌رده انتظار می‌رود.در مقایسه با تبلت‌های اندرویدی بازار نتایج کسب شده توسط این تبلت در سطح مطلوبی است.طبیعتاً اعداد و ارقام قابل قیاس با محصولات پرچمدار بازار نیست، اما در سطح یک تبلت میان‌رده سال 2020 رضایت بخش است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application.jpg"" data-slb-active=""1"" data-slb-asset=""1797648897"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application-614x1024.jpg.webp"" alt=""بنچمارک تبلت میت پد 10.4 هواوی"" class=""wp-image-234942"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005156_com.futuremark.dmandroid.application.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark.jpg"" data-slb-active=""1"" data-slb-asset=""2088588039"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark-614x1024.jpg.webp"" alt=""بنچمارک تبلت میت پد 10.4 هواوی"" class=""wp-image-234944"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_010253_com.antutu.ABenchMark.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2.jpg"" data-slb-active=""1"" data-slb-asset=""1101831373"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2-614x1024.jpg.webp"" alt=""بنچمارک تبلت میت پد 10.4 هواوی"" class=""wp-image-234943"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_005334_com.andromeda.androbench2.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark-614x1024.jpg"" data-slb-active=""1"" data-slb-asset=""310194941"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark-614x1024.jpg.webp"" alt=""بنچمارک تبلت میت پد 10.4 هواوی"" class=""wp-image-234947"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012104_com.futuremark.pcmark.android.benchmark.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""2128778315"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark-614x1024.jpg.webp"" alt=""بنچمارک تبلت میت پد 10.4 هواوی"" class=""wp-image-234948"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012725_com.futuremark.pcmark.android.benchmark.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5-614x1024.jpg"" data-slb-active=""1"" data-slb-asset=""1812511863"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5-614x1024.jpg.webp"" alt=""بنچمارک تبلت میت پد 10.4 هواوی"" class=""wp-image-234949"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210807_115943_com.primatelabs.geekbench5.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>



<p>سخت‌افزاری که هواوی برای میت پد 10.4 در نظر گرفته است، بالارده و مخصوص گیم نیست.طبیعتاً با 3 گیگابایت رم نیز نمی‌توان انتظار داشت که بازی‌های جدید و بروز در بالاترین سطح کیفیت اجرای روانی در این دستگاه داشته باشند.با این حال، در بررسی میت پد 10.4 هواوی ما به سراغ محبوب‌ترین بازی‌های موبایلی رفتیم. بازی کالاف دیوتی موبایل در این تبلت به صورت پیش‌فرض با کیفیت گرافیکی متوسط و فریم ریت متوسط اجرا می‌شود که این با در نظر گرفتن مشخصات سخت‌افزاری میت پد ایده‌ال است. البته امکان تنظیم گرافیک و فریم ریت در بالاترین سطح کیفیت نیز برای این دستگاه وجود دارد، اما می‌توان انتظار داشت که نتیجه کار آنگونه که باید رضایت‌بخش نباشد.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x696"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY5NiIgdmlld0JveD0iMCAwIDExNjAgNjk2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""696"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_180451_com.activision.callofduty.shooter-1160x696.jpg.webp"" alt=""کالاف دیوتی موبایل در تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234950"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_180451_com.activision.callofduty.shooter-1160x696.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_180451_com.activision.callofduty.shooter-400x240.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_180451_com.activision.callofduty.shooter-768x461.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_180451_com.activision.callofduty.shooter-1536x922.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_180451_com.activision.callofduty.shooter.jpg.webp 2000w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>میت پد 10.4 هواوی برای انجام عملیات معمول و روزمره مانند دور زدن در شبکه‌های اجتماعی، برقراری تماس، ارسال پیام، استفاده از دوربین و تماشای فیلم و ویدئو عملکرد خوبی دارد.به‌طورکی سخت‌افزار میت‌پد هواوی را در نسخه پایه آن نمی‌توان به عنوان یک نقطه قوت در نظر گرفت.اما با توجه به بازه قیمتی آن، نقطه ضعف نیز به حساب نمی‌آید. این محصول در بازه قیمتی محصولات میان‌رده راهی بازار شده است و با این مشخصات و امکانات می‌تواند گزینه کاملاً ایده‌آلی برای استفاده دانش‌آموزان جهت تحصیل از راه دور و کار با برنامه‌های مدارس و آموزش آنلاین مانند شاد باشد.</p>



<p>برای این تبلت یک روش احراز هویت بایومتریک در نظر گرفته شده که تشخیص چهره است. این عملیات از طریق دوربین سلفی دستگاه میسر خواهد شد که سرعت نسبتاً خوبی دارد.به غیر از تشخیص چهره، راه‌های معمول دیگر تأمین امنیت مانند استفاده از رمز عبور و پین نیز برای میت پد 10.4 لحاظ شده است.علاوه‌براین امکان فعال کردن رمز برای هر برنامه به صورت جداگانه نیز در نظر گرفته شده است که این رمز می‌تواند رمز اصلی تبلت و یا چیزی کاملاً متفاوت از آن باشد.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""273452150"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234951"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210806_012933_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""60091547"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234952"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165109_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>



<p>2 بلندگو در لبه بالا و 2 بلندگو در لبه پایین این تبلت قرار گرفته‌اند که صدا را به صورت استریو به کاربر ارائه می‌کنند.این بلندگوها که توسط هارمن کاردن تیون شده‌اند، تجربه تماشای ویدئو و بازی را در این تبلت بسیار دلچسب می‌کنند. برای من که صدای ارائه صدای استریو در گوشی‌های هوشمند یک نعمت به حساب می‌آید، وجود 4 بلندگو در این تبلت بهترین اتفاق است.البته در بخش صوت این تبلت چندان بی عیب و نقص هم نیست.هواوی برای این دستگاه جک 3.5 میلیمتری هدفون در نظر نگرفته است. هرچند که تبدیل این جک به USB-C در جعبه قرار دارد، اما نبود پورت اختصاصی هدفون یعنی امکان استفاده همزمان از هدفون و شارژر در میت پد 10.4 وجود ندارد. البته اینکه همچنان می‌توانید از هدفون‌هایی با پورت 3.5 میلیمتری با انتخاب خودتان استفاده کنید، خبر خوبی است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0343-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234979"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0343-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0343-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0343-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0343-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0343.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>یکی دیگر از نکات مثبت این تبلت اینست که از قلم پشتیبانی می‌کند.برای افرادی که به کار با قلم در نمایشگرهای بزرگ عادت دارند، تبلت میت پد 10.4 می‌تواند یک گزینه اقتصادی کاملاً ایده‌ال باشد.البته باید در نظر داشته باشید که این تبلت در بسته بندی خود فاقد قلم است و در صورت نیاز باید آن را جداگانه خریداری کند.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei.jpg"" data-slb-active=""1"" data-slb-asset=""602369394"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei-614x1024.jpg.webp"" alt=""پشتیبانی از قلم در تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234953"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223903_com.myscript.nebo_.huawei.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei.jpg"" data-slb-active=""1"" data-slb-asset=""2025486794"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei-614x1024.jpg.webp"" alt=""پشتیبانی از قلم در تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234955"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223914_com.myscript.nebo_.huawei.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei.jpg"" data-slb-active=""1"" data-slb-asset=""785579547"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei-614x1024.jpg.webp"" alt=""پشتیبانی از قلم در تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234954"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210726_223920_com.myscript.nebo_.huawei.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>



<h3 id = ""h--4"" > نرم‌افزار </ h3 >



< p > تبلت میت پد 10.4 با اندروید 10 که با رابط کاربری EMUI 10.1.0 شخصی سازی شده راهی بازار می‌شود.مانند دیگر محصولات جدید هواوی پس از تحریم آمریکا، این تبلت هم فاقد سرویس‌های موبایلی گوگل (Google Mobile Services) است.باوجود آنکه هواوی از زمان اعمال تحریم‌ها تا به امروز توانسته پلتفرم اختصاصی خود، یعنی Harmony OS 2 را با سرویس‌های موبایلی خودش(HMS) توسعه دهد.میت پد 10.4 همچنان دارای سیستم عامل اندروید است، با این حال HMS Core برای ارائه قابلیت‌های انحصاری هواوی در این دستگاه نیز حضور دارد.</p>



<div class=""full-size-image"" style=""background-image: url('/wp-content/uploads/2021/08/DSC_0345-2.jpg.webp');""></div>



<p>با جایگزینی HMS به جای GMS دیگر برنامه‌های پیش‌فرض گوگل(Play Store، Play Services، Youtube، Gmail، Chrome و …) بر روی محصولات جدید هواوی از جمله میت پد 10.4 نصب نیستند.البته تعدادی از این برنامه‌ها را می‌توان از طریق مارکت‌های مختلف دریافت کرد. امکان استفاده از برخی دیگر از سرویس‌های گوگل هم از طریق نسخه وب (مانند یوتیوب) کماکان در دسترس است.با این حال، برنامه‌هایی مانند گوگل پلی سرویس و پلی استور امکان نصب و استفاده ندارد.</p>



<p>هواوی در طول این مدت توانسته جانشین‌هایی را برای برنامه‌های پیش فرض گوگل در محصولاتش ارائه کند که از جمله آنها می‌توان به مرورگر، تقویم، ایمیل و … اشاره کرد. فروشگاه اصلی میت پد جدید هواوی، App Gallery است که از طریق آن می‌توانید برنامه‌های مورد نظر خود را نصب کنید.به غیر از این برنامه، مارکت‌های جانبی دیگری مانند کافه بازار، Aptoid و APKPure هم برای نصب برنامه در دسترس هستند. در کنار تمام این موارد موتور جستجوی اختصاصی هواوی، یعنی Petal Search نیز می‌تواند به کاربران در یافتن برنامه‌های مورد نیاز خود کمک کند و برنامه‌های مورد نیاز آنها را از فروشگاه‌های مختلف در اختیارشان قرار دهد. با کمک پتال سرچ نصب هر برنامه‌ای که اراده کنید در محصولات هواوی ممکن است، هرچند که ممکن است برخی از برنامه‌ها برای اجرا پایشان را در یک کفش کرده و فقط و فقط استفاده را منوط به وجود سرویس‌های گوگل در دستگاه کنند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_003811_com.google.android.youtube-614x1024.jpg.webp"" alt=""یوتیوب در تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234956"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_003811_com.google.android.youtube-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_003811_com.google.android.youtube-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_003811_com.google.android.youtube-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_003811_com.google.android.youtube-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_003811_com.google.android.youtube.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></figure>



<p>باوجود مارکت‌های اندروید فراوان که برای نصب برنامه وجود دارد، بعید به نظر می‌رسد که در این زمینه مشکلی وجود داشته باشد.با این حال، کماکان اجرای یک سری از برنامه‌ها نیازمند وجود گوگل پلی سرویس است و این همچنان آزاردهنده است.خوشبختانه این ایراد در مقایسه با محصولاتی که پیش‌تر از هواوی بررسی کرده بودیم، بسیار کمتر شده و به عنوان مثال در اجرای برنامه‌هایی مانند اسنپ و تپسی دیگر به آن بر نخوردیم.این یعنی می‌توان امیدوار بود که در آینده با آپدیت‌هایی که هواوی برای محصولاتش ارائه می‌کند، نیاز اجرای برنامه‌ها به وجود سرویس‌های گوگل را به حداقل برساند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0350-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234978"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0350-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0350-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0350-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0350-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0350.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>به صورت کلی محیط کاربری EMUI تفاوت چندانی با آنچه که پیش‌تر در گوشی و تبلت‌های هواوی دیده بودیم، ندارد.وجود تم تاریک یا همان Dark Mode به صورت یکپارچه در محیط تبلت از نکات مثبت این نرم‌افزار است.از دیگر امکانات کاربردی نرم‌افزاری این دستگاه می‌توان به فعال کردن منوی کشویی، ژست‌های حرکتی و روشن شدن نمایشگر با بلندکردن تبلت اشاره کرد.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher.jpg"" data-slb-active=""1"" data-slb-asset=""1288655598"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234957"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182217_com.huawei.android.launcher.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher.jpg"" data-slb-active=""1"" data-slb-asset=""1652905065"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234958"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182205_com.huawei.android.launcher.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>



<p>قابلیت دیجیتال بالانس(Digital Balance) جهت کنترل زمان استفاده از تبلت توسط شخص بزرگسال یا کودکان، App Assistant؛ برای بالابردن عملکرد بازی، مسدود کردن اعلانات و Game Space برای ایجاد محیطی جداگانه برای بازی‌ها از امکانات در کنار قابلیت مولتی تسکینگ از امکانات دیگر در محیط نرم‌افزار این تبلت هستند.به‌طورکلی باوجود ایراداتی که همچنان در اجرای برخی از برنامه و بازی‌های نیازمند به سرویس‌های گوگل وجود دارد، محیط نرم‌افزاری هواوی در مقایسه با قبل بسیار بهتر شده است و می‌توان امیدوار بود که با بروزرسانی‌های آینده، این مشکلات نیز برطرف شود.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings-614x1024.jpg"" data-slb-active=""1"" data-slb-asset=""2134448119"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234959"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164852_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol.jpg"" data-slb-active=""1"" data-slb-asset=""108916253"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234960"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165312_com.huawei.parentcontrol.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings.jpg"" data-slb-active=""1"" data-slb-asset=""1647679602"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings-614x1024.jpg.webp"" alt=""تنظیمات میت پد 10.4 هواوی"" class=""wp-image-234961"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_164939_com.android.settings.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>



<h3 id = ""h--5"" > دوربین </ h3 >



< p > ما معمولاً از تبلت‌ها انتظار نداریم که به خوبی گوشی‌های هوشمند عکاسی و فیلمبرداری کنند و معمولاً هم نمی‌توانند از پس این انتظار برآیند.به طورکلی به نظر نمی‌رسد که سازندگان تبلت برای زمان زیادی را صرف دوربین این محصولات کنند.هواوی هم از این قاعده کلی مستثنی نیست.این شرکت دوربین اصلی میت پد 10.4 از یک سنسور 8 مگاپیکسلی استفاده کرده که تصاویری با اندازه 3264×2448 پیکسل ثبت می‌کند و می‌تواند با کیفیت 1080p در نرخ 30 فریم بر ثانیه فیلمبرداری کند.دوربین سلفی این تبلت هم 8 مگاپیکسلی با امکان فیلمبرداری 1080p در نرخ 30 فریم است.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0379-1160x773.jpg.webp"" alt=""دوربین تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234975"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0379-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0379-400x267.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0379-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0379-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0379.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>دوربین اصلی میت پد 10.4 در نور روز و شرایطی که محیط به اندازه کافی روشن باشد می‌تواند تصاویر واضح با کیفیت مناسبی ثبت کند.رنگ‌ها در این تصاویر نزدیک به واقعیت ثبت می‌شوند اما جزئیات آنگونه که باید چشمگیر نیستند و با کمی بزرگنمایی وضعیت ناخوشایند می‌شود. فرقی ندارد که بخواهید در حالت زوم عکاسی کنید و یا پس از ثبت تصویر آن را زوم کنید، در هر حال، کیفیت کار مساعد نیست. با این حال، عکس‌های ثبت شده در حالت 1x قابل قبول هستند و چنانچه در شرایط نوری مناسب ثبت شوند قابلیت اشتراک گذاری در شبکه‌های اجتماعی را نیز خواهند داشت.</p>



<p>&nbsp; این موضوعی است که شاید برای عکاسی شب یا در شرایط کم نور صادق نباشد.تصاویری که این تبلت چه با دوربین اصلی و چه با دوربین سلفی در محیط‌های تاریک ثبت می‌کند، نویز فراوانی داشته و چندان قابل استفاده نخواهند بود.با این حال، کاربرد اصلی دوربین سلفی حداقل برای تبلت‌های اینچنینی ثبت تصاویر سلفی با کیفیت در شب نیست.استفاده برای برقراری تماس تصویری و برگزاری جلسات آنلاین کاری یا تحصیلی از طریق اسکایپ، زوم و … کاربرد اصلی این دوربین است و سنسور 8 مگاپیکسلی دوربین سلفی میت پد 10.4 از این نظر می‌تواند از پس مسئولیت خود برآید.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0325-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234976"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0325-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0325-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0325-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0325-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0325.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>نرم‌افزار دوربین در این تبلت بسیار ساده طراحی شده است.به غیر از حالت معمول عکاسی و فیلمبرداری، یک حالت Beauty برای تصاویر پرتره، Pro برای تنظیم دستی تنظیمات، پانوراما و HDR در نظر گرفته شده است. در بخش تنظیمات نیز امکان تغییر ابعاد عکس، فرمان صوتی، تغییر رزولوشن ویدئو، فریم ریت و … لحاظ شده است.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""396680995"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین  میت پد 10.4 هواوی"" class=""wp-image-234962"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182058_com.huawei.camera.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""922928069"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین  میت پد 10.4 هواوی"" class=""wp-image-234963"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182042_com.huawei.camera.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""1665789511"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین  میت پد 10.4 هواوی"" class=""wp-image-234964"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182018_com.huawei.camera.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera.jpg"" data-slb-active=""1"" data-slb-asset=""717895417"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera-614x1024.jpg.webp"" alt=""تنظیمات دوربین  میت پد 10.4 هواوی"" class=""wp-image-234965"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_182011_com.huawei.camera.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>
</div>
</div>



<h4 id = ""h-10-4-1"" > نمونه تصاویر ثبت شده در بررسی تبلت میت 10.4 هواوی</h4>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-234999"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123517-1160x870.jpg"" data-slb-active=""1"" data-slb-asset=""1700612753"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123517-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235000"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123517-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123517-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123517-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123517.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000307.jpg"" data-slb-active=""1"" data-slb-asset=""1369748297"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000307-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235001"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000307-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000307-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000307-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000307.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a></figure>
</div>
</div>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123607-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235002"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123607-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123607-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123607-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_123607.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131614-1160x870.jpg"" data-slb-active=""1"" data-slb-asset=""1516124222"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131614-1160x870.jpg"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی | 1x"" class=""wp-image-235003"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131614-1160x870.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131614-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131614-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131614.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a><figcaption>نرمال</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131620-1160x870.jpg"" data-slb-active=""1"" data-slb-asset=""1148305795"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131620-1160x870.jpg"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی | زوم 6x"" class=""wp-image-235004"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131620-1160x870.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131620-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131620-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_131620.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></a><figcaption>زوم</figcaption></figure>
</div>
</div>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132011-1160x870.jpg"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235005"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132011-1160x870.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132011-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132011-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132011.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-132054-1160x870.jpg"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی | HDR on"" class=""wp-image-235007"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-132054-1160x870.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-132054-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-132054-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4-132054.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""><figcaption>HDR</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132058-1160x870.jpg"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235008"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132058-1160x870.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132058-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132058-768x576.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_132058.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""><figcaption>بدون HDR</figcaption></figure>
</div>
</div>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101609-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235006"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101609-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101609-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101609-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101609.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000745-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی | Flash on"" class=""wp-image-235009"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000745-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000745-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000745-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000745.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""><figcaption>فلش روشن</figcaption></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000826-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی | Flash off"" class=""wp-image-235010"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000826-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000826-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000826-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000826.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""><figcaption>فلش خاموش</figcaption></figure>
</div>
</div>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000704-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235011"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000704-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000704-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000704-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_000704.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101808-1160x870.jpg.webp"" alt=""نمونه تصاویر ثبت شده در بررسی دوربین تبلت میت 10.4 هواوی"" class=""wp-image-235012"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101808-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101808-400x300.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101808-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Camera-Sample-huawei-Matepad-10.4_101808.jpg.webp 1440w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>
</div>
</div>



<h3 id = ""h--6"" > باتری </ h3 >



< p > میت پد 10.4 به یک باتری 7250 میلی آمپرساعتی مجهز شده است که بر روی کاغذ و از نظر ظرفیت کاملا منطقی و ایده‌آل به نظر می‌رسد.خوشبختانه باتری این دستگاه تنها یک عدد بالا نبود و عملکرد آن در استفاده عملی و روزمره نیز خیره کننده بود. این دستگاه حتی با استفاده مداوم از آن و انجام عملیات پرمصرفی مانند بازی و دوربین باز هم در پایان روز درصد قابل قبولی شارژ داشت.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0347-1160x773.jpg.webp"" alt=""باتری تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234973"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0347-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0347-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0347-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0347-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0347.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>در تست بنچمارک PCMark در بررسی میت پد 10.4 هواوی این تبلت توانست زمان 14 ساعت و 54 دقیقه را به ثبت برساند که این یکی از بالاترین امتیازاتی است که تا امروز در میان تبلت‌های بررسی شده در لابراتوار سخت‌افزار به ثبت رسیده است.جدا از امتیاز خوبی که باتری میت پد 10.4 در بنچمارک کسب کرد، استفاده روزمره و حتی سنگین از این دستگاه هم نشان داد این دستگاه از نظر شارژدهی یکی از بهترین‌ها در رده خود به حساب می‌آید.</p>



<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark.jpg"" data-slb-active=""1"" data-slb-asset=""1986721287"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark-614x1024.jpg.webp"" alt=""بنچمارک باتری  میت پد 10.4 هواوی"" class=""wp-image-234966"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210808_201319_com.futuremark.pcmark.android.benchmark.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark.jpg"" data-slb-active=""1"" data-slb-asset=""391900126"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark-614x1024.jpg.webp"" alt=""باتری  میت پد 10.4 هواوی"" class=""wp-image-234967"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_010014_com.antutu.ABenchMark.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>
</div>



<div class=""wp-block-column"">
<div class=""wp-block-columns"">
<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager.jpg"" data-slb-active=""1"" data-slb-asset=""1072149279"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager-614x1024.jpg.webp"" alt=""باتری  میت پد 10.4 هواوی"" class=""wp-image-234968"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165150_com.huawei.systemmanager.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>



<div class=""wp-block-column"">
<figure class=""wp-block-image size-large""><a href = ""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager.jpg"" data-slb-active=""1"" data-slb-asset=""1888451431"" data-slb-internal=""0"" data-slb-group=""234936""><img data-lazyloaded=""1"" data-placeholder-resp=""614x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2MTQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDYxNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""614"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager-614x1024.jpg.webp"" alt=""باتری  میت پد 10.4 هواوی"" class=""wp-image-234970"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager-614x1024.jpg.webp 614w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager-240x400.jpg.webp 240w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager-768x1280.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager-922x1536.jpg.webp 922w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/Screenshot_20210810_165231_com.huawei.systemmanager.jpg.webp 1200w"" data-sizes=""(max-width: 614px) 100vw, 614px""></a></figure>
</div>
</div>
</div>
</div>



<p>شارژری که هواوی برای این تبلت در جعبه قرار داده 10 وات(5 ولت/2آمپر) است.برای شارژ کامل باتری بزرگ این تبلت با این شارژر آرام به زمان زیادی در حدود 4 ساعت نیاز خواهید داشت.البته 50 درصد از ظرفیت باتری در حدود 1 ساعت و نیم شارژ خواهد شد و نیمه دوم کمی بیشتر طول خواهد کشید. &nbsp; در بخش تنظیمات باتری گزینه Performance Mode در نظر گرفته شده که بهترین سطح از عملکرد دستگاه را ارائه خواهد کرد، با فعال کردن این حالت میزان مصرف باتری کمی بالاتر خواهد رفت.گزینه Power Saving Mode نیز با محدود کردن اپ‌های پس‌زمینه و سینک خودکار میزان مصرف باتری را کمتر از حالت استاندارد خواهد کرد. امکان مدیریت لانچ برنامه‌ها، مشاهده میزان مصرف هر برنامه و جزئیات مصرف باتری نیز در این قسمت در نظر گرفته شده است.</p>



<h3 id = ""h--7"" > جمع‌بندی </ h3 >




< p > میت پد 10.4 هواوی محصول سال 2020 این شرکت به حساب می‌آید که به واسطه سن آن از ظاهر مدرن و امروز بهره می‌برد.لبه‌های صاف، قرار گیری مناسب کلیدهای تنظیم صدا و پاور، حاشیه‌های اطراف نمایشگر و چهار بلندگویی که در دو لبه دستگاه قرار گرفته‌اند، از نکات مثبت در طراحی این تبلت هستند. نمایشگر این تبلت با در نظر گرفتن بازه قیمتی از کیفیت مناسب با روشنایی ایده‌الی برخوردار است و برای تماشای فیلم و عکس و انجام مکالمات تصویری یا آموزش آنلاین مناسب است.</p>



<p>چیپستی که هواوی برای میت پد 10.4 انتخاب کرده، گزینه خوبی است که به راحتی می‌تواند از پس اجرای عملیات روزانه و حتی اجرای بازی در کیفیت متوسط برآید. با این حال، میزان کم حافظه رم و داخلی در پایین‌ترین نسخه می‌تواند در استفاده بلندمدت از دستگاه ایجاد مشکل کند. عملکرد باتری این دستگاه با وجود سرعت پایین شارژر آن کماکان خیره کننده است. با این حال، شاید همچنان اصلی‎ترین ایراد این دستگاه هم به بخش نرم‌افزار آن و نبود سرویس‌های موبایلی گوگل بازگردد.</p>



<p>البته اگر بخواهیم منصفانه به قضیه نگاه کنیم در مقایسه با یک سال گذشته پیشرفت هواوی در مدیریت این وضعیت به معنای واقعی تحسین برانگیز بوده و بسیاری از مشکلات ابتدایی کار تا به اینجا رفع شده‌اند.می‌توان امیدوار بود که در آینده نیز این مشکلات تا حد ممکن مرتفع شود و کاربران بتوانند با سیستم عامل هارمونی تجربه جدیدی از محصولات هواوی داشته باشند.</p>



<figure class=""wp-block-image size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0377-1160x773.jpg.webp"" alt=""تبلت  میت پد 10.4 هواوی | Huawei MatePad 10.4"" class=""wp-image-234974"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0377-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0377-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0377-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0377-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/08/DSC_0377.jpg.webp 1620w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure>



<p>Huawei MatePad 10.4 2020 در بازار ایران با قیمتی در حدود 6 میلیون و 700 هزار تومان(نسخه 3+32 گیگابایت) راهی بازار شده است.در این بازه قیمتی اصلی‎ترین رقیب این تبلت، <a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%af%d9%84%da%a9%d8%b3%db%8c-%d8%aa%d8%a8-a7-2020-%d8%b3%d8%a7%d9%85%d8%b3%d9%88%d9%86%da%af/"" > گلکسی تب A7 2020 سامسونگ</a> به حساب می‌آید.</p>



<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span>



<ul><li>طراحی مدرن و زیبا</li><li>وزن ایده‌آل</li><li>پشتیبانی از کارت حافظه</li><li>بلندگوهای چهارگانه استریو</li><li>صدای استریو و باکیفیت</li><li>عملکرد خیره‌کننده باتری</li></ul>



</div>



<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span>



<ul><li>میزان کم حافظه رم و داخلی در نسخه پایه</li><li>نداشتن سنسور اثر انگشت</li><li>عدم اجرای بازی و برنامه‌های وابسته به سرویس گوگل</li><li>عملکرد نه چندان چشمگیر دوربین‌ها</li><li>سرعت پایین شارژ</li></ul>



</div>



<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span>



<p>میت پد 10.4 هواوی گزینه‌ای مناسب برای افرادی است که به دنبال یک تبلت با نمایشگر بزرگ برای تماشای ویدیو، برقراری مکالمات تصویری، انجام کارهای روزمره و بازی سبک با آن هستند.این تبلت همچنین برای دانش‌آموزانی که قصد انجام تکالیف مدرسه خود از طریق سامانه‌های آموزشی آنلاین و یا پیام‌رسان‌ها را دارند نیز گزینه بسیار ایده‌الی به نظر می‌رسد.</p>



</div>



<span data-sr= ""enter"" class=""gk-review clearfix""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.87""><span>8.7</span></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.87""><span>8.7</span></span><span class=""gk-review-partial-label"">سخت‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.82""><span>8.2</span></span><span class=""gk-review-partial-label"">نرم‌افزار</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.82""><span>8.2</span></span><span class=""gk-review-partial-label"">صفحه نمایش</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.8""><span>8</span></span><span class=""gk-review-partial-label"">دوربین</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.91""><span>9.1</span></span><span class=""gk-review-partial-label"">صدا</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.91""><span>9.1</span></span><span class=""gk-review-partial-label"">طراحی</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.92""><span>9.2</span></span><span class=""gk-review-partial-label"">باتری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.9""><span>9</span></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>



<ul><li><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%d8%aa%d8%a8%d9%84%d8%aa-%d9%85%db%8c%d8%aa-%d9%be%d8%af-t10-%d9%87%d9%88%d8%a7%d9%88%db%8c/"" > بررسی تبلت MatePad T 10 هواوی</a></li><li><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%d9%87%d9%88%d8%a7%d9%88%db%8c-%d8%a8%d9%86%d8%af-6/"" > بررسی دستبند هوشمند هواوی بند 6</a></li><li><a href = ""https://sakhtafzarmag.com/%d8%a8%d8%b1%d8%b1%d8%b3%db%8c-%da%a9%db%8c%d8%b3-%da%a9%d8%a7%d9%85%d9%be%db%8c%d9%88%d8%aa%d8%b1-%da%af%d8%b1%db%8c%d9%86-%d9%85%d8%af%d9%84-aria/"" > بررسی کیس کامپیوتر گرین مدل ARIA</a></li></ul>



<p></p>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 21,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "MSI,لپ تاپ",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_00-1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "بررسی لپتاپ گیمینگ ام اس آی مدل MSI GF63 THIN 10SCSR",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 15,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

< p > سال گذشته میلادی برای محصولات لپتاپ شرکت MSI، سالی بسیار پر رونق و پر فروش بوده است.این شرکت فقط در نیمه اول سال 2020 بیش از 200 میلیون < a href = ""https://sakhtafzarmag.com/tag/%d9%84%d9%be-%d8%aa%d8%a7%d9%be/"" > لپتاپ </ a > گیمینگ در بازارهای جهانی به فروش رساند، که رکوردی بی سابقه در بازار محسوب می‌شود.علاقه بسیار زیادی به لپتاپ‌های گیمینگ MSI در میان تمامی ‌رده‌های کاربری وجود دارد.طراحی ظاهری جذاب، کارایی بالا، نرم افزار‌های جانبی کاربردی، کولینگ اختصاصی، و استفاده از کامپوننت‌های با کیفیت از جمله خصوصیات مثبت لپتاپ‌های ام اس آی به شمار می‌رود.امروز در لابراتوار سایت سخت افزار به بررسی یکی از لپتاپ‌های گیمینگ ام اس آی می‌پردازیم که در ایران هم موجود است و از قضا یکی از پر فروش ترین محصولات حال حاضر بازار به شمار می‌رود.لپتاپ گیمینگ ام اس آی GF63 THIN 10SCSR با پارت نامبر 464XML در رده پایه Entry Level لپتاپ‌های گیمینگ MSI طبقه بندی می‌شود و به نوعی یک گزینه اقتصادی برای گیمر‌ها با بودجه محدود است.</ p >



< h2 id = ""h-"" >< strong > مشخصات فنی محصول:</ strong ></ h2 >



< figure class=""wp-block-table""><table><tbody><tr><td><strong>قطعه</strong></td><td><strong><a href = ""https://www.msi.com/Laptop/GF63-Thin-10SX-GTX"" target=""_blank"" rel=""noreferrer noopener"">MSI GF63 THIN 10SCSR-464XML</a></strong></td><td><strong>مشخصات</strong><strong></strong></td></tr><tr class=""alt""><td><strong>پردازنده</strong><strong></strong></td><td>INTEL CORE I7-10750H</td><td>BOOST: 5.0 GHZ<br>6 CORE – 12 THREAD<br>12MB CACHE</td></tr><tr><td><strong>کارت گرافیک</strong><strong></strong></td><td>NVIDIA GEFORCE GTX1650 Ti MAX Q (+ INTEL UHD 630)</td><td>4GB GDDR6 128BIT GPU BOOST: 1200MHZ MEMORY: 10Gbps</td></tr><tr class=""alt""><td><strong>رم</strong><strong></strong></td><td>SAMSUNG 2X8G DDR4</td><td>2666MHz&nbsp;&nbsp;&nbsp; CL19</td></tr><tr><td><strong>درایو ذخیره سازی</strong><strong></strong></td><td>KIOXIA KBG30ZMV256G NVMe M.2 SSD 256G SEAGATE BARRACUDA ST1000LM048-2E7172 HDD 1TB</td><td>READ SEQ: 1600MB/S<br> WRITE: SEQ: 800MB/S</td></tr><tr class=""alt""><td><strong>نمایشگر</strong><strong></strong></td><td>AU OPTRONICS B156HAN13 AHVA PANEL 120HZ</td><td>1080P 15.6” 120Hz ANTIGLARE, 250 CD/M² 141PPI, sRGB 61%</td></tr><tr><td><strong>شبکه</strong><strong></strong></td><td>INTEL WIRELESS Wi-Fi6 AX201<br>REALTEK GIGABIT LAN<br> BLUETOOTH 5.1</td><td>GIGABIT WI-FI</td></tr><tr class=""alt""><td><strong>کارت صدا</strong></td><td>REALTEK ALC233</td><td>STEREO SPEAKERS</td></tr><tr><td><strong>کیبرد</strong></td><td>MEMBRANE TKL</td><td>کیبرد جزیره ای مجهز به بکلایت قرمز دارای لیبل فارسی</td></tr><tr class=""alt""><td><strong>وبکم</strong><strong></strong></td><td>HD</td><td>30FPS@720P</td></tr><tr><td><strong>درایو نوری</strong></td><td>ندارد</td><td>ندارد</td></tr><tr class=""alt""><td><strong>اسلات کارت حافظه</strong><strong></strong></td><td>ندارد</td><td>ندارد</td></tr><tr><td><strong>سیستم عامل</strong></td><td>ندارد</td><td>ندارد</td></tr><tr class=""alt""><td><strong>باطری</strong><strong></strong></td><td>LITHIOM-ION BATTERY</td><td>51W / Hour</td></tr><tr><td><strong>درگاه‌ها</strong></td><td>1X Type C USB3.2 (GEN1) 3X Type A USB3.2 (GEN1) 1X HDMI (4K @ 30Hz) 1X 3.5mm Jack Audio 1X 3.5mm Jack Mic-in 1X RJ45 LAN Jack</td><td>درگاه TYPE C تاندربولت نیست و خروجی تصویری ندارد</td></tr><tr class=""alt""><td><strong>آداپتور</strong><strong></strong></td><td>120W</td><td>–</td></tr><tr><td><strong>ابعاد</strong></td><td>359x254x21.7mm</td><td>لپتاپ 15.6 اینچی</td></tr><tr class=""alt""><td><strong>وزن بدون آداپتور</strong><strong></strong></td><td>1.8 KG</td><td>–</td></tr><tr><td><strong>وسایل همراه</strong></td><td>ندارد</td><td>ندارد</td></tr></tbody></table></figure>



<p>لپتاپ ام اس آی GF63 THIN 10SCSR یک محصول کامل است.بر عکس بسیاری از لپتاپ‌های اقتصادی که در همان ابتدا نیاز به ارتقای سخت افزار دارند، این لپتاپ به صورت پیش فرض مجهز به 16 گیگابایت رم دو کاناله از نوع DDR4 است که برای گیمینگ و حتی برای بسیاری از امور رندرینگ هم کافی است.پردازنده پر قدرت 12 رشته پردازی Intel Core i7 10750H با قدرت بوست 5 گیگاهرتزی توانمندی فوق العاده ای را برای انجام سریع تمام امور در اختیار کاربر قرار می‌دهد. ترکیب رویایی یک عدد SSD از نوع NVMe و یک‌هارد دیسک بخش ذخیره سازی لپتاپ را تشکیل می‌دهند.</p>



<p>مجهز بودن به کارت گرافیک مستقل NVIDIA GeForce GTX 1650 Ti Max-Q از دیگر امکانات این لپتاپ به شمار می‌رود.برای دوستانی که به مدل Ti کارت گرافیک GTX 1650 آشنایی ندارند، عرض کنم که رمهای کارت گرافیک GTX 1650 را از GDDR5 به GDDR6 ارتقا دادند و بدین ترتیب GTX 1650 Ti متولد شده است. وزن این لپتاپ تنها 1.8 کیلوگرم است و از این بابت جزو لپتاپ‌های سبک وزن بازار به حساب می‌آید. لیبل فارسی و بک لایت قرمز از خصوصیات خوب کیبورد و همچنین صفحه نمایش 120 هرتزی بسیار با کیفیت از دیگر ویژگی‌های خوب این لپتاپ است.خوشبختانه این مدل بدون سیستم عامل ارائه می‌شود و این موضوع برای ما که در ایران زندگی می‌کنیم، یک جور آزادی عمل در نصب هر نوع سیستم عامل دلخواه را به همراه می‌آورد.</p>



<p>از نظر قیمتی این لپتاپ در حال حاضر حدود 31 میلیون تومان با ضمانت رسمی ‌18 ماهه ماتریس در ایران به فروش می‌رسد.ناگفته پیداست که با بودجه 31 میلیون تومانی در ایران امکان ندارد چنین سخت افزاری را بتوان در کلاس دسکتاپ خریداری کرد. بنابراین اگر بودجه شما برای خرید سیستم گیمینگ و یا رندر، در همین حوالی است، این بررسی می‌تواند برای تصمیم گیری راحت تر، شما را یاری کند.</p>



<h2 id = ""h--1"" >< strong > آنباکس و اقلام همراه:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1600x743"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjAwIiBoZWlnaHQ9Ijc0MyIgdmlld0JveD0iMCAwIDE2MDAgNzQzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1600"" height=""743"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_01.jpg.webp"" alt="""" class=""wp-image-231542"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_01.jpg.webp 1600w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_01-400x186.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_01-1160x539.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_01-768x357.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_01-1536x713.jpg.webp 1536w"" data-sizes=""(max-width: 1600px) 100vw, 1600px""></figure></div>



<p>لپتاپ در باکس کارتن قهوه ای قرار دارد که در کنار آن برچسب سفید پارت نامبر و شماره سریال آن نوشته شده است.سپس درون این کارتن، یک جعبه تمام مشکی که آرم MSI به رنگ قرمز بر روی آن نقش بسته، قرار دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x818"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjgxOCIgdmlld0JveD0iMCAwIDExNjAgODE4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""818"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_02-1160x818.jpg.webp"" alt="""" class=""wp-image-231541"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_02-1160x818.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_02-400x282.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_02-768x541.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_02-1536x1083.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_02.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اقلام درون جعبه شامل دفترچه‌های راهنما، کارت گارانتی و آداپتور می‌شود.عملا هیچ وسیله همراهی عرضه نشده است و MSI از این نظر کاملا خساست به خرج داده است.</p>



<h2 id = ""h--2"" >< strong > سخت افزار:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x990"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijk5MCIgdmlld0JveD0iMCAwIDExNjAgOTkwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""990"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_03-1160x990.jpg.webp"" alt="""" class=""wp-image-231540"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_03-1160x990.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_03-400x342.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_03-768x656.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_03-1536x1311.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_03.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>به سراغ لپتاپ برویم.ابعاد دستگاه 359 x 254 x 21.7 میلیمتر است. با داشتن ارتفاعی نزدیک به تنها 2 سانتیمتر الحق و الانصاف کلمه Thin به معنای لاغر و باریک برازنده لپتاپ گیمینگ ام اس آی GF63 THIN 10SCSR است. از نظر متریال به کار رفته در بدنه، بخش تاپ یا همان پشت نمایشگر، بدنه فلزی آلومینومی‌برس خورده کار شده است و زیر لپتاپ پلاستیک مشکی سخت.</p>



<p>این مدل از نظر وزنی تنها 1.8 کیلوگرم است و جزو لپتاپ‌های کلاس گیمینگ سبک وزن به شمار می‌رود. از نظر طراحی ظاهری، تقریبا لبتاپ مستطیل شکل با زاویه‌های 90 درجه در گوشه‌ها است. در ضلع جلو البته کمی ‌بدنه خمیده است.در کل با ظاهری کاملا یونیفرم و تمام مشکی روبه رو هستیم که زیبا است.آرم اژدهای MSI هم به رنگ قرمز به صورت لاکی پشت لپتاپ چاپ شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x418"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQxOCIgdmlld0JveD0iMCAwIDExNjAgNDE4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""418"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_04-1160x418.jpg.webp"" alt="""" class=""wp-image-231539"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_04-1160x418.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_04-400x144.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_04-768x277.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_04-1536x554.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_04.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در حاشیه سمت راست لپتاپ، به ترتیب جایگاه قفل کنزینگتن، سوکت LAN، دو درگاه USB 3.2 Gen 1 و یک درگاه USB 3.2 Gen 1 Type C &nbsp;به همراه دو سوکت جک 3.5 میلیمتری برای هدفون و میکروفون قرار دارد.البته امروزه اکثر هدست‌های گیمینگ از سوکت کومبو استفاده می‌کنند اما به هر صورت همچنان سوکت مجزای میکروفون و هدفون طرفدار خاص خود را دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x412"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQxMiIgdmlld0JveD0iMCAwIDExNjAgNDEyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""412"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_05-1160x412.jpg.webp"" alt="""" class=""wp-image-231538"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_05-1160x412.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_05-400x142.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_05-768x273.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_05-1536x545.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_05.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در پشت دستگاه فقط یک خروجی HDMI قرار دارد که می‌تواند تا رزلوشن 4K با نرخ به روز رسانی 30Hz خروجی تصویری داشته باشد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x731"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjczMSIgdmlld0JveD0iMCAwIDExNjAgNzMxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""731"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_06-1160x731.jpg.webp"" alt="""" class=""wp-image-231537"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_06-1160x731.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_06-400x252.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_06-768x484.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_06-1536x968.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_06.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در حاشیه سمت چپ تنها یک پورت USB 3.2 Gen 1 و جایگاه فیش ورودی پاور قرار دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x837"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjgzNyIgdmlld0JveD0iMCAwIDExNjAgODM3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""837"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_07-1160x837.jpg.webp"" alt="""" class=""wp-image-231536"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_07-1160x837.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_07-400x289.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_07-768x554.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_07-1536x1108.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_07.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>زیر دستگاه بدنه تمام پلاستیکی سخت دارد.در زیر لپتاپ پایه‌های پلاستیکی قرار دارد تا از سر خوردن آن بر روی میز جلوگیری کند.اگر ارتفاع این پایه‌ها کمی ‌بیشتر بود، عملکرد تهویه به صورت بهینه تری صورت می‌گرفت.بخش‌های اطراف کولر اصلی به صورت شبکه ای باز است.می‌توانید به راحتی با استفاده از یک کول پد که فن آن به سمت جلوی دستگاه متمایل باشد آن را خنک کنید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x930"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjkzMCIgdmlld0JveD0iMCAwIDExNjAgOTMwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""930"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_08-1160x930.jpg.webp"" alt="""" class=""wp-image-231535"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_08-1160x930.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_08-400x321.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_08-768x616.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_08-1536x1232.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_08.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>وقتی لپتاپ را باز می‌کنیم با یک کیبرد تمام مشکی با پرینت قرمز رنگ روبه رو می‌شویم.تم محبوب قرمز و مشکی گیمر‌ها کاملا برای طراحی ظاهری لپتاپ رعایت شده است.حتی حاشیه دکمه‌های کیبورد هم قرمز کار شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x856"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg1NiIgdmlld0JveD0iMCAwIDExNjAgODU2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""856"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_09-1160x856.jpg.webp"" alt="""" class=""wp-image-231534"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_09-1160x856.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_09-400x295.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_09-768x566.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_09-1536x1133.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_09.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>کلید پاور در بخش فوقانی کیبرد در سمت راست قرار دارد.درون کلید یک LED قرمز رنگ ریز هم کار شده است.</p>



<h2 id = ""h--3"" >< strong > کیبرد و تاچ پد:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x777"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3NyIgdmlld0JveD0iMCAwIDExNjAgNzc3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""777"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_10-1160x777.jpg.webp"" alt="""" class=""wp-image-231533"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_10-1160x777.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_10-400x268.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_10-768x515.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_10-1536x1029.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_10.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>کیبرد فوق العاده با کیفیت و عالی کار شده است.عملکرد کلید‌ها بسیار نرم است. البته از نظر استاندارد، کیبورد لپتاپ TKL است و بخش ماشین حسابی ندارد.با توجه به کلاس گیمینگ این لپتاپ، فقدان بخش ماشین حسابی قابل اغماض است.بخش تاچ پد اگرچه ابعاد کوچکی دارد اما عملکرد آن بی نقص است.قطعا یک گیمر از موس برای بازی استفاده خواهد کرد اما به هر شکل، وجود یک تاچ پد قوی و Responsive در لپتاپ، همیشه یک موهبت است و خوشبختانه MSI در این بخش نیز کار را به خوبی در آورده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x774"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3NCIgdmlld0JveD0iMCAwIDExNjAgNzc0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""774"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_11-1160x774.jpg.webp"" alt="""" class=""wp-image-231532"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_11-1160x774.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_11-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_11-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_11-1536x1024.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_11.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>نورپردازی قرمز پس زمینه کلید‌ها بسیار عالی است.در اتاق پر نور هم می‌توان به راحتی نورپردازی قرمز را تشخیص داد.</p>



<h2 id = ""h--4"" >< strong > نمایشگر:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x953"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijk1MyIgdmlld0JveD0iMCAwIDExNjAgOTUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""953"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_12-1160x953.jpg.webp"" alt="""" class=""wp-image-231531"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_12-1160x953.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_12-400x329.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_12-768x631.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_12-1536x1261.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_12.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>نمایشگر لپتاپ گیمینگ ام اس آی GF63 THIN 10SCSR دقت فول اچ دی دارد.اما نرخ به روز رسانی آن 120Hz است. این مدل لپتاپ با نمایشگر 144Hz نیز در بازار عرضه شده است.عملکرد نمایشگر به عنوان پنل گیمینگ خوب است. مدل پنل نمایشگر 120 هرتزی B156HAN13 ساخت شرکت AU Optronics است.این پنل از نوع AHVA است اما از نظر کیفی MSI آن را هم سطح IPS می‌داند.اگرچه از نظر دقت نمایش رنگ sRGB سطح آن 61% است اما اگر منحصرا آن را به عنوان پنلی صرفا برای استفاده گیمینگ حساب کنیم، کیفیت قابل قبولی دارد. روکش مات ضد انعاکس نور و Flicker Free بودن از دیگر ویژگی‌های این پنل به شمار می‌رود.</p>



<h2 id = ""h--5"" >< strong > خنک کننده و باطری:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x496"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ5NiIgdmlld0JveD0iMCAwIDExNjAgNDk2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""496"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_13-1160x496.jpg"" alt="""" class=""wp-image-231530"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_13-1160x496.jpg 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_13-400x171.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_13-768x328.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_13-1536x657.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_13.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>لپتاپ ام اس آی GF63 THIN 10SCSR تنها از یک فن بزرگ برای خنک سازی پردازنده و کارت گرافیک استفاده کرده است.دو لوله انتقال حرارت از مس خالص وظیفه خنک کنندگی پردازنده و یک لوله انتقال حرارت که کمی ‌دورتر قرار دارد، وظیفه انتقال گرما از بلاک پشت کارت گرافیک را دارد. من حیث المجموع، این سیستم کولینگ برای کارت گرافیک خوب است اما با توجه به گرمای بسیار زیاد پردازنده Core i7 10750H، خیلی بهتر می‌شد اگر MSI از سه لوله انتقال حرارت برای پردازنده و یا حداقل 2 فن برای خنک کنندگی استفاده می‌کرد. جدا کردن بخش کولینگ پردازنده و کارا گرافیک نیز میتوانست به بهبود کولینگ کمک کند.البته MSI سیستمی‌تحت عنوان Cooler Boost برای لپتاپ در نظر گرفته است که به کمک آن می‌توان سرعت فن را به حداکثر رساند تا قدرت خنک کنندگی آن مناسب حال 10750H شود اما اینکار سرو صدای زیادی را به همراه دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x870"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg3MCIgdmlld0JveD0iMCAwIDExNjAgODcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""870"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_14-1160x870.jpg.webp"" alt="""" class=""wp-image-231529"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_14-1160x870.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_14-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_14-768x576.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_14-1536x1152.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_14.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>شرکت MSI از یک باطری پر قدرت برای این لپتاپ بهره برده است.شما می‌توانید با استفاده عادی و معمول از سیستم حدودا 6 الی 7 ساعت بدون نیاز به اتصال برق، با لپتاپ کار کنید.البته اگر بازی می‌کنید و یا رندر میگیرید، بهتر است حتما آن را به آداپتور متصل نمایید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x898"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg5OCIgdmlld0JveD0iMCAwIDExNjAgODk4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""898"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_15-1160x898.jpg.webp"" alt="""" class=""wp-image-231528"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_15-1160x898.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_15-400x310.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_15-768x595.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_15-1536x1189.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_15.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>مشخصات باطری و توانایی آن در تصویر فوق نمایان است.</p>



<h2 id = ""h--6"" >< strong > بخش صوتی:</strong></h2>



<p>شرکت MSI از چیپ صوتی REALTEK ALC233 برای این لپتاپ بهره گرفته است.این چیپ صوتی به دو اسپیکر در دو طرف لپتاپ متصل است. صدای برآمده از اسپیکر‌ها برای گیمینگ و تماشای فیلم قابل قبول است و کیفیت به نسبت خوبی را به کاربر منتقل می‌کند. اما برای لذت بردن از موسیقی نیاز به یک اسپیکر بهتر دارید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x627"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYyNyIgdmlld0JveD0iMCAwIDExNjAgNjI3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""627"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_16-1160x627.jpg.webp"" alt="""" class=""wp-image-231527"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_16-1160x627.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_16-400x216.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_16-768x415.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_16-1536x830.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_16.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>ناهیمیک همیار صوتی خوب و کابردی در محصولات MSI، در این لپتاپ هم خوش درخشیده.تنظیمات جانبی بسیار کاربردی برای گیم، فیلم و موسیقی، همه در کنترل شماست. چه شما برای گیمینگ از هدست استفاده کنید، خواه از نوع بلوتوثی و بی سیم باشد، خواه از نوع سیمی، تنظیمات ناهیمیک، بسیار کاربردی است.استفاده از این برنامه اعتیاد آور است و امکان ندارد از موسیقی بدون آن بتوان لذت برد.</p>



<h2 id = ""h--7"" >< strong > قدرت پردازش:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x629"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYyOSIgdmlld0JveD0iMCAwIDExNjAgNjI5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""629"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_17-1160x629.jpg.webp"" alt="""" class=""wp-image-231526"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_17-1160x629.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_17-400x217.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_17-768x417.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_17-1536x833.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_17.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>پردازنده Intel Core i7 10750H تقریبا یک پله از نسل پیشین خود، یعنی 9750H قوی تر است.پردازنده 10750H حداکثر فرکانس بوست 5 گیگاهرتزی دارد و مجهز به 6 هسته و 12 رشته پردازشی است که با 12 مگابایت کش Level 3 قدرت فوق العاده ای را برای انجام انواع پردازش‌های سنگین به کاربر می‌دهد.این پردازنده 45 واتی مجهز به گرافیک مجتمع Intel UHD630 است که در زمان استفاده از لپتاپ به صورت کاربری عمومی‌و اداری فعال است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x779"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3OSIgdmlld0JveD0iMCAwIDExNjAgNzc5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""779"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_18-1160x779.jpg.webp"" alt="""" class=""wp-image-231525"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_18-1160x779.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_18-400x269.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_18-768x516.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_18-1536x1032.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_18.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در قسمت حافظه موقت، این لپتاپ مجهز به دو کیت 8 گیگابایتی از نوع DDR4(مجموع 16 گیگابایت SDRAM دو کاناله) در فرکانس 2666MHz با تایمینگ CL19 می‌باشد.خوشبختانه این میزان رم برای انجام گیمینگ کافی است و برای اغلب رندر‌های نیمه حرفه ای هم مناسب است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1065x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDY1IiBoZWlnaHQ9IjEwMjQiIHZpZXdCb3g9IjAgMCAxMDY1IDEwMjQiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1065"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_19-1065x1024.jpg.webp"" alt="""" class=""wp-image-231524"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_19-1065x1024.jpg.webp 1065w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_19-400x385.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_19-768x738.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_19-1536x1476.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_19.jpg.webp 1600w"" data-sizes=""(max-width: 1065px) 100vw, 1065px""></figure></div>



<p>در بنچمارک AIDA64 میزان پهنای باند در دسترس و زمان تاخیر رم نمایان است.ترکیب این رم و پردازنده، قدرت پردازشی مناسبی را در اختیار کاربر قرار می‌دهد که می‌تواند از پس تمام بازی‌ها و همچنین رندرینگ‌های نیمه سنگین و انواع Multi-Tasking‌ها برآید.</p>



<h2 id = ""h--8"" >< strong > بخش گرافیک:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x743"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc0MyIgdmlld0JveD0iMCAwIDExNjAgNzQzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""743"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_20-1160x743.jpg.webp"" alt="""" class=""wp-image-231523"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_20-1160x743.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_20-400x256.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_20-768x492.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_20-1536x984.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_20.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>مشخصات گرافیک مجتمع Intel HD Graphics UHD630 و همچنین گرافیک قدرتمند GTX1650Ti MAX-Q به کار گرفته شده در این لپتاپ را مشاهده می‌کنید.فرکانس GPU گرافیک GTX1650Ti به کار رفته در این لپتاپ، در حالت Boost به 1200 مگاهرتز می‌رسد.سرعت حافظه گرافیک نیز به 10Gbps می‌رسد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x566"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU2NiIgdmlld0JveD0iMCAwIDExNjAgNTY2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""566"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_21-1160x566.jpg.webp"" alt="""" class=""wp-image-231522"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_21-1160x566.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_21-400x195.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_21-768x374.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_21-1536x749.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_21.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>کارت گرافیک GTX 1650 Ti مورد استفاده در این لپتاپ از فن آوری MAX-Q انویدیا استفاده میکند که ضمن امکانپذیر ساختن به کار گیری این گرافیک در ابعاد کوچکتر و نازکتر، از نظر دمایی نیز بتواند در حداکثر سازگاری با کولرهای لپتاپ، خنک شود.</p>



<h2 id = ""h--9"" >< strong > دمای کاری:</strong></h2>



<p>تعارف نداریم پردازنده Intel Core i7 10750H رسما یک بخاری برقی است و به این راحتی رام نمی‌شود. به همین دلیل شرکت MSI سیستمی‌تحت عنوان Cooler Boost برای تسکین دمای این پردازنده به کار گرفته است. دقت کنید بدون فعال سازی سیستم Cooler Boost ام اس آی، دمای پردازنده و کارت گرافیک به بالای 90 درجه می‌رسد. پس باید به هنگام استفاده پردازشی سنگین از لپتاپ، این گزینه را حتما فعال کنید.</p>



<p>مسلما دمای کاری کارت گرافیک GTX1650 Ti اینقدری بالا نیست اما به دلیل نزدیکی هر سه لوله انتقال حرارت در کنار یکدیگر ممکن است دمای کاری بالای 10750H بر روی دمای کاری کارت گرافیک تاثیر منفی بگذارد.</p>



<p>اما سیستم Cooler Boost به سان یک نجات دهنده با بالا بردن شدت گردش فن تحت لودهای سنگین، عملیات خنک سازی را به صورت بسیار بهینه تری انجام می‌دهد.تنها انتقاد ما البته صدای زیاد فن در چنین حالتی است.البته اگر از هدست به هنگام گیمینگ استفاده کنید، صدای فن مزاحمتی برای شما نخواهد داشت.</p>



<h2 id = ""h--10"" >< strong > درایو ذخیره سازی:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x499"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ5OSIgdmlld0JveD0iMCAwIDExNjAgNDk5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""499"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_22-1160x499.jpg.webp"" alt="""" class=""wp-image-231521"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_22-1160x499.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_22-400x172.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_22-768x330.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_22-1536x660.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_22.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>خوشبختانه دپارتمان ذخیره سازی لپتاپ ام اس آی GF63 THIN 10SCSR کامل است و شاهد حضور دو نوع درایو ذخیره سازی هستیم:</p>



<ul><li>یک عدد اس اس دی از نوع NVMe M.2 مدل KBG30ZMV256G ساخت شرکت کیوکشیا(توشیبا) با حجم 256 گیگابایت.</li><li>یک عدد‌هارد دیسک مدل Seagate ST1000LM048-2E7172 با سرعت 5400RPM و حجم 1 ترابایت</li></ul>



<p>وجود SSD در لپتاپ و یا حتی سیستم‌های دسکتاپ، از نان شب هم واجب تر است.چرا عملا کامپیوتر بدون SSD نمی‌تواند از تمام قدرت Multi-Tasking خود استفاده کند و‌هارد دیسک‌ها همواره گلوگاه سیستم خواهند بود.خوشبختانه MSI برای این لپتاپ به صورت پیش فرض از یک حافظه SSD بهره گرفته است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x639"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYzOSIgdmlld0JveD0iMCAwIDExNjAgNjM5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""639"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_23-1160x639.jpg.webp"" alt="""" class=""wp-image-231520"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_23-1160x639.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_23-400x221.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_23-768x423.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_23-1536x847.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_23.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اما اس اس دی KBG30ZMV256G کیوکشیا به نسبت دیگر SSD‌های روز دنیا، تقریبا ضعیف است.بنابراین برای دوستانی که به صورت جدی از این لپتاپ استفاده گیمینگ و یا رندر دارند، ارتقای این حافظه را توصیه می‌کنیم.مثلا عملکرد اس اس دی سامسونگ 970 Evo Plus تا بیش از 3 برابر سریعتر از این اس اس دی کیوکشیا است.دمای کاری این اس اس دی هم بالاست و به 75 درجه سانتیگراد می‌رسد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x750"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc1MCIgdmlld0JveD0iMCAwIDExNjAgNzUwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""750"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_24-1160x750.jpg.webp"" alt="""" class=""wp-image-231519"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_24-1160x750.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_24-400x259.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_24-768x497.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_24-1536x994.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_24.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>البته این اس اس دی به هر صورت ده‌ها برابر از‌هارد دیسک سریعتر است و از عهده انجام سریع کار‌های روزمره به خوبی بر می‌آید.به لطف وجود اس اس دی، اینترنت گردی، ایمیل زدن، و تمامی ‌امور پردازی، بسیار لذت بخش تر است.</p>



<h2 id = ""h--11"" >< strong > شبکه:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-1160x653.jpg.webp"" alt="""" class=""wp-image-231518"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_25.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در بخش شبکه، شرکت MSI سنگ تمام گذاشته است.استفاده از کارت شبکه با پهنای باند 2.4Gbps از نوع Wi-Fi 6 (802.11ax 2×2) ساخت اینتل در کنار بلوتوث نسخه 5.1، همه‌ی آن چیزی است که شما امروزه در بخش اتصالات بی سیم به آن نیاز دارید.نسبت به نسلهای پیشین همانطور که در تصویر مشخص است، این نسل از ارتباطات وایرلس، تقریبا نزدیک به 40% نسبت به نسل پیشین پهنای باند بیشتری دارد. این دستگاه همچنین دارای یک درگاه LAN گیگابیتی منشعب از کنترلر Realtek است.البته ما چیپ LAN ساخت اینتل را بیشتر می‌پسندیم اما Realtek هم شرکت خوبی است و وظیفه خود را به خوبی انجام می‌دهد.</p>



<h2 id = ""h--12"" >< strong > بخش نرم افزاری:</strong><strong></strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x650"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MCIgdmlld0JveD0iMCAwIDExNjAgNjUwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""650"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-1160x650.jpg.webp"" alt="""" class=""wp-image-231517"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-1160x650.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-400x224.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-768x430.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-1536x860.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_26.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>پکیج نرم افزاری اصلی همراه لپتاپ MSI Dragon Center است.مانیتورینگ دمای بخش‌های مختلف لپتاپ، میزان حجم در دسترس رم لپتاپ، حجم‌های آزاد درایو‌های ذخیره سازی، میزان مشغولیت گرافیک، پردازنده، ارتباطات بی سیم و وضعیت پاور از امکانات خوب این نرم افزار است.</p>



<p>سیستم Cooler Boost را از طریق این نرم افزار می‌توانید فعال کنید. همچنین تعریف پروفایل‌های مختلف کاری و حتی اورکلاک کارت گرافیک نیز از طریق این برنامه امکان پذیر است.</p>



<p>نرم افزار MSI Dragon Center یک حالت Gaming Mode هم دارد که به لطف آن تمامی ‌تنظیمات ویندوز و سخت افزار لپتاپ برای اجرای بازی‌ها در نهایت FPS بهینه سازی می‌شوند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x662"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY2MiIgdmlld0JveD0iMCAwIDExNjAgNjYyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""662"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_27-1160x662.jpg.webp"" alt="""" class=""wp-image-231516"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_27-1160x662.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_27-400x228.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_27-768x438.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_27-1536x876.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_27.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>یکی دیگر از شاهکارهای نرم افزاری همراه این لپتاپ، App Player است که حاصل همکاری MSI و شرکت سازنده شبیه ساز اندروید Bluestacks است.تقریبا همه‌ی ما با نرم افزار Bluestacks آشنا هستیم، این برنامه بهترین شبیه ساز اندروید در ویندوز است که به لطف آن می‌توان از اپ‌های اندرویدی در فضای ویندوز استفاده کرد.تنها مشکل Bluestacks تبلیغات آزاردهنده آن است که به لطف وجود شرکت MSI، این اپ پر طرفدار، بدون تبلیغات اضافی، با تمامی ‌امکانات به همراه لپتاپ گیمینگ ام اس آی GF63 THIN 10SCSR در اختیار شما قرار می‌گیرد. پس گیمینگ با اپ‌های اندرویدی هم به راحتی با این لپتاپ امکان پذیر است.</p>



<h2 id = ""h--13"" >< strong > تست‌های بازی:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1610x1040"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjEwIiBoZWlnaHQ9IjEwNDAiIHZpZXdCb3g9IjAgMCAxNjEwIDEwNDAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1610"" height=""1040"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_28.png.webp"" alt="""" class=""wp-image-231515"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_28.png.webp 1610w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_28-400x258.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_28-1160x749.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_28-768x496.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_28-1536x992.png.webp 1536w"" data-sizes=""(max-width: 1610px) 100vw, 1610px""></figure></div>



<p>کارت گرافیک GTX 1650Ti اگرچه در کلاس گیمینگ است اما کارت گرافیک Entry Level و پایه محسوب می‌شود.ما اصولا تست‌های گیمینگ را در سخت ترین شرایط با حداکثر تنظیمات و جزییات می‌گیریم.بنابراین اینکه مثلا در بازی Metro Exodus تنها 23 فریم ثبت شده است، در تنظیمات عادی و متوسط، خروجی FPS تقریبا دو برابر این میزان برای این بازی و بقیه عناوین است.از این رو تمامی ‌این بازی‌ها را به راحتی می‌توان با این لپتاپ انجام داد به شرطی که جزییات گرافیک در حد متوسط باشد. مثلا بازی جدید Cyber Punk به راحتی با این لپتاپ در جزییات کمتر امکان پذیر است.</p>



<p>من حیث المجموع، عملکرد کلی گیمینگ این لپتاپ، باتوجه به بضاعت GTX 1650 Ti در دقت FULL HD قابل قبول ارزیابی می‌شود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1610x1652"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjEwIiBoZWlnaHQ9IjE2NTIiIHZpZXdCb3g9IjAgMCAxNjEwIDE2NTIiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1610"" height=""1652"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_29.jpg.webp"" alt="""" class=""wp-image-231514"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_29.jpg.webp 1610w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_29-390x400.jpg.webp 390w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_29-998x1024.jpg.webp 998w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_29-768x788.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_29-1497x1536.jpg.webp 1497w"" data-sizes=""(max-width: 1610px) 100vw, 1610px""></figure></div>



<p>به سراغ 3D Mark و تست Time Spy می‌رویم.این بنچمارک یک تست گیمینگ محسوب می‌شود و امتیاز گرافیک (ستون زرد رنگ) قدرت گیمینگ لپتاپ را نشان می‌دهد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1723x1720"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzIzIiBoZWlnaHQ9IjE3MjAiIHZpZXdCb3g9IjAgMCAxNzIzIDE3MjAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1723"" height=""1720"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30.png.webp"" alt="""" class=""wp-image-231513"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30.png.webp 1723w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30-400x400.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30-1026x1024.png.webp 1026w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30-150x150.png.webp 150w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30-768x767.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_30-1536x1533.png.webp 1536w"" data-sizes=""(max-width: 1723px) 100vw, 1723px""></figure></div>



<p>بنچمارک Unigine Superposition با استفاده از موتور Unigine 2.0 به خلق تصاویری بدیع از یک اتاق کار و حرکت دوربین در آن می‌پردازد.این نرم افزار بسیار سنگین، قدرت سیستم شما را برای اجرای نرم افزار‌های شبیه سازی سه بعدی، Post-Processing، گیمینگ، و حرکت دوربین در میان یک ساختمان همزمان با اجرای الگوریتم‌های پیچیده رهگیری نور SSRTGI تحت فشار قرار می‌دهد و میزان توانایی آن را با امتیاز بیان می‌کند. دوستانی که به دنبال تهیه یک سیستم مناسب گیم و همچنین نرم افزار‌های شبیه سازی سه بعدی، واقعیت مجازی VR، انیمیشن، و صد البته گیمر‌ها، توجه ویژه ای به نتایج این بنچ مارک داشته باشند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1598x1334"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNTk4IiBoZWlnaHQ9IjEzMzQiIHZpZXdCb3g9IjAgMCAxNTk4IDEzMzQiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1598"" height=""1334"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_31.png.webp"" alt="""" class=""wp-image-231512"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_31.png.webp 1598w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_31-400x334.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_31-1160x968.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_31-768x641.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_31-1536x1282.png.webp 1536w"" data-sizes=""(max-width: 1598px) 100vw, 1598px""></figure></div>



<p>یکی از بهترین تست‌های بازی، بنچ مارک Final Fantasy است که قدرت سیستم را تحت یک امتیاز مشخص ارائه می‌کند.قدرت پردازنده، رم، کارت گرافیک و اس اس دی، در نتیجه این نرم افزار تاثیر مستقیم دارد.<strong></strong></p>



<h2 id = ""h--14"" >< strong > تست‌های رندر:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1642x1372"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjQyIiBoZWlnaHQ9IjEzNzIiIHZpZXdCb3g9IjAgMCAxNjQyIDEzNzIiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1642"" height=""1372"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_32.png.webp"" alt="""" class=""wp-image-231511"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_32.png.webp 1642w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_32-400x334.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_32-1160x969.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_32-768x642.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_32-1536x1283.png.webp 1536w"" data-sizes=""(max-width: 1642px) 100vw, 1642px""></figure></div>



<p>نرم افزار سینی‌بنچ(Cinebench) از معروفترین نرم افزار‌های سنجش قدرت پردازش سرعت CPU و کارت گرافیک است که یک صحنه سه بعدی را رندر می‌کند و پردازنده و کارت گرافیک شما را به اندازه معقولی تحت فشار قرار می‌دهد.هر چه سیستم شما بتواند صحنه مورد نظر را با سرعت بیشتری رندر کند، برنامه امتیاز بالاتری به آن می‌دهد.ما از سینی‌بنچ به صورت متمرکز برای سنجش قدرت پردازنده هم به صورت چند رشته پردازشی و هم به صورت تک رشته پردازشی استفاده می‌کنیم.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1720x1720"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzIwIiBoZWlnaHQ9IjE3MjAiIHZpZXdCb3g9IjAgMCAxNzIwIDE3MjAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1720"" height=""1720"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33.png.webp"" alt="""" class=""wp-image-231510"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33.png.webp 1720w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33-400x400.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33-1024x1024.png.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33-150x150.png.webp 150w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33-768x768.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_33-1536x1536.png.webp 1536w"" data-sizes=""(max-width: 1720px) 100vw, 1720px""></figure></div>



<p>موتور رندر Corona یکی از معروفترین موتور‌های رندر تصاویر واقعی در میان نرم افزار‌های رندر است که می‌تواند هم به صورت یک نرم افزار مجزا و هم به عنوان یک Plugin در برنامه‌های Autodesk 3ds Max و MAXON Cinema 4D مورد استفاده قرار گیرد.نرم افزار بنچمارک Corona Benchmark از موتور نسخه 1.3 کرونا رندر استفاده می‌کند و می‌تواند قدرت چند رشته پردازشی پردازنده را (تا حداکثر 72 رشته پردازشی) با رندر کردن تصویری واحد بسنجد.این نرم افزار بسیار سنگین است و هرچه پردازنده هسته‌های سریعتر و رشته‌های پردازشی بیشتری داشته باشد، سرعت رندر گیری در این نرم افزار بیشتر می‌شود و در نتیجه مدت زمان انجام پروژه کوتاه تر خواهد بود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1626x1165"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjI2IiBoZWlnaHQ9IjExNjUiIHZpZXdCb3g9IjAgMCAxNjI2IDExNjUiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1626"" height=""1165"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_34.png.webp"" alt="""" class=""wp-image-231509"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_34.png.webp 1626w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_34-400x287.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_34-1160x831.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_34-768x550.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_34-1536x1101.png.webp 1536w"" data-sizes=""(max-width: 1626px) 100vw, 1626px""></figure></div>



<p>تست V-RAY از جمله نرم افزار‌های سنجش قدرت رندر پردازنده است که هر چه مدت زمان به پایان رساندن پروژه کوتاه تر باشد، قدرت پردازنده بیشتر است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1653x722"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjUzIiBoZWlnaHQ9IjcyMiIgdmlld0JveD0iMCAwIDE2NTMgNzIyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1653"" height=""722"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_35.png.webp"" alt="""" class=""wp-image-231508"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_35.png.webp 1653w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_35-400x175.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_35-1160x507.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_35-768x335.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_35-1536x671.png.webp 1536w"" data-sizes=""(max-width: 1653px) 100vw, 1653px""></figure></div>



<p>بنچ مارک رندر V-Ray Next جدیدترین نرم افزار تست قدرت رندر کامپیوتر V-RAY است.این بنچمارک یک تصویر را به صورت زنده برای شما رندر کرده و امتیازی را به پردازنده و کارت گرافیک می‌دهد. از این جهت می‌توان از این نرم افزار برای تست قدرت رندر پردازنده و کارت گرافیک استفاده کرد که ما از آن برای تست هر دو بهره گرفته ایم و امتیازهای کارت گرافیک در سمت راست و امتیاز پردازنده در سمت چپ نمایان است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1654x696"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjU0IiBoZWlnaHQ9IjY5NiIgdmlld0JveD0iMCAwIDE2NTQgNjk2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1654"" height=""696"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_36.png.webp"" alt="""" class=""wp-image-231507"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_36.png.webp 1654w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_36-400x168.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_36-1160x488.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_36-768x323.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_36-1536x646.png.webp 1536w"" data-sizes=""(max-width: 1654px) 100vw, 1654px""></figure></div>



<p>یکی از بهترین نرم افزار‌های تست مبدل‌های ویدیویی، نرم افزار HEVC Decode Benchmark(Cobra) است که در سایت ثبت رکورد HWBOT مخصوص اورکلاکر‌ها یافت می‌شود.این نرم افزار با استفاده از کدک H.265 به تبدیل سه نوع ویدیو &nbsp;HD، FHD، و 4K می‌پردازد و سپس به هر بخش امتیاز داده و یک امتیاز کلی را هم برای سیستم در نظر می‌گیرد.کاربران عزیزی که در سیستم‌های خود تبدیل‌های ویدیویی دارند به نتایج این بنچ مارک توجه ویژه ای داشته باشند. لازم به ذکر است عملیات تبدیل ویدویی در این برنامه تماما بر روی پردازنده انجام می‌شود و از این لحاظ این نرم افزار تست پردازنده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1422x1180"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNDIyIiBoZWlnaHQ9IjExODAiIHZpZXdCb3g9IjAgMCAxNDIyIDExODAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1422"" height=""1180"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_37.png.webp"" alt="""" class=""wp-image-231506"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_37.png.webp 1422w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_37-400x332.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_37-1160x963.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_37-768x637.png.webp 768w"" data-sizes=""(max-width: 1422px) 100vw, 1422px""></figure></div>



<p>نرم افزار GEEKBENCH یکی از بهترین تست‌ها برای پردازنده و کارت گرافیک است.لپتاپ MSI GF63 THIN 10SCSR در این تست، صدر جدول را در میان دیگر لپتاپ‌های قدرتمند بررسی شده در لابراتوار سخت افزار از آن خود کرد.این نرم افزار با تمرکز بر روی قدرت CPU و رم سیستم، نسبت به سنجش قدرت چند رشته و تک رشته پردازشی اقدام می‌کند. در بخش پردازنده این نرم افزار با بررسی قدرت سیستم شما در زمینه ادیت تصاویر، انجام عملیات روزانه پردازشی و Augmented Reality و Machine Learning امتیازی را به پردازنده شما می‌دهد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1612x814"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjEyIiBoZWlnaHQ9IjgxNCIgdmlld0JveD0iMCAwIDE2MTIgODE0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1612"" height=""814"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_38.png.webp"" alt="""" class=""wp-image-231505"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_38.png.webp 1612w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_38-400x202.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_38-1160x586.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_38-768x388.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_38-1536x776.png.webp 1536w"" data-sizes=""(max-width: 1612px) 100vw, 1612px""></figure></div>



<p>در بخش سنجش کارت گرافیک یا&nbsp; Compute Benchmark، این نرم افزار می‌تواند با انجام تست‌های متمرکز بر OpenCL و CUDA نسبت به امتیاز دهی به بخش گرافیکی سیستم شما اقدام می‌کند.امتیاز Compute Benchmark بیانگر قدرت کارت گرافیک در نرم افزار‌های تدوین ویدیو، تدوین عکس، و همچنین گیمینگ، بر اساس قدرت هسته‌های Cuda و OpenCL است.</p>



<h2 id = ""h--15"" >< strong > تست‌های عمومی:</strong><strong></strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1720x2190"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzIwIiBoZWlnaHQ9IjIxOTAiIHZpZXdCb3g9IjAgMCAxNzIwIDIxOTAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1720"" height=""2190"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39.png.webp"" alt="""" class=""wp-image-231504"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39.png.webp 1720w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39-314x400.png.webp 314w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39-804x1024.png.webp 804w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39-768x978.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39-1206x1536.png.webp 1206w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_39-1608x2048.png.webp 1608w"" data-sizes=""(max-width: 1720px) 100vw, 1720px""></figure></div>



<p>و در آخر به سراغ تست PCMARK 10 می‌رویم که شامل طیف متنوعی از تست‌های کاری روزمره به همراه تست‌های سنگین کاربردی رندرینگ است.این برنامه امتیازات مشخصی را به هر بخش از یک سیستم اعطا می‌کند. همانطور که در تصویر مشخص است، نتیجه ی تست Essentials، شامل تست سرعت اجرای برنامه‌ها، ویدیو کنفرانس، و گشت و گذار در اینترنت نتیجه فوق العاده ای دارد همچنین در بحث Productivity، شامل تست‌های اداری، از جمله صفحات محاسبانی، نوشتار و عملکرد Office نتیجه عالی است.در بخش تولید محتوی دیجیتال هم این نرم افزار امتیاز خوبی را به این لپتاپ اعطا کرده است. پکیج نرم افزاری PC MARK 10 یک تست ترکیبی اداری و رندرینگ است، و تقریبا یک لپتاپ را از همه‌ی جهات می‌سنجد.</p>



<p><strong>نتیجه گیری:</strong></p>



<p>یک لپتاپ خوب مناسب گیمینگ، نیاز به فن آوری نرم افزاری، هنر، سلیقه، سخت افزار قوی و به همان نسبت کولینگ پر قدرت دارد.شرکت MSI بارها ثابت کرده است که تمامی ‌این خصوصیات را دارد و یکی از توانمند ترین پیشتازان صنعت ساخت لپتاپ در دنیاست.همیشه<a href=""https://sakhtafzarmag.com/tag/%d9%84%d9%be-%d8%aa%d8%a7%d9%be-msi/""> لپتاپ‌های MSI</a>، چه آنها که برای گیم ساخته می‌شوند و چه دیگر مدلها، جایگاه خاصی برای من داشتند.از این رو با خیال راحت محصولات این برند را به خریداران پیشنهاد می‌دهم.</p>



<p>اما برویم به سراغ لپتاپ&nbsp; MSI GF63 THIN 10SCSR، که تقریبا دو هفته میهمان ما در لابراتوار بود.وزن بسیار مناسب 1.8 کیلوگرمی‌اولین نکته مثبتی است که می‌توان در مورد این دستگاه بیان کرد.مدل مورد بررسی ما نمایشگر 120 هرتزی داشت اما مدلهای موجود در بازار ایران نمایشگر 144 هرتزی دارند، به هر شکل، از نمایشگر لپتاپ GF63 THIN 10SCSR، کیفیت تصویر، عملکرد گیمینگ، و سیستم محافظت از چشم آن، رضایت کامل داریم.</p>



<p>تعداد پورت‌های USB و تنوع آنها خوب است. البته انتظار داشتیم درگاه USB Type-C لپتاپ قوی تر از USB 3.2 Gen 1 باشد، اما به هر جهت، این موردی نیست که در این رنج قیمتی انتظارش را نداشته باشیم. کیفیت و عملکرد کیبرد و تاچ پد همه قابل قبول است.از نظر طراحی ظاهری لپتاپ، جنس بدنه، کیفیت متریال مورد استفاده در ساخت آن، تماما رضایت کامل را داریم.لپتاپ مشکی و نور پس زمینه کیبرد قرمز است، و این دو ترکیب کلاسیک گیمینگ هستند اما امروزه سیستم‌های گیمینگ، اکثرا به سمت RGB رفته اند. صد البته این لپتاپ را باید در سال 2020 بررسی می‌کردیم و برای آن زمان شاید قابل قبول بود، اما در سال 2021، واقعا انتظار نورپردازی RGB از یک لپتاپ مخصوص گیم را داریم.</p>



<p>در بخش صدا، کیفیت صدای خروجی بسیار مطلوب ارزیابی می شود. در زمینه نرم افزار های همراه، MSI سنگ تمام گذاشته است و نرم افزار بهینه ساز و سیستم مانیتورینگ خوبی را برای استفاده بهینه از لپتاپ در نظر گرفته است.</p>



<p>از نظر قیمتی، این لپتاپ در حدود 31 میلیون تومان در بازار عرضه شده است که، البته ارزان نیست، ولی گران قیمت هم نیست.با نگاهی به دیگر مدلهای موجود در این رنج قیمتی، می‌توان گفت این هزینه معقولی برای یک لپتاپ با این مشخصات، در وضعیت و شرایط امروز بازار کامپیوتر ایران است.از نظر رقابتی، صد البته از برند MSI یک مدل لپتاپ دیگر MSI GF75 Thin 10SCSR در بازار ایران موجود است که از نظر پردازنده و کارت گرافیک، سخت افزار مشابهی با مدل مورد بررسی ما دارد.اگر از سایز 17 اینچی و کیبورد فول سایز GF75 بگذریم، مهمترین برتری آن نسبت به GF63، در بخش کولینگ است. شرکت MSI در مدل GF75 کولینگ به مراتب قوی تر و بهتری استفاده کرده، اما در مدل GF63، تا حد ممکن اقتصادی ترین راهکار را در بخش کولر به کار گرفته است. بنابراین، برای دوستانی که مطلقا امکان افزایش بودجه بیشتر از 31 میلیون تومان را ندارند، لپتاپ MSI GF63 THIN 10SCSR، یک سیستم گیمینگ قابل قبول در سطح Entry Level است که می‌تواند نیاز‌های گیمینگ را در حد خودش به خوبی برآورده کند. اما اگر قرار است برای مدت طولانی از لپتاپ تحت فشار‌های سنگین استفاده کنید و کارهای جدی مانند رندرینگ انجام دهید، درصورت توانمندی مالی حتما توصیه می کنیم افزایش بودجه دهید و مدل MSI GF75 Thin 10SCSR را در نظر بگیرید.</p>



<p>در پایان، ضمن تشکر ویژه از دفتر محترم MSI برای در اختیار قرار دادن این لپتاپ برای بررسی، نشان ارزش خرید سایت سخت افزار را به آن اهدا می‌کنیم.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><img data-lazyloaded=""1"" data-placeholder-resp=""255x341"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNTUiIGhlaWdodD0iMzQxIiB2aWV3Qm94PSIwIDAgMjU1IDM0MSI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_40.jpg.webp"" alt="""" class=""wp-image-231503"" width=""255"" height=""341"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_40.jpg.webp 340w, https://sakhtafzarmag.com/wp-content/uploads/2021/07/MSI-GF63-THIN-10SCSR_40-300x400.jpg.webp 300w"" data-sizes=""(max-width: 255px) 100vw, 255px""></figure></div>



<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span>



<ul><li>باریک و سبک وزن</li><li>پکیج نرم افزاری کامل</li><li>مجهز به SSD و HDD</li><li>فاقد سیستم عامل اورجینال</li><li>طراحی بدنه بسیار خوب و محکم</li><li>کیبرد با لیبل فارسی و مجهز به نور پس زمینه قرمز</li><li>دارای نمایشگر 120 هرتزی AHVA (در سطح IPS)</li><li>مجهز به پردازنده قدرتمند اینتل Core i7-10750H</li><li>مجهز به کارت گرافیک مستقل GTX1650 Ti Max-Q</li><li>مجهز به 16 گیگابایت رم 2666 مگاهرتز به صورت دو کاناله</li><li>نرم افزار شبیه ساز اندروید مجازی برای اجرای اپ‌های اندرویدی</li></ul>



</div>



<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span>



<ul><li>فاقد RGB</li><li>هیچ وسیله همراه اضافی ندارد</li><li>صدای فن در لود‌های بالا شنیده می‌شود</li><li>سرعت حافظه SSD پایین و عملکرد آن داغ است</li><li>خرید کول پد برای خنک نگه داشتن لپتاپ توصیه می‌شود</li><li>عملکرد داغ در برخی سناریو‌های پردازشی طولانی رندرینگ با محوریت پردازنده</li></ul>



</div>



<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span>



<ul><li>در حال حاضر لپتاپ ام اس آی GF63 THIN 10SCSR یکی از انتخاب‌های خوب برای گیمینگ در رنج قیمتی 31 میلیون تومان است</li></ul>



</div>



<span data-sr=""enter"" class=""gk-review clearfix""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.84""><span>8.4</span></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.85""><span>8.5</span></span><span class=""gk-review-partial-label"">پرفورمنس</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.89""><span>8.9</span></span><span class=""gk-review-partial-label"">صفحه‌نمایش</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.68""><span>6.8</span></span><span class=""gk-review-partial-label"">کولینگ لپتاپ</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.9""><span>9</span></span><span class=""gk-review-partial-label"">طراحی‌ظاهری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.85""><span>8.5</span></span><span class=""gk-review-partial-label"">باطری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.83""><span>8.3</span></span><span class=""gk-review-partial-label"">کیبورد و تاچ پد</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.88""><span>8.8</span></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 22,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "GIGABYTE,لپ تاپ",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_00-1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 15,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

< p > شرکت گیگابایت از سال 2017، سری لپتاپ‌های Workstation خود را تحت خانواده Aero به بازار عرضه کرده است.اگر خانواده Aorus را سری مخصوص گیمینگ در نظر بگیریم، لپتاپ‌های Aero، سری مخصوص تولید کنندگان محتوی و رندرکاران به شمار می‌رود.بسیاری از پاور یوزرهایی که لپتاپ‌های & nbsp; رده بالای گیمینگ تهیه می‌کنند، بیشتر برای استفاده از قدرت بالای سخت افزار آنها برای اهدافی به غیر از گیمینگ است. اینجاست که گیگابایت، سری Aero را مخصوص این کاربران طراحی و عرضه کرده است.در حقیقت، لپتاپ‌های آئرو به مانند یک ایستگاه کاری حرفه ای، عملکردی در سطح بالا، برای انجام امور جدی دارد.اما، لزوما همه چیز به کار محدود نمی‌شود، از آنجایی که تجهیزات به کار رفته در این نوع لپتاپ‌ها بسیار رده بالاست، از آن می‌توان برای مصارف گیمینگ هم استفاده کرد.پس در کلاس آئرو اول کار در اولویت است، و بعد کارهای دیگر مانند بازی در رده دوم اهمیت قرار می‌گیرند.</ p >
 



 < p > امروز در اتاق تست سخت افزار به بررسی لپتاپ مخصوص طراحی<a href= ""https://sakhtafzarmag.com/tag/%DA%AF%DB%8C%DA%AF%D8%A7%D8%A8%D8%A7%DB%8C%D8%AA/"" > گیگابایت </ a > مدل < a href= ""https://www.gigabyte.com/Laptop/AERO-15-OLED--Intel-11th-Gen/"" target= ""_blank"" rel= ""noreferrer noopener"" > GIGABYTE AERO 15 OLED XD</ a > می‌پردازیم.این لپتاپ از طرف شرکت محترم گیگابایت در اختیار ما قرار گرفته است که از ایشان کمال تشکر را داریم.خوشبختانه امید است این سری از لپتاپ‌ها، با گارانتی رسمی ‌آواژنگ ظرف هفته‌های آتی وارد بازار ایران شوند. پس بدون معطلی پس از تماشای ویدیو معرفی، به سراغ مشخصات لپتاپ می‌رویم.</ p >
               



               < figure class=""wp-block-embed is-type-rich is-provider-جاسازی-نگهدارنده wp-block-embed-جاسازی-نگهدارنده""><div class=""wp-block-embed__wrapper"">
<div class=""h_iframe-aparat_embed_frame""><span></span><iframe data-lazyloaded=""1"" src=""https://www.aparat.com/video/video/embed/videohash/OR5Ct/vt/frame"" data-src=""https://www.aparat.com/video/video/embed/videohash/OR5Ct/vt/frame"" allowfullscreen=""true"" webkitallowfullscreen=""true"" mozallowfullscreen=""true"" class=""litespeed-loaded"" data-was-processed=""true""></iframe></div>
</div></figure>



<h2 id = ""h-"" >< strong > مشخصات فنی محصول:</strong></h2>



<figure class=""wp-block-table""><table><tbody><tr><td><strong>قطعه</strong></td><td><strong>GIGABYTE AERO 15 OLED XD</strong><strong></strong></td><td><strong>مشخصات</strong><strong></strong></td></tr><tr class=""alt""><td><strong>پردازنده</strong><strong></strong></td><td>INTEL CORE I7-11800H</td><td>BOOST: 4.6GHz<br>8CORE – 16THREADs<br>24MB CACHE</td></tr><tr><td><strong>کارت گرافیک</strong><strong></strong></td><td>NVIDIA RTX 3070 MAX-Q (+ INTEL UHD G4)</td><td>8GB GDDR6 256BIT<br> GPU BOOST: 1290MHz<br> MEMORY: 12Gbps</td></tr><tr class=""alt""><td><strong>رم</strong><strong></strong></td><td>MICRON 2X16G DDR4</td><td>2666MHz&nbsp;&nbsp;&nbsp; CL19</td></tr><tr><td><strong>درایو ذخیره سازی</strong><strong></strong></td><td>ESR01TBTLG-E6GBTNB4 NVMe M.2 SSD 1TB<br> PCIe Gen.4 x4</td><td>READ SEQ: 5000MB/s<br> WRITE: SEQ: 2500MB/s</td></tr><tr class=""alt""><td><strong>نمایشگر</strong><strong></strong></td><td>SAMSUNG 4K ATNA56WR14-0 AMOLED 100% DCI-P3<br> PANTONE VALIDATED</td><td>4K 15.6” 60Hz HDR400 GLARE<br>440CD/M² 283PPI, X-RITE COLOR</td></tr><tr><td><strong>شبکه</strong><strong></strong></td><td>INTEL WIRELESS Wi-Fi6 AX200<br>REALTEK 2.5 GIGABIT LAN<br>BLUETOOTH 5.2</td><td>GIGABIT WI-FI</td></tr><tr class=""alt""><td><strong>کارت صدا</strong></td><td>REALTEK ALC255</td><td>2W STEREO SPEAKERS</td></tr><tr><td><strong>کیبورد</strong></td><td>FULL SIZE GIGABYTE FUSION RGB</td><td>کیبورد جزیره ای مجهز به بکلایت آر جی بی<br> فاقد لیبل فارسی</td></tr><tr class=""alt""><td><strong>وبکم</strong><strong></strong></td><td>HD</td><td>30FPS@720P</td></tr><tr><td><strong>درایو نوری</strong></td><td>ندارد</td><td>ندارد</td></tr><tr class=""alt""><td><strong>اسلات کارت حافظه</strong><strong></strong></td><td>دارد</td><td>UHS-II</td></tr><tr><td><strong>سیستم عامل</strong></td><td>دارد</td><td>WINDOWS 10</td></tr><tr class=""alt""><td><strong>باطری</strong><strong></strong></td><td>LITHIOM-ION BATTERY</td><td>99W / Hour</td></tr><tr><td><strong>درگاه‌ها</strong></td><td>1X Type C USB 4.0 / Thunderbolt 4 (40Gbps) <br>3X Type A USB3.2 (GEN1)<br>1X HDMI 2.1 (4K@120Hz / 8K@60Hz) 1X Mini-DP 1.4 <br>1X 3.5mm Jack Audio &amp; Mic COMBO<br>1X RJ45 LAN Jack</td><td>درگاه TYPE C تاندربولت است<br> خروجی تصویری دارد</td></tr><tr class=""alt""><td><strong>آداپتور</strong><strong></strong></td><td>230W</td><td>–</td></tr><tr><td><strong>ابعاد</strong></td><td>356x25x21.3mm</td><td>لپتاپ 15.6 اینچی</td></tr><tr class=""alt""><td><strong>وزن بدون آداپتور</strong><strong></strong></td><td>2.3KG</td><td>–</td></tr><tr><td><strong>وسایل همراه</strong></td><td>ترمال پد اضافی برای اس اس دی NVMe</td><td>–&nbsp;</td></tr></tbody></table></figure>



<p>لپتاپ AERO 15 OLED XD گیگابایت مجهز به پردازنده‌های 10 نانومتری نسل یازدهم Tiger Lake<a href=""https://sakhtafzarmag.com/tag/%d8%a7%db%8c%d9%86%d8%aa%d9%84/""> اینتل</a> است.در کانفیگ لپتاپ مورد بررسی ما، از پردازنده 11800H استفاده شده و پردازنده‌های مختوم به H معرف High Performance، و رده بالا بودن این دستگاه‌ها را نشان می‌دهد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-1160x653.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243475"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_01.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اینتل در نسل 11 پردازنده‌های موبایل خود، تغییرات اساسی نسبت به نسل‌های پیشین داده است.پشتیبانی از PCIe Gen.4 و به دنبال آن افزایش خطوط PCIe Lane، پشتیبانی از ارتباط Thunderbolt 4، پشتیبانی از رمهای DDR4 تا فرکانس 3200Mhz، پشتیبانی از ارتباط شبکه بی سیم Wi-Fi 6E و غیرو، مواردی است که اینتل در این نسل از پردازنده‌های خود برای کاربران حرفه ای به ارمغان آورده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x653"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MyIgdmlld0JveD0iMCAwIDExNjAgNjUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""653"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-1160x653.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243474"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-1160x653.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-400x225.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-768x432.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-1536x864.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02-960x540.jpg.webp 960w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_02.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>ناگفته نماند اینتل در رده پردازنده‌های موبایل لپتاپ، یه سرو گردن از رقیب خود AMD پیشتر است بنابراین مهمترین رقیب اینتل در هر نسل، محصولات خودش است.از این رو ارتقای توانمندی نسل 11 را نسبت به نسل 10 اینتل در برخی برنامه‌های تولید محتوی را در تصویر فوق مشاهده می‌کنید.</p>



<p>اما بازگردیم به<a> لپتاپ </a>AERO 15 OLED XD گیگابایت، که در کنار پردازنده قوی، از کارت گرافیک RTX 3070 Mobile انویدیا استفاده کرده است.این کارت گرافیک مستقل، از طراحی MAX-Q انویدیا بهره می‌برد. به لطف فن آوری MAX-Q انویدیا، کارتهای گرافیک تا جای ممکن باریک ساخته می‌شوند و کم مصرفتر کار می‌کنند میزان گرامی‌تولید شده در لپتاپ کاهش یابد اما در عین حال از کارایی کارت گرافیک زیاد کم نشود.در عوض طول عمر باطری افزایش یافته و لپتاپ سبکتر خواهد بود.</p>



<p>شاهکار گیگابایت در این لپتاپ به کارت گرافیک و پردازنده محدود نمی‌شود. نمایشگر فوق العاده زیبای OLED سامسونگ، الماس و گل سر سبد این لپتاپ است که تصاویر در آن، در عین زیبایی، با دقت رنگ بسیاری بالا و استاندارد فوق حرفه ای به نمایش در می‌آید.&nbsp;&nbsp;</p>



<p>از نظر قیمت این لپتاپ در خارج از ایران در حال حاضر حدود 2100 دلار قیمت دارد و انتظار می‌رود در صورت موجودی در بازار ایران در رنج قیمتی 65 الی 70 میلیون تومان به فروش برسد.بنابراین اگر بودجه شما برای خرید سیستم ایستگاه کاری و تولید محتوی، در همین حدود است، این بررسی می‌تواند برای تصمیم گیری راحت تر، شما را یاری کند.</p>



<h2 id = ""h--1"" >< strong > جعبه و اقلام همراه:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x890"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg5MCIgdmlld0JveD0iMCAwIDExNjAgODkwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""890"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_03-1160x890.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243473"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_03-1160x890.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_03-400x307.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_03-768x589.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_03-1536x1179.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_03.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>لپتاپ در باکس تمام مشکی سامسونیت مانند قرار دارد.بر روی باکس کلمه گیگابایت به رنگ سفید و آرم AERO به شکل RGB چاپ شده است.از همان رنگ و لعاب جعبه مشخص است که با لپتاپی مجهز به نورپردازی RGB سروکار داریم.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x602"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYwMiIgdmlld0JveD0iMCAwIDExNjAgNjAyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""602"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_04-1160x602.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243472"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_04-1160x602.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_04-400x208.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_04-768x398.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_04-1536x797.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_04.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اقلام درون جعبه شامل دفترچه‌های راهنما، کارت گارانتی و آداپتور می‌شود.البته یک جفت ترمال پد کوچک برای یک درایو SSD اضافی از نوع M.2 نیز همراه لپتاپ است که بسیار کوچک و ریز بودند.عملا هیچ وسیله همراهی با لپتاپ عرضه نشده است و گیگابایت کاملا خساست به خرج داده است.</p>



<h2 id = ""h--2"" >< strong > سخت افزار:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x304"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjMwNCIgdmlld0JveD0iMCAwIDExNjAgMzA0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""304"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_05-1160x304.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243471"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_05-1160x304.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_05-400x105.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_05-768x201.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_05-1536x402.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_05.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>از رو به رو با نگاهی به ضخامت 2.1 سانتیمتری لپتاپ، متوجه ساخت و مهندسی یک دست و بی نظیر آن می‌شویم.این لپتاپ اگرچه تجهیزات قدرتمندی دارد اما به غایت در کلاس خود باریک ساخته شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x851"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg1MSIgdmlld0JveD0iMCAwIDExNjAgODUxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""851"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_06-1160x851.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243470"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_06-1160x851.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_06-400x294.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_06-768x564.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_06-1536x1127.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_06.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>آرم شکسته زیبای AERO در پشت اسکرین لپتاپ به صورت قالبی نمایان است.متریال به کار رفته در ساخت بدنه، در همه بخش‌ها فلزی است. &nbsp;بخش تاپ یا همان پشت نمایشگر، بدنه فلزی آلومینومی‌‌دارد که در بخش انتهای آن، باریکه ای از طرحی مشبک و زیبا با استفاده از لیتوگرافی چاپی نانو با فلز و CNC قالب گیری شده و بدین شکل زیبا درآمده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x910"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjkxMCIgdmlld0JveD0iMCAwIDExNjAgOTEwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""910"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_07-1160x910.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243469"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_07-1160x910.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_07-400x314.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_07-768x602.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_07-1536x1205.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_07.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>به هنگام روشن شدن دستگاه آرم AERO نیز به رنگ سفید روشن می‌شود و نمایی جدی و کاری را از پشت لپتاپ ایجاد می‌کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x357"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjM1NyIgdmlld0JveD0iMCAwIDExNjAgMzU3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""357"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_08-1160x357.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243468"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_08-1160x357.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_08-400x123.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_08-768x236.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_08-1536x472.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_08.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در لبه پشتی صرفا سوراخ‌های تهویه وجود دارند و در وسط آنها بج فلزی با آرم AERO قرار گرفته است.پشت لپتاپ شبیه لامبورگینی طراحی شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x399"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjM5OSIgdmlld0JveD0iMCAwIDExNjAgMzk5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""399"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_09-1160x399.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243467"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_09-1160x399.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_09-400x138.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_09-768x264.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_09-1536x528.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_09.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در حاشیه سمت چپ، خروجی HDMI 2.1 که توانایی 4K@120Hz دارد، در کنار خروجی Mini-DP، و یک درگاه USB 3.2 Gen.1 قرار دارد.یک عدد جک 3.5 میلیمتری از نوع مشترک Combo برای اتصال هندزفری و در آخر اتصال LAN از نوع 2.5 گیگابیتی قرار گرفته است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x422"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQyMiIgdmlld0JveD0iMCAwIDExNjAgNDIyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""422"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_10-1160x422.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243466"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_10-1160x422.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_10-400x146.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_10-768x279.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_10-1536x559.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_10.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در حاشیه سمت راست دستگاه، دو عدد درگاه USB 3.2 Gen.1، به همراه پورت Type C تاندربولت 4، شیار کارتخوان پر سرعت UHS-II که توانایی خوانش تا سرعت 300MB/s دارد قرار گرفته است.در انتها نیز اتصال برق آداپتور قرار دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x918"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjkxOCIgdmlld0JveD0iMCAwIDExNjAgOTE4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""918"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_11-1160x918.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243465"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_11-1160x918.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_11-400x317.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_11-768x608.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_11-1536x1215.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_11.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در زیر لپتاپ یک درپوش سراسری قرار گرفته که نیمی‌از آن برای عبور هوا تقریبا باز است.پیچ‌های لپتاپ از نوع ستاره ای است که کمی ‌بازکردن و سرویس آن را برای کاربران عادی مشکل می‌کند.در زیر لپتاپ پایه‌های پلاستیکی قرار دارد تا از سر خوردن آن بر روی میز جلوگیری کند. اگر ارتفاع این پایه‌ها کمی ‌‌بیشتر بود، عملکرد تهویه به صورت بهینه تری صورت می‌گرفت. استفاده از خنک کننده‌های کولپدی که بتوانند در انتها ایجاد تهویه مناسبی داشته باشند، برای این لپتاپ به شدت پیشنهاد می‌شود</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x990"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijk5MCIgdmlld0JveD0iMCAwIDExNjAgOTkwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""990"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_12-1160x990.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243464"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_12-1160x990.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_12-400x342.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_12-768x656.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_12-1536x1311.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_12.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در لپتاپ تنها با یک دست و بدون نیاز به نگه داشتن بدنه باز می‌شود.همچنین به هنگام بستن درب آن در فاصله 10 سانتیمتری به صورت اتوماتیک بسته می‌شود.</p>



<h2 id = ""h--3"" >< strong > کیبورد و تاچ پد:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x771"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MSIgdmlld0JveD0iMCAwIDExNjAgNzcxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""771"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_13-1160x771.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243463"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_13-1160x771.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_13-400x266.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_13-768x510.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_13-1536x1020.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_13.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>یکی از بخش‌های جذاب لپتاپ AERO 15 OLED XD گیگابایت کیبورد فوق العاده زیبای آن است.کلید‌ها برجسته اما عملکرد نرمی‌دارند.نسبت به کیبورد‌های مخصوص تایپ، کلید‌ها کمی ‌مسافت بیشتری دارند.خوشبختانه کیبورد کامل است و بخش ماشین حسابی دارد.نورپردازی کیبورد کاملا RGB است و الگوی نورپردازی از طریق نرم افزار قابل شخصی سازی، در حدی که می‌توان برای هر کلید رنگ خاصی را مشخص کرد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_50-1160x773.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243462"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_50-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_50-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_50-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_50-1536x1023.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_50.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>بخش تاچ پد اگرچه ابعاد به نسبت کوچکی دارد اما عملکرد آن رضایت بخش است.چه طراح، چه گیمر، صد در صد از ماوس مناسب استفاده خواهد کرد اما وجود یک تاچ پد قوی و Responsive در لپتاپ، همیشه یک موهبت است و خوشبختانه گیگابایت در این بخش کم نگذاشته است.در حاشیه بالای این تاچ پد یک حسگر تشخیص اثر انگشت نیز قرار دارد تا از نظر امنیتی، ورود به ویندوز و استفاده از لپتاپ، محدود به مالک آن باشد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x650"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY1MCIgdmlld0JveD0iMCAwIDExNjAgNjUwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""650"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-1160x650.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243461"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-1160x650.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-400x224.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-768x431.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-1536x861.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-384x216.jpg.webp 384w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14-576x324.jpg.webp 576w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_14.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اثری از پرینت فارسی بر روی کلید‌ها نیست.همچنین نورپردازی کلید‌ها صرفا عملکرد اصلی آنها را دربرمی‌گیرد، و عملکرد ثانویه یا FN نورپردازی ندارد.از نظر شدت نور، کیبورد RGB <a>لپتاپ</a> AERO 15 OLED XD گیگابایت حتی در اتاق پر نور هم به زیبایی خودنمایی می‌کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_15-1160x773.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243460"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_15-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_15-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_15-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_15-1536x1023.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_15.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>گیگابایت وبکم دستگاه را در بخش فوقانی کیبورد قرار داده است.این وبکم دقت HD دارد و نسبت به مابقی دستگاه از جایگاه پایینتری برخوردار است.جایگاه وبکم به گونه است که زاویه مستقیم به سوراخ دماغ کاربر دارد.بنابراین از نظر نگارنده این جایگاه چندان محل مناسبی برای وبکم نیست.اما از یاد نبریم، این یک لپتاپ مخصوص تولید محتوی و طراحی است، لزوما قرار نیست با آن ویدیو کنفرانس اداری دهید و یا با آن استریم کنید، بنابراین، وبکم در چنین دستگاهی جایگاه خاصی ندارد، به همین خاطر سختگیری خاصی نمی‌کنیم.&nbsp;</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x583"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU4MyIgdmlld0JveD0iMCAwIDExNjAgNTgzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""583"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_16-1160x583.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243459"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_16-1160x583.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_16-400x201.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_16-768x386.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_16-1536x772.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_16.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>گیگابایت برای حفظ امنیت کاربر، یک درپوش پلاستیکی بر روی وبکم قرار داده است تا در صورت عدم استفاده از وبکم، روی آن پوشیده باشد.</p>



<h2 id = ""h--4"" >< strong > نمایشگر:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x676"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY3NiIgdmlld0JveD0iMCAwIDExNjAgNjc2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""676"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_17-1160x676.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243458"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_17-1160x676.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_17-400x233.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_17-768x447.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_17-1536x895.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_17.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>نمایشگر به کار رفته در این لپتاپ، از نوع AMOLED است.این نوع نمایشگر‌ها به دلیل شفافیت فوق العاده بالا، و دقت بی نظیرشان در نمایش تصاویر، بسیار گران قیمت هستند. برخلاف دیگر انواع پنل که یک بکلیت کلی در پشت پنل به پیکسل‌ها نوردهی می‌کند، در پنل‌های OLED، هر پیکسل تک به تک نوردهی مخصوص به خود را دارد، و به هنگام نمایش تصاویر تاریک، کاملا خاموش می‌شود.این موضوع باعث برتری مطلق نمایشگر‌های OLED در به تصویر کشیدن سایه‌ها و رنگ سیاه دارد. گیگابایت اما، چند قدم پا را فراتر گذاشته و یک پنل OLED، با دقت 4K به سامسونگ سفارش داده است. این پنل با پارت نامبر ATNA56WR14-0 و تراکم پیکسلی فوق العاده 283PPI، دارای شدت نور بی نظیر 440cd/m2 است.</p>



<p>کیفیت تصاویر در نمایشگر بسیار دیدنی است، اگر تجربه کار با نمایشگرهای محصولات اپل داشته باشید و یا با نمایشگر های آلترابوکهای سرفیس مایکروسافت، به خوبی کیفیت بی نظیر نمایشگر OLED این لپتاپ را متوجه می شوید.نمایشگر های OLED در موبایل های رده بالا نیز استفاده می شوند و از نظر تشابه تصویری، تجربه کار با این پنل بسیار نزدیک به کار با موبایل های پرچمدار برند های معروف سامسونگ و اپل است.</p>



<p>تصاویر بسیار زنده، شفاف، و رنگ ها کاملا طبیعی جلوه می نماید. دوستان عکاس وجود کوچکترین ضعف در تصاویر را می توانند به خوبی بر روی نمایشگر این لپتاپ شناسایی کنند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x895"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg5NSIgdmlld0JveD0iMCAwIDExNjAgODk1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""895"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_18-1160x895.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243457"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_18-1160x895.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_18-400x309.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_18-768x592.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_18-1536x1185.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_18.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>موضوع اما بدین مشخصات ختم نمی‌شود، پنل 3 میلیمتری لپتاپ AERO 15 OLED XD گیگابایت با قدرت 100% sRGB قابلیت HDR400 دارد.از نظر دقت نمایش رنگها، این نمایشگر کالیبره شده X-Rite از کارخانه است با مقدار Delta-E کمتر از 1. کالیبراسیون رنگ یکی از مهمترین ویژگی‌ها برای لپتاپ‌های مخصوص طراحی و دیزاین است.</p>



<p>اما به سراغ سیستم رنگ پنتون Pantone برویم که فضایی رنگی و اختصاصی است و در صنایع به‌ ویژه چاپ استفاده می‌شود. از این سیستم رنگی در تولید رنگ، پارچه و پلاستیک نیز استفاده می‌شود. سیستم تطبیق رنگ پنتون سیستمی‌برای تولید رنگ استاندارد است.نمایشگر لپتاپ AERO 15 OLED XD گیگابایت مجهز به پروفایل رنگی مورد تایید پنتون است تا تصاویری که نمایش می‌دهد مطابق سیستم رنگی پنتون باشد و کاربر مطمئن باشد رنگ انتخابی دقیقا همان‌ رنگی است که می‌خواهند پروژه را بر اساس آن تولید کنند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x662"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY2MiIgdmlld0JveD0iMCAwIDExNjAgNjYyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""662"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_19-1160x662.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243456"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_19-1160x662.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_19-400x228.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_19-768x438.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_19-1536x876.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_19.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>استفاده از پروفایل رنگی مناسب، هم از طریق نرم&nbsp; افزار گیگابایت و هم از طریق تنظیمات مخصوص نمایشگر در ویندوز 10 قابل انتخاب است.اگرچه این نمایشگر برای طراحی کاملا ایده آل است، اما نرخ به روز رسانی 60 هرتزی آن برای گیمر‌ها عادی است.البته با توجه به قدرت RTX 3070 در رزلوشن 4K نیازی به نرخ به روز رسانی بالاتر از 60 هرتز هم نیست.</p>



<h2 id = ""h--5"" >< strong > خنک کننده و باطری:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x388"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjM4OCIgdmlld0JveD0iMCAwIDExNjAgMzg4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""388"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_20-1160x388.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243455"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_20-1160x388.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_20-400x134.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_20-768x257.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_20-1536x514.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_20.jpg.webp 1648w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>گیگابایت برای کولینگ این لپتاپ از دو فن مخصوص با پروانه‌های 71 پره ای استفاده کرده است.تعداد 4 لوله انتقال حرارت در لپتاپ استفاده شده است که دو تا آن سراسری بین پردازنده و پردازشگر گرافیکی کشیده شده و دو تای دیگر به صورت اختصاصی یکی برای پردازنده و دیگری برای گرافیک مورد اسفاده قرار گرفته است.</p>



<p>من حیث المجموع، این سیستم کولینگ که گیگابایت آن را WINDFORCE نامگذاری کرده، به خوبی می‌تواند از پس خنک سازی تجهیزات پیشرفته این لپتاپ بر آید، اما به جهت آنکه این لپتاپ اصولا برای کار رندرینگ استفاده می‌شود، حتما توصیه می‌کنیم از کولپد مناسب برای هدایت جریان هوا به داخل این سیستم کولینگ استفاده کنید.</p>



<p>گیگابایت از یک باطری پر قدرت 99W/h برای این لپتاپ بهره برده است که شما با آن می‌توانید در صورتی که شدت نور نمایشگر را در حد متوسط نگاه دارید و استفاده عادی داشته باشید تا حدود 8 ساعت از لپتاپ بدون برق استفاده کنید.اما از آنجایی که نمایشگر OLED این لپتاپ، بهترین تصاویر را با حداکثر روشنایی خود نشان می‌دهد، میزان نگه داری باطری می‌تواند تا حدود 5 ساعت کاهش یابد.به هر شکل، اگر بازی می‌کنید و یا رندر میگیرید، بهتر است حتما لپتاپ را به آداپتور متصل نمایید.</p>



<h2 id = ""h--6"" >< strong > بخش صوتی:</strong></h2>



<p>صدای لپتاپ AERO پرقدرت اما فراگیر نیست.هرچند دامنه پخش صدای آن قابل قبول است و اگر شما فیلم و یا موسیقی با آن گوش دهید، راضی کننده است اما آنطوری نیست که شما را به وجد آورد.به هر حال برای بازی و استفاده عادی و معمول سیستم، صدای لپتاپ کاملا مناسب است.شرکت Gigabyte از چیپ صوتی Realtek ALC255 برای این لپتاپ بهره گرفته است.این چیپ صوتی به دو اسپیکر در دو طرف لپتاپ متصل است.</p>



<h2 id = ""h--7"" >< strong > قدرت پردازش:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x766"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc2NiIgdmlld0JveD0iMCAwIDExNjAgNzY2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""766"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_21-1160x766.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243454"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_21-1160x766.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_21-400x264.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_21-768x507.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_21-1536x1014.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_21.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>پردازنده Core i7 11800H مجهز به 8 هسته، و 16 رشته پردازشی است.این پردازنده دارای فرکانس پایه 2.3 گیگاهرتز و فرکانس بوست 4.6 گیگاهرتز است.همچنین مجهز به گرافیک مجتمع داخلی است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x472"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ3MiIgdmlld0JveD0iMCAwIDExNjAgNDcyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""472"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_22-1160x472.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243453"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_22-1160x472.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_22-400x163.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_22-768x312.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_22-1536x625.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_22.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>این پردازنده در میانه جدول قدرتمند ترین پردازنده‌های نسل 11 اینتل قرار دارد و می‌تواند به خوبی از پس امور جدی مانند رندرینگ بر آید.ناگفته نماند، از نظر گیمینگ نیز، با توجه به قدرت فوق العاده تک هسته‌های آن، درجه یک است.در قسمت حافظه موقت، این لپتاپ مجهز به دو کیت 16 گیگابایتی از نوع DDR4(مجموع 32 گیگابایت SDRAM دو کاناله) در فرکانس 2666MHz با تایمینگ CL19 می‌باشد.خوشبختانه این میزان رم برای انجام گیمینگ و تقریبا کار با اکثر نرم افزار‌های طراحی کافی است و می‌توان به راحتی از آن برای اغلب رندر‌های نیمه حرفه ای و حرفه ای استفاده کرد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x474"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ3NCIgdmlld0JveD0iMCAwIDExNjAgNDc0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""474"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_23-1160x474.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243452"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_23-1160x474.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_23-400x164.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_23-768x314.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_23-1536x628.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_23.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در بنچمارک AIDA64 میزان پهنای باند در دسترس و زمان تاخیر رم نمایان است.ترکیب این رم و پردازنده، قدرت پردازشی مناسبی را در اختیار کاربر قرار می‌دهد که می‌تواند از پس تمام بازی‌ها و همچنین رندرینگ‌های سنگین و انواع Multi-Tasking‌ها برآید.</p>



<h2 id = ""h--8"" >< strong > بخش گرافیک:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x561"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU2MSIgdmlld0JveD0iMCAwIDExNjAgNTYxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""561"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-1160x561.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243451"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-1160x561.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-400x194.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-768x372.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-1536x743.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-1240x600.jpg.webp 1240w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24-1920x930.jpg.webp 1920w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_24.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>مشخصات گرافیک مجتمع Intel HD Graphics UHD و همچنین گرافیک قدرتمند RTX3070 MAX-Q به کار گرفته شده در این لپتاپ را مشاهده می‌کنید.فرکانس GPU گرافیک RT3070 به کار رفته در این لپتاپ، در حالت Boost به 1290 مگاهرتز می‌رسد.سرعت حافظه گرافیک نیز به 12Gbps می‌رسد. پشتیبانی از Resizable Bar از خصوصیات خوب این گرافیک است. این کارت از فن آوری MAX-Q انویدیا استفاده میکند که ضمن امکانپذیر ساختن به کار گیری این گرافیک در ابعاد کوچکتر و نازکتر، از نظر دمایی نیز بتواند در حداکثر سازگاری با کولرهای لپتاپ، خنک شود.</p>



<h2 id = ""h--9"" > بررسی < strong > دمای کاری:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x679"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY3OSIgdmlld0JveD0iMCAwIDExNjAgNjc5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""679"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_25-1160x679.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243450"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_25-1160x679.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_25-400x234.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_25-768x450.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_25-1536x900.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_25.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>کمی‌جلوتر در بخش نرم افزار، به وجود چهار پروفایل مختلف کاری در لپتاپ AERO 15 OLED XD گیگابایت خواهیم پرداخت.این لپتاپ، در قویترین پروفایل کاری خود، حالت Turbo، فرکانس پردازنده را تا حداکثر 4.6 گیگاهرتر قفل می‌کند.همچنین با اورکلاک پردازشگر گرافیکی، فرکانس آن را به بیش از 1300 مگاهرتز افزایش می‌دهد.بر روی این پروفایل سیستم برای رندر آماده می‌شود و هر دو فن لپتاپ بر روی حداکثر دور خود قرار می‌گیرند.در این حالت نسبت به تست رندر اقدام کردیم و نتایج فوق بدست آمد.در بخش گرافیک خوشبختانه دما در حد نصاب تحت کنترل باقی ماند.</p>



<p>پردازنده اما به حداکثر دمای مجاز خود که 90 درجه باشد رسید.دو هسته برای چند ثانیه کمی ‌دچار Throttle شدند که برای پردازنده بسیار داغ 11800H قابل انتظار بود. در کل با توجه به سخت افزار به کار رفته، نمره کولینگ لپتاپ 9 از 10 است، و از این نظر، برای عملکرد خوب کولر کلاه از سر برمیداریم. خوشبختانه در پروفایل‌های گیمینگ و حالت خلاقیت Creator Mode، دما و صدای فنها تحت کنترل و مدیریت بهتری قرار دارند.اما به هر جهت، از آنجایی که مهمترین عملکرد این لپتاپ، رندر است، انتظار زیادی از کولینگ آن می‌رود که خوشبختانه تا حد زیادی، این انتظار را پاسخ داد.</p>



<h2 id = ""h--10"" >< strong > درایو ذخیره سازی:</strong></h2>



<p>در بخش ذخیره سازی لپتاپ AERO 15 OLED XD گیگابایت شاهد حضور یک اس اس دی OEM از نوع PCIe Gen.4 هستیم.این درایو با پارت نامبر ESR01TBTLG-E6GBTNB4 به نظر از نسل اول SSD‌های متداول PCIe Gen.4 x4 است که مجهز به کنترلر E-16 ساخت Phison بودند و حداکثر سرعت خواندن و نوشتن متوالی آنها به ترتیب به 5000MB/s و 2500MB/s می‌رسید. به هر صورت، با یک حافظه پر سرعت طرف هستیم که بالانس خوبی میان دوام و سرعت دارد و امتحان خود را در تمام برند‌هایی که از این نوع کنترلر استفاده کرده اند، پس داده است.وجود SSD در لپتاپ از نان شب هم واجب تر است.چرا که عملا کامپیوتر بدون SSD نمی‌تواند از تمام قدرت Multi-Tasking خود استفاده کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x776"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3NiIgdmlld0JveD0iMCAwIDExNjAgNzc2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""776"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_26-1160x776.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243449"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_26-1160x776.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_26-400x268.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_26-768x514.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_26-1536x1028.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_26.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>تست سرعت این اس اس دی OEM را در بنچمارک Anvil مشاهده می‌کنید.بدک نیست و انصافا سرعت خوبی دارد.برای دوستانی که به صورت جدی از این لپتاپ استفاده گیمینگ و یا رندر دارند، استفاده از همین حافظه پیشنهاد می‌شود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x639"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYzOSIgdmlld0JveD0iMCAwIDExNjAgNjM5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""639"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_27-1160x639.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243448"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_27-1160x639.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_27-400x220.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_27-768x423.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_27-1536x846.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_27.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در حالیکه تست بنچمارک کریستال را از اس اس دی مشاهده می‌کنید، لازم به ذکر است این اس اس دی تنها آپشن موجود در این لپتاپ نیست.بلکه یک درگاه M.2 اضافی دیگر در کنار این حافظه قرار دارد.البته آن درگاه از نوع PCIe Gen.3 x4 است، اما به هر صورت وجودش بسیار لازم و حیاتی است. رندر کاران و تولید کنندگان محتوی می‌توانند از حافظه اصلی برای کار رندر و با خرید یک SSD از نوع PCIe Gen.3 و قرار دادن آن در جایگاه دوم M.2، از آن برای آرشیو استفاده کنند.</p>



<h2 id = ""h--11"" >< strong > شبکه:</strong></h2>



<p>در بخش شبکه، گیگابایت با استفاده از کارت شبکه Wi-Fi 6 AX200 ساخت اینتل در کنار بلوتوث نسخه 5.1، سنگ تمام گذاشته است. این مجموعه تمام آن چیزی است که شما امروزه در بخش اتصالات بی سیم به آن نیاز دارید. خوشبختانه این دستگاه همچنین دارای یک درگاه LAN 2.5 گیگابیتی منشعب از کنترلر Realtek است.</p>



<h2 id = ""h--12"" >< strong > تست‌های بازی:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1614x1040"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjE0IiBoZWlnaHQ9IjEwNDAiIHZpZXdCb3g9IjAgMCAxNjE0IDEwNDAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1614"" height=""1040"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_28.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243447"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_28.png.webp 1614w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_28-400x258.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_28-1160x747.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_28-768x495.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_28-1536x990.png.webp 1536w"" data-sizes=""(max-width: 1614px) 100vw, 1614px""></figure></div>



<p>اگرچه لپتاپ AERO 15 OLED XD گیگابایت با هدف جذب طراحان و تولید کنندگان محتوی ساخته شده است، اما سخت افزار قدرتمند آن توانایی بسیار مطلوبی برای گیمینگ دارد.بنابراین قدرت این لپتاپ در گیمینگ را در تست‌ها می‌سنجیم.از آنجایی که این اولین لپتاپ با دقت 4K است که بررسی می‌کنیم، ابتدا تست‌های سابق را بر اساس دقت FHD گرفته و ثبت کردیم. نتایج فوق العاده بود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1705x1104"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzA1IiBoZWlnaHQ9IjExMDQiIHZpZXdCb3g9IjAgMCAxNzA1IDExMDQiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1705"" height=""1104"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_29.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243446"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_29.png.webp 1705w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_29-400x259.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_29-1160x751.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_29-768x497.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_29-1536x995.png.webp 1536w"" data-sizes=""(max-width: 1705px) 100vw, 1705px""></figure></div>



<p>حال مجدد همان بازی‌ها را با حداکثر جزییات در دقت 4K تست کردیم.در مجموع قابل قبول بود و می‌توان گفت، اغلب بازی‌ها به لطف کارت گرافیک RTX 3070 می‌توانند با جزییات متوسط 50 فریم به بالا در 4K اجرا شوند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1707x1745"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzA3IiBoZWlnaHQ9IjE3NDUiIHZpZXdCb3g9IjAgMCAxNzA3IDE3NDUiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1707"" height=""1745"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_30.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243445"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_30.png.webp 1707w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_30-391x400.png.webp 391w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_30-1002x1024.png.webp 1002w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_30-768x785.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_30-1503x1536.png.webp 1503w"" data-sizes=""(max-width: 1707px) 100vw, 1707px""></figure></div>



<p>به سراغ 3D Mark و تست Time Spy می‌رویم.این بنچمارک یک تست گیمینگ محسوب می‌شود و امتیاز گرافیک (ستون زرد رنگ) قدرت گیمینگ لپتاپ را نشان می‌دهد.تقریبا لپتاپ AERO 15 OLED XD گیگابایت با فاصله بسیار کمی، در صدر جدول ایستاد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1723x1725"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzIzIiBoZWlnaHQ9IjE3MjUiIHZpZXdCb3g9IjAgMCAxNzIzIDE3MjUiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1723"" height=""1725"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243444"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31.png.webp 1723w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31-400x400.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31-1023x1024.png.webp 1023w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31-150x150.png.webp 150w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31-768x769.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_31-1534x1536.png.webp 1534w"" data-sizes=""(max-width: 1723px) 100vw, 1723px""></figure></div>



<p>بنچمارک Unigine Superposition با استفاده از موتور Unigine 2.0 به خلق تصاویری بدیع از یک اتاق کار و حرکت دوربین در آن می‌پردازد.این نرم افزار بسیار سنگین، قدرت سیستم شما را برای اجرای نرم افزار‌های شبیه سازی سه بعدی، Post-Processing، گیمینگ، و حرکت دوربین در میان یک ساختمان همزمان با اجرای الگوریتم‌های پیچیده رهگیری نور SSRTGI تحت فشار قرار می‌دهد و میزان توانایی آن را با امتیاز بیان می‌کند. دوستانی که به دنبال تهیه یک سیستم مناسب گیم و همچنین نرم افزار‌های شبیه سازی سه بعدی، واقعیت مجازی VR، انیمیشن، و صد البته گیمر‌ها، توجه ویژه ای به نتایج این بنچ مارک داشته باشند. صدر جدول از آن لپتاپ AERO 15 OLED XD گیگابایت شد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1688x1440"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjg4IiBoZWlnaHQ9IjE0NDAiIHZpZXdCb3g9IjAgMCAxNjg4IDE0NDAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1688"" height=""1440"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_32.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243443"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_32.png.webp 1688w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_32-400x341.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_32-1160x990.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_32-768x655.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_32-1536x1310.png.webp 1536w"" data-sizes=""(max-width: 1688px) 100vw, 1688px""></figure></div>



<p>یکی از بهترین تست‌های بازی، بنچ مارک Final Fantasy است که قدرت سیستم را تحت یک امتیاز مشخص ارائه می‌کند.قدرت پردازنده، رم، کارت گرافیک و اس اس دی، در نتیجه این نرم افزار تاثیر مستقیم دارد.<strong></strong></p>



<h2 id = ""h--13"" >< strong > تست‌های رندر:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1644x1372"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjQ0IiBoZWlnaHQ9IjEzNzIiIHZpZXdCb3g9IjAgMCAxNjQ0IDEzNzIiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1644"" height=""1372"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_33.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243442"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_33.png.webp 1644w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_33-400x334.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_33-1160x968.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_33-768x641.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_33-1536x1282.png.webp 1536w"" data-sizes=""(max-width: 1644px) 100vw, 1644px""></figure></div>



<p>نرم افزار سینی‌بنچ(Cinebench) از معروفترین نرم افزار‌های سنجش قدرت پردازش سرعت CPU و کارت گرافیک است که یک صحنه سه بعدی را رندر می‌کند و پردازنده و کارت گرافیک شما را به اندازه معقولی تحت فشار قرار می‌دهد.هر چه سیستم شما بتواند صحنه مورد نظر را با سرعت بیشتری رندر کند، برنامه امتیاز بالاتری به آن می‌دهد.ما از سینی‌بنچ به صورت متمرکز برای سنجش قدرت پردازنده هم به صورت چند رشته پردازشی و هم به صورت تک رشته پردازشی استفاده می‌کنیم. مجددا لپتاپ AERO 15 OLED XD گیگابایت در صدر جدول ما قرار گرفت.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1722x1722"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzIyIiBoZWlnaHQ9IjE3MjIiIHZpZXdCb3g9IjAgMCAxNzIyIDE3MjIiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1722"" height=""1722"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243441"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34.png.webp 1722w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34-400x400.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34-1024x1024.png.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34-150x150.png.webp 150w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34-768x768.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_34-1536x1536.png.webp 1536w"" data-sizes=""(max-width: 1722px) 100vw, 1722px""></figure></div>



<p>موتور رندر Corona یکی از معروفترین موتور‌های رندر تصاویر واقعی در میان نرم افزار‌های رندر است که می‌تواند هم به صورت یک نرم افزار مجزا و هم به عنوان یک Plugin در برنامه‌های Autodesk 3ds Max و MAXON Cinema 4D مورد استفاده قرار گیرد.نرم افزار بنچمارک Corona Benchmark از موتور نسخه 1.3 کرونا رندر استفاده می‌کند و می‌تواند قدرت چند رشته پردازشی پردازنده را (تا حداکثر 72 رشته پردازشی) با رندر کردن تصویری واحد بسنجد.این نرم افزار بسیار سنگین است و هرچه پردازنده هسته‌های سریعتر و رشته‌های پردازشی بیشتری داشته باشد، سرعت رندر گیری در این نرم افزار بیشتر می‌شود و در نتیجه مدت زمان انجام پروژه کوتاه تر خواهد بود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1722x1230"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzIyIiBoZWlnaHQ9IjEyMzAiIHZpZXdCb3g9IjAgMCAxNzIyIDEyMzAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1722"" height=""1230"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_35.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243440"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_35.png.webp 1722w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_35-400x286.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_35-1160x829.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_35-768x549.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_35-1536x1097.png.webp 1536w"" data-sizes=""(max-width: 1722px) 100vw, 1722px""></figure></div>



<p>تست V-RAY از جمله نرم افزار‌های سنجش قدرت رندر پردازنده است که هر چه مدت زمان به پایان رساندن پروژه کوتاه تر باشد، قدرت پردازنده بیشتر است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1600x357"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjAwIiBoZWlnaHQ9IjM1NyIgdmlld0JveD0iMCAwIDE2MDAgMzU3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1600"" height=""357"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_36.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243439"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_36.jpg.webp 1600w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_36-400x89.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_36-1160x259.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_36-768x171.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_36-1536x343.jpg.webp 1536w"" data-sizes=""(max-width: 1600px) 100vw, 1600px""></figure></div>



<p>بنچ مارک رندر V-Ray Next جدیدترین نرم افزار تست قدرت رندر کامپیوتر V-RAY است.این بنچمارک یک تصویر را به صورت زنده برای شما رندر کرده و امتیازی را به پردازنده و کارت گرافیک می‌دهد. از این جهت می‌توان از این نرم افزار برای تست قدرت رندر پردازنده و کارت گرافیک استفاده کرد که ما از آن برای تست هر دو بهره گرفته ایم و امتیازهای کارت گرافیک در سمت راست و امتیاز پردازنده در سمت چپ نمایان است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1652x696"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjUyIiBoZWlnaHQ9IjY5NiIgdmlld0JveD0iMCAwIDE2NTIgNjk2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1652"" height=""696"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_37.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243438"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_37.png.webp 1652w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_37-400x169.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_37-1160x489.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_37-768x324.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_37-1536x647.png.webp 1536w"" data-sizes=""(max-width: 1652px) 100vw, 1652px""></figure></div>



<p>یکی از بهترین نرم افزار‌های تست مبدل‌های ویدیویی، نرم افزار HEVC Decode Benchmark(Cobra) است که در سایت ثبت رکورد HWBOT مخصوص اورکلاکر‌ها یافت می‌شود.این نرم افزار با استفاده از کدک H.265 به تبدیل سه نوع ویدیو &nbsp;HD، FHD، و 4K می‌پردازد و سپس به هر بخش امتیاز داده و یک امتیاز کلی را هم برای سیستم در نظر می‌گیرد.کاربران عزیزی که در سیستم‌های خود تبدیل‌های ویدیویی دارند به نتایج این بنچ مارک توجه ویژه ای داشته باشند. لازم به ذکر است عملیات تبدیل ویدویی در این برنامه تماما بر روی پردازنده انجام می‌شود و از این لحاظ این نرم افزار تست پردازنده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1703x2203"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzAzIiBoZWlnaHQ9IjIyMDMiIHZpZXdCb3g9IjAgMCAxNzAzIDIyMDMiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1703"" height=""2203"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243437"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38.png.webp 1703w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38-309x400.png.webp 309w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38-792x1024.png.webp 792w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38-768x993.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38-1187x1536.png.webp 1187w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_38-1583x2048.png.webp 1583w"" data-sizes=""(max-width: 1703px) 100vw, 1703px""></figure></div>



<p>نرم افزار GEEKBENCH یکی از بهترین تست‌ها برای پردازنده و کارت گرافیک است.لپتاپ AERO 15 OLED XD گیگابایت در این تست، صدر جدول را در میان دیگر لپتاپ‌های قدرتمند بررسی شده در لابراتوار سخت افزار از آن خود کرد.این نرم افزار با تمرکز بر روی قدرت CPU و رم سیستم، نسبت به سنجش قدرت چند رشته و تک رشته پردازشی اقدام می‌کند. در بخش پردازنده این نرم افزار با بررسی قدرت سیستم شما در زمینه ادیت تصاویر، انجام عملیات روزانه پردازشی و Augmented Reality و Machine Learning امتیازی را به پردازنده شما می‌دهد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1701x1267"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzAxIiBoZWlnaHQ9IjEyNjciIHZpZXdCb3g9IjAgMCAxNzAxIDEyNjciPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1701"" height=""1267"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_39.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243436"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_39.png.webp 1701w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_39-400x298.png.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_39-1160x864.png.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_39-768x572.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_39-1536x1144.png.webp 1536w"" data-sizes=""(max-width: 1701px) 100vw, 1701px""></figure></div>



<p>در بخش سنجش کارت گرافیک یا&nbsp; Compute Benchmark، این نرم افزار می‌تواند با انجام تست‌های متمرکز بر OpenCL و CUDA نسبت به امتیاز دهی به بخش گرافیکی سیستم شما اقدام می‌کند.امتیاز Compute Benchmark بیانگر قدرت کارت گرافیک در نرم افزار‌های تدوین ویدیو، تدوین عکس، و همچنین گیمینگ، بر اساس قدرت هسته‌های Cuda و OpenCL است.</p>



<h2 id = ""h--14"" >< strong > تست‌های عمومی:</strong><strong></strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1716x2180"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNzE2IiBoZWlnaHQ9IjIxODAiIHZpZXdCb3g9IjAgMCAxNzE2IDIxODAiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1716"" height=""2180"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40.png.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243435"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40.png.webp 1716w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40-315x400.png.webp 315w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40-806x1024.png.webp 806w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40-768x976.png.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40-1209x1536.png.webp 1209w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_40-1612x2048.png.webp 1612w"" data-sizes=""(max-width: 1716px) 100vw, 1716px""></figure></div>



<p>و در آخر به سراغ تست PCMARK 10 می‌رویم که مهمترین بنچمارک برای تولید کنندگان محتوی به شمار می‌رود.این تست شامل طیف متنوعی از تست‌های کاری روزمره به همراه تست‌های سنگین کاربردی رندرینگ است.این برنامه امتیازات مشخصی را به هر بخش از یک سیستم اعطا می‌کند. همانطور که در تصویر مشخص است، نتیجه ی تست Essentials، شامل تست سرعت اجرای برنامه‌ها، ویدیو کنفرانس، و گشت و گذار در اینترنت نتیجه فوق العاده ای دارد همچنین در بحث Productivity، شامل تست‌های اداری، از جمله صفحات محاسبانی، نوشتار و عملکرد Office نتیجه عالی است.در بخش تولید محتوی دیجیتال هم این نرم افزار امتیاز خوبی را به این لپتاپ اعطا کرده است. پکیج نرم افزاری PC MARK 10 یک تست ترکیبی اداری و رندرینگ است، و تقریبا یک لپتاپ را از همه‌ی جهات می‌سنجد.لپتاپ AERO 15 OLED XD گیگابایت در صدر این جدول ایستاد.</p>



<h2 id = ""h--15"" >< strong > بخش نرم افزاری:</strong></h2>



<p>گیگابایت برای مدیریت نرم افزار لپتاپ AERO 15 OLED XD از یک پکیج نرم افزاری تحت عنوان Control Center استفاده کرده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x666"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY2NiIgdmlld0JveD0iMCAwIDExNjAgNjY2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""666"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_41-1160x666.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243434"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_41-1160x666.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_41-400x230.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_41-768x441.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_41-1536x882.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_41.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در صفحه اصلی و داشبورد این نرم افزار، میزان لود پردازنده، کارت گرافیک و حجم اشغال شده از رم و درایو ذخیره سازی نمایش داده می‌شود.سلامت اس اس دی و باطری، سرعت فنها و دیگر مشخصات اصلی نیز نمایش داده می‌شود تا کاربر با یک نگاه به این صفحه، تمامی‌اطلاعات مهم در مورد سیستم را بدست آورد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x257"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjI1NyIgdmlld0JveD0iMCAwIDExNjAgMjU3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""257"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_42-1160x257.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243433"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_42-1160x257.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_42-400x89.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_42-768x170.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_42-1536x340.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_42.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>بخش مهم دیگر نرم افزار Control Center، هوش مصنوعی آن است که تحت عنوان AI شامل چهار پروفایل برای تنظیم اتوماتیک دستگاه مبتنی بر نیاز کاربر می‌شود.حالت‌های Creator Mode برای بهینه سازی برای تولید محتوی، حالت Turbo Mode برای استفاده به هنگام رندر، حالت Gaming Mode برای استفاده گیمینگ، و Maximum Battery برای استفاده در زمانی است که دسترسی به برق برای شارژ دستگاه امکان پذیر نیست.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x668"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY2OCIgdmlld0JveD0iMCAwIDExNjAgNjY4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""668"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_43-1160x668.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243432"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_43-1160x668.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_43-400x230.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_43-768x442.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_43-1536x884.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_43.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در بخش Manager کاربر می‌تواند به طور مستقیم به تنظیمات اصلی، مانند Wi-Fi، شدت نور نمایش گر، مدیریت شارژ باطری و موارد دیگر دسترسی داشته باشد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x754"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc1NCIgdmlld0JveD0iMCAwIDExNjAgNzU0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""754"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_44-1160x754.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243431"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_44-1160x754.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_44-400x260.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_44-768x499.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_44-1536x998.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_44.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در صفحه Fusion، شما می‌توانید به تنظیم کلید‌های کیبورد، ثبت و ضبط حالات Macro، و مدیریت نورپردازی آن بپردازید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x647"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY0NyIgdmlld0JveD0iMCAwIDExNjAgNjQ3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""647"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_45-1160x647.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243430"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_45-1160x647.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_45-400x223.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_45-768x429.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_45-1536x857.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_45.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در بخش Device Setting می‌توانید، نسبت به اعمال تنظیم خودتان برای مدیریت فن‌ها و شدت گردش آنها مبتنی بر دمای پردازنده اقدام کنید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x674"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY3NCIgdmlld0JveD0iMCAwIDExNjAgNjc0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""674"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_46-1160x674.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243429"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_46-1160x674.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_46-400x233.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_46-768x446.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_46-1536x893.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_46.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>بخش آخر شامل مشخصات درایور‌ها و امکان به روز رسانی اتوماتیک آنها می‌شود.همچنین دفترچه لپتاپ در این بخش در دسترس است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x668"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjY2OCIgdmlld0JveD0iMCAwIDExNjAgNjY4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""668"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_47-1160x668.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243428"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_47-1160x668.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_47-400x231.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_47-768x443.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_47-1536x885.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_47.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>یکی دیگر از امکانات این لپتاپ، نرم افزار پشتیبان گیری از اطلاعات بر روی USB است.به لطف این نرم افزار می‌توانید نسبت به گرفتن Back up از اطلاعات خود اقدام کنید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x740"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc0MCIgdmlld0JveD0iMCAwIDExNjAgNzQwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""740"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_48-1160x740.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243427"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_48-1160x740.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_48-400x255.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_48-768x490.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_48-1536x979.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_48.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>نرم افزار مدیریت پهنای باند شبکه Dragon نیز، در این بخش قرار دارد و بر اساس نیاز خود، بخصوص به هنگام گیمینگ می‌توانید پهنای باند شبکه لپتاپ را منحصرا به بازی‌های خود اختصاص دهید.</p>



<h2 id = ""h--16"" >< strong > نتیجه گیری:</strong></h2>



<p>لپتاپ GIGABYTE AERO 15 OLED XD یک محصول فوق العاده جذاب برای پاور یوزر‌ها، تولید کنندگان محتوی و کاربران حرفه ای است. این لپتاپ چند بخش برجسته و مهم دارد که هر کدام به تنهایی می‌توانند یک Selling Point قوی برای یک لپتاپ باشد. اولین مورد استفاده از پردازنده قدرتمند 11800H است که در رندرینگ و گیمینگ فوق العاده عمل می‌کند.دوم، کارت گرافیک RTX 3070 Max-Q است که عملکردی بسیار عالی در گیم و رندر مبتنی بر GPU دارد. سومین بخش مهم و جذاب این دستگاه بدون شک نمایشگر فوق العاده آن است که با دقت 4K، کالیبراسیون رنگ حرفه ای دارد.پروفایل رنگی سیستم پنتون به همراه HDR400 و اینکه پنل OLED ساخت سامسونگ است، همه از جذابیت‌های بی نظیر این نمایشگر به حساب می‌آیند.</p>



<p>دو بخش دیگر مهم این لپتاپ، وجود حجم رم 32 گیگابایت به صورت پیش فرض در پیکربندی دو کاناله است؛ همچنین استفاده از 1 ترابایت اس اس دی از نوع PCIe Gen.4 x4 NVMe. هر کدام از این موارد در هر لپتاپی باشد، یک کاربر حرفه ای قطعا به راحتی از کنار آن نمی‌گذرد.حال لپتاپ AERO 15 OLED XD گیگابایت مجهز به تمامی این موارد است و نمی‌توان آن را نخرید.</p>



<p>علاوه بر تجهیزات پر&nbsp; قدرت سخت افزاری، گیگابایت از یک بدنه تمام آلومینیومی‌ سبک برای این دستگاه استفاده کرده است.کولینگ مناسب دو فنه، در کنار استفاده از یک باطری قدرتمند که با شدت روشنایی متوسط نمایشگر می‌تواند هفت هشت ساعت نیروی مورد نیاز لپتاپ را تامین کند، همه از دیگر موارد مثبت این دستگاه به شمار می‌رود.</p>



<p>از نظر انتقادی، تنها موردی که با سختگیری فراوان می‌توان به آن اشاره کرد، جایگاه وبکم است که زاویه جالبی نسبت به صورت ندارد. همچنین هوش مصنوعی دستگاه، که الحق و الانصاف عملکرد بی نظیری دارد، اما هر از گاهی تنظیم نور نمایشگر را کاهش می‌دهد. این حالت Auto-Dim در اکثر نمایشگر‌های OLED وجود دارد، و در این لپتاپ هم به همان شکل عمل می‌کند. نمایشگر‌های OLED بهترین تصویر خود با شدت نور صد در صد ارائه می‌کنند و خوب طبیعی است کاربر هنگام استفاده از نمایشگر، آن را بر روی حداکثر شدت نور تنظیم کند. اما این حالت ممکن است در تداخل با پروفایل‌های از پیش تعیین شده هوش مصنوعی باشد. از این رو گاهی هوش مصنوعی علی رغم ست کردن نور به صورت دستی، وارد عمل شده و شدت نور نمایشگر را کم می‌کند که این مورد کمی ‌آزار دهنده بود. امیدواریم در آینده با ارائه آپدیت، هوش مصنوعی در زمانی که کاربر خودش نسبت به تنظیم دستی شدت نور تصویر اقدام می‌کند، در این بخش دخالت نکند.</p>



<p>از نظر اتصالات، خوشبختانه شاهد تنوع خوبی هستیم و نکته مهم این است که گیگابایت در هر دو طرف دستگاه، از پورت‌های USB استفاده کرده است. وجود درگاه Thunderbolt 4 Type C نیز از امکانات جالب توجه این لپتاپ به شمار می‌رود. شما همچنین می‌توانید به درگاه HDMI 2.1 این لپتاپ، یک مانیتور 4K با نرخ به روز رسانی 120 هرتز متصل کنید که عالی است. کیبورد لپتاپ تمام RGB است و به مانند یک کیبورد گیمینگ می‌توانید حالات Micro را ذخیره سازی کنید و یا الگو‌های نورپردازی مورد دلخواه خود را به آن بدهید.</p>



<p>یکی از خصوصیات لپتاپ های مخصوص تولید محتوی، بحث عکاسی است. شیار مخصوص خواندن کارتهای حافظه SD برای دوربین، یک بخش مهم در این زمینه است. گیگابایت برای این لپتاپ، درگاه با کلاس UHS-II در نظر گرفته است که قدرت خواندن و نوشتن آن به 300MB/s می رسد. شما حتی می توانید با تهیه یک حافظه SD با حجم بالا، از این بخش به عنوان یک حافظه اکسترنال نیز استفاده کنید.</p>



<p>از نظر کارایی، این لپتاپ تقریبا در تمامی‌تست‌های ما در صدر جدول ایستاد. تاکنون هیچ لپتاپی در گیم و در رندر به قدرت لپتاپ AERO 15 OLED XD گیگابایت در لابراتوار نداشتیم و از این نظر، نشان کارایی سخت افزار را به آن اهدا می‌کنیم. همچنین، با توجه به رضایت کلی ما از مجموع امکانات، اتصالات، ویژگی‌ها و خصوصیات سخت افزاری خوب این دستگاه، نشان منتخب سردبیر را نیز به آن اعطا می‌نماییم.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large is-resized""><img data-lazyloaded=""1"" data-placeholder-resp=""510x341"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI1MTAiIGhlaWdodD0iMzQxIiB2aWV3Qm94PSIwIDAgNTEwIDM0MSI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_49.jpg.webp"" alt=""بررسی لپتاپ مخصوص طراحی گیگابایت مدل GIGABYTE AERO 15 OLED XD"" class=""wp-image-243426"" width=""510"" height=""341"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_49.jpg.webp 680w, https://sakhtafzarmag.com/wp-content/uploads/2021/10/GIGABYTE-AERO-15-OLED-XD_49-400x267.jpg.webp 400w"" data-sizes=""(max-width: 510px) 100vw, 510px""></figure></div>



<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span>



<ul><li>ضمانت آواژنگ</li><li>طراحی زیبا و جدی</li><li>پرفورمنس فوق العاده بالا</li><li>مجهز به سنسور اثر انگشت</li><li>مجهز به سیستم کولینگ دو فنه</li><li>مجهز به اتصال Thunderbolt 4</li><li>ساخت بدنه آلومینیومی‌سبک و محکم</li><li>مجهز به کارت شبکه LAN 2.5 گیگابیتی</li><li>مجهز به سیستم عامل ویندوز 10 اورجینال</li><li>یک لپتاپ ایده آل مخصوص طراحی و رندر</li><li>دارای نمایشگر OLED 4K ساخت سامسونگ</li><li>در کلاس لپتاپ‌های High End وزن مناسبی دارد</li><li>مجهز به شبکه بی سیم Wi-Fi 6 و Bluetooth 5.2</li><li>مجهز به پردازنده قدرتمند اینتل Core i7-11800H</li><li>مجهز به کارت گرافیک مستقل RTX3070 Max-Q</li><li>مجهز به یک ترابایت SSD از نوع PCIe Gen.4 x4</li><li>وجود کارتخوان کلاس UHS-II با سرعت 300MB/s</li><li>مجهز به 32 گیگابایت رم 2666 مگاهرتز به صورت دو کاناله</li><li>باطری قدرتمند با دوام بالای 8 ساعت استفاده عادی از سیستم</li><li>گیگابایت پکیج نرم افزاری خوبی به همراه دستگاه ارائه کرده است</li><li>مجهز به یک درگاه اضافی SSD M.2 از نوع PCIe Gen.3 x4 NVMe</li><li>مجهز به نمایشگر کالیبره شده رنگی کارخانه ای به همراه پروفایل رنگی Pantone</li><li>کیبورد مجهز به نور پس زمینه تمام RGB با قابلیت شخصی سازی و مدیریت نورپردازی</li><li>مجهز به اتصال HDMI 2.1 با توانمندی پخش بر روی نمایشگر 4K@120Hz اکسترنال</li></ul>



</div>



<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span>



<ul><li>وسیله همراه اضافی خاصی ندارد</li><li>صدای فن در لود‌های بالا شنیده می‌شود</li><li>انتظار بیشتری از کیفیت پخش صدای اسپیکر‌های لپتاپ داشتیم</li><li>خرید کول پد برای خنک نگه داشتن لپتاپ به هنگام رندر توصیه می‌شود</li><li>کیفیت وبکم HD است و زاویه آن نسبت به صورت کاربر چندان جالب نیست</li><li>مکانیسم دخالت هوش مصنوعی در فعال سازی Auto-Dim نمایشگر گاهی کاربر را اذیت می‌کند</li></ul>



</div>



<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span>



<ul><li>اگر طراح و یا تولید کننده محتوی هستید، لپتاپ AERO 15 OLED XD گیگابایت بهترین انتخاب پیش روی شماست.</li></ul>



</div>



<span data-sr=""enter"" class=""gk-review clearfix""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.94""><span>9.4</span></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.98""><span>9.8</span></span><span class=""gk-review-partial-label"">پرفورمنس</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.99""><span>9.9</span></span><span class=""gk-review-partial-label"">صفحه نمایش</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.9""><span>9</span></span><span class=""gk-review-partial-label"">کولینگ لپتاپ</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.94""><span>9.4</span></span><span class=""gk-review-partial-label"">طراحی ظاهری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.95""><span>9.5</span></span><span class=""gk-review-partial-label"">باطری</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.94""><span>9.4</span></span><span class=""gk-review-partial-label"">صفحه کلید و تاچ پد</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.89""><span>8.9</span></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 23,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "ASUS,دسک تاپ",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_00-1920x930.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "آشنایی با پردازنده‌های Alder Lake و چیپست Z690",
                    Title = "بررسی اجمالی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 16,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

< p > از امروز کلیه مادربرد‌های Z690 ایسوس در بازار ایران عرضه خواهند شد.البته برای تهیه پردازنده‌ها و رمهای DDR5 کمی‌ به فرصت بیشتری نیاز هست تا آنها هم کم کم وارد بازار شوند.به یمن ورود نسل جدید پردازنده‌های Alder Lake، چند هفته پیش ویندوز 11 رونمایی شد، رمهای DDR5 وارد بازار می‌شوند و ظرف چند ماه آینده اولین کارت گرافیک PCIe Gen.5 جهان هم وارد بازار خواهد شد.بدین ترتیب شاهد یک نقطه عطف در تاریخ صنعت کامپیوتر هستیم که به واسطه نوآوری شرکت اینتل، تعداد زیادی از قطعات جدید را همزمان تجربه خواهیم کرد.نسل جدید مادربرد‌های چیپست Z690 حرفهای زیادی برای گفتن دارند.این مادربرد‌ها نسبت به نسل گذشته خود، چند پله ارتقا یافته اند.سوکت جدید LGA1700 با خود پشتیبانی از PCIe Gen.5 را در کنار رمهای DDR5 به همراه آورده است.شرکت ایسوس بزرگترین تولید کنندگان مادربرد در دنیا با عرضه ده‌ها مدل مادربرد Z690، محصولات متنوعی را برای استفاده کاربران از جدیدترین فن آوری اینتل ارائه کرده است.امروز برای اولین بار در ایران، به بررسی اجمالی مادربرد ایسوس ASUS ROG STRIX Z690 - F GAMING WIFI می‌پردازیم که از طرف نماینده رسمی‌‌ ایسوس در ایران در اختیار ما قرار گرفته است.بدیهی است بررسی کامل این مادربرد را در ادامه این مقاله در آینده، تقدیم شما دوستان خواهیم کرد.اما ابتدا، کمی‌‌ در مورد تغییرات بزرگ این پلتفرم جدید و پردازنده‌های آن صحبت خواهیم کرد و سپس به آنباکس و بررسی مادربرد می‌پردازیم.</ p >



< h2 id = ""h-alder-lake-z690"" >< strong > آشنایی با پردازنده‌های </ strong >< strong > Alder Lake </ strong >< strong > و چیپست </ strong >< strong > Z690 </ strong >< strong ></ strong ></ h2 >



< div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x716"" src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-1160x716.jpg.webp"" loading=""lazy"" width=""1160"" height=""716"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-1160x716.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247595 litespeed-loaded"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-1160x716.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-400x247.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-768x474.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-1536x948.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px"" sizes=""(max-width: 1160px) 100vw, 1160px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-1160x716.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-400x247.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-768x474.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01-1536x948.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_01.jpg.webp 1600w"" data-was-processed=""true""></figure></div>



<p>شرکت اینتل در ابتدای سال 2021، مدیر عامل جدیدی را برای خود برگزید.پاتریک گِلسینگر 60 ساله، معمار بزرگ پردازنده 80486 اینتل، تمام جوانی خود را در اینتل خدمت کرده بود و حتی در 32 سالگی به رده مدیریت میانی دست یافته بود.پاتریک گِلسینگر در سال 2009 اینتل را ترک کرد تا مدیریت EMC را بر عهده بگیرد.حال هیئت مدیره اینتل، پس از سالها درجا زدن، به امید بازگشت به روزهای رویایی، دست به دامان وی شده است. پاتریک با یک برنامه مشخص، قدم به قدم در حال باز سازی اینتل و تبدیل کردن آن به بزرگترین و پیشرفته ترین تولید کننده چیپ در دنیا است.در حالیکه اینتل با رقبایی سرسخت مانند TSMC، AMD، Samsung و حتی Apple دست و پنجه نرم می‌کند، پاتریک در طی مصاحبه ای اعلام کرد ما فقط به 2 سال کار و تلاش فشرده نیاز داریم تا به جایگاه مناسب خود در بازار دست پیدا کنیم.</p>



<figure class=""wp-block-embed is-type-rich is-provider-جاسازی-نگهدارنده wp-block-embed-جاسازی-نگهدارنده""><div class=""wp-block-embed__wrapper"">
<div class=""h_iframe-aparat_embed_frame""><span></span><iframe data-lazyloaded=""1"" src=""about:blank"" data-src=""https://www.aparat.com/video/video/embed/videohash/VH3Fr/vt/frame"" allowfullscreen=""true"" webkitallowfullscreen=""true"" mozallowfullscreen=""true""></iframe></div>
</div></figure>



<p>نسل 12 پردازنده‌های اینتل، اولین قدم این شرکت برای بازگشت به دوران شکوه است.در این پردازنده‌ها برای اولین بار از یک ساختار هیبریدی بهره گرفته شده است.بدین معنا که برای انجام امور پردازشی متفاوت، یک واحد مدیریتی تحت عنوان Intel Thread Director ایجاد شده که با تشخیص نوع پردازش، کارهای سبک را به سمت هسته‌های Efficient هدایت می‌کند و کارهای سنگین را به سمت هسته‌های Performance ارسال می‌کند.بدین ترتیب با این تقسیم کار، پردازش‌ها سریعتر انجام می‌پذیرند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1600x1158"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjAwIiBoZWlnaHQ9IjExNTgiIHZpZXdCb3g9IjAgMCAxNjAwIDExNTgiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1600"" height=""1158"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_02.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247594"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_02.jpg.webp 1600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_02-400x290.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_02-1160x840.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_02-768x556.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_02-1536x1112.jpg.webp 1536w"" data-sizes=""(max-width: 1600px) 100vw, 1600px""></figure></div>



<p>نو آوری آلدرلیک تنها به ساختار هیبریدی آن ختم نمی‌شود، اینتل برای اولین بار از ارتباط سریع PCIe Gen.5 بهره گرفته است تا نسل آتی کارتهای گرافیک را پشتیبانی کند.اینتل همچنین برای اولین بار از رمهای DDR5 پشتیبانی می‌کند.رمهای DDR5 تفاوت‌های عمده ای با رمهای DDR4 دارند. اولین و مهمترین تفاوت آنها، بحث مدار تغذیه است. تا پیش از این رگلاتوری برق رمهای DDR4 &nbsp; همیشه بر روی مادربرد قرار داشته است.اما در رمهای DDR5، بخش VRM رم به داخل ماژول جابه جا شده، بنابراین با وضعیتی روبه رو هستیم که رمها هر کدام رگلاتوری برق خود را دارند.از این نظر هم قیمت آنها بالاتر است، و هم اینکه نوع ولتاژ ورودی تفاوت کرده است.در رمهای DDR5، ولتاژ ورودی ریل 5V به صورت مستقیم از پاور گرفته می‌شود.تفاوت دیگر بحث فرکانس است که به نظر رمهای DDR5 به مانند پردازنده به هنگام کارهای سنگین فرکانس خود را Boost می‌کنند.یک تفاوت عمده دیگر رمهای DDR5، بحث نوع آنهاست.دو نوع رم DDR5 خواهیم داشت، یکی با ولتاژ ثابت و غیر قابل اورکلاک، و دیگری با ولتاژ متغییر و با قابلیت اورکلاک.بحث بعدی افزایش 4 برابری میزان حجم مورد پشتیبانی بیشتر در هر ماژول رم DDR5 نسبت به نسل گذشته است که در آینده بیشتر راجب این مورد خواهیم شنید.صد البته رمهای DDR5 فعلا بسیار گران است. به همین جهت، اینتل با فراهم آوردن پشتیبانی از رمهای DDR4 و DDR5، عملا به کاربران امکان بهره گیری از رمهای نسل فعلی و یا نسل آتی را داده است.صد البته این موضوع بستگی به مادربرد انتخابی شما دارد که از کدام نوع رم پشتیبانی می‌کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1600x1819"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjAwIiBoZWlnaHQ9IjE4MTkiIHZpZXdCb3g9IjAgMCAxNjAwIDE4MTkiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1600"" height=""1819"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_03.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247593"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_03.jpg.webp 1600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_03-352x400.jpg.webp 352w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_03-901x1024.jpg.webp 901w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_03-768x873.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_03-1351x1536.jpg.webp 1351w"" data-sizes=""(max-width: 1600px) 100vw, 1600px""></figure></div>



<p>در حال حاضر اینتل از خانواده آلدرلیک فقط 3 مدل پردازنده، i5 12600K(F)، i7 12700K(F) و i9 12900K(F) را معرفی کرده است.که مشخصات و تفاوت‌های آنها را در جدول فوق مشاهده می‌کنید.</p>



<figure class=""wp-block-embed is-type-rich is-provider-جاسازی-نگهدارنده wp-block-embed-جاسازی-نگهدارنده""><div class=""wp-block-embed__wrapper"">
<div class=""h_iframe-aparat_embed_frame""><span></span><iframe data-lazyloaded=""1"" src=""about:blank"" data-src=""https://www.aparat.com/video/video/embed/videohash/kQsUr/vt/frame"" allowfullscreen=""true"" webkitallowfullscreen=""true"" mozallowfullscreen=""true""></iframe></div>
</div></figure>



<p>این پردازنده‌ها به واقع قهرمانان اینتل در گیم هستند.هیچ پردازنده دیگری از AMD و یا حتی پردازنده‌های قدیمی‌اینتل، از نظر قدرت گیمینگ به پای نسل 12 نمی‌رسند.بنابراین اینتل همچنان با قدرت تاج پادشاهی تولید کننده بهترین پردازنده‌های گیمینگ در دنیا را برای خود حفظ کرده است.در بخش پردازش‌های موازی هم شاهد قلدری و زور بازوی قابل توجهی از سمت این پردازنده‌ها هستیم. به گونه ای که در اغلب Task‌های رندرینگ هم، پردازنده‌های اینتل، در صدر جدول قرار می‌گیرند.البته کلیه این موارد را به هنگامی‌که این پردازنده‌ها به دست ما برسد تست و آزمایش خواهیم کرد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1600x1492"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjAwIiBoZWlnaHQ9IjE0OTIiIHZpZXdCb3g9IjAgMCAxNjAwIDE0OTIiPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9IiNlOWViZWUiLz48L3N2Zz4="" loading=""lazy"" width=""1600"" height=""1492"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_04.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247592"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_04.jpg.webp 1600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_04-400x373.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_04-1098x1024.jpg.webp 1098w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_04-768x716.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_04-1536x1432.jpg.webp 1536w"" data-sizes=""(max-width: 1600px) 100vw, 1600px""></figure></div>



<p>و در آخر به چیپست Z690 می‌رسیم، جایی که مهمترین تغییر آن، افزایش پهنای باند چیپست و پردازنده به 8 خط PCIe Gen.4 است، که تقریبا در مقام قیاس با Z590، افزایش 2 برابری پهنای باند را شاهد هستیم.این موضوع باعث می‌شود مادربرد‌های Z690 ارتباطات گسترده تر و بیشتری را برای اتصال تجهیزات و Peripheral‌های مختلف را به خود داشته باشند. دیاگرام Z690 را در تصویر فوق مشاهده می‌کنید.</p>



<h2 id = ""h-"" >< strong > معرفی محصول:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-full""><img data-lazyloaded=""1"" data-placeholder-resp=""1600x839"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNjAwIiBoZWlnaHQ9IjgzOSIgdmlld0JveD0iMCAwIDE2MDAgODM5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1600"" height=""839"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_05.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247591"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_05.jpg.webp 1600w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_05-400x210.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_05-1160x608.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_05-768x403.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_05-1536x805.jpg.webp 1536w"" data-sizes=""(max-width: 1600px) 100vw, 1600px""></figure></div>



<p>مادربرد موضوع بررسی امروز لابراتوار سخت افزار، مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس است که در میان محصولات Z690 ایسوس، یک میان رده پر قدرت به حساب می‌آید.این مادربرد از خانواده ROG Strix است و پس از مدل Z690-E، قدرتمند ترین مادربرد این گروه به حساب می‌آید. حرف F مخفف Formula است که جایگاه رفیع این مادربرد میان رده را به خوبی بیان می‌کند.این مادربرد 400 دلاری از VRM مجهز به یک مدار 1+16 فازی تمام دیجیتال برای تغذیه پردازنده‌های نسل 12 استفاده می‌کند. ارتباط شبکه Wi-Fi 6E در کنار بلوتوث نسخه 5.2 و کارت شبکه 2.5 گیگابیتی از جمله دیگر امکانات این مادربرد به شمار می‌رود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x434"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQzNCIgdmlld0JveD0iMCAwIDExNjAgNDM0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""434"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_06-1160x434.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247590"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_06-1160x434.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_06-400x150.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_06-768x288.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_06-1536x575.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_06.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس از نظر ظاهری یک مادربرد گیمینگ محسوب می‌شود، وجود یک حالت شیشه ای شفاف در بالای گارد I/O پنل، جلوه بسیار زیبایی را برای این مادربرد به ارمغان آورده است.اما امکانات این مادربرد تنها به کاربری گیمینگ محدود نیست که پیشتر به آن اشاره خواهیم کرد.اتصال پر سرعت USB 3.2 Gen 2×2 از نوع Type C یک در پنل پشتی و یکی هم برای اتصال به فرانت پنل کیس، تعداد 3 درگاه USB 3.2 Gen 2، تعداد 6 درگاه USB 3.2 Gen.1، و شش درگاه USB 2.0، از مهمترین خصوصیات این مادربرد DDR5 است.</p>



<p>شکاف اصلی PCI Express این مادربرد از نوع PCIe Gen.5 X16 است و عملا با توجه به اینکه دیگر استفاده از چند کارت گرافیک به شکل کراس فایر و SLI منسوخ شده است، ایسوس با حذف دیگر شکاف‌های اضافی، خیال خود را راحت کرده است.بنابراین تنها با یک اسلات مخصوص نصب کارت گرافیک در این مادربرد روبه رو هستیم که از نوع تقویت شده است. البته ایسوس به جهت پشتیبانی از Add in Card، یک شکاف X16 متصل به چیپست را به همراه یک شکاف با پهنای باند x1 را همچنان بر روی این برد به کار گرفته است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x609"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYwOSIgdmlld0JveD0iMCAwIDExNjAgNjA5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""609"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_07-1160x609.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247589"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_07-1160x609.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_07-400x210.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_07-768x403.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_07-1536x806.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_07.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>یکی از بخش‌های مهم برای رندرکاران، بحث امکانات ذخیره سازی است.این مادربرد مجهز به 4 درگاه M.2 از نوع PCIe Gen.4 x4 است که همگی مجهز به سینک خنک کننده هستند.سه درگاه M.2 متصل به چیپست این مادربرد، امکان RAID 0 را نیز دارند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x707"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjcwNyIgdmlld0JveD0iMCAwIDExNjAgNzA3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""707"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_08-1160x707.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247588"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_08-1160x707.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_08-400x244.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_08-768x468.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_08-1536x936.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_08.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>از نظر نورپردازی، شرکت ایسوس 3 خروجی 3 پین ARGB نسل دومی، و یک خروجی 4 پین RGB را برای این مادربرد در نظر گرفته است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x605"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjYwNSIgdmlld0JveD0iMCAwIDExNjAgNjA1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""605"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_09-1160x605.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247587"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_09-1160x605.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_09-400x209.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_09-768x401.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_09-1536x802.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_09.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>هر اسلات رم DDR5 این مادربرد قادر به پشتیبانی از ماژول‌های 32 گیگابایتی DDR5 است که با پشتیبانی کامل از پروفایل نسل سوم XMP رمهای DDR5 تمی‌تواند رمهای شما را تا فرکانس بالای 6400Mhz اجرا کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x586"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU4NiIgdmlld0JveD0iMCAwIDExNjAgNTg2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""586"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_10-1160x586.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247586"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_10-1160x586.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_10-400x202.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_10-768x388.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_10-1536x776.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_10.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>ایسوس برای اولین بار همراه با مادربرد‌های Z690 که به رمهای DDR5 مجهز هستند، فن آوری مخصوصی را تحت عنوان AEMP عرضه می‌کند.این فن آوری با تشخیص مدار تغذیه روی رمهای DDR5، به صورت اتوماتیک بهترین تنظیمات از نظر فرکانس، تایمینگ، و پروفایل ولتاژی مناسب را برای رمهای شما تنظیم می‌کند تا بتوانید بدون دردسر از حداکثر توانمندی رمهای DDR5 خود بهره ببرید.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x584"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU4NCIgdmlld0JveD0iMCAwIDExNjAgNTg0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""584"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_11-1160x584.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247585"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_11-1160x584.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_11-400x201.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_11-768x386.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_11-1536x773.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_11.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>یکی از بخش‌های مهم مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس، بخش VRM آن است.جایی که شما با یک جفت ورودی 8 پین برق پردازنده، 600 وات را در اختیار این مدار تغذیه قرار می‌دهید.ایسوس با استفاده از کانکتور تقویت شده PRO COOL II، ضمن استحکام بخشی به این کانکتور، از اتصال کامل پین‌های کابل برق پاور به این بخش اطمینان حاصل می‌کند.ایسوس همچنین با استفاده از 16 ماسفت مجتمع 70 آمپری، که همزمان شامل ماسفت‌های Low-side و High-Side درون یک پکیج هستند، از مداری قدرتمند برای ثبات و رگلاتوری مناسب پردازنده‌های نسل 12 اینتل، استفاده کرده است.چوک‌های آلیاژ آلومینویمی‌و خازن‌های 10K از دیگر کامپوننت‌های مهم این بخش به شمار می‌روند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x479"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQ3OSIgdmlld0JveD0iMCAwIDExNjAgNDc5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""479"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_12-1160x479.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247584"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_12-1160x479.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_12-400x165.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_12-768x317.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_12-1536x634.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_12.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس مجهز به اتصال بی سیم Wi-Fi 6E و بلوتوث نسخه 5.2 است.این نوع Wi-Fi از ارتباط 6 گیگاهرتزی نیز پشتیبانی می‌کند.نسخه 5 بلوتوث نیز نسبت به نسل‌های پیشین خود، 4 برابر رنج وسیعتری را پشتیبانی ‌می‌کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x553"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjU1MyIgdmlld0JveD0iMCAwIDExNjAgNTUzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""553"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_13-1160x553.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247583"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_13-1160x553.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_13-400x191.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_13-768x366.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_13-1536x732.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_13.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>بخش صدای مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس نیز از چیپ جدید و ارتقا یافته ALC 4080 شرکت Realtek در کنار DAC قدرتمند Savitech SV3H712 بهره می‌برد.این ترکیب در حال حاضر یکی از قدرتمند ترین بخش‌های صوتی در این رنج قیمتی را تشکیل می‌دهند.</p>



<h2 id = ""h--1"" >< strong > جعبه و وسایل همراه:</strong></h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x812"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjgxMiIgdmlld0JveD0iMCAwIDExNjAgODEyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""812"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_14-1160x812.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247582"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_14-1160x812.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_14-400x280.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_14-768x538.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_14-1536x1075.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_14.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>جعبه مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس تم زیبای همیشگی ROG قرمز و مشکی ایسوس را دارد.مهمترین ویژگی‌های نوشته شده بر روی جعبه پشتیبانی از نسل 12 اینتل، ویندوز 11، نورپردازی Aura Sync، ارتباط سریع PCIe Gen.5، رمهای DDR5 و کارت شبکه Wi-Fi 6E است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x850"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijg1MCIgdmlld0JveD0iMCAwIDExNjAgODUwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""850"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_15-1160x850.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247581"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_15-1160x850.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_15-400x293.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_15-768x563.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_15-1536x1125.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_15.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در پشت جعبه، امکانات مادربرد، شامل اتصالات پنل پشتی، تعداد و نوع درگاه‌ها به تفصیل توضیح داده شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x917"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjkxNyIgdmlld0JveD0iMCAwIDExNjAgOTE3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""917"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_16-1160x917.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247580"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_16-1160x917.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_16-400x316.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_16-768x607.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_16-1536x1214.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_16.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اگر چه مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس در کلاس ROG، یک مادربرد میان رده است، اما وسایل همراه کمابیش مطلوبی دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x372"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjM3MiIgdmlld0JveD0iMCAwIDExNjAgMzcyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""372"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_17-1160x372.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247579"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_17-1160x372.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_17-400x128.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_17-768x246.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_17-1536x492.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_17.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>اقلام داخل جعبه شامل:</p>



<ul><li>دی وی دی نصب</li><li>چهار عدد کابل ساتا</li><li>آنتن بلوتوث و Wi-Fi</li><li>یک جاسوییچی ROG</li><li>دفترچه راهنما و بروشور نصب</li><li>یک برگه استیکر مخصوص ROG</li><li>یک عدد بست چسبی کابل ROG (چسبیده به چیپست مادربرد)</li></ul>



<p>می‌شود.البته به شخصه حاضر بودم اون جاسوییچی، دی وی دی درایور، بست چسبی و یک برگ استیکر رو به خود ایسوس پس بدم به جاش یک کابل اکستنشن RGB و یک کابل اکستنشن ARGB ازشون بگیرم.این دو تا کابل&nbsp; بیش از اون اقلام به کار کاربر می‌آید.</p>



<h2 id = ""h--2"" >< strong > طراحی و امکانات سخت افزاری</strong>:</h2>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x962"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijk2MiIgdmlld0JveD0iMCAwIDExNjAgOTYyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""962"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_18-1160x962.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247578"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_18-1160x962.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_18-400x332.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_18-768x637.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_18-1536x1274.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_18.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>مادربرد ROG STRIX Z690-F GAMING WIFI ایسوس از نظر طراحی ظاهری فوق العاده زیباست.بخش اعظم مادربرد در قسمت فوقانی را سینک‌های بزرگ VRM تشکیل می‌دهد، و در قسمت پایین هم سینک‌های بزرگ خنک کننده SSD‌های M.2. ایسوس یک بست سفید رنگ چسبی با نوشته GAMERS برای مرتب کردن کابل‌ها بر روی چیپست چسبانده است که مسلما به دلیل زیبایی خاصی که دارد، در 99.9% موارد از طرف کاربران برای مدیریت کابل کشی استفاده نخواهد شد و صرفا برای دکور مادربرد است.اگر در آینده برای ارتقا قصد فروش مادربرد را دارید، پیشنهاد می‌کنم پس از نصب اولیه و عکاسی از سیستم خود، این بست چسبی را درون جعبه مادربرد نگهداری کنید تا همینطور نو و زیبا بماند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x814"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjgxNCIgdmlld0JveD0iMCAwIDExNjAgODE0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""814"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_19-1160x814.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247577"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_19-1160x814.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_19-400x281.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_19-768x539.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_19-1536x1078.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_19.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در کل لاین آپ Z690 ایسوس، هیچ گارد I/O پنلی به زیبایی گارد I/O پنل مادربرد Z690-F نیست.از این نظر واقعا طراحی این بخش با استفاده از یک قاب شیشه ای تابلو مانند، بسیار خاص است و نمای کلی مادربرد را از این رو به آن رو کرده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""833x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI4MzMiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDgzMyAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""833"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_20-833x1024.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247576"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_20-833x1024.jpg.webp 833w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_20-325x400.jpg.webp 325w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_20-768x944.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_20-1249x1536.jpg.webp 1249w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_20.jpg.webp 1600w"" data-sizes=""(max-width: 833px) 100vw, 833px""></figure></div>



<p>نمای پشتی مادربرد به غیر از نقش و نگاری که ایسوس بر روی آن طراحی کرده، مطلب خاصی ندارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""834x1024"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI4MzQiIGhlaWdodD0iMTAyNCIgdmlld0JveD0iMCAwIDgzNCAxMDI0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""834"" height=""1024"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_21-834x1024.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247575"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_21-834x1024.jpg.webp 834w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_21-326x400.jpg.webp 326w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_21-768x943.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_21-1251x1536.jpg.webp 1251w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_21.jpg.webp 1600w"" data-sizes=""(max-width: 834px) 100vw, 834px""></figure></div>



<p>در بخش فوقانی، از چپ به راست، مهمترین بخش‌ها شامل ورودی دوگانه برق پردازنده، شامل دو سوکت 8 پین برق است که مجموع 600 وات را برای تغذیه پردازنده فراهم می‌کند.در کنار این دو سوکت یک هدر 4 پین فن قرار دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x743"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc0MyIgdmlld0JveD0iMCAwIDExNjAgNzQzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""743"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_22-1160x743.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247574"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_22-1160x743.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_22-400x256.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_22-768x492.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_22-1536x984.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_22.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در سمت راست تصویر، از بالا یک هدر 4 پین فن PWM مخصوص CPU FAN، یک جامپر برای Over-Volt کردن پردازنده به هنگام اورکلاک است، سپس اتصالات سفید رنگ نورپردازی شامل یک خروجی 4 پین RGB و یک خروجی 3 پین ARGB ‌دیده می‌شود.</p>



<p>در لبه پایینی از سمت راست، به کمک 4 چراغ LED بخش Q_Code، در صورتی که به هنگام بوت، مادربرد در هر بخشی شامل پردازنده، رم، کارت گرافیک، و یا درایو بوت مشکلی داشته باشد و نتواند آن را حل کند، چراغ مخصوص آن بخش مشکل زا به صورت ثابت روشن می‌ماند، در غیر این صورت، این بخش به شکل یک چراغ راهنمایی، صرفا Pass شدن و عبور موفقیت آمیز مادربرد را از هر کدام از این بخش‌ها به نمایش می‌گذارد.</p>



<p>سپس سوکت 24 پین مادربرد، یک درگاه اتصال USB 3.2 Gen.2 x2 Type C و یک درگاه اتصال USB 3.0 برای فرانت پنل کیس قرار دارد. این مادربرد مجهز به 4 اسلات رم DDR5 از نوع تقویت شده است که از فرکانس کاری بالای 6400 مگاهرتز و حجم 128 گیگابایت (4 ماژول 32 گیگابایتی) را پشتیبانی ‌می‌کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x340"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjM0MCIgdmlld0JveD0iMCAwIDExNjAgMzQwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""340"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_23-1160x340.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247573"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_23-1160x340.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_23-400x117.jpg 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_23-768x225.jpg 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_23-1536x450.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_23.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>دکمه Q-Release ابتکار جدید ایسوس برای رهایی از مشکل آزاد سازی قفل اسلات کارت گرافیک در نسل جدید Z690 است.کارت‌های گرافیک امروزی به لطف بک پلیت‌های بسیار ضخیم و درشتی که دارند، با وجود سینک‌های فراوان در بالا و پایین اسلات گرافیک مادربرد، پس از نصب گیر می‌کنند و دیگر انگشت نمی‌تواند به راحتی به ضامن اسلات گرافیک برسد.بنابراین در چنین شرایطی ناچار هستید یک پیچ گوشتی دراز را راهی سطح مادربرد کنید که در رفتن آن پیچ گوشتی می‌تواند به مادربرد آسیب برساند.ایسوس برای خلاص شدن از این وضعیت، دکمه Q-Release را ابداع کرده است که با اتصال تسمه ای فلزی به ضامن اسلات گرافیک، می‌تواند به راحتی آن را آزاد کند.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x407"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjQwNyIgdmlld0JveD0iMCAwIDExNjAgNDA3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""407"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_24-1160x407.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247572"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_24-1160x407.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_24-400x141.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_24-768x270.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_24-1536x540.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_24.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>تعداد 6 عدد درگاه SATA III نیز در لبه ی سمت راست مادربرد به چشم ‌می‌خورد که برای مدریت کابل کشی راحت تر با زاویه 90 درجه طراحی شده اند.در سمت چپ آنها یک هدر‌ PWM فن دیگر قرار دارد.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x762"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc2MiIgdmlld0JveD0iMCAwIDExNjAgNzYyIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""762"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_25-1160x762.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247571"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_25-1160x762.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_25-400x263.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_25-768x504.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_25-1536x1009.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_25.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>در لبه پایینی مادربرد کلکسیونی از انواع اتصالات حضور دارند که از سمت راست، اتصالات فرانت پنل کیس، دو عدد هدر 4 پین فن PWM، هدر مخصوص TPM، هدر سنسور دما، یک جفت درگاه USB 2.0 برای فرانت پنل، هدر اتصال تاندربولت، و مجددا اتصالات نورپردازی شامل یک جفت خروجی 3 پین ARGB در این بخش قرار دارند.در انتهای لبه پایین سمت چپ نیز اتصال Audio فرانت پنل کیس در نزدیکی چیپ صدا قرار دارد.در بخش صوتی شاهد حضور چیپ پر قدرت Realtek ALC4080 هستیم که ایسوس از آن در ترکیب با دک Savitech SV3H712 بهره گرفته است.ایزولاسیون کامل بخش ضدا و استفاده از خازن‌های کلاس صوتی چمیکون ژاپنی از دیگر خصوصیات این بخش به شمار می‌رود.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x770"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MCIgdmlld0JveD0iMCAwIDExNjAgNzcwIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""770"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_26-1160x770.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247570"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_26-1160x770.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_26-400x266.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_26-768x510.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_26-1536x1020.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_26.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>از نظر کولینگ، ایسوس مجموع 8 خروجی PWM را به شرح فوق برای این مادربرد در نظر گرفته است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x305"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9IjMwNSIgdmlld0JveD0iMCAwIDExNjAgMzA1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""305"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_27-1160x305.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247569"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_27-1160x305.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_27-400x105.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_27-768x202.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_27-1536x403.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_27.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>پنل پشتی مادربرد از نوع ثابت است و اتصالات آن شامل پنج خروجی جک صوتی و یک درگاه خروجی صدای اپتیکال، درگاه 2.5 گیگابیتی LAN، تعداد یک عدد USB 3.2 Gen.2 x2 نوع C، سه عدد USB 3.2 Gen.2 از نوع A و C، همچنین چهار عدد USB 3، یک درگاه DP 1.4 و یک درگاه HDMI 2.1، برای خروجی تصویری گرافیک مجتمع، دکمه Clear CMOS، بایوس فلش بک و یک جفت درگاه USB 2.0 نیز در نظر گرفته شده است.دو اتصال طلایی رنگ هم برای اتصال آنتن SMA برای چیپ وایرلس Wi-Fi6E و بلوتوث نسخه 5 قرار داده شده است.</p>



<div class=""wp-block-image""><figure class=""aligncenter size-large""><img data-lazyloaded=""1"" data-placeholder-resp=""1160x773"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMTYwIiBoZWlnaHQ9Ijc3MyIgdmlld0JveD0iMCAwIDExNjAgNzczIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" width=""1160"" height=""773"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_28-1160x773.jpg.webp"" alt=""بررسی مادربرد ایسوس ASUS ROG STRIX Z690-F GAMING WIFI"" class=""wp-image-247568"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_28-1160x773.jpg.webp 1160w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_28-400x267.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_28-768x512.jpg.webp 768w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_28-1536x1023.jpg.webp 1536w, https://sakhtafzarmag.com/wp-content/uploads/2021/11/ASUS-ROG-STRIX-Z690-F-GAMING_28.jpg.webp 1600w"" data-sizes=""(max-width: 1160px) 100vw, 1160px""></figure></div>



<p>بخش VRM مادربرد ROG STRIX Z690-F GAMING WIFI از جمله قدرتمند ترین بخش‌های آن است که در ترکیب با پردازنده‌ها قدرت و توانایی خود را نشان می‌دهد.انشاالله پس از بررسی، در مورد این بخش توضیح بیشتری خواهیم داد.</p>



<p><strong>در این قسمت به بررسی اجمالی این مادربرد پرداختیم، در بخش دوم تست با پردازنده و همچنین امکانات دیگر را خواهیم پرداخت که در ظرف روزهای آینده در ادامه همین مقاله منتشر خواهیم کرد.</strong></p>
																																																									</div>",
                });
                News.Add(new Tables.News()
                {
                    Id = 24,
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "کولر,دسک تاپ",
                    MainImageUri = "https://sakhtafzarmag.com/wp-content/uploads/2018/03/k2_items_src_0b3ace58bed7bf765a81a5be8df96b7c-960x540.jpg.webp",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = string.Empty,
                    Title = "بررسی کولر زیبا و خنکCooler Master MasterAir MA620P",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28),
                    CategoryId = 16,
                    Body = @"<div itemprop=""articleBody"" class=""entry - inner"">

                                            < p > تعداد شرکت های قابل اتکا در طراحی و تولید ایرکولرهای حرفه ای مجهز به لوله های انتقال حرارت یا Heat - Pipes، از تعداد انگشتان یک دست هم کمتر است.در میان این شمار محدود، شرکت کولر مستر، یکی از بهترین هاست که دقیقا حرفه اش مرتبط با طراحی و تولید انواع سیستم های گرمایشی و کنترل دما است.کولر مستر در سال 2001 اولین کولر پردازنده مجهز به لوله های دفع کننده حرارت یا Heat - Pipes را تولید کرد.</ p >
< p >< span id = ""more-22474"" ></ span ></ p >
< p > از جمله موفق ترین محصولات این شرکت در سال 2010 ایرکولر Cooler Master Hyper 212 Plus بود که گوی سبقت را، چه از نظر قدرت خنک کنندگی و چه از نظر نویز و طراحی، از رقبا ربود.جالب اینکه، این کولر خوش ساخت، بعد از 8 سال، همچنان در حال تولید و فروش است.دور از واقعیت نیست که ادعا کنیم، بسیاری از شرکت ها، حتی برخی شرکت های به نام، طراحی ایرکولر های خود را با کپی از Hyper 212 تولید و وارد بازار کردند تا شاید مورد توجه کاربران قرار گیرند.اما اعتماد کاربران به کولر مستر، این برند خوش نام، همواره برقرار بوده است.به پشتوانه این اعتماد، کولر مستر، به صورت مستمر در حال بهبود طراحی خنک کننده ها و در عین حال، تولید محصولات جدید و به روز است.</ p >
< p >< img data - lazyloaded = ""1"" data - placeholder - resp = ""1024x868"" src = ""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1.jpg.webp"" loading = ""lazy"" class=""size-full wp-image-100664 litespeed-loaded"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""868"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1-400x339.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1-768x651.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px"" sizes=""(max-width: 1024px) 100vw, 1024px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1-400x339.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P1-768x651.jpg.webp 768w"" data-was-processed=""true""></p>
<p>جدید ترین سری از ایرکولر های Cooler Maser، تحت نام خانواده MasterAir نامگذاری و وارد بازار می شوند.از جمله مهمترین ویژگی های این ایرکولر ها تجهیز اغلب آنها به نورپردازی LED RGB، و توجه خاص کولر مستر به طراحی و نمای این کولر ها بوده است.تقریبا یک هفته ای است که در اتاق تست سخت افزار، مشغول بررسی ایرکولر MaterAir MA620P هستیم. این کولر خوش ساخت از جمله محصولات High End کولر مستر به شمار می رود که به تازگی وارد بازار های جهانی و به زودی وارد بازار ایران نیز می شود.با این بررسی نیز همراه ما باشید.</p>
<h3>جعبه و مشخصات فنی</h3>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x458"" src= ""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2.jpg.webp"" loading= ""lazy"" class=""size-full wp-image-100665 litespeed-loaded"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""458"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2-400x179.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2-768x344.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px"" sizes=""(max-width: 1024px) 100vw, 1024px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2-400x179.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P2-768x344.jpg.webp 768w"" data-was-processed=""true""></p>
<p>علاقمندی خاص کولرمستر به رنگ بنفش از روی جعبه اغلب محصولات این شرکت نمایان است.جعبه کولر MA620P نیز از این قائده مستثنا نیست، و تم بنفش و مشکی نوک مدادی دارد.از جمله مهمترین مشخصه های این کولر که بر روی جعبه خودنمایی می کند، پشتیبانی نورپردازی آن از اکوسیستم نورپردازی اغلب شرکت های به نام مادربردسازی است.دارا بودن دو برج خنک کننده مجزا، استفاده از 6 لوله مسی خنک کننده با فن آوری CDC 2.0، و همچنین دارا بودن کنترلر نورپردازی انحصاری کولر مستر از دیگر مشخصات این ایرکولر به شمار می رود که پیشتر مفصل به توضیح آنها می پردازیم.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x694"" src= ""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3.jpg.webp"" loading= ""lazy"" class=""size-full wp-image-100666 litespeed-loaded"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""694"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3-400x271.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3-768x521.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px"" sizes=""(max-width: 1024px) 100vw, 1024px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3-400x271.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P3-768x521.jpg.webp 768w"" data-was-processed=""true""></p>
<p>در جعبه کولر، علاوه بر قطعات براکت نصب برای سوکت های Intel و AMD، یک کابل دو به یک RGB برای اتصال به کنترلر RGB وجود دارد که می توان آن را به خروجی RGB مادربرد و یا به کنترلر مخصوص کولر مستر که داخل جعبه موجود است متصل کرد.در بخش انتهایی ایرکولر جای فن سومی هم وجود دارد که می توان آن را به فن سوم نیز مجهز کرد ولیکن خبری از وجود گیره اضافی برای اتصال فن سوم در اقلام داخل جعبه نبود، اما طی صحبتی که با دفتر محترم شرکت کولرمستر انجام شد، مشخص گردید در جعبه محصول نهایی، یک جفت گیره فن برای اتصال فن سوم هم داخل جعبه خواهد بود تا پتانسیل امکان نصب فن سوم نیز حفظ شود.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""991x826"" src= ""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4.jpg.webp"" loading= ""lazy"" class=""size-full wp-image-100667 litespeed-loaded"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""991"" height=""826"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4.jpg.webp 991w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4-400x333.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4-768x640.jpg.webp 768w"" data-sizes=""(max-width: 991px) 100vw, 991px"" sizes=""(max-width: 991px) 100vw, 991px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4.jpg.webp 991w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4-400x333.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P4-768x640.jpg.webp 768w"" data-was-processed=""true""></p>
<p>جدول مشخصات MA620P نشان از پشتیبانی کامل از کلیه سوکت های Intel و AMD دارد؛ البته به غیر از سوکت TR4 که برای آن کولر مستر به صورت اختصاصی کولر MA621P را روانه بازار خواهد کرد تا کاربران Threadripper نیز از این کولر پرقدرت بی نصیب نمانند.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x854"" src= ""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5.jpg"" loading= ""lazy"" class=""size-full wp-image-100668 litespeed-loaded"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5.jpg"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""854"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5.jpg 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5-400x334.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5-768x641.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px"" sizes=""(max-width: 1024px) 100vw, 1024px"" srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5.jpg 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5-400x334.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P5-768x641.jpg.webp 768w"" data-was-processed=""true""></p>
<p>طول، عرض و ارتفاع کولر برابر 116 در 110.1 در 158.4 میلی متر و وزن آن به همراه فن ها تقریبا برابر یک کیلوگرم می باشد و از این نظر کولر سنگین وزنی به شمار می رود.از جمله نکات خوب کولر های سنگین وزن فشار مناسبی است که بلاک روی پردازنده می آورد تا حداکثر چسبندگی و به تبع آن، حداکثر خنک کنندگی ایجاد می شود. باید اضافه کرد، ارتفاع زیر 159 سانتیمتری آن نیز امکان سازگاری با اغلب کیس ها را فراهم می کند.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x863"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9Ijg2MyIgdmlld0JveD0iMCAwIDEwMjQgODYzIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100669"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P6.jpg"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""863"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P6.jpg 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P6-400x337.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P6-768x647.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>تعداد شش عدد لوله مسی در دو برج خنک کننده به صورت مجزا از هم به فاصله برابر قرار دارند.سطح نامتقارن صفحات آلومینیومی خنک کننده در این برج ها، به ایجاد گردش هوای حداکثری برای خنک سازی لوله های دفع کننده گرما کمک می کند. بواقع کولر مستر به عنوان استاد ایرکولر ها می تواند بهترین طراحی برای خنک سازی لوله های مسی ناقل گرمای پردازنده را انجام دهد.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x835"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjgzNSIgdmlld0JveD0iMCAwIDEwMjQgODM1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100670"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P7.jpg"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""835"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P7.jpg 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P7-400x326.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P7-768x626.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>در نمای کناری، می توان به خوبی جای ماشین کاری را در پایین لوله های مسی مشاهده کرد.کولرمستر تلاش کرده است تا جای ممکن برج های خنک کننده را مرتفع و با فاصله ی مناسب نسبت به کف مادربرد قرار دهد تا حدکثر فضای مناسب برای نصب رم بوجود آید.البته اینکه چقدر موفق به این کار شده است را در بخش بررسی به آن می پردازیم.</p>
<p><img data-lazyloaded=""1"" data-placeholder-resp=""1024x861"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9Ijg2MSIgdmlld0JveD0iMCAwIDEwMjQgODYxIj48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading=""lazy"" class="" size-full wp-image-100671"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P8.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""861"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P8.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P8-400x336.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P8-768x646.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>نمای مسترایر MA620 از بالا بدین صورت است.طراحی فوقانی هرچند متقارن و زیبا، اما می توانست دارای علائم جذابتری باشد، مثلا دو آرم برند کولر مستر می توانست از جنس براق آلومینیومی باشد و یا اینکه در سه نقطه ی خروجی لوله های مسی، دیودهای نوری RGB نصب باشند که خوب، ارزان ترین راهکار همان استفاده از بج های آلومینیومی و یا براق کولرمستر نمای بهتری می توانست به آن بدهد. اما درکل، همین طراحی نیز تدایی کننده درون موتور های چند سلیندر ماشین های مسابقه ای است، و بهتر از نمای استوک صفهات آلومینیومی خالی است.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x957"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9Ijk1NyIgdmlld0JveD0iMCAwIDEwMjQgOTU3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100672"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P9.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""957"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P9.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P9-400x374.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P9-768x718.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>در بخش اصلی داستان کولرمستر سنگ تمام گذاشته است.لوله های مسی از دو طرف به صورت فشرده و کاملا ماشین شده در کنار هم قرارگرفته اند تا سطحی وسیع و بسیار فراتر از پشت پردازنده را پوشش دهند.این لوله های بسیار با کیفیت از جنس مس که بهترین آلیاژ انتقال گرما است ساخته شده اند.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x695"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjY5NSIgdmlld0JveD0iMCAwIDEwMjQgNjk1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100673"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P10.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""695"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P10.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P10-400x271.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P10-768x521.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>در نمایی نزدیکتر از بلاک کولر می توان معنای فن آوری Continuous Direct Contact Technology 2.0 را درک کرد: اتصال مداوم، بی واسطه، و تمام تخت سطح لوله های خنک کننده با پشت پردازنده، به لطف این فن آوری در طراحی و ساخت فراهم شده است.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x586"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjU4NiIgdmlld0JveD0iMCAwIDEwMjQgNTg2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100674"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P11.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""586"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P11.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P11-400x229.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P11-768x440.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>دو عدد فن MasterFan MF120R RGB موتور خنک کننده ایرکولر MA620P را تشکیل می دهند.این فن های فوق العاده بی صدا هستند و حتی در حالت گردش حداکثری خود، کمترین میزان نویز را ایجاد می کنند.بواقع به سختی بتوان از درون کیس متوجه صدای فن ها شد.این فنهای 12 سانتیمتری، از دامنه گردش 600 تا 1800 دور در دقیقه برخوردار هستند و از نورپردازی RGB پشتیبانی می کنند. کابل RGB آن مجزا از کابل PWM فن است و صرفا به کابل مخصوص RGB دو به یک داخل پک کولر متصل می شود.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x715"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjcxNSIgdmlld0JveD0iMCAwIDEwMjQgNzE1Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100675"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P12.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""715"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P12.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P12-400x279.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P12-768x536.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<h3>سیستم تست</h3>
<h3><strong>پردازنده Intel Core i7 7700K</strong></h3>
<p>یکی از بهترین گزینه ها برای انجام تست ها و بازی ها این پردازنده است.از ویژگی های این پردازنده می توان به معماری ساخت 14 نانومتری، 4 هسته و 8 رشته موازی، فرکانس کاری 4.2 گیگاهرتز و مقدار 8 مگابایت برای کش اشاره کرد.عملکرد بی نظیر این پردازنده آدرنالین خون کاربر را به بالاترین حد خود می رساند.پردازنده مورد نظر را شرکت خوش نام دانش و فناوری مازستا یا DFM به دفتر مجله سخت افزار هدیه داده اند تا سیستم تست مجموعه بی نصیب از این پردازنده نماند.</p>
<h3><strong>مادربرد MSI Z270 GAMING M5</strong></h3>
<p>مادربرد گیمینگ از ام اس آی با کیفیت ساخت بالا و امکانات عالی برای راه اندازی یک سیستم گیم قدرتمند. استفاده از سوکت 1151 و چیپ ست Z270 به همراه فرکانس حافظه 4133 مگاهرتز در حالت اورکلاک، سینک های حرارتی کارآمد، وجود خروجی ها و ورودی های کامل، نرم افزارهای جانبی کامل برای مدیریت بهتر سیستم، نورپردازی زیبا، قابلیت اورکلاک بالا و دریافت بیش از 20 جایزه و نشان برتر تنها بخشی از ویژگی های این مادربرد است.</p>
<h3><strong>حافظه &nbsp; G.SKILL TridentZ 2X8G 3600 CL16</strong></h3>
<p>این سری از حافظه های جی اسکیل برای دست یابی به فرکانس بالا بهینه شده است.کیت مورد استفاده 2 ماژول 8 گیگابایتی DDR4 است که با فعال کردن XMP از حداکثر فرکانس کاری 3600 مگاهرتز با زمان تاخیر CL16 بهره می برد.</p>
<h3><strong>منبع تغذیه GREEN GP850B-OCPT</strong></h3>
<p>نسل جدید پاور های پلاتینیومی خانواده Overclocking Evo شرکت گرین است.این پاور از یک ریل پرقدرت 12 ولت 70 آمپری بهره می برد، که به لطف مبدل‌ DC-DC برای ایجاد ولتاژ شاخه‌های +5V و +3.3V در بخش ثانویه، باعث تثبیت هر چه بیشتر ولتاژ شاخه‌های خروجی در این پاور می شود.امکان تحقق عملی Zero Voltage Switching در بخش ورودی این پاور به کمک توپولوژی Half-Bridge در کنار مبدل LLC Resonant ایجاد شده است تا حامل‌های EMI، RF و تلفات سوئیچینگ در ورودی کاهش یافته و بازدهی مصرف انرژی افزایش یابد. حالت Fan-Less بودن و استفاده از قطعات صنعتی برای افزایش طول عمر، فن خنک کننده با عملکرد بهینه و نویز بسیار کم از دیگر امکانات آن به شمار می رود. این پاور با 10 سال گارانتی، توسط شرکت گرین برای سیستم تست در اختیار ما قرار داده شده است.</p>
<h3><strong>درایو ذخیره سازی ADATA SX9000</strong></h3>
<p>اس اس دی Adata XPG SX9000NP 256G با سرعت انتقال فوق&nbsp; العاده از طریق رابط PCIe Gen3x4، پروتکل NVME نسخه ۱٫۲، فناوری SLC Caching&nbsp; و DRAM Cache Buffer، موتور تصحیح خطای پیشرفته LDPC ECC و ۵ سال گارانتی تعویض شرکت آونگ نماینده رسمی Adata بهره می برد.از مزیت های این حافظه سرعت نوشتن 990 مگابایت در ثانیه و سرعت خواندن 2700 مگابایت در ثانیه است.این اس اس دی پرچمدار Adata را شرکت محترم آونگ برای سیستم تست دفتر سخت افزار مگ ارسال کردند.</p>
<h3>بررسی مکانیزم نصب</h3>
<p>مکانیزم نصب ایرکولر MA620P شامل یک بک پلیت نگهدارنده پلاستیکی محکم می شود که در پشت مادربرد در پشت سینک سوکت پردازنده قرار می گیرد.سپس مهره هایی با دو سر پیچ از جلوی مادربرد به بک پلیت متصل می شوند. بعد کولر با کمک دو براکت عرضی، در نهایت به پیچ-مهره های میانی با کمک 4 مهره دیگر پیچ می شود.واقعیتش نصب این کولر چندان آسان نبود.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x756"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9Ijc1NiIgdmlld0JveD0iMCAwIDEwMjQgNzU2Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100676"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P13.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""756"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P13.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P13-400x295.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P13-768x567.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>آچاری که کولرمستر برای پیچ کردن مهره ها در نظر گرفته بود کمی کوتاه بود و به راحتی نمی شد به مهره هایی که در بخش فوقانی مادربرد قرار می گرفت دسترسی داشت.عملا با بقل گرفت مادربرد و جداسازی همه قطعات دیگر توانستیم این کولر را به مادربرد متصل کنیم.بنابراین امکان ندارد بتوانید بدون خارج کردن مادربرد نسبت به نصب این کولر اقدام کنید.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x769"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9Ijc2OSIgdmlld0JveD0iMCAwIDEwMjQgNzY5Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100677"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P14.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""769"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P14.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P14-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P14-768x577.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>مورد بعدی ارتفاع برج های خنک کننده عملا اسلات اول رم را اشغال کردند.با اینکه کولر مستر تمام توانش را به کار گرفت تا با ارتفاع دادن به برج ها از اشغال فضای مورد نیاز رمها جلوگیری کند اما همچنان رمهای TridentZ ما نتوانستند در شکاف اول رم نصب شوند. البته این نکته چندان منفی نیست، چراکه اشغال اولین اسلات رم در قریب به اتفاق ایرکولر های همه ی برند ها صدق می کند. مهم این است که توانستیم در اسلات دوم به بعد رمها را نصب کنیم و فقط با چند میلیمتر ارتفاع دادن به فن، آن را به راحتی بالای سر رمها نصب کنیم تا نمای کولر دست نخورده باقی بماند. در تصاویر فوق کاملا مشخص است که فن بر روی شانه ی رمها نشسته است ولی شکل کلی کولر حفظ شده و به خوبی با قطعات ما مچ شده است.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x768"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9Ijc2OCIgdmlld0JveD0iMCAwIDEwMjQgNzY4Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100678"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P15.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""768"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P15.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P15-400x300.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P15-768x576.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>اگر چه عملیات نصب کولر آسان نبود ولیکن پس از یک بار باز و بسته کردن، با نگاهی به سطح خمیر سیلیکن نشسته شده در پشت بلاک کاملا مشخص است که پردازنده در تماس صد در صدی با تمامی سطح بلاک قرار داشته است که این موضوع نشان از موفقیت فنی این مکانیزم نصب می باشد.</p>
<h3>بررسی عملکرد</h3>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x807"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjgwNyIgdmlld0JveD0iMCAwIDEwMjQgODA3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100679"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P16.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""807"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P16.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P16-400x315.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P16-768x605.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>با متوسط دمای اتاق تست 25 درجه سانتیگراد، به سراغ تست این ایرکولر در حالت پیش فرض می رویم.دمای پردازنده ی Core i7 7700K در زمان بیکاری در ولتاژ و فرکانس دیفالت برابر 35 درجه سانتیگراد و در حالت لود کامل برابر 70 درجه سانتیگراد قرار داشت.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1024x344"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDI0IiBoZWlnaHQ9IjM0NCIgdmlld0JveD0iMCAwIDEwMjQgMzQ0Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100680"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P17.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1024"" height=""344"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P17.jpg.webp 1024w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P17-400x134.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P17-768x258.jpg.webp 768w"" data-sizes=""(max-width: 1024px) 100vw, 1024px""></p>
<p>در حالت اورکلاک بدون افزایش ولتاژ، ضریب پردازنده را روی 47 قرار داده و مجدد تست را تکرار می کنیم.دمای هسته های پردازنده در حالت بیکاری بر روی 36 درجه سانتیگراد و در حالت لود به حداکثر 78 درجه می رسد.</p>
<p><img data-lazyloaded=""1"" data-placeholder-resp=""896x757"" src=""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI4OTYiIGhlaWdodD0iNzU3IiB2aWV3Qm94PSIwIDAgODk2IDc1NyI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading=""lazy"" class="" size-full wp-image-100681"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P18.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""896"" height=""757"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P18.jpg.webp 896w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P18-400x338.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P18-768x649.jpg.webp 768w"" data-sizes=""(max-width: 896px) 100vw, 896px""></p>
<p>حال میزان اورکلاک Core i7 7700K را تا 5 گیگاهرتز افزایش می دهیم و به طبع آن ولتاژ هسته ها را نیز کمی بالا می بریم.در این حالت تست XTU را نیز اجرا کرده تا حداکثر دما برای این پردازنده در فرکانس 5 گیگاهرتز برابر 86 درجه سانتیگراد ثبت شود.این میزان دما در این فرکانس، بسیار عالی است.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""1023x757"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMDIzIiBoZWlnaHQ9Ijc1NyIgdmlld0JveD0iMCAwIDEwMjMgNzU3Ij48cmVjdCB3aWR0aD0iMTAwJSIgaGVpZ2h0PSIxMDAlIiBmaWxsPSIjZTllYmVlIi8+PC9zdmc+"" loading= ""lazy"" class="" size-full wp-image-100682"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P19.jpg.webp"" alt=""بررسی کولر زیبا و خنکCooler Master MasterAir MA620P"" width=""1023"" height=""757"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P19.jpg.webp 1023w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P19-400x296.jpg.webp 400w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P19-768x568.jpg.webp 768w"" data-sizes=""(max-width: 1023px) 100vw, 1023px""></p>
<p>پس از یک ساعت تست و کار با سیستم در حالت اورکلاک تا 5 گیگاهرتز، حداکثر دمای ثبت شده در حالت بیکاری به 40 درجه سانتیگراد و در حالت لود به حداکثر 87 درجه سانتیگراد رسید.میزان دمای ایدآل برای پردازنده ی i7 7700K برای استفاده 24/7 می بایستی پایینتر از 90 درجه سانتیگراد باشد و کولر MasterAir MA620P موفق شد در حداکثر اورکلاک 5 گیگاهرتز در سخت ترین تست ها، حداکثر دمای پردازنده را زیر این میزان نگه دارد.بنابراین این کولر به خوبی توانست تست های سخت افزار را با نتیجه ی مطلوب برای خود ثبت کند.</p>
<h3>نتیجه گیری</h3>
<p>محصولی با طراحی منحصر به فرد که می تواند با نورپردازی RGB خود، ضمن زیبایی بخشیدن به نمای Rig گیمینگ شما، از پس سخت ترین تست های فشار بر روی پردازنده به خوبی بر آید. این کولر برای اورکلاک و گیمینگ به صورت 24/7 مناسب است و می توان بر روی آن حساب ویژه ای باز کرد.از نظر تولید صدا و نویز، به لطف فن های خوش ساخت Master Air، عملا در زمره بی صدا ترین ایرکولر ها طبقه بندی می شود که می توان با خیال راحت در اتاق خواب نیز از آن استفاده کرد.نورپردازی 16.7 میلیون رنگی فنهای RGB کولرمستر، ضمن امکان اتصال به کنترلر RGB همراه کولر، می تواند به صورت مستقل به خروجی RGB مادربرد های شرکت های معروف متصل شود.این کولر گواهی سازگاری با Asus Aura Sync، Asrock RGB LED، MSI Mystic Light Sync، و Gigabyte RGB را داراست و به راحتی با اکو سیستم RGB هر نوع مادربردی همسان سازی می شود.این کولر با قیمت حدودی 70 دلار در بازار های جهانی عرضه شده است و به زودی در بازار ایران نیز با گارانتی 2 ساله آواژنگ عرضه خواهد شد.انتظار قیمتی بین 320 تا 380 هزار تومان برای آن می توان در نظر گرفت که امیدواریم تا جای ممکن خوش قیمت تر نیز باشد.</p>
<p><img data-lazyloaded= ""1"" data-placeholder-resp= ""340x454"" src= ""data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzNDAiIGhlaWdodD0iNDU0IiB2aWV3Qm94PSIwIDAgMzQwIDQ1NCI+PHJlY3Qgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgZmlsbD0iI2U5ZWJlZSIvPjwvc3ZnPg=="" loading= ""lazy"" class=""size-full wp-image-100683 aligncenter"" data-src=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P20.jpg.webp"" width=""340"" height=""454"" data-srcset=""https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P20.jpg.webp 340w, https://sakhtafzarmag.com/wp-content/uploads/2018/03/images_items_MA620P20-300x400.jpg.webp 300w"" data-sizes=""(max-width: 340px) 100vw, 340px""></p>
<p>با توجه به موارد فوق، به خصوص کارایی خوب این کولر در تست ها، سایت سخت افزار نشان پیشنهاد شده توسط سخت افزار را به این کولر اعطا می کند.</p>
<span class=""gk-review clearfix"" style=""visibility: visible; transform: translateundefined(8px) scale(0.95); opacity: 0; -webkit-transform: translateundefined(8px) scale(0.95); opacity: 0; ""><span class=""gk-review-sum""><span class=""gk-review-sum-value"" data-final=""0.85""><span>8.5</span><svg viewBox = ""0 0 100 100"" style=""display: block; width: 100%;""><path d = ""M 50,50 m 0,-48 a 48,48 0 1 1 0,96 a 48,48 0 1 1 0,-96"" stroke=""#eee"" stroke-width=""4"" fill-opacity=""0""></path><path d = ""M 50,50 m 0,-48 a 48,48 0 1 1 0,96 a 48,48 0 1 1 0,-96"" stroke=""#07c958"" stroke-width=""4"" fill-opacity=""0"" style=""stroke-dasharray: 301.635, 301.635; stroke-dashoffset: 301.635;""></path></svg></span><span class=""gk-review-sum-label"">امتیاز</span></span><span class=""gk-review-partials""><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.9""><span>9</span><svg viewBox = ""0 0 100 100"" style=""display: block; width: 100%;""><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#eee"" stroke-width=""3"" fill-opacity=""0""></path><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#46a7f5"" stroke-width=""3"" fill-opacity=""0"" style=""stroke-dasharray: 304.844, 304.844; stroke-dashoffset: 304.844;""></path></svg></span><span class=""gk-review-partial-label"">راندمان</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.85""><span>8.5</span><svg viewBox = ""0 0 100 100"" style=""display: block; width: 100%;""><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#eee"" stroke-width=""3"" fill-opacity=""0""></path><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#46a7f5"" stroke-width=""3"" fill-opacity=""0"" style=""stroke-dasharray: 304.844, 304.844; stroke-dashoffset: 304.844;""></path></svg></span><span class=""gk-review-partial-label"">کیفیت قطعات</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.85""><span>8.5</span><svg viewBox = ""0 0 100 100"" style=""display: block; width: 100%;""><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#eee"" stroke-width=""3"" fill-opacity=""0""></path><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#46a7f5"" stroke-width=""3"" fill-opacity=""0"" style=""stroke-dasharray: 304.844, 304.844; stroke-dashoffset: 304.844;""></path></svg></span><span class=""gk-review-partial-label"">طراحی</span></span><span class=""gk-review-partial""><span class=""gk-review-partial-value"" data-final=""0.8""><span>8</span><svg viewBox = ""0 0 100 100"" style=""display: block; width: 100%;""><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#eee"" stroke-width=""3"" fill-opacity=""0""></path><path d = ""M 50,50 m 0,-48.5 a 48.5,48.5 0 1 1 0,97 a 48.5,48.5 0 1 1 0,-97"" stroke=""#46a7f5"" stroke-width=""3"" fill-opacity=""0"" style=""stroke-dasharray: 304.844, 304.844; stroke-dashoffset: 304.844;""></path></svg></span><span class=""gk-review-partial-label"">ارزش خرید</span></span></span></span>

												
													<div class=""pros""><span><i class=""far fa-thumbs-up""></i> مزایا:</span> پشتیبانی حداکثری از سوکت های رایج، مجهز به فن آوری نور پردازی RGB مستقل، قابلیت همسان سازی نورپردازی با انواع مادربرد ها، کارایی عالی و قدرت خنک کنندگی فوق العاده، فن های بی صدا، ارتفاع مناسب، استفاده از خمیر سیلیکن مرغوب</div>

												
												
													<div class=""cons""><span><i class=""far fa-thumbs-down""></i> معایب:</span> مکانیزم نصب کاربر را ناگزیر به خارج کردن مادربرد از کیس می کند</div>

												
												
													<div class=""offer""><span><i class=""fab fa-angellist""></i> پیشنهاد سخت‌افزارمگ:</span> ایرکولر Cooler Master MasterAir MA620P یک کولر پرقدرت و توانمند برای رام کردن داغترین پردازنده هاست.</div>

												
											
										</div>",
                });

                Comments.Add(new Comment()
                {
                    Id = 1,
                    NewsId = 1,
                    SenderName = "محمدمهدی حمزه",
                    SenderMail = "m.hamzeh@yahoo.com",
                    Text = "کامنت اول",
                    CreatedOn = System.DateTime.Now,
                    PublicId = Guid.NewGuid()
                });
                Comments.Add(new Comment()
                {
                    Id = 2,
                    NewsId = 1,
                    SenderName = "محمدمهدی حمزه",
                    SenderMail = "m.hamzeh@yahoo.com",
                    Text = "کامنت دوم",
                    CreatedOn = System.DateTime.Now,
                    PublicId = Guid.NewGuid()
                });
                Comments.Add(new Comment()
                {
                    Id = 3,
                    NewsId = 1,
                    SenderName = "محمدمهدی حمزه",
                    SenderMail = "m.hamzeh@yahoo.com",
                    Text = "کامنت سوم",
                    CreatedOn = System.DateTime.Now,
                    PublicId = Guid.NewGuid()
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
