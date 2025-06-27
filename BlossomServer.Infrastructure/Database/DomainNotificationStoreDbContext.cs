using BlossomServer.Domain.DomainNotifications;
using BlossomServer.Infrastructure.Configuration.EventSourcing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Database
{
    public class DomainNotificationStoreDbContext : DbContext
    {
        public virtual DbSet<StoredDomainNotification> StoredDomainNotifications { get; set; } = null!;

        public DomainNotificationStoreDbContext(DbContextOptions<DomainNotificationStoreDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoredDomainNotificationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
