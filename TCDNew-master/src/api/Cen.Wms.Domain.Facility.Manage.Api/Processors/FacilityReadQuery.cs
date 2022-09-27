using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Manage.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cen.Wms.Domain.Facility.Manage.Api.Processors
{
    public class FacilityReadQuery : IQueryProcessor<ByIdReq, RpcResponse<FacilityListModel>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public FacilityReadQuery(ILogger logger, UnitOfWork<WmsContext> unitOfWork, WmsContext wmsContext, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _mapper = mapper;
        }
        
        public async Task<RpcResponse<FacilityListModel>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var facility = 
                await _wmsContext.Facility
                    .Where(e => e.Id == request.Id)
                    .ProjectTo<FacilityListModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            
            return RpcResponse<FacilityListModel>.WithSuccess(facility);
        }
    }
}