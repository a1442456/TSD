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
    public class PurchaseTaskLineReadByBarcodeQuery : IQueryProcessor<PurchaseTaskLineReadByBarcodeReq, RpcResponse<PurchaseTaskLineDto>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskLineReadByBarcodeQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<PurchaseTaskLineDto>> Run(IUserIdProvider userIdProvider,
            PurchaseTaskLineReadByBarcodeReq request)
        {
            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Pallets)
                .Include(e => e.PacHeads)
                .Include(e => e.Users)
                .Include(e => e.Lines)
                .ThenInclude(e => e.PurchaseTaskLineState)
                .Include(e => e.Lines)
                .ThenInclude(e => e.PurchaseTaskLinePalletedStates)
                .FirstOrDefaultAsync(e => e.Id == request.PurchaseTaskId);
            if (purchaseTask == null)
                return RpcResponse<PurchaseTaskLineDto>.WithError(null, CommonErrors.NotFound("задание"));
            if (purchaseTask.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<PurchaseTaskLineDto>.WithError(null, CommonErrors.InvalidOperation);
            
            var userIsIncludedInTask = purchaseTask.Users.Any(e => e.UserId == userIdProvider.UserGuid);
            if (!(purchaseTask.IsPubliclyAvailable || userIsIncludedInTask))
                return RpcResponse<PurchaseTaskLineDto>.WithError(null, CommonErrors.AccessDenied);
            
            var purchaseTaskLine = purchaseTask.Lines.FirstOrDefault(e => e.ProductBarcodes.Contains(request.Barcode));

            // TODO: replace by automapper
            var result = purchaseTaskLine != null
                ? new PurchaseTaskLineDto
                {
                    Id = purchaseTaskLine.Id.Value,
                    Quantity = purchaseTaskLine.Quantity,
                    Product = new ProductDto
                    {
                        Id = purchaseTaskLine.ProductExtId, Abc = purchaseTaskLine.ProductAbc,
                        Barcodes = purchaseTaskLine.ProductBarcodes, Name = purchaseTaskLine.ProductName
                    },
                    State =
                        new PurchaseTaskLineStateDto
                        {
                            ExpirationDate = purchaseTaskLine.PurchaseTaskLineState.ExpirationDate?.AtMidnight().InUtc().ToInstant(),
                            ExpirationDaysPlus = purchaseTaskLine.PurchaseTaskLineState.ExpirationDaysPlus,
                            QtyNormal = purchaseTaskLine.PurchaseTaskLineState.QtyNormal,
                            QtyBroken = purchaseTaskLine.PurchaseTaskLineState.QtyBroken
                        }
                }
                : null;
            
            return RpcResponse<PurchaseTaskLineDto>.WithSuccess(result);
        }
    }
}