using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FakeNews.Services.ModelServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<Category>> Add(Category model, int currentUserId);
        Task<ServiceResponse<Category>> Update(Category model, int currentUserId);
        Task<ServiceResponse> Delete(int id, int currentUserId);
        Task<ServiceResponse<IEnumerable<Category>>> GetCategoriesTreeView();
    }

    public class CategoryService : ICategoryService
    {
        #region Fields and ctor

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Category> _repository;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GenericRepository<Category>();
        }

        #endregion

        #region Select

        public async Task<ServiceResponse<IEnumerable<Category>>> GetCategoriesTreeView()
        {
            var allCategories = await _repository.SelectAll();

            var categoriesTreeView = new List<Category>();

            foreach (var cat1 in allCategories.Where(i => i.ParentCategoryId.HasValue == false))
            {
                foreach (var cat2 in allCategories.Where(i => i.ParentCategoryId == cat1.Id))
                {
                    cat1.ChildCategories.Add(cat2);
                }
                categoriesTreeView.Add(cat1);
            }

            return new ServiceResponse<IEnumerable<Category>>(categoriesTreeView, HttpStatusCode.OK);
        }

        #endregion

        #region Add

        public async Task<ServiceResponse<Category>> Add(Category model, int currentUserId)
        {
            var entity = await _repository.Insert(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            return new ServiceResponse<Category>(entity, HttpStatusCode.OK);
        }

        #endregion

        #region Update

        public async Task<ServiceResponse<Category>> Update(Category model, int currentUserId)
        {
            _repository.Update(model, currentUserId);
            var rowsAffected = await _unitOfWork.Save();
            var entity = await _repository.SelectById(model.Id);
            return new ServiceResponse<Category>(entity, HttpStatusCode.OK);
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
