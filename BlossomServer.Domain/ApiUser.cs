﻿using BlossomServer.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlossomServer.Domain
{
    public sealed class ApiUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string? _name;
        private Guid _userId = Guid.Empty;

        public ApiUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserEmail()
        {
            var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Email));

            if(!string.IsNullOrWhiteSpace(claim?.Value))
            {
                return claim.Value;
            }

            return string.Empty;
        }

        public Guid GetUserId()
        {
            if(_userId != Guid.Empty)
            {
                return _userId;
            }

            var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.NameIdentifier));

            if (claim is null) return Guid.NewGuid();

            if(Guid.TryParse(claim?.Value, out var userId))
            {
                _userId = userId;
                return userId;
            }

            throw new AggregateException("Could not parse user id to guid");
        }

        public string Name
        {
            get
            {
                if (_name is not null)
                {
                    return _name;
                }

                var identity = _httpContextAccessor.HttpContext?.User.Identity;
                if (identity is null)
                {
                    _name = string.Empty;
                    return string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(identity.Name))
                {
                    _name = identity.Name;
                    return identity.Name;
                }

                var claim = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
                _name = claim ?? string.Empty;
                return _name;
            }
        }
    }
}
