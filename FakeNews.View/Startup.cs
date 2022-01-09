using FakeNews.Database.Config;
using FakeNews.Database.Tables.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeoTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNews.View
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                ;

            FakeNews.Services.Helper.IocHandler.ResolveUnitOfWorkIoc(services);
            FakeNews.Services.Helper.IocHandler.ResolveServicesIoc(services);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRazorPages();

            services.AddSeoTags(seoInfo =>
            {
                seoInfo.SetSiteInfo(
                    siteTitle: "محمدمهدی حمزه",
                    siteTwitterId: "@MM_Hamzeh",  //optional
                    siteFacebookId: "https://facebook.com/mohammadmahdi_hamzeh",  //optional
                    robots: "index, follow"  //optional
                );

                seoInfo.AddPreload(new Preload("https://site.com/css/site.css"),
                    new Preload("https://site.com/js/app.js"),
                    new Preload("https://site.com/lib/bootstrap/dist/css/bootstrap.rtl.min.css"),
                    new Preload("https://site.com/lib/bootstrap/dist/js/bootstrap.bundle.min.js"));

                seoInfo.SetLocales("fa", new string[] { "fa-IR" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
