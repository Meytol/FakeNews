using FakeNews.Database.Tables;
using FakeNews.Services.ModelServices;
using FakeNews.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNews.View.Pages.Shared
{
    [ViewComponent]
    public class RecentNewsByCategory : ViewComponent
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        public RecentNewsByCategory(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _categoryService = new CategoryService(_unitOfWork);
        }

        public async Task<IViewComponentResult> InvokeAsync(int catId)
        {
            if (catId == default)
            {
                return null;
            }

            CatId = catId;
            var categoryResponse = await _categoryService.GetNewsByCategoryId(catId, true);

            if (categoryResponse.IsSuccessful is false || categoryResponse.Data is null)
            {
                return null;
            }

            RecentNews = categoryResponse.Data;
            CurrentCategory =  categoryResponse.Data.FirstOrDefault().Category;

            return View(viewName: "Default", model: this);
        }

        public int CatId { get; set; } = 0;
        public IList<News> RecentNews { get; set; } = new List<News>();
        public Category CurrentCategory { get; set; } = new Category();
    }
}
