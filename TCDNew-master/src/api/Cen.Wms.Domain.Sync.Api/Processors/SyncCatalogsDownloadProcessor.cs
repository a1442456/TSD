using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Sync.Api.Dtos;
using Cen.Wms.Domain.Sync.Models;
using Serilog;

namespace Cen.Wms.Domain.Sync.Api.Processors
{
    public class SyncCatalogsDownloadProcessor: IQueryProcessor<SyncCatalogsReq, RpcResponse<SyncResp>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly ISyncSession _syncSession;
        private readonly ISyncPositionProvider _syncPositionProvider;
        private readonly ISyncPositionsStore _syncPositionsStore;
        private readonly ISyncSource<object, FacilityExt> _syncSourceFacility;
        private readonly ISyncDestination<object, FacilityExt> _syncDestinationFacility;
        private readonly ISyncSource<object, UserExt> _syncSourceUser;
        private readonly ISyncDestination<object, UserExt> _syncDestinationUser;

        public SyncCatalogsDownloadProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            ISyncSession syncSession, ISyncPositionProvider syncPositionProvider, ISyncPositionsStore syncPositionsStore, 
            ISyncSource<object, FacilityExt> syncSourceFacility, ISyncDestination<object, FacilityExt> syncDestinationFacility,
            ISyncSource<object, UserExt> syncSourceUser, ISyncDestination<object, UserExt> syncDestinationUser)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _syncSession = syncSession;
            _syncPositionProvider = syncPositionProvider;
            _syncPositionsStore = syncPositionsStore;
            _syncSourceFacility = syncSourceFacility;
            _syncDestinationFacility = syncDestinationFacility;
            _syncSourceUser = syncSourceUser;
            _syncDestinationUser = syncDestinationUser;
        }
        
        public async Task<RpcResponse<SyncResp>> Run(IUserIdProvider userIdProvider, SyncCatalogsReq request)
        {
            await _syncSession.StoreSessionStart();
            await _unitOfWork.Context.SaveChangesAsync();
            
            await _syncDestinationFacility.WriteSource(_syncSession, _syncPositionProvider, _syncPositionsStore, "Facility", _syncSourceFacility, null);
            await _unitOfWork.Context.SaveChangesAsync();
            
            await _syncDestinationUser.WriteSource(_syncSession, _syncPositionProvider, _syncPositionsStore, "User", _syncSourceUser, null);
            await _unitOfWork.Context.SaveChangesAsync();
            
            await _syncSession.StoreSessionFinish();
            await _unitOfWork.Context.SaveChangesAsync();
            
            _unitOfWork.Commit();

            return RpcResponse<SyncResp>.WithSuccess(
                new SyncResp { IsSuccessful = true }
            );
        }
    }
}