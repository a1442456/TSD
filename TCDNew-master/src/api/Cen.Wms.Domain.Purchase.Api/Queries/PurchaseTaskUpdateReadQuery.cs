using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Purchase.Enums;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskUpdateReadQuery: IQueryProcessor<PurchaseTaskUpdateReadReq, RpcResponse<PurchaseTaskUpdateDto>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskUpdateReadQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<PurchaseTaskUpdateDto>> Run(IUserIdProvider userIdProvider,
            PurchaseTaskUpdateReadReq request)
        {
            var response = new PurchaseTaskUpdateDto {PurchaseTaskId = request.PurchaseTaskId};

            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Pallets)
                .Include(e => e.PacHeads)
                .Include(e => e.Users)
                .Include(e => e.Lines)
                .ThenInclude(e => e.PurchaseTaskLineState)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == request.PurchaseTaskId);
            
            if (purchaseTask == null)
                return RpcResponse<PurchaseTaskUpdateDto>.WithError(null, CommonErrors.NotFound("задание"));

            if (purchaseTask.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<PurchaseTaskUpdateDto>.WithError(null, CommonErrors.InvalidOperation);

            var userIncludedInTask = purchaseTask.Users.Any(e => e.UserId == userIdProvider.UserGuid);
            if (!(purchaseTask.IsPubliclyAvailable || userIncludedInTask))
                return RpcResponse<PurchaseTaskUpdateDto>.WithError(null, CommonErrors.AccessDenied);
            
            response.PurchaseTaskVersion = purchaseTask.ChangedAt.ToUnixTimeMilliseconds();
            var linesModified = 
                purchaseTask.Lines.Where(e =>
                    e.ChangedAt.ToUnixTimeMilliseconds() > request.PurchaseTaskVersion
                    || e.PurchaseTaskLineState.ChangedAt.ToUnixTimeMilliseconds() > request.PurchaseTaskVersion
                );
            var palletsModified = 
                purchaseTask.Pallets.Where(e => e.ChangedAt.ToUnixTimeMilliseconds() > request.PurchaseTaskVersion);

            // TODO: replace by automapper
            response.LinesUpdated = linesModified.Select(e => new PurchaseTaskLineDto
            {
                Id = e.Id.Value,
                Quantity = e.Quantity,
                Product = new ProductDto { Id = e.ProductExtId, Abc = e.ProductAbc, Barcodes = e.ProductBarcodes, Name = e.ProductName },
                State =
                    new PurchaseTaskLineStateDto
                    {
                        ExpirationDate = e.PurchaseTaskLineState.ExpirationDate?.AtMidnight().InUtc().ToInstant(),
                        ExpirationDaysPlus = e.PurchaseTaskLineState.ExpirationDaysPlus,
                        QtyNormal = e.PurchaseTaskLineState.QtyNormal,
                        QtyBroken = e.PurchaseTaskLineState.QtyBroken
                    }
            }).ToList();
            response.PalletsUpdated = palletsModified.Select(e => new PurchaseTaskPalletDto()
            {
                Code = e.Code,
                Abc = e.Abc
            }).ToList();

            return RpcResponse<PurchaseTaskUpdateDto>.WithSuccess(response);
        }
    }
}