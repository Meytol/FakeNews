﻿using FakeNews.Database.Tables;
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
        const string DbConnection = @"Data Source=185.55.224.120;Initial Catalog=mmhamze1_StaionGamesDb;Persist Security Info=True;User ID=mmhamze1_DbAdmin;Password=Leomleom19(&;";
#else
        const string DbConnection = @"Data Source=127.0.0.1;Initial Catalog=mmhamze1_StaionGamesDb;Persist Security Info=True;User ID=mmhamze1_DbAdmin;Password=Leomleom19(&;";
#endif

        public ApplicationDbContext()
        {
            //var options = new DbContextOptions<ApplicationDbContext>();


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(DbConnection);
            optionsBuilder.UseInMemoryDatabase(DbConnection);

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
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsUserSeen> NewsUserSeens { get; set; }

        #endregion

        public void Seed()
        {
            FeedSeedData();
            base.SaveChanges();
        }



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
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var rowsAffected = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await GenerateSeedDataFileAsync();
            return rowsAffected;
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
                    NewsCategories.AddRange(data.NewsCategories);
                    
                    data = null;
                    System.GC.Collect();
                    isDataFound = true;
                }
            }

            if (isDataFound is false)
            {
                Users.Add(new User()
                {
                    Email = "m.hamzeh@test.com",
                    UserName = "meytol",
                    CreatedOn = System.DateTime.Now,
                    IsDeleted = false,
                    LockoutEnabled = false,
                    PublicId = System.Guid.Parse("57D8F436-99E8-43A3-8751-8EFCD0B6B3AB"),
                    NormalizedEmail = "m.hamzeh@test.com".Normalize(),
                    NormalizedUserName = "meytol".Normalize(),
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
                    TitleEn = "Main",
                    TitleFa = "اصلی",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1
                });
                Categories.Add(new Category()
                {
                    Id = 2,
                    TitleEn = "No2",
                    TitleFa = "شماره 2",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1
                });
                Categories.Add(new Category()
                {
                    Id = 3,
                    TitleEn = "No3",
                    TitleFa = "شماره 3",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });
                Categories.Add(new Category()
                {
                    Id = 4,
                    TitleEn = "No4",
                    TitleFa = "شماره 4",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });
                Categories.Add(new Category()
                {
                    Id = 5,
                    TitleEn = "No5",
                    TitleFa = "شماره 5",
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    ParentCategoryId = 2
                });

                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 1,
                    Keywords = "هيات دولت - وزارت راه و شهرسازي - مدیر عامل شرکت راه‌آهن",
                    MainImageUri = "https://cdn.isna.ir/d/2021/07/28/3/61990238.jpg",
                    SeenCount = 1,
                    PublicId = System.Guid.Parse("E2955EF9-9B01-4D1D-BA67-5C88CFA9DC21"),
                    HeadLine = "از سوی معاون اول رئیس جمهور انجام شد",
                    Id = 1,
                    Body = @"معاون اول رئیس جمهور مصوبه مربوط به انتصاب مدیرعامل جدید شرکت راه آهن جمهوری اسلامی ایران را ابلاغ کرد.

به گزارش ایسنا به نقل از پایگاه اطلاع رسانی دفتر هیئت دولت،‌ در جلسه هیئت وزیران مورخ ۲۱ مهر ۱۴۰۰، سید میعاد صالحی به پیشنهاد وزارت راه و شهرسازی و تصویب دولت، به عنوان مدیرعامل شرکت راه آهن جمهوری اسلامی ایران تعیین شد.

سید میعاد صالحی فارغ‌التحصیل دکتری مهندسی مکانیک دانشگاه صنعتی شریف و عضو هیأت علمی دانشگاه علم و صنعت ایران و همچنین رتبه اول جشنواره خوارزمی و عضو بنیاد ملی نخبگان بوده و مدیر عامل صندوق بازنشستگی کشوری، عضو هیأت عامل صندوق نوآوری و شکوفایی، مشاور وزیر و رییس طرح های صنایع نوین وزارت صمت، رئیس کمیته صنعت و معدن دبیرخانه مجمع تشخیص مصلحت نظام و مشاور معاون علمی و فناوری رئیس‌جمهور از جمله سوابق وی می‌باشد.

صالحی دارای سوابق علمی و تخصصی در حوزه حمل و نقل ریلی بوده که از آن جمله می توان به طرح برگزیده جشنواره خوارزمی و چاپ و ارائه چندین مقاله در مجلات آی اس آی و کنفرانس های معتبر بین المللی در حوزه ریلی و همچنین سابقه فعالیت در شرکت های ریلی مسافری و باری اشاره کرد.",
                    Title = "ابلاغ مصوبه انتصاب مدیرعامل شرکت راه آهن",
                    PublishDate = new System.DateTime(year: 2021, month: 7, day: 28)
                });
                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 2,
                    Keywords = "سهیل مهدی - لیگ برتر فوتبال ایران - کمیته مسابقات لیگ برتر فوتبال ایران",
                    MainImageUri = "https://cdn.isna.ir/d/2021/09/28/3/62043290.jpg",
                    SeenCount = 2,
                    PublicId = System.Guid.Parse("9A3AFC10-43A1-484C-A6FA-5C67D3DE76E3"),
                    HeadLine = "در گفت‌وگو با ایسنا اعلام شد:",
                    Id = 2,
                    Body = @"رییس کمیته برگزاری مسابقات لیگ برتر فوتبال ایران اعلام کرد که رقابت‌های بیست و یکمین دوره لیگ برتر فوتبال ایران قطعا بدون حضور تماشاگران آغاز می‌شود.

