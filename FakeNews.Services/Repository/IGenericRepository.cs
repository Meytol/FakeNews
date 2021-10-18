using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FakeNews.Services.Repository
{
    public interface IGenericRepository<T> where T : class, IDbTableProperties
    {
        #region Create

        /// <summary>
        /// Insert object to database async
        /// </summary>
        /// <param name="entity">object</param>
        /// <param name="createdById">creator id</param>
        /// <returns>
        /// return inserted object with id
        /// </returns>
        Task<T> Insert(T entity, int createdById = 0);

        #endregion

        #region Read by admin

        IQueryable<T> WhereAdmin(Expression<Func<T, bool>> expression);

        Task<T> SelectByIdAdmin(int id);

        Task<T> SelectByPublicIdAdmin(Guid Publicid);

        Task<IEnumerable<T>> SelectAllAdmin();

        Task<int> CountAdmin();

        Task<int> CountAdmin(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAdmin(int id);

        Task<bool> ExistsAdmin(Expression<Func<T, bool>> predicate);

        #endregion

        #region Read

        IQueryable<T> AsQueryable();

        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Find available object async by id
        /// </summary>
        /// <param name="id"> object id </param>
        /// <returns>
        /// return object if that's found else method returns null
        /// </returns>
        Task<T> SelectById(int id);

        /// <summary>
        /// Find available object async by Public id
        /// </summary>
        /// <param name="id"> object public id </param>
        /// <returns>
        /// return object if that's found else method returns null
        /// </returns>
        Task<T> SelectByPublicId(Guid Publicid);

        /// <summary>
        /// Get all available objects async
        /// </summary>
        /// <returns>
        /// Return objects if those are found else method returns null
        /// </returns>
        Task<IEnumerable<T>> SelectAll();

        /// <summary>
        /// Count available objects quantity async
        /// </summary>
        /// <returns>
        /// return objects quantity
        /// </returns>
        Task<int> Count();

        /// <summary>
        /// Count available objects quantity async
        /// </summary>
        /// <returns>
        /// return objects quantity
        /// </returns>
        Task<int> Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Check available object exists by id
        /// </summary>
        /// <param name="id">object id</param>
        /// <returns></returns>
        Task<bool> Exists(int id);

        /// <summary>
        /// Check available object exists by id
        /// </summary>
        /// <param name="predicate">Exists condition</param>
        /// <returns></returns>
        Task<bool> Exists(Expression<Func<T, bool>> predicate);

        #endregion

        #region Update

        /// <summary>
        /// update an object data
        /// </summary>
        /// <param name="newValue">object must have id</param>
        /// <param name="updatedById">modifire id</param>
        /// <returns>
        /// returns updated object
        /// </returns>
        void Update(T newValue, int updatedById = 0);

        /// <summary>
        /// Logically delete an object async
        /// set object view state to Deleted
        /// </summary>
        /// <param name="id">object id</param>
        /// <param name="updatedById">modifire id</param>
        void Disable(int id, int updatedById = 0);

        /// <summary>
        /// Restore an object async
        /// set object view state to Available
        /// </summary>
        /// <param name="id">object id</param>
        /// <param name="updatedById">modifire id</param>
        void Enable(int id, int updatedById = 0);

        #endregion

        #region Delete

        /// <summary>
        /// delete prementaly an object from database
        /// </summary>
        /// <param name="entity">object</param>
        void Delete(T entity);


        /// <summary>
        /// delete prementaly an object from database with objectId
        /// </summary>
        /// <param name="entityId">object Id</param>
        void Delete(int entityId);

        #endregion

        #region Dispose

        void Dispose();

        Task DisposeAsync();

        #endregion
    }

    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class, IDbTableProperties, new()
    {

        #region Ctor

        private ApplicationDbContext _dbContext = null;
        private readonly DbSet<T> _entities = null;
        private bool _isDisposed;
        private object lockObj;

        public GenericRepository(IUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {

        }

        public GenericRepository(ApplicationDbContext context)
        {
            _isDisposed = false;
            _dbContext = context;
            _entities = _dbContext.Set<T>();
        }

        #endregion

        #region Insert

        public async Task<T> Insert(T entity, int createdById)
        {
            CreateDbContextIfDisposed();

            entity.CreatedOn = DateTime.Now;
            entity.CreatorId = createdById;

            if (entity.PublicId == default)
                entity.PublicId = Guid.NewGuid();

            await _entities.AddAsync(entity);

            return entity;
        }

        #endregion

        #region Read by admin

        public IQueryable<T> WhereAdmin(Expression<Func<T, bool>> expression)
            => _entities.Where(expression);

        public async Task<T> SelectByIdAdmin(int id)
            => await _entities.FindAsync(id);

        public async Task<T> SelectByPublicIdAdmin(Guid publicId)
            => await _entities.FirstOrDefaultAsync(e => e.PublicId == publicId);

        public async Task<IEnumerable<T>> SelectAllAdmin()
            => await _entities.ToListAsync();

        public async Task<int> CountAdmin()
            => await _entities.CountAsync();

        public async Task<int> CountAdmin(Expression<Func<T, bool>> predicate)
            => await _entities.CountAsync(predicate);

        public async Task<bool> ExistsAdmin(int id)
            => await _entities.AnyAsync(e => e.Id.Equals(id));

        public async Task<bool> ExistsAdmin(Expression<Func<T, bool>> predicate)
            => await _entities.AnyAsync(predicate);

        #endregion

        #region Read

        public IQueryable<T> AsQueryable()
            => _entities.AsQueryable();

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
            => _entities.Where(e => e.IsDeleted == false).Where(expression);

        public async Task<T> SelectById(int id)
            => await _entities.FirstOrDefaultAsync(e => e.IsDeleted == false && e.Id == id);

        public async Task<T> SelectByPublicId(Guid publicId)
            => await _entities.FirstOrDefaultAsync(e => e.PublicId == publicId);

        public async Task<IEnumerable<T>> SelectAll()
            => await _entities.Where(e => e.IsDeleted == false).ToListAsync();

        public async Task<int> Count()
            => await _entities.Where(e => e.IsDeleted == false).CountAsync();

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
            => await _entities.Where(e => e.IsDeleted == false).CountAsync(predicate);

        public async Task<bool> Exists(int id)
            => await _entities.Where(e => e.IsDeleted == false).AnyAsync(e => e.Id.Equals(id));

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
            => await _entities.Where(e => e.IsDeleted == false).AnyAsync(predicate);

        #endregion

        #region Update

        public void Update(T newValue, int updatedById)
        {
            CreateDbContextIfDisposed();

            newValue.ModifiedOn = DateTime.Now;
            newValue.ModifierId = updatedById;

            if (_dbContext.Entry(newValue).State == EntityState.Detached)
                _entities.Attach(newValue);

            _dbContext.Entry(newValue).State = EntityState.Modified;
            _dbContext.Entry(newValue).Property(nameof(IDbTable.Id)).IsModified = false;
            _dbContext.Entry(newValue).Property(nameof(IDbTable.PublicId)).IsModified = false;
            _dbContext.Entry(newValue).Property(nameof(IDbTable.CreatedOn)).IsModified = false;
        }

        public void Disable(int id, int updatedById)
        {
            CreateDbContextIfDisposed();

            var model = new T()
            {
                Id = id,
                IsDeleted = true,
                ModifiedOn = DateTime.Now,
                ModifierId = updatedById
            };

            _entities.Attach(model);
            _dbContext.Entry(model).Property(nameof(IDbTable.IsDeleted)).IsModified = true;
            _dbContext.Entry(model).Property(nameof(IDbTable.ModifiedOn)).IsModified = true;
            _dbContext.Entry(model).Property(nameof(IDbTable.ModifierId)).IsModified = true;
        }

        public void Enable(int id, int updatedById)
        {
            CreateDbContextIfDisposed();

            var model = new T()
            {
                Id = id,
                IsDeleted = false,
                ModifiedOn = DateTime.Now,
                ModifierId = updatedById
            };

            _entities.Attach(model);
            _dbContext.Entry(model).Property(nameof(IDbTable.IsDeleted)).IsModified = true;
            _dbContext.Entry(model).Property(nameof(IDbTable.ModifiedOn)).IsModified = true;
            _dbContext.Entry(model).Property(nameof(IDbTable.ModifierId)).IsModified = true;
        }

        #endregion

        #region Delete

        public void Delete(T entity)
        {
            CreateDbContextIfDisposed();

            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _entities.Attach(entity);

            _entities.Remove(entity);
        }

        public void Delete(int entityId)
        {
            CreateDbContextIfDisposed();

            var model = new T()
            {
                Id = entityId
            };

            _entities.Attach(model);
            _dbContext.Entry(model).State = EntityState.Deleted;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            _dbContext?.Dispose();
            _isDisposed = true;
        }

        public async Task DisposeAsync()
        {
            if (_dbContext != null)
                await _dbContext.DisposeAsync();
            _isDisposed = true;
        }

        #endregion

        #region Methods

        private void CreateDbContextIfDisposed()
        {
            if (_dbContext is null || _isDisposed)
                lock (lockObj)
                    if (_dbContext is null || _isDisposed)
                    {
                        _dbContext = new ApplicationDbContext();
                        _isDisposed = false;
                    }
        }

        #endregion

    }

}