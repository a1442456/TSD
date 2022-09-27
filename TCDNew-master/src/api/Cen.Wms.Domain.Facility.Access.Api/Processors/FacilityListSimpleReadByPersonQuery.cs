using System.Linq;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cen.Wms.Domain.Facility.Access.Api.Processors
{
    public class FacilityListSimpleReadByPersonQuery : IQueryProcessor<object, RpcResponse<ViewModelSimple[]>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;

        public FacilityListSimpleReadByPersonQuery(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, 
            WmsContext wmsContext
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<ViewModelSimple[]>> Run(IUserIdProvider userIdProvider, object request)
        {
            var facilitiesList =
                await _wmsContext.Facility
                    .Join(
                        _wmsContext.FacilityAccess.Where(fa => fa.UserId == userIdProvider.UserGuid),
                        f => f.Id,
                        fa => fa.FacilityId,
                        (f, fa) => f
                    )
                    .Select(
                        f => new ViewModelSimple {Id = f.Id.ToString(), Code = f.ExtId, Name = f.Name}
                    )
                    .ToArrayAsync();

            return RpcResponse<ViewModelSimple[]>.WithSuccess(facilitiesList);
        }
    }
}