به گزارش ایسنا، از اواخر سال ۹۸ بود که به دلیل شیوع ویروس کرونا، ممنوعیت حضور تماشاگران ایرانی روی سکوی ورزشگاه ها و سالن های ورزشی رقم خورد و این ممنوعیت تاکنون در مورد ورزشگاه های سالنی و غیرمسقف در ایران ادامه دارد و هرچند به نظر می‌رسید ۲۰ مهر با حضور تماشاگران در دیدار تیم‌های ملی فوتبال ایران و کره‌جنوبی از سری رقابت‌های مرحله نهایی انتخابی جام جهانی در ورزشگاه آزادی شاهد پایان این مساله باشد و پس از آن حضور تماشاگران در ورزشگاه‌های فوتبال را همزمان با شروع بیست و یکمین دوره لیگ برتر فوتبال ایران رقم بخورد اما در نهایت چنین اتفاقی رخ نداد.

سهیل مهدی، رییس کمیته برگزاری مسابقات لیگ برتر فوتبال ایران در گفت‌وگو با ایسنا، با بیان اینکه بیست و یکمین دوره لیگ برتر فوتبال ایران قطعا بدون حضور تماشاگران شروع می‌شود، گفت: در تلاش هستیم تا در صورت بهبود اوضاع و صدور مجوز از سوی مراجع ذی‌ربط شاهد حضور مجدد تماشاگران در ورزشگاه‌های کشور باشیم، هرچند در این رابطه زمان دقیقی برای بازگشت تماشاگران مشخص نیست.

به گزارش ایسنا، ‌طبق اعلام سازمان لیگ فوتبال ایران، مسابقات لیگ برتر فوتبال فصل ۱۴۰۱-۱۴۰۰  ۲۷ مهر ۱۴۰۰ شروع می‌شود.,",
                    Title = "شروع قطعی لیگ‌ برتر فوتبال بدون حضور تماشاگران ",
                    PublishDate = new System.DateTime(year: 2021, month: 9, day: 28)
                });
                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 3,
                    Keywords = "فرونشست زمین",
                    MainImageUri = "https://cdn.isna.ir/d/2021/05/08/3/61918067.jpg",
                    SeenCount = 3,
                    PublicId = System.Guid.Parse("B4D9E6B1-A89B-48AE-8885-340054FDE155"),
                    HeadLine = "تست 3",
                    Id = 3,
                    Body = @"رییس مرکز تحقیقات راه، مسکن و شهرسازی با تاکید بر اینکه فرونشست در کشور به مرحله هشدار رسید بر ایجاد محدودیت‌ برای برداشت آب‌های زیرزمینی در محدوده های فرودگاهی و خطوط راه آهن خبر داد.

