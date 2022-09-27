using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Config.Abstract;
using Cen.Wms.Domain.Facility.Config.Api.Dtos;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Facility.Config.Api.Processors
{
    public class FacilityConfigSetProcessor: IQueryProcessor<FacilityConfigSetReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityConfigRepository _facilityConfigRepository;

        public FacilityConfigSetProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, 
            IFacilityRepository facilityRepository, IFacilityConfigRepository facilityConfigRepository
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _facilityRepository = facilityRepository;
            _facilityConfigRepository = facilityConfigRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, FacilityConfigSetReq request)
        {
            var facilityExistsResult = await _facilityRepository.FacilityExists(request.FacilityId);
            if (!facilityExistsResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, facilityExistsResult.Errors);
            if (!facilityExistsResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("торговый объект"));

            var facilityConfigSetResult = await _facilityConfigRepository.FacilityConfigSet(request.FacilityId, request.FacilityConfigEditModel);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return facilityConfigSetResult;
        }
    }
}