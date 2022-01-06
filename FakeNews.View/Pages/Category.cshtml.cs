using FakeNews.Database.Tables;
using FakeNews.Services.ModelServices;
using FakeNews.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNews.View.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryModel> _logger;

        public CategoryModel(ILogger<CategoryModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _categoryService = new CategoryService(_unitOfWork);
        }

        public async Task OnGet(int catId)
        {
            if (catId == 0)
            {
                RedirectToPage(pageName: "Index");
            }

            CurrentCategory = (CategoryDto)((await _categoryService.Get(catId)).Data ?? new CategoryDto());
            ChildCategories = (await _categoryService.GetCategoriesTreeView(catId)).Data.ToList() ?? new List<CategoryDto>() ;
            CategoryNews = (await _categoryService.GetNewsByCategoryId(catId)).Data ?? new List<News>();
        }

        public CategoryDto CurrentCategory { get; set; }
        public IList<CategoryDto> ChildCategories { get; set; }
        public IList<News> CategoryNews { get; set; }
    }
}
