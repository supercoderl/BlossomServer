using FluentValidation;

namespace BlossomServer.Domain.Commands.Conversations.CreateConversation
{
    public sealed class CreateConversationCommandValidation : AbstractValidator<CreateConversationCommand>
    {
        public CreateConversationCommandValidation()
        {

        }
    }
}
