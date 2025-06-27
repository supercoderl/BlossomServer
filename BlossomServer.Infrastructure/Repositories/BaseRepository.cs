using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Repositories
{
    public class BaseRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : Entity<TId> where TId : IEquatable<TId>
    {
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<bool> ExistsAsync(TId id)
        {
            return await DbSet.AnyAsync(e => e.Id.Equals(id));
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public IQueryable<TEntity> GetAllAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(TEntity entity, bool hardDelete = false)
        {
            if(hardDelete)
            {
                DbSet.Remove(entity);
                return;
            }

            entity.Delete();
        }

        public void RemoveRange(IEnumerable<TEntity> entities, bool hardDelete = false)
        {
            if(hardDelete)
            {
                DbSet.RemoveRange(entities);
                return;
            }
           
            foreach(var entity in entities)
            {
                entity.Delete();
            }
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public int SaveChange()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
