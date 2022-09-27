using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace Cen.Common.Domain.Interfaces
{
    public interface ISyncable
    {
        [MaxLength(36)]
        string ExtId { get; set; }
        
        public Instant ChangedAt { get; set; }
    }
}