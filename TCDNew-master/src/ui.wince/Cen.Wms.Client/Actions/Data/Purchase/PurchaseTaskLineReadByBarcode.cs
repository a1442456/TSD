using System;
using System.Linq;
using System.Net;
using Cen.Wms.Client.Actions.UI.Utility;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Models.Dtos;
using Cen.Wms.Client.Models.Exceptions;
using Cen.Wms.Client.Services;
using NLog;

namespace Cen.Wms.Client.Actions.Data.Purchase
{
    class PurchaseTaskLineReadByBarcode
    {
        public static PurchaseTaskLineDto Run(string purchaseTaskId, string barcode, string currentPalletCode)
        {
            PurchaseTaskLineDto result = null;

            try
            {
                if (!GStateProvider.Instance.StateAuth.IsAuthorised)
                    throw new UnauthorizedAccessException();

                result = RpcService.Instance.PurchaseTaskLineReadByBarcode(
                    new PurchaseTaskLineReadByBarcodeReq
                    {
                        PurchaseTaskId = purchaseTaskId,
                        Barcode = barcode,
                        CurrentPalletCode = currentPalletCode
                    },
                    GStateProvider.Instance.StateAuth.AuthToken
                );
            }
            catch (RpcException exception)
            {
                ShowModalMessage.Run(Messages.ErrorServer, string.Join(Environment.NewLine, exception.Errors.Select(e => e.ErrorText).ToArray()));
            }
            catch (WebException exception)
            {
                var logger = LogManager.GetLogger(Messages.LoggerNetName);
                logger.Error(exception);

                ShowModalMessage.Run(Messages.ErrorTransport, Messages.ErrorPurchaseTaskLineReadByBarcode);
            }
            catch (Exception exception)
            {
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);

                ShowModalMessage.Run(Messages.ErrorUnknown, Messages.ErrorPurchaseTaskLineReadByBarcode);
            }

            return result;
        }
    }
}
