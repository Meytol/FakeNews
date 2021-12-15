using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.Services.ModelServices
{
    public interface ICommentService
    {
        Task<ServiceResponse<Comment>> Add(Comment model, int currentUserId);
        Task<ServiceResponse<Comment>> Update(Comment model, int currentUserId);
        Task<ServiceResponse> Delete(int id, int currentUserId);
        Task<ServiceResponse<IList<Comment>>> GetByNewsId(int newsId);
    }

    public class CommentService : ICommentService
    {
        #region Fields and ctor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Comment> _repository;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GenericRepository<Comment>();
        }

        #endregion

        #region Select

        public async Task<ServiceResponse<IList<Comment>>> GetByNewsId(int newsId)
        {
            var comments = await _repository.Where(e => e.NewsId == newsId).OrderBy(e => e.CreatedOn).AsNoTracking().ToListAsync();
            return new ServiceResponse<IList<Comment>>(comments);
        }

        #endregion

        #region Add

        public async Task<ServiceResponse<Comment>> Add(Comment model, int currentUserId)
        {
            var entity = await _repository.Insert(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            return new ServiceResponse<Comment>(entity, HttpStatusCode.OK);
        }

        #endregion

        #region Update

        public async Task<ServiceResponse<Comment>> Update(Comment model, int currentUserId)
        {
            _repository.Update(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            var entity = await _repository.SelectById(model.Id);
            return new ServiceResponse<Comment>(entity, HttpStatusCode.OK);
        }

        #endregion

        #region Delete

        public async Task<ServiceResponse> Delete(int id, int currentUserId)
        {
            _repository.Disable(id, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            return new ServiceResponse();
        }

        #endregion
    }
}
