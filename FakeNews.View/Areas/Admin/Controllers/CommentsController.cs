using FakeNews.Common.Database.Enums;
using FakeNews.Database.Tables;
using FakeNews.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNews.View.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericRepository<Comment> _commentsRepo;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _commentsRepo = _unitOfWork.GenericRepository<Comment>();
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _commentsRepo
                .AsQueryable()
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreatedOn)
                .AsNoTracking()
                .ToListAsync();

            return View(model: comments);
        }

        [HttpPatch]
        public async Task ApproveComment( int commentId)
        {
            var comment = await _commentsRepo.SelectById(commentId);

            if (comment is null)
            {
                throw new Exception("کامنت مورد نظر پیدا نشد");
            }

            if (comment.Status == CommentStatus.Accepted)
            {
                return;
            }

            comment.Status = CommentStatus.Accepted;
            await _unitOfWork.Save();
        }

        [HttpPatch]
        public async Task RejectComment( int commentId)
        {
            var comment = await _commentsRepo.SelectById(commentId);

            if (comment is null)
            {
                throw new Exception("کامنت مورد نظر پیدا نشد");
            }

            if (comment.Status == CommentStatus.Rejected)
            {
                return;
            }

            comment.Status = CommentStatus.Rejected;
            await _unitOfWork.Save();
        }

        [HttpDelete]
        public async Task Delete(int commentId)
        {
            var comment = await _commentsRepo.SelectById(commentId);

            if (comment is null)
            {
                throw new Exception("کامنت مورد نظر پیدا نشد");
            }

            _commentsRepo.Delete(comment);
            await _unitOfWork.Save();
        }
    }
}
