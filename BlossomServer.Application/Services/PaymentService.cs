using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Payments.GetAll;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMediatorHandler _bus;

        public PaymentService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task<PagedResult<PaymentViewModel>> GetAllPaymentsAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllPaymentsQuery(query, includeDeleted, searchTerm, sortQuery));
        }
    }
}
