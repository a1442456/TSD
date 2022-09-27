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
    public class SyncPacsDownloadProcessor: IQueryProcessor<SyncPacsDownloadReq, RpcResponse<SyncResp>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly ISyncSession _syncSession;
        private readonly ISyncPositionProvider _syncPositionProvider;
        private readonly ISyncPositionsStore _syncPositionsStore;
        private readonly ISyncSource<ReqPacInterval, PacExt> _syncSourcePacExt;
        private readonly ISyncDestination<ReqPacInterval, PacExt> _syncDestinationPacExt;

        public SyncPacsDownloadProcessor(
            ILogger logger,  UnitOfWork<WmsContext> unitOfWork,
            ISyncSession syncSession, ISyncPositionProvider syncPositionProvider, ISyncPositionsStore syncPositionsStore,
            ISyncSource<ReqPacInterval, PacExt> syncSourcePacExt, ISyncDestination<ReqPacInterval, PacExt> syncDestinationPacExt
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _syncSession = syncSession;
            _syncPositionProvider = syncPositionProvider;
            _syncPositionsStore = syncPositionsStore;
            _syncSourcePacExt = syncSourcePacExt;
            _syncDestinationPacExt = syncDestinationPacExt;
        }
        
        public async Task<RpcResponse<SyncResp>> Run(IUserIdProvider userIdProvider, SyncPacsDownloadReq request)
        {
            await _syncSession.StoreSessionStart();
            await _syncDestinationPacExt.WriteSource(
                _syncSession, _syncPositionProvider, _syncPositionsStore, "PacExt", 
                _syncSourcePacExt,
                new ReqPacInterval { PacDateTimeFrom = request.PacDateTimeFrom, PacDateTimeTo = request.PacDateTimeTo }
            );
            await _syncSession.StoreSessionFinish();

            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
                
            return RpcResponse<SyncResp>.WithSuccess(
                new SyncResp { IsSuccessful = true }
            );
        }
    }
}