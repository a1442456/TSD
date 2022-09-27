using Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering;

namespace Cen.Common.Data.DataSource.Infrastructure.Implementation
{
    using System;

    internal class PredicateFilterDescription : FilterDescription
    {
        private readonly Delegate predicate;

        public PredicateFilterDescription(Delegate predicate)
        {
            this.predicate = predicate;
        }

        public override bool SatisfiesFilter(object dataItem)
        {
            return (bool) this.predicate.DynamicInvoke(dataItem);
        }
    }
}