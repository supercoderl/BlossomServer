using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public abstract class Entity<TId> where TId : IEquatable<TId>
    {
        public TId Id { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }

        protected Entity(TId id)
        {
            Id = id;
        }

        public void SetId(TId id)
        {
            if(id is null || (id is Guid guid && guid == Guid.Empty) || (id is string str && string.IsNullOrEmpty(str)))
            {
                throw new ArgumentException($"{nameof(id)} may not be null, empty or default.");
            }
            Id = id;
        }

        public void Delete()
        {
            DeletedAt = TimeZoneHelper.ConvertUtcToLocal(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        }

        public void Undelete()
        {
            DeletedAt = null;
        }
    }
}
