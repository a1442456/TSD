using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Sync.Abstract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cen.Wms.Domain.Sync.Api.Processors
{
    public class SyncPacsUploadSingleProcessor : IQueryProcessor<ByIdReq, RpcResponse<object>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;
        private readonly IPacUploader _pacUploader;

        public SyncPacsUploadSingleProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, WmsContext wmsContext, IPacUploader pacUploader)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _pacUploader = pacUploader;
        }
        
        public async Task<RpcResponse<object>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var pacToUpload = 
                await _wmsContext.PacHead
                    .Include(e => e.ResponsibleUser)
                    .Include(e => e.PacState)
                    .Include(e => e.Lines)
                    .ThenInclude(e => e.PacLineStates)
                    .FirstOrDefaultAsync(e => e.PacState.IsBusy && e.PacState.IsProcessed && !e.PacState.IsExported && e.Id == request.Id);

            if (pacToUpload != null)
            {
                var uploadPacResult = await _pacUploader.Upload(new [] {pacToUpload}, string.Empty);
                if (!uploadPacResult.IsSuccess)
                    return RpcResponse<object>.WithErrors(null, uploadPacResult.Errors);

                pacToUpload.PacState.IsExported = true;
                _wmsContext.PacState.Update(pacToUpload.PacState);
            }

            await _wmsContext.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return RpcResponse<object>.WithSuccess(null);
        }
    }
}