به گزارش ایسنا، محمد شکرچی‌زاده رییس مرکز تحقیقات راه، مسکن و شهرسازی در خصوص فرونشست زمین و مطالعات و راهکارهای ارایه شده توسط کارشناسان توضیحاتی ارایه کرد.

رییس مرکز تحقیقات راه، مسکن و شهرسازی با یادآوری این مطلب که فرونشست زمین در کشور به مرحله هشدار رسید، گفت: با توجه به اینکه یک سوم دشت‌های کشور تحت‌تاثیر فرونشست زمین قرار دارند حل این  مشکل و یا کند کردن روند آن باید در برنامه‌ها و سیاست‌های دولتمردان قرار بگیرد.

شکرچی‌زاده تغییر الگوهای کشاورزی و همچنین تغییر در برداشت از آب‌های زیرزمینی را راهکارهایی برای حل معضل فرونشست و روشی برای کند کردن این مشکل عنوان کرد و افزود: وزارت راه و شهرسازی به دلیل حوزه مسئولیت گسترده خود و انجام پروژه‌های مختلف در فضای سرزمینی، بیشتر از سایر دستگاه‌ها، تحت‌تاثیر فرونشست زمین قرار دارد و به طور ویژه تاثیر فرونشست در فرودگاه ها و خطوط راه آهن قابل مطالعه و ارزیابی است.

به گفته وی، انجام مطالعات فرونشست به شکل ویژه در ۱۰ فرودگاه اصفهان، مشهد، تبریز، اردبیل، یزد، ایلام، شیراز، کرمان، کرمانشاه و بم از سال ۹۸ به کارفرمایی شرکت فرودگاه‌ها در دستور کار مرکز قرار گرفت. بررسی نتایج فرونشست در ۱۰ فرودگاه به سرانجام رسید و گزارش آن هفته گذشته به کارفرما تحویل شد. بر اساس بررسی‌های مرکز تحقیقات، هم‌اکنون وضعیت اغلب این فرودگاه‌ها نگران کننده نیست. اما از آنجاییکه محدوده فرونشستی رو به گسترش است می‌تواند به فضای فرودگاهی نزدیک بشود. بنابراین با اینکه فرودگاه ها در شرایط بحرانی نیستند و استفاده از آنها مخاطره ای را به همراه نخواهد داشت اما لازم است برای فرودگاه‌ها حریم‌هایی را قائل شویم. برداشت آب‌های زیرزمینی و کشاورزی در حریم فرودگاهی باید مدیریت هوشمندانه باشد تا بتوان موضوع فرونشست را حتی الامکان به تاخیر انداخت و یا روند آن را کند کرد.

شکرچی‌زاده از تدوین و تنظیم درخواستی برای مدیریت برداشت آب‌های زیرزمینی و کشاورزی در حریم فرودگاهی خبر داد و افزود: مقرر شد تا شرکت فرودگاه‌ها با همراهی مرکز تحقیقات راه، مسکن و شهرسازی، درخواستی را تهیه و تنظیم و به وزیر راه ارایه بدهد و درخواست با تصویب در هیات وزیران و سازمان مدیریت بحران کشور عملیاتی و اجریی شود. بر اساس مصوبه هیات وزیران،   سازمان آب و وزارت نیرو، محدودیت هایی را برای برداشت آب های زیرزمینی در پهنه فرودگاه ها و در حریم فرودگاه ها انجام خواهند داد. الزام هنوز ایجاد نشده است و در تلاش هستیم تا با تصویب هیات وزیران و همکاری سازمان مدیریت بحران اینکار به الزام برسد. با توجه به برداشت های بی رویه، این معضل مشکلات جدی را ایجاد خواهد کرد.

