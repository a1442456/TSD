using System.Collections.Generic;
using System.Linq;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Infrastructure;

namespace Cen.Common.Data.DataSource.AgGrid
{
    public class TableRowsReq {
        // The first row index to get.
        public int StartRow { get; set; }
        
        // The first row index to NOT get.
        public int EndRow { get; set; }

        // If doing Server-side sorting, contains the sort model
        public IEnumerable<AgSortModelItem> SortModel { get; set; }

        // If doing Server-side filtering, contains the filter model
        // filterModel: any;

        public DataSourceRequest GetDataSourceRequest()
        {
            var pageSize = EndRow - StartRow;
            var request = new DataSourceRequest();

            request.Page = (StartRow / pageSize) + 1;
            request.PageSize = pageSize;
            
            if (SortModel != null)
            {
                request.Sorts = SortModel.Select(e => new SortDescriptor
                {
                    Member = char.ToUpper(e.ColId[0]) + e.ColId.Substring(1),
                    SortDirection = e.Sort == "asc" ? ListSortDirection.Ascending : ListSortDirection.Descending
                }).ToList();
            }

//            request.Filters = FilterDescriptorFactory.Create(
//                string.Join("~and~",
////                    (lazyLoadEvent.Filters)
////                    .Where(f => f.Value != null)
////                    .Select(f => $"{f.Key}{f.Value.ToString()}")
////                    .AsEnumerable()
//                )
//            );

            return request;
        }
    }

    public class TableRowsWithParamReq<T> : TableRowsReq
    {
        public T Data { get; set; }
    }
}