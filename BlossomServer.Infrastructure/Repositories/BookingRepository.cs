using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using BlossomServer.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class BookingRepository : BaseRepository<Booking, Guid>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public BookingRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<object> CalculateRevenueSQL(string dateStart, string dateEnd, CancellationToken cancellationToken = default)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_calculate_revenue", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                return new
                {
                    CurrentTotalRevenue = reader.GetDecimal("CurrentTotalRevenue"),
                    PreviousTotalRevenue = reader.GetDecimal("PreviousTotalRevenue"),
                    CurrentTransactionCount = reader.GetInt32("CurrentTransactionCount"),
                    PreviousTransactionCount = reader.GetInt32("PreviousTransactionCount"),
                    AvgTransactionDifference = reader.GetDecimal("AvgTransactionDifference"),
                    RevenuePercentageChange = reader.GetDecimal("RevenuePercentageChange"),
                    RevenueTrend = reader.GetString("RevenueTrend")
                };
            }

            return new
            {
                CurrentTotalRevenue = 0m,
                PreviousTotalRevenue = 0m,
                CurrentTransactionCount = 0,
                PreviousTransactionCount = 0,
                AvgTransactionDifference = 0m,
                RevenuePercentageChange = 0m,
                RevenueTrend = "Stable"
            };
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var bookings = new List<Booking>();
            var bookingDetails = new List<BookingDetail>();
            var services = new List<Service>();
            var serviceOptions = new List<ServiceOption>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getBookings", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Bookings)
            while (await reader.ReadAsync(cancellationToken))
            {
                var booking = new Booking(
                    reader.GetGuid("Id"),
                    reader.IsDBNull("CustomerId") ? null : reader.GetGuid("CustomerId"),
                    reader.IsDBNull("TechnicianId") ? null : reader.GetGuid("TechnicianId"),
                    reader.GetDateTime("ScheduleTime"),
                    reader.GetDecimal("TotalPrice"),
                    (BookingStatus)Enum.Parse(typeof(BookingStatus), reader.GetString("Status")),
                    reader.IsDBNull("Note") ? null : reader.GetString("Note"),
                    reader.IsDBNull("GuestName") ? null : reader.GetString("GuestName"),
                    reader.IsDBNull("GuestPhone") ? null : reader.GetString("GuestPhone"),
                    reader.IsDBNull("GuestEmail") ? null : reader.GetString("GuestEmail")
                );

                booking.SetCreatedAt(reader.GetDateTime("CreatedAt"));
                booking.SetUpdatedAt(reader.IsDBNull("UpdatedAt") ? null : reader.GetDateTime("UpdatedAt"));

                bookings.Add(booking);
            }

            // Move to second result set (BookingDetails with Service and ServiceOption data)
            await reader.NextResultAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                // Create Service object from the query results
                Service? service = null;
                if (!reader.IsDBNull("MainServiceId"))
                {
                    service = new Service(
                        reader.GetGuid("MainServiceId"),
                        reader.GetString("Name"),
                        reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        reader.IsDBNull("CategoryId") ? null : reader.GetGuid("CategoryId"),
                        reader.IsDBNull("Price") ? null : reader.GetDecimal("Price"),
                        reader.IsDBNull("DurationMinutes") ? null : reader.GetInt32("DurationMinutes"),
                        reader.IsDBNull("RepresentativeImage") ? null : reader.GetString("RepresentativeImage")
                    );
                }

                // Create ServiceOption object from the query results
                ServiceOption? serviceOption = null;
                if (!reader.IsDBNull("ServiceOptionId"))
                {
                    serviceOption = new ServiceOption(
                        reader.GetGuid("ServiceOptionId"),
                        reader.GetGuid("OptionServiceId"),
                        reader.GetString("VariantName"),
                        reader.GetDecimal("PriceFrom"),
                        reader.IsDBNull("PriceTo") ? (decimal?)null : reader.GetDecimal("PriceTo"),
                        reader.IsDBNull("DurationMinutes") ? null : reader.GetInt32("DurationMinutes")
                    );
                }

                // Create BookingDetail with Service and ServiceOption
                var bookingDetail = new BookingDetail(
                    reader.GetGuid("Id"),
                    reader.GetGuid("BookingId"), // You need to add this to your stored procedure SELECT
                    reader.IsDBNull("ServiceId") ? null : reader.GetGuid("ServiceId"),
                    reader.IsDBNull("ServiceOptionId") ? null : reader.GetGuid("ServiceOptionId"),
                    reader.GetInt32("Quantity"),
                    reader.GetDecimal("UnitPrice")
                );

                // Set the Service and ServiceOption on the BookingDetail
                // You'll need to add these properties/methods to your BookingDetail class
                if (service != null)
                {
                    bookingDetail.SetService(service);
                }

                if (serviceOption != null)
                {
                    bookingDetail.SetServiceOption(serviceOption);
                }

                bookingDetails.Add(bookingDetail);
            }

            // Combine the results
            var bookingDetailsLookup = bookingDetails.ToLookup(bd => bd.BookingId);

            foreach (var b in bookings)
            {
                var details = bookingDetailsLookup[b.Id];
                b.SetBookingDetails(details.ToList());
            }

            return bookings;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsIncommingBySQL(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var bookings = new List<Booking>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getBookings_incomming", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Bookings)
            while (await reader.ReadAsync(cancellationToken))
            {
                var booking = new Booking(
                    reader.GetGuid("Id"),
                    reader.IsDBNull("CustomerId") ? null : reader.GetGuid("CustomerId"),
                    reader.IsDBNull("TechnicianId") ? null : reader.GetGuid("TechnicianId"),
                    reader.GetDateTime("ScheduleTime"),
                    0,
                    (BookingStatus)Enum.Parse(typeof(BookingStatus), reader.GetString("Status")),
                    reader.IsDBNull("Note") ? null : reader.GetString("Note"),
                    reader.IsDBNull("GuestName") ? null : reader.GetString("GuestName"),
                    reader.IsDBNull("GuestPhone") ? null : reader.GetString("GuestPhone"),
                    reader.IsDBNull("GuestEmail") ? null : reader.GetString("GuestEmail")
                );

                if (!reader.IsDBNull("TechnicianId"))
                {
                    var technician = new Technician(
                        reader.GetGuid("TechnicianId"),
                        Guid.Empty,
                        string.Empty,
                        0,
                        0
                    );

                    technician.SetUser(new User(
                        Guid.Empty,
                        string.Empty,
                        reader.IsDBNull("TechnicianFirstName") ? string.Empty : reader.GetString("TechnicianFirstName"),
                        reader.IsDBNull("TechnicianLastName") ? string.Empty : reader.GetString("TechnicianLastName"),
                        reader.GetString("TechnicianEmail"),
                        string.Empty,
                        string.Empty,
                        null,
                        Gender.Unknow,
                        null,
                        DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                        UserRole.Technician
                    ));

                    booking.SetTechnician(technician);
                }

                if (!reader.IsDBNull("CustomerId"))
                {
                    var customer = new User(
                        reader.GetGuid("CustomerId"),
                        string.Empty,
                        reader.IsDBNull("CustomerFirstName") ? string.Empty : reader.GetString("CustomerFirstName"),
                        reader.IsDBNull("CustomerLastName") ? string.Empty : reader.GetString("CustomerLastName"),
                        reader.GetString("CustomerEmail"),
                        string.Empty,
                        string.Empty,
                        null,
                        Gender.Unknow,
                        null,
                        DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                        UserRole.Customer
                    );

                    booking.SetCustomer(customer);
                }

                bookings.Add(booking);
            }

            return bookings;
        }

        public async Task<object> GetBookingCountSQL(string dateStart, string dateEnd, CancellationToken cancellationToken = default)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getBooking_count", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                return new {
                    CurrentPeriodBookings = reader.GetInt32("CurrentPeriodBookings"),
                    PreviousPeriodBookings = reader.GetInt32("PreviousPeriodBookings"),
                    BookingsDifference = reader.GetInt32("BookingsDifference"),
                    PercentageChange = reader.GetDecimal("PercentageChange"),
                    Trend = reader.GetString("Trend")
                };
            }

            return new
            {
                CurrentPeriodBookings = 0,
                PreviousPeriodBookings = 0,
                BookingsDifference = 0,
                PercentageChange = 0m,
                Trend = "Stable"
            };
        }

        public async Task<IEnumerable<object>> GetBookingStatusBreakdownSQL(string dateStart, string dateEnd, CancellationToken cancellationToken)
        {
            var results = new List<object>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getBooking_status_breakdown", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                var result = new
                {
                    Status = reader.GetString("Status"),
                    Count = reader.GetInt32("Count"),
                    Percentage = reader.GetDecimal("Percentage")
                };

                results.Add(result);
            }

            return results;
        }

        public async Task<decimal> GetConversionRateSQL(string dateStart, string dateEnd, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getConversion_rate", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetDecimal("ConversionRate");
            }

            return 0m;
        }

        public async Task<int> GetCustomerCountSQL(string dateStart, string dateEnd, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getCustomer_count", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@DateStart", dateStart);
            command.Parameters.AddWithValue("@DateEnd", dateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetInt32("TotalCustomers");
            }

            return 0;
        }

        public async Task<object> GetCustomerRetentionRateSQL(string currentDateStart, string currentDateEnd, string previousDateStart, string previousDateEnd, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getCustomer_retention_rate", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CurrentDateStart", currentDateStart);
            command.Parameters.AddWithValue("@CurrentDateEnd", currentDateEnd);
            command.Parameters.AddWithValue("@PreviousDateStart", previousDateStart);
            command.Parameters.AddWithValue("@PreviousDateEnd", previousDateEnd);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                return new
                {
                    PreviousCustomers = reader.GetInt32("PreviousCustomers"),
                    ReturningCustomers = reader.GetInt32("ReturningCustomers"),
                    CustomerRetentionRate = reader.GetDouble("CustomerRetentionRate")
                };
            }

            return new
            {
                PreviousCustomers = 0,
                ReturningCustomers = 0,
                CustomerRetentionRate = 0m
            };
        }

        public async Task<IEnumerable<object>> GetScheduleByDateSQL(string date, CancellationToken cancellationToken)
        {
            var schedules = new List<object>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getScheduleByDate", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@ScheduleDate", date);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set
            while (await reader.ReadAsync(cancellationToken))
            {
                var schedule = new
                {
                    Id = reader.GetGuid("Id"),
                    CustomerName = reader.GetString("CustomerName"),
                    TechnicianName = reader.GetString("TechnicianName"),
                    CustomerPhone = reader.GetString("CustomerPhone"),
                    StartTime = reader.GetString("StartTime"),
                    EndTime = reader.GetString("EndTime"),
                    DurationMinutes = reader.GetInt32("DurationMinutes"),
                    Status = (BookingStatus)Enum.Parse(typeof(BookingStatus), reader.GetString("Status")),
                    Note = reader.IsDBNull("Note") ? null : reader.GetString("Note")
                };

                schedules.Add(schedule);
            }

            return schedules;
        }

        public async Task<IEnumerable<(DateTime start, TimeSpan duration)>> GetScheduleTimes(Guid technicianId, DateTime selectedDate)
        {
            var bookings = await DbSet
                .Where(b => b.TechnicianId == technicianId)
                .Where(b => b.ScheduleTime.Date == selectedDate.Date)
                .Where(b => b.Status == Domain.Enums.BookingStatus.Pending || b.Status == Domain.Enums.BookingStatus.Confirmed)
                .ToListAsync();

            var result = new List<(DateTime start, TimeSpan duration)>();

            foreach (var booking in bookings)
            {
                var details = await _context.BookingDetails
                        .Where(d => d.BookingId == booking.Id)
                        .Include(d => d.ServiceOption)
                        .Include(d => d.Service)
                        .ToListAsync();

                int totalDuration = 0;

                foreach (var detail in details)
                {
                    if (detail.ServiceOptionId.HasValue)
                    {
                        totalDuration += detail.ServiceOption?.DurationMinutes ?? 0;
                    }
                    else if (detail.ServiceId.HasValue)
                    {
                        totalDuration += detail.Service?.DurationMinutes ?? 0;
                    }
                }

                result.Add((booking.ScheduleTime, TimeSpan.FromMinutes(totalDuration)));
            }

            return result;
        }
    }
}
