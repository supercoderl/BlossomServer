﻿using BlossomServer.Domain.Errors;
using BlossomServer.Domain.Extensions.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Commands.Users.CreateUser
{
    public sealed class CreateUserCommandValidation : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidation()
        {
            AddRuleForId();
            AddRuleForEmail();
            AddRuleForFirstName();
            AddRuleForLastName();
            AddRuleForPassword();
            AddRuleForPhoneNumber();
        }

        private void AddRuleForId()
        {
            RuleFor(cmd => cmd.UserId)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyId)
                .WithMessage("User id may not be empty");
        }

        private void AddRuleForEmail()
        {
            RuleFor(cmd => cmd.Email)
                .EmailAddress()
                .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
                .WithMessage("Email is not a valid email address");
        }

        private void AddRuleForFirstName()
        {
            RuleFor(cmd => cmd.FirstName)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyFirstName)
                .WithMessage("FirstName may not be empty");
        }

        private void AddRuleForLastName()
        {
            RuleFor(cmd => cmd.LastName)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyLastName)
                .WithMessage("LastName may not be empty");
        }

        private void AddRuleForPassword()
        {
            RuleFor(cmd => cmd.Password)
                .Password();
        }

        private void AddRuleForPhoneNumber()
        {
            RuleFor(cmd => cmd.PhoneNumber)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyPhoneNumber)
                .WithMessage("PhoneNumber may not be empty");
        }
    }
}
