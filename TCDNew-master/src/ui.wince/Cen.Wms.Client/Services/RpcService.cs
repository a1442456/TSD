using System;
using System.Collections.Generic;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Models.Dtos;
using Cen.Wms.Client.Models.Exceptions;
using Cen.Wms.Client.Utils;

namespace Cen.Wms.Client.Services {
    
    public class RpcService
    {
        private static RpcService _instance;

        public static RpcService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RpcService();

                return _instance;
            }
        }

        public UserTokenResp UserLogin(UserCredentials request)
        {
            var r = RemoteCall.Invoke<UserCredentials, UserTokenResp>(Urls.UserLoginUrl, request, null);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public List<ViewModelSimple> FacilityListSimpleReadByPerson(string bearer)
        {
            var r = RemoteCall.Invoke<object, List<ViewModelSimple>>(Urls.FacilityListSimpleReadByPersonUrl, null, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public FacilityConfigEditModel FacilityConfigGet(ByIdReq request, String bearer)
        {
            var r = RemoteCall.Invoke<object, FacilityConfigEditModel>(Urls.FacilityConfigGetUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public PacHeadDto PacHeadReadByBarcode(PacHeadReadByBarcodeReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PacHeadReadByBarcodeReq, PacHeadDto>(Urls.PacHeadReadByBarcodeUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public PurchaseTaskDto[] PurchaseTaskListReadByPerson(PurchaseTaskListReadByPersonReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PurchaseTaskListReadByPersonReq, PurchaseTaskDto[]>(Urls.PurchaseTaskListByPersonUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public string PurchaseTaskCreateFromPacs(PurchaseTaskCreateFromPacsReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PurchaseTaskCreateFromPacsReq, string>(Urls.PurchaseTaskCreateFromPacsUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public PurchaseTaskUpdateDto PurchaseTaskUpdateRead(PurchaseTaskUpdateReadReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PurchaseTaskUpdateReadReq, PurchaseTaskUpdateDto>(Urls.PurchaseTaskUpdateReadUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public PurchaseTaskLineDto PurchaseTaskLineReadByBarcode(PurchaseTaskLineReadByBarcodeReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PurchaseTaskLineReadByBarcodeReq, PurchaseTaskLineDto>(Urls.PurchaseTaskLineReadByBarcodeUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public bool PurchaseTaskLineUpdatePost(PurchaseTaskLineUpdatePostReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PurchaseTaskLineUpdatePostReq, bool>(Urls.PurchaseTaskLineUpdatePostUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public bool PurchaseTaskFinish(PurchaseTaskFinishReq request, string bearer)
        {
            var r = RemoteCall.Invoke<PurchaseTaskFinishReq, bool>(Urls.PurchaseTaskFinishUrl, request, bearer);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }

        public DateTime TimeRead()
        {
            var r = RemoteCall.Invoke<object, DateTime>(Urls.TimeReadUrl, null, null);
            if (r.Errors != null)
                if (r.Errors.Length > 0)
                    throw new RpcException(r.Errors);

            return r.Data;
        }
    }
}
