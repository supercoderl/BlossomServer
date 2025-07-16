namespace BlossomServer.Shared.Events.Conversation
{
    public sealed class ConversationCreatedEvent : DomainEvent
    {
        public ConversationCreatedEvent(Guid conversationId) : base(conversationId)
        {

        }
    }
}
