using BlossomServer.Domain.Entities;
using BlossomServer.Domain.Enums;
using BlossomServer.Domain.Interfaces.Repositories;
using BlossomServer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlossomServer.Infrastructure.Repositories
{
    public sealed class PasswordResetTokenRepository : BaseRepository<PasswordResetToken, Guid>, IPasswordResetTokenRepository
    {
        private readonly string _connectionString;

        public PasswordResetTokenRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new SqlNullValueException("An error occured when fetching sql connection string");
        }

        public async Task<PasswordResetToken?> GetByCode(string code, CancellationToken cancellationToken = default)
        {
            PasswordResetToken? token = null;

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new SqlCommand("sp_getResetPasswordByCode", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Code", code);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Read first result set (PasswordResetToken)
            while (await reader.ReadAsync(cancellationToken))
            {
                token = new PasswordResetToken(
                    reader.GetGuid("Id"),
                    reader.GetGuid("UserId"),
                    reader.GetString("Token")
                );

                token.SetExpirationDate(reader.GetDateTime("ExpirationDate"));
                token.SetIsUsed(reader.GetBoolean("IsUsed"));
            }

            return token;
        }
    }
}
