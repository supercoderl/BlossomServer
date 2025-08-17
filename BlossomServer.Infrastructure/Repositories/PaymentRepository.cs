using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class PaymentRepository : BaseRepository<Payment, Guid>, IPaymentRepository
    {
        private readonly string _connectionString;

        public PaymentRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var payments = new List<Payment>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getPayments", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Payments)
            while (await reader.ReadAsync(cancellationToken))
            {
                var payment = new Payment(
                    reader.GetGuid("Id"),
                    reader.GetGuid("BookingId"),
                    reader.GetDecimal("Amount"),
                    (PaymentMethod)Enum.Parse(typeof(PaymentMethod), reader.GetString("Method")),
                    reader.GetString("TransactionCode")
                );

                payment.SetCreatedAt(reader.GetDateTime("CreatedAt"));

                payments.Add(payment);
            }

            return payments;
        }
    }
}
