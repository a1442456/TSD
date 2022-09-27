using System;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.Sync.Abstract;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Models;
using Cen.Wms.Domain.Sync.Models;
using Serilog;

namespace Cen.Wms.Domain.Sync.Providers.Internal.Destinations
{
    public class InternalPacDestination: SyncDestination<ReqPacInterval, PacExt>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPacRepository _pacRepository;

        public InternalPacDestination(ILogger logger, IMapper mapper, IPacRepository pacRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _pacRepository = pacRepository;
        }
        
        protected override int GetProgressStep()
        {
            return 10;
        }

        protected override Task PreProcess()
        {
            return Task.CompletedTask;
        }

        protected override async Task WriteItem(PacExt item, int syncSessionId)
        {
            var pacIdResult = await _pacRepository.PacIdByExtId(item.PacId);
            if (!pacIdResult.IsSuccess)
                throw new Exception();

            var pac = _mapper.Map<PacHeadEditModel>(item);
            pac.Id = pacIdResult.Data;
            
            var pacStoreResult = await _pacRepository.PacStore(pac);
            if (!pacStoreResult.IsSuccess)
                throw new Exception();
        }

        protected override Task PostProcess()
        {
            return Task.CompletedTask;
        }
    }
}