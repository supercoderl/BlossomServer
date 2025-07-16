using BlossomServer.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Application.ViewModels.Users
{
    public sealed record UpdateUserViewModel(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        IFormFile? AvatarFile,
        IFormFile? CoverPhoto,
        Gender Gender,
        string? Website,
        DateOnly DateOfBirth,
        UserRole Role
    );
}
