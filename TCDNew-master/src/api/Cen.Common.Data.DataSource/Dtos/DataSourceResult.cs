using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cen.Common.Data.DataSource.Dtos
{
    public class DataSourceResult
    {
        public IEnumerable Items { get; set; }
        public int Total { get; set; }

        public DataSourceResult<T> AsTyped<T>()
        {
            return new DataSourceResult<T>
            {
                Items = Items.Cast<T>(), 
                Total = Total
            };
        }
    }

    public class DataSourceResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        
        public static DataSourceResult<T> Empty()
        {
            return new DataSourceResult<T> {Items = new List<T>(), Total = 0};
        }
    }
}
