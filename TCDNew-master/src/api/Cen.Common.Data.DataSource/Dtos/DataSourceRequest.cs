using System.Collections.Generic;
using Cen.Common.Data.DataSource.Infrastructure;

namespace Cen.Common.Data.DataSource.Dtos
{
    public class DataSourceRequest
    {
        public DataSourceRequest()
        {
            Page = 1;
        }

        public int Page
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public IList<SortDescriptor> Sorts
        {
            get;
            set;
        }

        public IList<IFilterDescriptor> Filters
        {
            get;
            set;
        }
    }
}
