using BlossomServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain
{
    public static class CacheKeyGenerator
    {
        public static string GetEntityCacheKey<TEntity>(TEntity entity) where TEntity : Entity<Guid>
        {
            return $"{typeof(TEntity)}-{entity.Id}";
        }

        public static string GetEntityCacheKey<TEntity>(Guid id) where TEntity : Entity<Guid>
        {
            return $"{typeof(TEntity)}-{id}";
        }
    }
}
