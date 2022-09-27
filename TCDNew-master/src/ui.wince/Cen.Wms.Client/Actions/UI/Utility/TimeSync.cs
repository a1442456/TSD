using System;
using System.Linq;
using System.Net;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Models.Exceptions;
using Cen.Wms.Client.Services;
using Cen.Wms.Client.Utils;
using NLog;

namespace Cen.Wms.Client.Actions.UI.Utility
{
    class TimeSync
    {
        public static bool Run()
        {
            bool result = false;

            try
            {
                TimeUtils.SetNow(RpcService.Instance.TimeRead());

                result = true;
            }
            catch (RpcException exception)
            {
                ShowModalMessage.Run(Messages.ErrorServer, string.Join(Environment.NewLine, exception.Errors.Select(e => e.ErrorText).ToArray()));
            }
            catch (WebException exception)
            {
                var logger = LogManager.GetLogger(Messages.LoggerNetName);
                logger.Error(exception);

                ShowModalMessage.Run(Messages.ErrorTransport, Messages.ErrorSyncTime);
            }
            catch (Exception exception)
            {
                var logger = LogManager.GetLogger(Messages.LoggerLocalName);
                logger.Error(exception);

                ShowModalMessage.Run(Messages.ErrorUnknown, Messages.ErrorSyncTime);
            }

            return result;
        }
    }
}
