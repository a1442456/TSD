using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Access.Abstract;
using Cen.Wms.Domain.Facility.Access.Api.Dtos;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.User.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Facility.Access.Api.Processors
{
    public class FacilityAccessGrantProcessor: IQueryProcessor<FacilityAccessEditReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityAccessRepository _facilityAccessRepository;

        public FacilityAccessGrantProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IFacilityRepository facilityRepository, IFacilityAccessRepository facilityAccessRepository, IUserRepository userRepository
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _facilityRepository = facilityRepository;
            _facilityAccessRepository = facilityAccessRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, FacilityAccessEditReq request)
        {
            var facilityExistsResult = await _facilityRepository.FacilityExists(request.FacilityId);
            if (!facilityExistsResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, facilityExistsResult.Errors);
            if (!facilityExistsResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("торговый объект"));
            
            var userExistsResult = await _userRepository.UserExists(request.UserId);
            if (!userExistsResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, userExistsResult.Errors);
            if (!userExistsResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("пользователь"));

            var facilityAccessIsGrantedResult = await _facilityAccessRepository.FacilityAccessIsGranted(request.FacilityId, request.UserId);
            if (!facilityAccessIsGrantedResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, facilityAccessIsGrantedResult.Errors);
            if (facilityAccessIsGrantedResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            var facilityAccessGrantResult = 
                await _facilityAccessRepository.FacilityAccessGrant(request.FacilityId, request.UserId);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return facilityAccessGrantResult;
        }
    }
}