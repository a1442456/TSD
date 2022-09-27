using System;
using System.Net.Http;

namespace Cen.Common.Http.Server.OpenApi
{
    /// <summary>
    /// A class to supply OpenApi data
    /// </summary>
    public class RouteMetaData
    {
        /// <summary>
        /// A description about the route
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A tag to group routes by
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// An array of <see cref="RouteMetaDataRequest"/>s that must be sent.
        /// </summary>
        public RouteMetaDataRequest[] Requests { get; set; }

        /// <summary>
        /// An array of possible <see cref="RouteMetaDataResponse"/>s that can be returned from the route
        /// </summary>
        public RouteMetaDataResponse[] Responses { get; set; }
        
        /// <summary>
        /// A unique identifier for the route
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// The name of a security schema used in the API
        /// </summary>
        public string SecuritySchema { get; set; }
        
        /// <summary>
        /// An array of possible <see cref="QueryStringParameter"/>s that a route may use
        /// </summary>
        public QueryStringParameter[] QueryStringParameter { get; set; }

        /// <summary>
        /// The <see cref="HttpContent"/> type associated with a request.
        /// </summary>
        public Type Content { get; set; }
    }
}