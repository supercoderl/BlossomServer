using BlossomServer.Domain.DomainEvents;
using BlossomServer.Infrastructure.Configuration.EventSourcing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Database
{
    public class EventStoreDbContext : DbContext
    {
        public virtual DbSet<StoredDomainEvent> StoredDomainEvents { get; set; } = null!;

        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoredDomainEventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
