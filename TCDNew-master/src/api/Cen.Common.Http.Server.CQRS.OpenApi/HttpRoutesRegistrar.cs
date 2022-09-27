using System.Collections.Generic;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Http.Server.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Common.Http.Server.CQRS.OpenApi
{
    public class HttpRoutesRegistrar<T> where T: class
    {
        private readonly Dictionary<string, RequestDelegate> _moduleProcessors;
        private readonly Dictionary<(string verb, string path), RouteMetaData> _moduleMetaData;

        private readonly string _moduleName;
        private readonly HttpQueryRunner _httpQueryRunner;

        public HttpRoutesRegistrar(HttpQueryRunner httpQueryRunner)
        {
            _moduleName = typeof(T).Name.Replace("Module", "");
            _httpQueryRunner = httpQueryRunner;

            _moduleProcessors = new Dictionary<string, RequestDelegate>();
            _moduleMetaData = new Dictionary<(string verb, string path), RouteMetaData>();
        }
        
        public void RegisterProcessor<TReq, TResp, TProcessor>(string pattern)
            where TProcessor : class, IQueryProcessor<TReq, TResp>
        {
            _moduleProcessors.Add(pattern, _httpQueryRunner.Run<TReq, TResp, TProcessor>);
            _moduleMetaData.Add(
                ("POST", pattern),
                GetMetaData<TReq, TResp, TProcessor>(_moduleName)
            );
        }

        public void Commit(string metadataPattern, OpenApiSpecVersion specVersion, IEndpointRouteBuilder endpointRouteBuilder)
        {
            foreach (var moduleProcessor in _moduleProcessors)
            {
                endpointRouteBuilder.MapPost(moduleProcessor.Key, moduleProcessor.Value);
            }
            
            endpointRouteBuilder.MapGet(
                metadataPattern,
                CarterOpenApi.BuildOpenApiResponse(
                    new OpenApiOptions { DocumentTitle = $"{_moduleName} OpenAPI definition" },
                    _moduleMetaData,
                    specVersion
                )
            );
        }

        private static RouteMetaData GetMetaData<TReq, TResp, TProcessor>(string tag)
            where TProcessor: class, IQueryProcessor<TReq, TResp>
        {
            return new RouteMetaData
            {
                Tag = tag,
                Requests = new[] {new RouteMetaDataRequest {Request = typeof(TReq)}},
                Responses = new[] {new RouteMetaDataResponse {Code = 200, Response = typeof(TResp)}},
                OperationId = typeof(TProcessor).Name.Replace("Processor", "")
            };
        }
    }
}