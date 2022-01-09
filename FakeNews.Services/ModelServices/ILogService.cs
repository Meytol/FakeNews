using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using FakeNews.Services.Repository;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FakeNews.Services.ModelServices
{
    public interface ILogService
    {
        Task<ServiceResponse<Log>> Add(Log model, int currentUserId);
        Task<ServiceResponse> Delete(int id);
        Task AddError(Exception ex, int currentUserId = 0);
    }

    public class LogService : ILogService
    {
        #region Fields and ctor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Log> _repository;

        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GenericRepository<Log>();
        }

        #endregion

        public async Task<ServiceResponse<Log>> Add(Log model, int currentUserId)
        {
            await _repository.Insert(model, currentUserId);
        }

        public async Task AddError(Exception ex, int currentUserId = 0)
        {
            var model = new Log()
            {
                Title = ex.ToString(),
                Description = ex.Message,
                IsError = true,
                FullDetail = JsonConvert.SerializeObject(ex)
            };

            await _repository.Insert(model, currentUserId);
        }

        public async Task<ServiceResponse> Delete(int id)
        {
            _repository.Delete(id);
            var rowsAffected = await _unitOfWork.Save();
            return new ServiceResponse();
        }
    }
}
