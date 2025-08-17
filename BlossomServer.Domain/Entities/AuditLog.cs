using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class AuditLog : Entity<Guid>
    {
        public string TableName { get; private set; }
        public string Operation { get; private set; }
        public string PrimaryKey { get; private set; }
        public string ColumnName { get; private set; }
        public string? OldValue { get; private set; }
        public string? NewValue { get; private set; }
        public DateTime ChangedDate { get; private set; }
        public string? ChangedBy { get; private set; }
        public string? ApplicationUser { get; private set; }
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }
        public string? SessionId { get; private set; }

        public AuditLog(
            Guid id,
            string tableName,
            string operation,
            string primaryKey,
            string columnName,
            string? oldValue,
            string? newValue,
            DateTime changedDate,
            string? changedBy,
            string? applicationUser,
            string? ipAddress,
            string? userAgent,
            string? sessionId
        ) : base(id)
        {
            TableName = tableName;
            Operation = operation;
            PrimaryKey = primaryKey;
            ColumnName = columnName;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedDate = changedDate;
            ChangedBy = changedBy;
            ApplicationUser = applicationUser;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            SessionId = sessionId;
        }

        public void SetTableName(string tableName) { TableName = tableName; }
        public void SetOperation(string operation) { Operation = operation; }
        public void SetPrimaryKey(string primaryKey) { PrimaryKey = primaryKey; }
        public void SetColumnName(string columnName) { ColumnName = columnName; }
        public void SetOldValue(string? oldValue) { OldValue = oldValue; }
        public void SetNewValue(string? newValue) { NewValue = newValue; }
        public void SetChangedDate(DateTime changedDate) { ChangedDate = changedDate; }
        public void SetChangedBy(string? changedBy) { ChangedBy = changedBy; }
        public void SetApplicationUser(string? applicationUser) { ApplicationUser = applicationUser; }
        public void SetIpAddress(string? ipAddress) { IpAddress = ipAddress; }
        public void SetUserAgent(string? userAgent) { UserAgent = userAgent; }
        public void SetSessionId(string? sessionId) { SessionId = sessionId; }
    }
}
