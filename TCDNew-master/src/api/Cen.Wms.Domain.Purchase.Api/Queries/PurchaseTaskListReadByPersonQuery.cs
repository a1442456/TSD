using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Purchase.Enums;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Cen.Wms.Domain.Purchase.Models;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    /// <summary>
    /// Получаем список доступных заданий (активных, которым присвоен текущий пользователь)
    /// </summary>
    public class PurchaseTaskListReadByPersonQuery: IQueryProcessor<PurchaseTaskListReadByPersonReq, RpcResponse<IEnumerable<PurchaseTaskDto>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskListReadByPersonQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<IEnumerable<PurchaseTaskDto>>> Run(IUserIdProvider userIdProvider,
            PurchaseTaskListReadByPersonReq request)
        {
            var purchaseTaskDtoList = await _wmsContext.PurchaseTaskHead
                .Include(e => e.CreatedByUser)
                .Where(e => 
                    e.FacilityId == request.FacilityId 
                    && e.PurchaseTaskState == PurchaseTaskState.InProgress 
                    && (e.IsPubliclyAvailable || e.Users.Any(u => u.UserId == userIdProvider.UserGuid))
                )
                .ProjectTo<PurchaseTaskHeadListModel>(_mapper.ConfigurationProvider)
                .ProjectTo<PurchaseTaskDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return RpcResponse<IEnumerable<PurchaseTaskDto>>.WithSuccess(purchaseTaskDtoList);
        }
    }
}