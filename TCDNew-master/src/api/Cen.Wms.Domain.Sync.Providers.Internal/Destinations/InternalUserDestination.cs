using System;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.Domain.Models;
using Cen.Common.Sync.Abstract;
using Cen.Wms.Domain.Facility.Access.Abstract;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.User.Manage.Abstract;
using Cen.Wms.Domain.User.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.Sync.Providers.Internal.Destinations
{
    public class InternalUserDestination: SyncDestination<object, UserExt>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityAccessRepository _facilityAccessRepository;

        public InternalUserDestination(ILogger logger, IMapper mapper, IUserRepository userRepository, IFacilityAccessRepository facilityAccessRepository, IFacilityRepository facilityRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _facilityAccessRepository = facilityAccessRepository;
            _facilityRepository = facilityRepository;
        }
        
        protected override int GetProgressStep()
        {
            return 10;
        }

        protected override Task PreProcess()
        {
            return Task.CompletedTask;
        }

        protected override async Task WriteItem(UserExt item, int syncSessionId)
        {
            var userIdResult = await _userRepository.UserIdByExtId(item.UserId);
            if (!userIdResult.IsSuccess)
                throw new Exception();
            
            var user = _mapper.Map<UserEditModel>(item);
            user.Id = userIdResult.Data;
            var userStoreResult = await _userRepository.UserStore(user);
            if (!userStoreResult.IsSuccess)
                throw new Exception();

            var facilityIdResult = await _facilityRepository.FacilityIdByExtId(item.FacilityId);
            if (!facilityIdResult.IsSuccess)
                throw new Exception();

            if (facilityIdResult.Data != Guid.Empty)
            {
                var switchToResult =
                    await _facilityAccessRepository.DefaultFacilityAccessSwitchTo(
                        new ByIdReq { Id = facilityIdResult.Data },
                        new ByIdReq { Id = userStoreResult.Data }
                    );
                if (!switchToResult.IsSuccess)
                    throw new Exception();                
            }
        }

        protected override Task PostProcess()
        {
            return Task.CompletedTask;
        }
    }
}