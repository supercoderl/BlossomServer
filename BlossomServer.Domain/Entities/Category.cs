using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Category : Entity<Guid>
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public int Priority { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        [InverseProperty("Category")]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

        public Category(
            Guid id,
            string name,
            bool isActive,
            int priority
        ) : base(id)
        {
            Name = name;
            IsActive = isActive;
            Priority = priority;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetName( string name ) { Name = name; }
        public void SetIsActive( bool isActive ) { IsActive = isActive; }
        public void SetPriority( int priority ) { Priority = priority; }
        public void SetUpdatedAt() { UpdatedAt = TimeZoneHelper.GetLocalTimeNow(); }
    }
}
