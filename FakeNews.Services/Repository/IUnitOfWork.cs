using FakeNews.Common.Database.Interfaces;
using FakeNews.Database.Config;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeNews.Services.Repository
{
    public interface IUnitOfWork
    {
        ApplicationDbContext Context { get; }

        Task CreateTransaction();

        Task Commit();

        Task Rollback();

        Task<int> Save();

        GenericRepository<T> GenericRepository<T>() where T : class, IDbTableProperties, new();

        void Dispose();

        Task DisposeAsync();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Ctor

        private readonly ApplicationDbContext _context;
        private bool _disposed;
        private IDbContextTransaction _objTran;
        private IDictionary<string, object> _repositories;
        public ApplicationDbContext Context => _context;

        public UnitOfWork(ApplicationDbContext context = null)
        {
            _context = context ?? new ApplicationDbContext();
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    await _context.DisposeAsync();
            _disposed = true;
        }

        #endregion

        #region Create transaction

        public async Task CreateTransaction()
        {
            _objTran = await _context.Database.BeginTransactionAsync();
        }

        #endregion

        #region Commit

        public async Task Commit()
        {
            if (_objTran != null)
                await _objTran.CommitAsync();
        }

        #endregion

        #region Rollback

        public async Task Rollback()
        {
            if (_objTran != null)
            {
                await _objTran.RollbackAsync();
                await _objTran.DisposeAsync();
            }
        }

        #endregion

        #region Save

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion

        public GenericRepository<T> GenericRepository<T>() where T : class, IDbTableProperties, new()
        {
            if (_repositories is null)
                _repositories = new Dictionary<string, object>();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var makeGenericType = repositoryType.MakeGenericType(typeof(T));
                var repositoryInstance = Activator.CreateInstance(makeGenericType, _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)_repositories[type];
        }
    }
}