رییس مرکز تحقیقات راه، مسکن و شهرسازی با اشاره به اینکه در مجموع شرایط فرونشست برای ۱۰ فرودگاه مطالعه شده، بحرانی نیست، تاکید کرد: وضعیت فرونشست به گونه‌ای است که در فرودگاه‌ها، ظرفیت و استعداد اینکه در آینده مشکل جدی ایجاد شود، وجود دارد.

وی از انجام مطالعه در تعداد دیگری از فرودگاه‌های کشور خبر داد و گفت: مرکز تحقیقات، هم‌اکنون مطالعاتی برای راه آهن تهران- مشهد انجام شده و توصیه‌هایی برای کاهش مخاطرات فرونشست در خطوط راه‌آهن برای شرکت راه آهن ارسال شده است. همچنین، برای تعدادی دیگر از خطوط راه آهن در حال اجرای مطالعاتی با شرکت راه آهن هستیم و تعامل خوبی در این زمینه وجود دارد.

وی تاکید کرد: شرکت راه آهن نیز به کاهش مخاطرات فرونشست در خطوط ریلی توجه دارد و امیدواریم با تعامل ایجاد شده مشکلات حل شوند.

شکرچی زاده تصریح کرد: برای طراحی جدید خطوط راه آهن توصیه هایی داریم که حتی الامکان در مناطقی باشد که در زون کم فرونشست باشد.

",
                    Title = "فرونشست در کشور به مرحله هشدار رسید",
                    PublishDate = new System.DateTime(year: 2021, month: 5, day: 3)
                });
                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 4,
                    Keywords = "کرونا در ایران - همه_باهم_علیه_کرونا - من ماسک میزنم - طرح فاصله‌گذاری اجتماعی",
                    MainImageUri = "https://cdn.isna.ir/d/off/qazvin/2021/09/28/3/62043268.jpg",
                    SeenCount = 4,
                    PublicId = System.Guid.Parse("8BFE837A-4FF1-4D61-9EAE-35E730B68E12"),
                    HeadLine = "بنابر اعلام وزارت بهداشت در شبانه روز گذشته متاسفانه ۱۸۱ بیمار کووید۱۹ در کشور به دلیل این بیماری جان خود را از دست دادند.",
                    Id = 4,
                    Body = @"بنابر اعلام وزارت بهداشت در شبانه روز گذشته متاسفانه ۱۸۱ بیمار کووید۱۹ در کشور به دلیل این بیماری جان خود را از دست دادند.

به گزارش ایسنا، بنابر اعلام مرکز روابط عمومی و اطلاع رسانی وزارت بهداشت، از دیروز تا امروز ۲۴ مهرماه ۱۴۰۰ و بر اساس معیارهای قطعی تشخیصی، ۷ هزار و ۵۱۵ بیمار جدید مبتلا به کووید۱۹ در کشور شناسایی شد که یک هزار و ۱۹۷ نفر از آنها بستری شدند.

مجموع بیماران کووید۱۹ در کشور به ۵ میلیون و ۷۷۳ هزار و ۴۱۹ نفر رسید.

متاسفانه در طول ۲۴ ساعت گذشته، ۱۸۱ بیمار کووید۱۹ جان خود را از دست دادند و مجموع جان باختگان این بیماری به ۱۲۳ هزار و ۸۷۶ نفر رسید.

خوشبختانه تا کنون ۵ میلیون و ۳۰۹ هزار و ۹۹۲ نفر از بیماران، بهبود یافته و یا از بیمارستان‌ها ترخیص شده‌اند.

۴ هزار و ۸۴۰ نفر از بیماران مبتلا به کووید۱۹ در بخش‌های مراقبت‌های ویژه بیمارستان‌ها تحت مراقبت قرار دارند.

تا کنون ۳۳ میلیون و ۸۹۸ هزار و ۹۷۸ آزمایش تشخیص کووید۱۹ در کشور انجام شده است.

