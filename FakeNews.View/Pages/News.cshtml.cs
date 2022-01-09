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
using Microsoft.AspNetCore.Antiforgery;
using System.Text.RegularExpressions;

namespace FakeNews.View.Pages
{
    public class NewsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;
        private readonly IAntiforgery _antiForgeryHandler;
        private readonly ICommentService _commentService;
        private readonly ILogService _logService;

        public NewsModel(IUnitOfWork unitOfWork, IAntiforgery antiForgeryHandler)
        {
            _unitOfWork = unitOfWork;
            _newsService = new NewsService(_unitOfWork);
            _commentService = new CommentService(_unitOfWork);
            _logService = new LogService(_unitOfWork);
            _antiForgeryHandler = antiForgeryHandler;
        }


        public async Task OnGetAsync(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    RedirectToPage(pageName: "Index");
                    return;
                }

                await FillPageModelData(newsId: id);
            }
            catch (Exception e)
            {
                await _logService.AddError(e);
                RedirectToPage(pageName: "Index");
            }      
        }

        public async Task OnPostAsync(string senderNameText, string senderMailText, string newCommentText, int newsId)
        {
            try
            {
                await _antiForgeryHandler.ValidateRequestAsync(HttpContext);

                var senderName = senderNameText.Trim();
                var senderMail = senderMailText.Trim();
                var comment = newCommentText.Trim();

                if (string.IsNullOrEmpty(senderName) || senderName.Length > 50)
                {
                    RedirectToPage(pageName: "Index");
                    return;
                }

                if (string.IsNullOrEmpty(senderMail) || senderMail.Length > 50 || Regex.IsMatch(senderMail, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$") is false)
                {
                    RedirectToPage(pageName: "Index");
                    return;
                }

                if (string.IsNullOrEmpty(comment) || comment.Length > 500)
                {
                    RedirectToPage(pageName: "Index");
                    return;
                }

                await _commentService.Add(new Comment()
                {
                    Text = newCommentText,
                    SenderName = senderNameText,
                    SenderMail = senderMailText,
                    NewsId = newsId
                },
                0);

                await FillPageModelData(newsId);
            }
            catch (Exception e)
            {
                await _logService.AddError(e);
                RedirectToPage(pageName: "Index");
            }
        }

        private async Task FillPageModelData(int newsId)
        {
            try
            {
                var desiredNewsResult = await _newsService.GetById(newsId);

                if (desiredNewsResult.IsSuccessful is false)
                {
                    RedirectToPage(pageName: "Index");
                    return;
                }

                DesiredNews = desiredNewsResult.Data;
                Comments = desiredNewsResult.Data.Comments.ToList();
            }
            catch (Exception e)
            {
                await _logService.AddError(e);
                RedirectToPage(pageName: "Index");
            }
        }

        public News DesiredNews { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}
