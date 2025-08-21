namespace BlossomServer.Shared.Events.Admin
{
    public class NotificationWrapper
    {
        public Guid? ReceiverId { get; set; }
        public object Notification { get; set; } = new { };
        public string Type { get; set; } = "group";
    }

    public sealed class NotificationRequiredEvent : DomainEvent
    {
        public IEnumerable<NotificationWrapper> Data { get; set; }

        public NotificationRequiredEvent(
            IEnumerable<NotificationWrapper> data
        ) : base(Guid.NewGuid())
        {
            Data = data;
        }
    }
}
