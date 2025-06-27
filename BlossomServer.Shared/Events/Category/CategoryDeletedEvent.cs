using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Shared.Events.Category
{
    public sealed class CategoryDeletedEvent : DomainEvent
    {
        public CategoryDeletedEvent(Guid categoryId) : base(categoryId)
        {
            
        }
    }
}
