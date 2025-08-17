using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Mails
{
    public sealed record SendMailViewModel
    (
        string To,
        string Subject,
        string Content,
        bool IsHtml
    );
}   
