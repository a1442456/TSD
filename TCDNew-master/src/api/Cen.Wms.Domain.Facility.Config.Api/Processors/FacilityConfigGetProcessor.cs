using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Config.Abstract;
using Cen.Wms.Domain.Facility.Config.Models;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Facility.Config.Api.Processors
{
    public class FacilityConfigGetProcessor: IQueryProcessor<ByIdReq, RpcResponse<FacilityConfigEditModel>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityConfigRepository _facilityConfigRepository;

        public FacilityConfigGetProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, 
            IFacilityRepository facilityRepository, IFacilityConfigRepository facilityConfigRepository
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _facilityRepository = facilityRepository;
            _facilityConfigRepository = facilityConfigRepository;
        }

        public async Task<RpcResponse<FacilityConfigEditModel>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var facilityExistsResult = await _facilityRepository.FacilityExists(request);
            if (!facilityExistsResult.IsSuccess)
                return RpcResponse<FacilityConfigEditModel>.WithErrors(null, facilityExistsResult.Errors);
            if (!facilityExistsResult.Data)
                return RpcResponse<FacilityConfigEditModel>.WithError(null, CommonErrors.NotFound("торговый объект"));

            var facilityConfigGetResult = await _facilityConfigRepository.FacilityConfigGet(request);
            
            _unitOfWork.Rollback();
            
            return facilityConfigGetResult;
        }
    }
}