using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity, TId> : IDisposable where TEntity : Entity<TId> where TId : IEquatable<TId>
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAsNoTracking();

        Task<TEntity?> GetByIdAsync(TId id);

        void Update(TEntity entity);

        Task<bool> ExistsAsync(TId id);

        public void Remove(TEntity entity, bool hardDelete = false);

        void RemoveRange(IEnumerable<TEntity> entities, bool hardDelete = false);
    }
}
