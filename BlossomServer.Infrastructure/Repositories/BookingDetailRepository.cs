using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class BookingDetailRepository : BaseRepository<BookingDetail, Guid>, IBookingDetailRepository
    {
        public BookingDetailRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
