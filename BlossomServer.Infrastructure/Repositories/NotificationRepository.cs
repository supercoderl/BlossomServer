using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using BlossomServer.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class NotificationRepository : BaseRepository<Notification, Guid>, INotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsBySQL(
            string searchTerm, 
            bool includeDeleted, 
            int page, 
            int pageSize, 
            Guid receiverId, 
            UserRole role,
            string sortColumn, 
            string sortDirection, 
            CancellationToken cancellationToken = default
        )
        {
            var notifications = new List<Notification>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getNotifications", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@ReceiverId", receiverId);
            command.Parameters.AddWithValue("@Role", role);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Notifications)
            while (await reader.ReadAsync(cancellationToken))
            {
                var notification = new Notification(
                    reader.GetGuid("Id"),
                    reader.GetGuid("UserId"),
                    reader.GetString("Title"),
                    reader.GetString("Message"),
                    (NotificationType)Enum.Parse(typeof(NotificationType), reader.GetString("NotificationType")),
                    reader.GetInt32("Priority"),
                    reader.IsDBNull("ExpiresAt") ? null : reader.GetDateTime("ExpiresAt"),
                    reader.IsDBNull("ActionUrl") ? null : reader.GetString("ActionUrl"),
                    reader.IsDBNull("RelatedEntityId") ? null : reader.GetGuid("RelatedEntityId")
                );

                notification.SetIsRead(reader.GetBoolean("IsRead"));
                notification.SetCreatedAt(reader.GetDateTime("CreatedAt"));

                notifications.Add(notification);
            }

            return notifications;
        }
    }
}
