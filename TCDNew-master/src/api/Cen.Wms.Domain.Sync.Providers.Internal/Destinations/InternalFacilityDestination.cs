using System;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.Domain.Models;
using Cen.Common.Sync.Abstract;
using Cen.Wms.Domain.Facility.Config.Abstract;
using Cen.Wms.Domain.Facility.Config.Models;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Facility.Manage.Models;
using Cen.Wms.Domain.Sync.Models;
using Serilog;

namespace Cen.Wms.Domain.Sync.Providers.Internal.Destinations
{
    public class InternalFacilityDestination: SyncDestination<object, FacilityExt>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityConfigRepository _facilityConfigRepository;

        public InternalFacilityDestination(
            ILogger logger, IMapper mapper, 
            IFacilityRepository facilityRepository, IFacilityConfigRepository facilityConfigRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _facilityRepository = facilityRepository;
            _facilityConfigRepository = facilityConfigRepository;
        }
        
        protected override int GetProgressStep()
        {
            return 10;
        }

        protected override Task PreProcess()
        {
            return Task.CompletedTask;
        }

        protected override async Task WriteItem(FacilityExt item, int syncSessionId)
        {
            var facilityIdResult = await _facilityRepository.FacilityIdByExtId(item.FacilityId);
            if (!facilityIdResult.IsSuccess)
                throw new Exception();
            
            var facility = _mapper.Map<FacilityEditModel>(item);
            facility.Id = facilityIdResult.Data;
            var facilityStoreResult = await _facilityRepository.FacilityStore(facility);
            if (!facilityStoreResult.IsSuccess)
                throw new Exception();
            
            var facilityConfigSetResult = await _facilityConfigRepository.FacilityConfigSetIfNotExists(
                new ByIdReq { Id = facilityStoreResult.Data },
                FacilityConfigEditModel.GetDefault()
            );
            if (!facilityConfigSetResult.IsSuccess)
                throw new Exception();
        }

        protected override Task PostProcess()
        {
            return Task.CompletedTask;
        }
    }
}