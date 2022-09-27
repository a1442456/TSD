using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Sync.Api.Dtos;
using Cen.Wms.Domain.Sync.Api.Processors;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.Sync.Api
{
    public class SyncModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<SyncCatalogsDownloadProcessor>(Lifestyle.Scoped);
            container.Register<SyncPacsDownloadProcessor>(Lifestyle.Scoped);
            container.Register<SyncPacsUploadBatchProcessor>(Lifestyle.Scoped);
        }

        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<SyncModule>(httpQueryRunner);
            
            r.RegisterProcessor<SyncCatalogsReq, RpcResponse<SyncResp>, SyncCatalogsDownloadProcessor>("/sync/catalogs/download");
            r.RegisterProcessor<SyncPacsDownloadReq, RpcResponse<SyncResp>, SyncPacsDownloadProcessor>("/sync/pacs/download");
            r.RegisterProcessor<SyncPacsUploadReq, RpcResponse<SyncResp>, SyncPacsUploadBatchProcessor>("/sync/pacs/upload/batch");
            r.RegisterProcessor<ByIdReq, RpcResponse<object>, SyncPacsUploadSingleProcessor>("/sync/pacs/upload/single");
            
            r.Commit("/sync/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}