using BlossomServer.Application.Interfaces;
using BlossomServer.Application.Queries.Technicians.GetAll;
using BlossomServer.Application.Queries.Technicians.GetById;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels.Technicians;
using BlossomServer.Domain.Commands.Technicians.DeleteTechnician;
using BlossomServer.Domain.Commands.Technicians.UpdateTechnician;
using BlossomServer.Domain.Interfaces;

namespace BlossomServer.Application.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly IMediatorHandler _bus;

        public TechnicianService(IMediatorHandler bus)
        {
            _bus = bus;
        }

        public async Task DeleteTechnicianAsync(Guid technicianId)
        {
            await _bus.SendCommandAsync(new DeleteTechnicianCommand(technicianId));
        }

        public async Task<PagedResult<TechnicianViewModel>> GetAllTechniciansAsync(PageQuery query, bool includeDeleted, string searchTerm = "", SortQuery? sortQuery = null)
        {
            return await _bus.QueryAsync(new GetAllTechniciansQuery(query, includeDeleted, searchTerm, sortQuery));
        }

        public async Task<TechnicianViewModel?> GetTechnicianByTechnicianIdAsync(Guid technicianId)
        {
            return await _bus.QueryAsync(new GetTechnicianByIdQuery(technicianId));
        }

        public async Task UpdateTechnicianAsync(UpdateTechnicianViewModel technician)
        {
            await _bus.SendCommandAsync(new UpdateTechnicianCommand(
                technician.TechnicianId,
                technician.Bio,
                technician.Rating,
                technician.YearsOfExperience
            ));
        }
    }
}
