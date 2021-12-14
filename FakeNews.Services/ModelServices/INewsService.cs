using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using Ganss.XSS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FakeNews.Services.ModelServices
{
    public interface INewsService
    {
        Task<ServiceResponse<News>> Add(News model, int currentUserId);
        Task<ServiceResponse<News>> Update(News model, int currentUserId);
        Task<ServiceResponse> Delete(int id, int currentUserId);
        Task<ServiceResponse<List<News>>> GetRecentNews(PagingModel pagingModel);
        Task<ServiceResponse<News>> GetById(int id);
    }

    public class NewsService : INewsService
    {
        #region Fields and ctor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<News> _repository;
        private readonly Lazy<HtmlSanitizer> _htmlSanitizer;
        
        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GenericRepository<News>();
            _htmlSanitizer = new Lazy<HtmlSanitizer>(() => new HtmlSanitizer());
        }

        #endregion

        #region Select

        public async Task<ServiceResponse<List<News>>> GetRecentNews(PagingModel pagingModel)
        {
            var news = await _repository
                .AsQueryable()
                //.Where(e => e.IsPublished)
                .Include(e => e.Author)
                .Include(e => e.Category)
                .OrderByDescending(e => e.PublishDate)
                .Skip(pagingModel.Skip)
                .Take(pagingModel.Take)
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in news)
            {
                item.Body = SanitizeHtml(item.Body);
            }

            return new ServiceResponse<List<News>>(news, HttpStatusCode.OK);
        }

        public async Task<ServiceResponse<News>> GetById(int id)
        {
            var news = await _repository
                .Where(e => e.Id == id)
                .Include(e => e.Author)
                .Include(ie => ie.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            news.Body = SanitizeHtml(news.Body);

            return new ServiceResponse<News>(news, HttpStatusCode.OK);
        }

        #endregion

        #region Add

        public async Task<ServiceResponse<News>> Add(News model, int currentUserId)
        {
            var entity = await _repository.Insert(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            return new ServiceResponse<News>(entity, HttpStatusCode.OK);
        }

        #endregion

        #region Update

        public async Task<ServiceResponse<News>> Update(News model, int currentUserId)
        {
            _repository.Update(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            var entity = await _repository.SelectById(model.Id);
            return new ServiceResponse<News>(entity, HttpStatusCode.OK);
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

        #region private methods

        private string SanitizeHtml(string html)
        {
            return _htmlSanitizer.Value.Sanitize(html);
        }

        #endregion

    }
}
