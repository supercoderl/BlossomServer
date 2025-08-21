namespace BlossomServer.Shared.Events.Notification
{
    public sealed class NotificationCreatedEvent : DomainEvent
    {
        public Guid ReceiverId { get; set; }
        public object Notification { get; set; }
        public string Type { get; set; }

        public NotificationCreatedEvent(
            Guid notificationId,
            Guid receiverId,
            object notification,
            string type
        ) : base(notificationId)
        {
            ReceiverId = receiverId;
            Notification = notification;
            Type = type;
        }
    }
}
