using System.Linq;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Sync.Abstract;
using Cen.Wms.Domain.Sync.Api.Dtos;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cen.Wms.Domain.Sync.Api.Processors
{
    public class SyncPacsUploadBatchProcessor: IQueryProcessor<SyncPacsUploadReq, RpcResponse<SyncResp>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;
        private readonly IPacUploader _pacUploader;

        public SyncPacsUploadBatchProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, WmsContext wmsContext, IPacUploader pacUploader)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _pacUploader = pacUploader;
        }
        
        public async Task<RpcResponse<SyncResp>> Run(IUserIdProvider userIdProvider, SyncPacsUploadReq request)
        {
            var pacsToUpload = 
                _wmsContext.PacHead
                    .Include(e => e.ResponsibleUser)
                    .Include(e => e.PacState)
                    .Include(e => e.Lines)
                    .ThenInclude(e => e.PacLineStates)
                    .Where(e => e.PacState.IsBusy && e.PacState.IsProcessed && !e.PacState.IsExported)
                    .ToList();
            
            foreach (var pacToUpload in pacsToUpload)
            {
                var uploadPacResult = await _pacUploader.Upload(new [] {pacToUpload}, string.Empty);
                if (uploadPacResult.IsSuccess)
                {
                    pacToUpload.PacState.IsExported = true;
                    _wmsContext.PacState.Update(pacToUpload.PacState);
                }
            }

            await _wmsContext.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return RpcResponse<SyncResp>.WithSuccess(new SyncResp());
        }
    }
}