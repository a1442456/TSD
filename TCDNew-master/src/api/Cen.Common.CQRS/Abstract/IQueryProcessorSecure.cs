using System.Security.Claims;

namespace Cen.Common.CQRS.Abstract
{
    public interface IQueryProcessorSecure
    {
        public bool Authorize { get; }
        public Claim[] Claims { get; }
    }
}