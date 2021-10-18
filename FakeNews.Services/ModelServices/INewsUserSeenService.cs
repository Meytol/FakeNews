using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using System.Net;
using System.Threading.Tasks;

namespace FakeNews.Services.ModelServices
{
    public interface INewsUserSeenService
    {
        Task<ServiceResponse<NewsUserSeen>> Add(NewsUserSeen model, int currentUserId);
        Task<ServiceResponse<NewsUserSeen>> Update(NewsUserSeen model, int currentUserId);
        Task<ServiceResponse> Delete(int id, int currentUserId);
    }

    public class NewsUserSeenService : INewsUserSeenService
    {
        #region Fields and ctor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<NewsUserSeen> _repository;

        public NewsUserSeenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GenericRepository<NewsUserSeen>();
        }

        #endregion

        #region Select



        #endregion

        #region Add

        public async Task<ServiceResponse<NewsUserSeen>> Add(NewsUserSeen model, int currentUserId)
        {
            var entity = await _repository.Insert(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            return new ServiceResponse<NewsUserSeen>(entity, HttpStatusCode.OK);
        }

        #endregion

        #region Update

        public async Task<ServiceResponse<NewsUserSeen>> Update(NewsUserSeen model, int currentUserId)
        {
            _repository.Update(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            var entity = await _repository.SelectById(model.Id);
            return new ServiceResponse<NewsUserSeen>(entity, HttpStatusCode.OK);
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
