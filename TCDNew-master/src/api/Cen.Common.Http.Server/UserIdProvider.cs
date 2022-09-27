using System;
using System.Linq;
using System.Security.Claims;
using Cen.Common.CQRS.Abstract;
using Microsoft.AspNetCore.Http;

namespace Cen.Common.Http.Server
{
    public class UserIdProvider: IUserIdProvider
    {
        private readonly HttpContext _httpContext;

        public UserIdProvider(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        
        public string UserId => _httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        public Guid UserGuid => Guid.Parse(this.UserId);
    }
}