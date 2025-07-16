using BlossomServer.Application.ViewModels.Payments;
using BlossomServer.Application.ViewModels.Sorting;
using BlossomServer.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlossomServer.Application.ViewModels.Messages;

namespace BlossomServer.Application.Queries.Messages.GetAll
{
    public sealed record GetAllMessagesQuery(
        PageQuery Query,
        bool IncludeDeleted,
        Guid ConversationId,
        string SearchTerm = "",
        SortQuery? SortQuery = null) :
        IRequest<PagedResult<MessageViewModel>>;
}
