using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;

namespace BlossomServer.Application.Interfaces
{
    public interface IPaymentService
    {
        public Task<PagedResult<PaymentViewModel>> GetAllPaymentsAsync(
            PageQuery query,
            bool includeDeleted,
            string searchTerm = "",
            SortQuery? sortQuery = null);
    }
}
