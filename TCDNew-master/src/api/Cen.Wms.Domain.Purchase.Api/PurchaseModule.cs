using System;
using System.Collections.Generic;
using SimpleInjector;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Cen.Wms.Domain.Purchase.Api.Processors;
using Cen.Wms.Domain.Purchase.Api.Profiles;
using Cen.Wms.Domain.Purchase.Api.Queries;
using Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework;
using Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework.Profiles;
using Cen.Wms.Domain.Purchase.Models;
using Cen.Wms.Domain.User.Manage.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Api
{
    public class PurchaseModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IPacRepository, EntityFrameworkPacRepository>(Lifestyle.Scoped);
            container.Register<IPurchaseTaskRepository, EntityFrameworkPurchaseTaskRepository>(Lifestyle.Scoped);
            
            container.Register<TimeReadQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskListReadByPersonQuery>(Lifestyle.Scoped);
            container.Register<PacHeadReadByBarcodeQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskCreateFromPacsProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskCreateEmptyProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskUpdateReadQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskLineReadByBarcodeQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskLineUpdatePostProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskFinishProcessor>(Lifestyle.Scoped);
            
            container.Register<PacHeadListByFacilityIdQuery>(Lifestyle.Scoped);
            container.Register<PacLineListQuery>(Lifestyle.Scoped);
            
            container.Register<PurchaseTaskHeadListByFacilityIdQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskLineListQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskPacListIncludedQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskPacListAvailableQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskPacIncludeProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskPacExcludeProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskUserListIncludedQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskUserListAvailableQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskUserIncludeProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskUserExcludeProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskPalletListQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskLineUpdateListQuery>(Lifestyle.Scoped);
            container.Register<PurchaseTaskStartProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskStopProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskUploadProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskStopAndUploadProcessor>(Lifestyle.Scoped);
            container.Register<PurchaseTaskCancelProcessor>(Lifestyle.Scoped);
        }

        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<PurchaseModule>(httpQueryRunner);
            
            r.RegisterProcessor<object, RpcResponse<Instant>, TimeReadQuery>("/time/read");
            r.RegisterProcessor<PurchaseTaskListReadByPersonReq, RpcResponse<IEnumerable<PurchaseTaskDto>>, PurchaseTaskListReadByPersonQuery>("/purchase/task/list/by_person");
            r.RegisterProcessor<PacHeadReadByBarcodeReq, RpcResponse<PacHeadDto>, PacHeadReadByBarcodeQuery>("/pac/head/read");
            r.RegisterProcessor<PurchaseTaskCreateFromPacsReq, RpcResponse<Guid>, PurchaseTaskCreateFromPacsProcessor>("/purchase/task/create/from_pacs");
            r.RegisterProcessor<PurchaseTaskCreateEmptyReq, RpcResponse<Guid>, PurchaseTaskCreateEmptyProcessor>("/purchase/task/create/empty");
            r.RegisterProcessor<PurchaseTaskUpdateReadReq, RpcResponse<PurchaseTaskUpdateDto>, PurchaseTaskUpdateReadQuery>("/purchase/task/update/read");
            r.RegisterProcessor<PurchaseTaskLineReadByBarcodeReq, RpcResponse<PurchaseTaskLineDto>, PurchaseTaskLineReadByBarcodeQuery>("/purchase/task/line/read/by_barcode");
            r.RegisterProcessor<PurchaseTaskLineUpdatePostReq, RpcResponse<bool>, PurchaseTaskLineUpdatePostProcessor>("/purchase/task/line/update/post");
            r.RegisterProcessor<PurchaseTaskFinishReq, RpcResponse<bool>, PurchaseTaskFinishProcessor>("/purchase/task/finish");
            
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacHeadListModel>>, PacHeadListByFacilityIdQuery>("/pac/list/by_facility_id");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacLineListModel>>, PacLineListQuery>("/pac/line/list");
            
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskHeadListModel>>, PurchaseTaskHeadListByFacilityIdQuery>("/purchase/task/list/by_facility_id");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskLineListModel>>, PurchaseTaskLineListQuery>("/purchase/task/line/list");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacHeadListModel>>, PurchaseTaskPacListIncludedQuery>("/purchase/task/pac/list/included");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacHeadListModel>>, PurchaseTaskPacListAvailableQuery>("/purchase/task/pac/list/available");
            r.RegisterProcessor<PurchaseTaskPacHeadEditReq, RpcResponse<bool>, PurchaseTaskPacIncludeProcessor>("/purchase/task/pac/include");
            r.RegisterProcessor<PurchaseTaskPacHeadEditReq, RpcResponse<bool>, PurchaseTaskPacExcludeProcessor>("/purchase/task/pac/exclude");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<UserListModel>>, PurchaseTaskUserListIncludedQuery>("/purchase/task/user/list/included");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskLineUpdateListModel>>, PurchaseTaskLineUpdateListQuery>("/purchase/task/line/update/list");
            r.RegisterProcessor<TableRowsWithParamReq<PurchaseTaskUserListAvailableReq>, RpcResponse<DataSourceResult<UserListModel>>, PurchaseTaskUserListAvailableQuery>("/purchase/task/user/list/available");
            r.RegisterProcessor<PurchaseTaskUserEditReq, RpcResponse<bool>, PurchaseTaskUserIncludeProcessor>("/purchase/task/user/include");
            r.RegisterProcessor<PurchaseTaskUserEditReq, RpcResponse<bool>, PurchaseTaskUserExcludeProcessor>("/purchase/task/user/exclude");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskPalletListModel>>, PurchaseTaskPalletListQuery>("/purchase/task/pallets/list");
            
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, PurchaseTaskStartProcessor>("/purchase/task/start");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, PurchaseTaskStopProcessor>("/purchase/task/stop");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, PurchaseTaskUploadProcessor>("/purchase/task/upload");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, PurchaseTaskStopAndUploadProcessor>("/purchase/task/stop_n_upload");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, PurchaseTaskCancelProcessor>("/purchase/task/cancel");

            r.Commit("/purchase/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<PurchaseProfile>();
            mapperConfigurationExpression.AddProfile<PacProfile>();
            mapperConfigurationExpression.AddProfile<PurchaseTaskProfile>();
        }
    }
}