using System;

namespace Cen.Wms.Domain.Auth.Providers.Models
{
    public class UserIdentity
    {
        private readonly Guid _userId;

        public UserIdentity(Guid userId)
        {
            _userId = userId;
        }

        public Guid UserId => _userId;
    }
}