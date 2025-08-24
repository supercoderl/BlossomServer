using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.SocialLogin
{
    public sealed class SocialLoginCommandValidation : AbstractValidator<SocialLoginCommand>    
    {
        public SocialLoginCommandValidation()
        {
            
        }
    }
}
