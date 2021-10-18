using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeNews.Services.ModelServices;
using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FakeNews.View.Pages
{
    public class NewsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;

        public NewsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _newsService = new NewsService(_unitOfWork);

        }


        public async Task OnGetAsync(int id = 0)
        {
            if (id == 0)
            {
                RedirectToPage(pageName: "Index");
                return;
            }

            var desiredNewsResult = await _newsService.GetById(id);

            if (desiredNewsResult.IsSuccessful is false)
            {
                RedirectToPage(pageName: "Index");
                return;
            }

            DesiredNews = desiredNewsResult.Data;
        }

        public News DesiredNews { get; set; }
    }
}
