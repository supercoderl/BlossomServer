using BlossomServer.SharedKernel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Entities
{
    public class Notification : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime CreatedAt { get; private set; }

        [ForeignKey("UserId")]
        [InverseProperty("Notifications")]
        public virtual User? User { get; set; }

        public Notification(
            Guid id,
            Guid userId,
            string title,
            string message
        ) : base(id)
        {
            UserId = userId;
            Title = title;
            Message = message;
            IsRead = false;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetUserId( Guid userId ) { UserId = userId; }
        public void SetTitle( string title ) { Title = title; }
        public void SetMessage( string message ) { Message = message; }
        public void SetIsRead( bool isRead ) { IsRead = isRead; }
    }
}
