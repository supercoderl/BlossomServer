using BlossomServer.Application.Interfaces;
using BlossomServer.Application.SortProviders;
using BlossomServer.Application.ViewModels;
using BlossomServer.Application.ViewModels.Bookings;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Notifications;
using BlossomServer.Models;
using BlossomServer.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;

namespace BlossomServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/v1/[controller]")]
    public sealed class BookingController : ApiController
    {
        private readonly IBookingService _bookingService;

        public BookingController(
            INotificationHandler<DomainNotification> notifications,
            IBookingService bookingService) : base(notifications)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [SwaggerOperation("Get a list of all bookings")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<PagedResult<BookingViewModel>>))]
        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery] PageQuery query,
            [FromQuery] string searchTerm = "",
            [FromQuery] bool includeDeleted = false,
            [FromQuery] [SortableFieldsAttribute<BookingViewModelSortProvider,BookingViewModel, Booking>]
        SortQuery? sortQuery = null)
        {
            var bookings = await _bookingService.GetAllBookingsAsync(
                query,
                includeDeleted,
                searchTerm,
                sortQuery);
            return Response(bookings);
        }

        [HttpGet("time-slots")]
        [SwaggerOperation("Get a list of all time slots for techinician")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<IEnumerable<(DateTime start, TimeSpan duration)>>))]
        public async Task<IActionResult> GetAllTimeSlotForCustomerAsync(
            [FromQuery] Guid technicianId,
            [FromQuery] string selectedDate,
        SortQuery? sortQuery = null)
        {
            if (!DateTime.TryParseExact(
                selectedDate,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                return BadRequest("Invalid date format. Expected yyyy-MM-dd.");
            }

            var bookings = await _bookingService.GetAllTimeSlotForTechinicianAsync(technicianId, date);
            return Response(bookings);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation("Get a booking by id")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<BookingViewModel>))]
        public async Task<IActionResult> GetBookingByIdAsync([FromRoute] Guid id)
        {
            var booking = await _bookingService.GetBookingByBookingIdAsync(id);
            return Response(booking);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Create a new booking")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> CreateBookingAsync([FromBody] CreateBookingViewModel viewModel)
        {
            var bookingId = await _bookingService.CreateBookingAsync(viewModel);
            return Response(bookingId);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete a booking")]
        [SwaggerResponse(200, "Request successful", typeof(ResponseMessage<Guid>))]
        public async Task<IActionResult> DeleteBookingAsync([FromRoute] Guid id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return Response(id);
        }
    }
}