در حال حاضر ۹ شهر کشور در وضعیت قرمز، ۱۰۶ شهر در وضعیت نارنجی، ۲۲۸ شهر در وضعیت زرد و ۱۰۵ شهر در وضعیت آبی قرار دارند.",
                    Title = "۱۸۱فوتی و ۷۵۱۵ ابتلای جدید کرونا در کشور",
                    PublishDate = new System.DateTime(year: 2021, month: 9, day: 28)
                });
                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 5,
                    Keywords = "رئيس جمهور - سيدابراهيم رئيسي - ستاد ملی کرونا - بازگشایی مدارس - واکسیناسیون سراسری کرونا",
                    MainImageUri = "https://cdn.isna.ir/d/2021/10/16/3/62059665.jpg",
                    SeenCount = 5,
                    PublicId = System.Guid.Parse("9857F570-E772-4323-BDE9-4234C2DDC4C0"),
                    HeadLine = "رئیس جمهور در ستاد ملی مقابله با کرونا:",
                    Id = 5,
                    Body = @"رئیس جمهور با تاکید بر اینکه واکسیناسیون عمومی باید با قوت ادامه یابد، گفت: دانشگاهیان و صاحبان تریبون در ترغیب و تشویق مردم به رعایت شیوه‌نامه‌های بهداشتی و واکسینه شدن در برابر ویروس کرونا نقش بسزایی دارند.

به گزارش ایسنا،  آیت‌الله سید ابراهیم رئیسی روز شنبه در جلسه ستاد ملی مقابله با کرونا با اشاره به دستاوردهای مهم در تولید و واردات واکسن کرونا، گفت: وزارت بهداشت، درمان و آموزش پزشکی باید از واکسن تولید شرکت‌های داخلی حمایت کند تا محصولات‌ آنان با کیفیت لازم به بازار عرضه شود.

رئیس جمهور با قدردانی از کشورهایی که در ارسال واکسن به ایران همکاری می‌کنند، گفت: از همه کشورهایی که واکسن ارسال کرده‌اند به‌ویژه از کشور چین صمیمانه قدردانی می‌کنم و معتقدم اینگونه همکاری‌های انسان‌دوستانه همواره باید بین کشورهای دوست و منطقه ادامه پیدا کند.

 رئیسی با بیان اینکه ادامه راه مقابله با بیماری کرونا نیازمند تدوین نقشه‌راه جامع است، گفت: در این نقشه‌راه باید همه موارد بهداشتی، اجتماعی، اقتصادی و درمانی به طوردقیق لحاظ شود تا با روشن کردن وضع آینده، ما را از اقدامات انفعالی در برابر شیوع بیماری خارج کند.

رئیس جمهور در ادامه با تاکید بر اینکه واکسیناسیون از راه‌های پیشگیری است اما کافی نیست، گفت: عادی‌انگاری می‌تواند تبعات خطرناکی داشته باشد بنابراین از مردم تقاضا می‌کنم در کنار واکسیناسیون، شیوه‌نامه‌های بهداشتی را هم با دقت رعایت کنند.

 رئیسی با اشاره به لزوم رعایت کامل اصول بهداشتی در روند بازگشایی مدارس، دانشگاهها، ورزشگاهها و نماز جمعه، گفت: مسئولان مربوطه در روند این بازگشایی‌ها باید برنامه‌ریزی‌ها و نظارت‌های لازم را انجام دهند تا سلامت مردم به خطر نیفتد.

وی با تاکید بر اهمیت اقناع‌ افکار عمومی و اطلاع رسانی به موقع و دقیق در زمینه بیماری کرونا، گفت: سیاست ما در این مورد اقناعی است نه اجباری و بر این باور هستیم که با تبیین و روشنگری می‌توان اعتماد مردم را جلب و آنان را همراه کرد.

وی همچنین پاسخگویی به شبهات مطرح در فضای مجازی درباره درمان کرونا را ضروری دانست و گفت: دانشگاه‌های علوم پزشکی باید بخشی را برای پاسخ علمی و منطقی به شبهاتی که مطرح می‌شود، اختصاص دهند.

