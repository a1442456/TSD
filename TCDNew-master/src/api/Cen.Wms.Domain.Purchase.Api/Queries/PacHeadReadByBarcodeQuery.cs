using System;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PacHeadReadByBarcodeQuery: IQueryProcessor<PacHeadReadByBarcodeReq, RpcResponse<PacHeadDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPacRepository _pacRepository;
        private readonly IFacilityRepository _facilityRepository;

        public PacHeadReadByBarcodeQuery(IMapper mapper, IPacRepository pacRepository, IFacilityRepository facilityRepository)
        {
            _mapper = mapper;
            _pacRepository = pacRepository;
            _facilityRepository = facilityRepository;
        }
        
        public async Task<RpcResponse<PacHeadDto>> Run(IUserIdProvider userIdProvider, PacHeadReadByBarcodeReq request)
        {
            var pacIdResult = await _pacRepository.PacIdByExtId(request.Barcode);
            if (!pacIdResult.IsSuccess)
                return RpcResponse<PacHeadDto>.WithErrors(null, pacIdResult.Errors);
            if (pacIdResult.Data == Guid.Empty)
                return RpcResponse<PacHeadDto>.WithSuccess(null);

            var pacIsBusyResult = await _pacRepository.PacGetBusy(new ByIdReq { Id = pacIdResult.Data });
            if (!pacIsBusyResult.IsSuccess)
                return RpcResponse<PacHeadDto>.WithErrors(null, pacIsBusyResult.Errors);
            if (pacIsBusyResult.Data)
                return RpcResponse<PacHeadDto>.WithError(null, CommonErrors.InvalidOperation);
            
            var pacReadResult = await _pacRepository.PacRead(new ByIdReq { Id = pacIdResult.Data });
            if (!pacReadResult.IsSuccess)
                return RpcResponse<PacHeadDto>.WithErrors(null, pacReadResult.Errors);
            if (pacReadResult.Data == null)
                return RpcResponse<PacHeadDto>.WithError(null, CommonErrors.NotFound("КЛП"));

            var facilityReadResult = await _facilityRepository.FacilityRead(new ByIdReq { Id = request.FacilityId });
            if (!facilityReadResult.IsSuccess)
                return RpcResponse<PacHeadDto>.WithErrors(null, facilityReadResult.Errors);
            if (facilityReadResult.Data == null)
                return RpcResponse<PacHeadDto>.WithError(null, CommonErrors.NotFound("ТО"));

            if (facilityReadResult.Data.ExtId != pacReadResult.Data.FacilityId)
                return RpcResponse<PacHeadDto>.WithError(null, CommonErrors.NotFound("КЛП на текущем ТО"));
            
            var pacHead = _mapper.Map<PacHeadDto>(pacReadResult.Data);
            return RpcResponse<PacHeadDto>.WithSuccess(pacHead);
        }
    }
}