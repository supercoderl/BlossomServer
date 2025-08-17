using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using BlossomServer.SharedKernel.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Text;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class ContactRepository : BaseRepository<Contact, Guid>, IContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<IEnumerable<Contact>> GetAllContactsByEmailSQL(bool includeResponses, string email, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var contacts = new List<Contact>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getContacts_withEmail", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IncludeResponses", includeResponses);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Categories)
            while (await reader.ReadAsync(cancellationToken))
            {
                var contact = new Contact(
                    reader.GetGuid("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Email"),
                    reader.GetString("Message")
                );

                contact.SetCreatedAt(reader.GetDateTime("CreatedAt"));
                contact.SetHasResponse(!reader.IsDBNull("ResponseId"));

                var contactResponse = reader.IsDBNull("ResponseId") ? null : new ContactResponse(
                    reader.GetGuid("ResponseId"),
                    contact.Id,
                    reader.GetString("ResponseText"),
                    reader.GetGuid("ResponderId")
                );

                User? responder = null;
                if (!reader.IsDBNull("ResponderId"))
                {
                    responder = new User(
                        reader.GetGuid("ResponderId"),
                        string.Empty,
                        reader.IsDBNull("ResponderFirstName") ? string.Empty : reader.GetString("ResponderFirstName"),
                        reader.IsDBNull("ResponderLastName") ? string.Empty : reader.GetString("ResponderLastName"),
                        string.Empty,
                        string.Empty,
                        reader.IsDBNull("ResponderAvatarUrl") ? string.Empty : reader.GetString("ResponderAvatarUrl"),
                        null,
                        Domain.Enums.Gender.Unknow,
                        null,
                        DateOnly.FromDateTime(TimeZoneHelper.GetLocalTimeNow()),
                        Domain.Enums.UserRole.Manager
                    );
                }

                if (contactResponse != null)
                {
                    contactResponse.SetCreatedAt(reader.GetDateTime("ResponseDate"));
                    contactResponse.SetUser(responder);
                }

                contact.SetContactResponse(contactResponse);

                contacts.Add(contact);
            }

            return contacts;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsBySQL(string searchTerm, bool includeDeleted, int page, int pageSize, string sortColumn, string sortDirection, CancellationToken cancellationToken = default)
        {
            var contacts = new List<Contact>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getContacts", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
            command.Parameters.AddWithValue("@IncludeDeleted", includeDeleted);
            command.Parameters.AddWithValue("@Page", page);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (Categories)
            while (await reader.ReadAsync(cancellationToken))
            {
                var contact = new Contact(
                    reader.GetGuid("Id"),
                    reader.GetString("Name"),
                    reader.GetString("Email"),
                    reader.GetString("Message")
                );

                contact.SetCreatedAt(reader.GetDateTime("CreatedAt"));
                contact.SetHasResponse(reader.GetInt32("HasResponse") == 1);

                contacts.Add(contact);
            }

            return contacts;
        }
    }
}
