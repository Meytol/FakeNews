using FakeNews.Services.ModelServices;
using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNews.View.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _newsService = new NewsService(_unitOfWork);
            _categoryService = new CategoryService(_unitOfWork);
        }

        public async Task OnGet()
        {
            var getNewsResponse = await _newsService.GetRecentNews(new PagingModel() { Page = 1, Take = 6, OrderByAsc = false });

            if (getNewsResponse.IsSuccessful is true)
            {
                RecentNews = getNewsResponse.Data;
            }

            ServiceResponse<IEnumerable<Category>> getCategoriesResponse = await _categoryService.GetCategoriesTreeView();

            if (getCategoriesResponse.IsSuccessful is true)
            {
                TreeviewCategories = getCategoriesResponse.Data;
            }
        }

        public IEnumerable<News> RecentNews { get; set; } = new List<News>();
        public IEnumerable<Category> TreeviewCategories { get; set; } = new List<Category>();

    }
}
