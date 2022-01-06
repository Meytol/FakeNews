using FakeNews.Services.Repository;
using FakeNews.Common.Models;
using FakeNews.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.Services.ModelServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<Category>> Add(Category model, int currentUserId);
        Task<ServiceResponse<Category>> Update(Category model, int currentUserId);
        Task<ServiceResponse> Delete(int id, int currentUserId);
        Task<ServiceResponse<IEnumerable<Category>>> GetCategoriesTreeView(int parent = 0, IEnumerable<Category> cachedCategories = null);
        Task<ServiceResponse<IList<News>>> GetNewsByCategoryId(int categoryId, bool getChildsContent = true);
        Task<ServiceResponse<Category>> Get(int categoryId);

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

        public async Task<ServiceResponse<IEnumerable<Category>>> GetCategoriesTreeView(int parentId = 0, IEnumerable<Category> cachedCategories = null)
        {
            var allCategories = cachedCategories ?? (await _repository.SelectAll()).Select(i => (Category)i);

            var categoriesTreeView = new List<Category>();

            foreach (var cat1 in allCategories.Where(i => parentId == 0 ? i.ParentCategoryId == null : i.ParentCategoryId == parentId))
            {
                var childs = (await GetCategoriesTreeView(cat1.Id, allCategories)).Data.ToList();

                foreach (var child in childs)
                {
                    cat1.ChildCategories.Add(child);
                }

                categoriesTreeView.Add(cat1);
            }

            return new ServiceResponse<IEnumerable<Category>>(categoriesTreeView, HttpStatusCode.OK);
        }

        public async Task<ServiceResponse<IList<News>>> GetNewsByCategoryId(int categoryId, bool getChildsContent = true)
        {
            var categoryIds = new List<int>
            {
                categoryId
            };

            if (getChildsContent)
                categoryIds.AddRange((await GetCategoriesTreeView(categoryId)).Data.Select(c => c.Id));

            var categoryNewsList = await _repository.Where(category => categoryIds.Contains(category.Id))
                .Include(e => e.News)
                .ThenInclude(e => e.Author)
                .Include(e => e.News)
                .ThenInclude(e => e.Category)
                .SelectMany(e => e.News)
                .Distinct()
                .AsNoTracking()
                .OrderBy(e => e.CreatedOn)
                .ToListAsync();

            return new ServiceResponse<IList<News>>(categoryNewsList, HttpStatusCode.OK);
        }
        public async Task<ServiceResponse<Category>> Get(int categoryId)
        {
            var category = await _repository.Where(e => e.Id == categoryId).AsNoTracking().FirstOrDefaultAsync();

            return new ServiceResponse<Category>(category);
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
