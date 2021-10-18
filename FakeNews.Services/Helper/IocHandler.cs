using FakeNews.Services.ModelServices;
using FakeNews.Services.Repository;
using Ganss.XSS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNews.Services.Helper
{
    public static class IocHandler
    {
        public static void ResolveUnitOfWorkIoc(IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<INewsCategoryService, NewsCategoryService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsUserSeenService, NewsUserSeenService>();
            services.AddSingleton<HtmlSanitizer>(new HtmlSanitizer());
        }

        public static void ResolveServicesIoc(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
