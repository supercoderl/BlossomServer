using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.ConversationParticipants.CreateConversationParticipant
{
    public sealed class CreateConversationParticipantCommandValidation : AbstractValidator<CreateConversationParticipantCommand>
    {
        public CreateConversationParticipantCommandValidation()
        {
            
        }
    }
}
