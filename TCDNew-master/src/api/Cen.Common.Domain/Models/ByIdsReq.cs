using System;
using System.Collections.Generic;

namespace Cen.Common.Domain.Models
{
    public class ByIdsReq
    {
        public IEnumerable<Guid> Id { get; set; }
    }
}