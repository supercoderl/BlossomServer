namespace BlossomServer.Shared.Events.Subscriber
{
    public sealed class SubscribeEvent : DomainEvent
    {
        public string Email { get; set; }

        public SubscribeEvent(Guid subscriberId, string email) : base(subscriberId)
        {
            Email = email;
        }
    }
}