رئیس جمهور با تاکید مجدد بر کنترل دقیق ورود و خروج در مرزها و فرودگاه‌های بین المللی کشور، اظهار داشت: استانداران استان‌های مرزی و مسئولان مربوطه باید ورود و خروج از کشور را به دقت کنترل کنند و ورود به کشور حتما با واکسیناسیون همراه باشد.

 رئیسی با اشاره به اینکه کاهش محدودیت‌های کرونایی باید با ضوابط همراه باشد، گفت: سیر نزولی بیماری با اجرای طرح شهید سلیمانی، آموزش عمومی و رعایت شیوه‌نامه‌های بهداشتی از شرایط کاهش محدودیت‌ها است که باید به آنها توجه شود.

رئیس جمهور توجه به آموزش عمومی را در کنار اطلاع‌رسانی دقیق و مستمر ضروری دانست و گفت: دانشگاهیان و صاحبان تریبون نقش مهمی در این زمینه دارند و در زمینه اثربخشی واکسن، لازم است مراکز علمی و دانشگاهی به طور مستمر رصد علمی و پژوهشی کنند؛ چرا که اقدامات آینده منوط به نتایج این پژوهش‌ها است.

وی همچنین با قدردانی از همکاری‌های گسترده و سازنده ناجا با ستاد ملی مقابله با کرونا گفت: نیروی انتظامی اقدامات انتظامی، امنیتی و اجتماعی سازنده‌ای در سراسر کشور دارد و همکاری‌های مفیدی با ستاد ملی مقابله با کرونا دارد که از تلاش همه این عزیزان قدردانی می‌کنم.",
                    Title = "بازگشایی‌ها با برنامه‌ریزی و نظارت دقیق باشد",
                    PublishDate = new System.DateTime(year: 2021, month: 10, day: 16)
                });
                News.Add(new Tables.News()
                {
                    AuthorId = 1,
                    CreatedOn = System.DateTime.Now,
                    CreatorId = 6,
                    Keywords = "هشدار هواشناسی - سرما - هشدار کشاورزی",
                    MainImageUri = "https://cdn.isna.ir/d/2020/12/08/3/61798554.jpg",
                    SeenCount = 6,
                    PublicId = System.Guid.Parse("9857F570-E772-4323-BDE9-4234C2DDC4C0"),
                    HeadLine = "سازمان هواشناسی هشدار داد",
                    Id = 6,
                    Body = @"سازمان هواشناسی نسبت به کاهش دمای هشت استان به زیر صفر طی دوشنبه و سه شنبه (۲۶ و ۲۷ مهرماه) هشدار داد و توصیه‌هایی را به کشاورزان، دامداران، باغداران و ... ارایه کرد.

به گزارش ایسنا سازمان هواشناسی با صدور هشدار زردرنگ آورده است: نفوذ هوای سرد به مناطق غربی کشور، کاهش دما به زیر صفر و احتمال یخ‌زدگی محصولات کشاورزی را دوشنبه(۲۶ مهرماه) در استان‌های کردستان، آذربایجان غربی، آذربایجان شرقی، زنجان، همدان، ارتفاعات استان‌های کرمانشاه و لرستان و سه شنبه(۲۷ مهرماه) در استان چهارمحال و بختیاری در پی دارد بنابراین وارد شدن خسارت به محصولات کشاورزی دور از انتظار نیست.

سارمان هواشناسی به صاحبان مزارع و باغات تسریع در برداشت محصولات باغی و زراعی، محلول‌پاشی مزارع کلزا با کودهای اسیدآمینه و ضد استرس، روشن کردن آتش و ایجاد مه دود در باغات در ساعات نزدیک صبح و بامداد را توصیه می‌کند و از آنان می‌خواهد که از مبارزه شیمیایی در مناطقی که احتمالا دما به زیر صفر می‌رسد، خودداری کنند، از کودهای ازته همچون نیترات آمونیوم هنگام اوج سرما به دلیل آبکی بافت درختان استفاده نکنند و از انجام هرس سبز درختان خودداری کنند.

صاحبان سالن‌های پرورش قارچ، گلخانه و انبارنیز تهویه، تنظیم دما و تامین سوخت سالن‌ها با توجه به کاهش دما و بسته نگهداشتن دریچه‌های گلخانه‌ها هنگام کاهش دما، سالن‌های مرغداری و دامداری تهویه، تنظیم دما و تامین سوخت سالن‌ها با توجه به کاهش دما، تهیه خوراک پر انرژی برای دام‌ها با توجه به کاهش دما و جلوگیری از نفوذ هوای سرد به داخل سالن‌ها و صاحبان استخرهای پرورش ماهی خودداری از کوددهی به استخرهای پرورش ماهی گرم‌آبی و کاهش غذادهی به میزان نصف نرمال به دلیل کاهش دما را در دستور کار قرار دهند.

سازمان هواشناسی همچنین به عشایر و زنبورداران نسبت به خودداری از عبور در ارتفاعات همچنین عایق‌بندی و محافظت از کندوها در مقابل کاهش دما و وزش باد و به صاحبان ماشین آلات و ادوات کشاورزی نسبت به تخلیه آب و جمع‏‌آوری لوله‌ها، اتصالات و تجهیزات سیستم آبیاری تحت فشار در مناطق سردسیر و استفاده از ضد یخ در رادیاتور ماشین‏‌آلات کشاورزی را توصیه می‌کند.

به گزارش ایسنا هشدار زرد رنگ به معنای این است که پدیده‌ای جوی رخ خواهد داد که ممکن است در سفرها و انجام کارهای روزمره اختلالاتی را ایجاد کند.این هشدار برای آگاهی مردم صادر می‌شود تا بتوانند آمادگی لازم را برای مواجهه با پدیده‌ای جوی داشته باشند که از حالت معمول کمی شدت بیشتری دارد.از سوی دیگر مسئولان نیز در جریان این هشدارها قرار می‌گیرند تا اگر لازم باشد تمهیداتی را بیندیشند.",
                    Title = "کاهش دمای ۸ استان به زیر صفر/ از محصولات کشاورزی محافظت کنید",
                    PublishDate = new System.DateTime(year: 2020, month: 8, day: 3)
                });

                NewsCategories.Add(new NewsCategory()
                {
                    Id = 1,
                    CategoryId = 1,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("19DCECFA-34A3-4A38-8D61-1E42C8F39EF3"),
                    NewsId = 1
                });
                NewsCategories.Add(new NewsCategory()
                {
                    Id = 2,
                    CategoryId = 1,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("36BFB6B4-EAED-4C66-A4F3-5EAC965BE6DC"),
                    NewsId = 2
                });
                NewsCategories.Add(new NewsCategory()
                {
                    Id = 3,
                    CategoryId = 1,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("EF5137E9-6626-4E29-AB1C-4AC9C1A4E9BA"),
                    NewsId = 2
                });
                NewsCategories.Add(new NewsCategory()
                {
                    Id = 4,
                    CategoryId = 1,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("9CF3D0E8-F529-4BB8-BA89-9758FCEDBD5A"),
                    NewsId = 3
                });
                NewsCategories.Add(new NewsCategory()
                {
                    Id = 5,
                    CategoryId = 4,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("1442776A-BFE9-4C39-83CC-8BB34A5C03F3"),
                    NewsId = 4
                });
                NewsCategories.Add(new NewsCategory()
                {
                    Id = 6,
                    CategoryId = 1,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("9CF3D0E8-F529-4BB8-BA89-9758FCEDBD5A"),
                    NewsId = 5
                });
                NewsCategories.Add(new NewsCategory()
                {
                    Id = 7,
                    CategoryId = 1,
                    CreatedOn = System.DateTime.Now,
                    PublicId = System.Guid.Parse("CABDF7A1-5B3F-4DE9-ADB6-44347BA58445"),
                    NewsId = 6
                });
            }
        }

    }
}
