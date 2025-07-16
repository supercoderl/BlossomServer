using BlossomServer.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Users
{
    public sealed record CreateUserViewModel(
        string Email,
        string FirstName,
        string LastName,
        string Password,
        string PhoneNumber,
        Gender Gender,
        string Website,
        DateOnly DateOfBirth,
        UserRole Role
    );
}
