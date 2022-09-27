using Cen.Common.CQRS;

namespace Cen.Wms.Domain.Purchase
{
    public static class PurchaseErrors
    {
        public static readonly RpcError PurchaseTaskLineQuantityIsExceeded = new RpcError { ErrorCode = "PURCH000", ErrorText = "Превышено максимальное количество!" };
    }
}