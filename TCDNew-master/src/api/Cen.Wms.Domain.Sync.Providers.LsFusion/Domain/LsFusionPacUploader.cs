using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Errors;
using Cen.Common.Http.Client;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Domain.Sync.Abstract;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos;
using NodaTime;
using Serilog;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Domain
{
    public class LsFusionPacUploader: IPacUploader
    {
        private readonly ILogger _logger;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly SyncProvidersLsFusionOptions _syncProvidersLsFusionOptions;
        private readonly string _basicAuthenticationHeaderEncodedValue;
        private readonly IClock _clock;

        public LsFusionPacUploader(ILogger logger, HttpQueryCall httpQueryCall, SyncProvidersLsFusionOptions syncProvidersLsFusionOptions, IClock clock)
        {
            _logger = logger;
            _httpQueryCall = httpQueryCall;
            _syncProvidersLsFusionOptions = syncProvidersLsFusionOptions;
            _clock = clock;
            _basicAuthenticationHeaderEncodedValue = BasicAuthHelper.GetBasicAuthHeaderString(
                _syncProvidersLsFusionOptions.WMSServiceBaseLogin,
                _syncProvidersLsFusionOptions.WMSServiceBasePassword
            );
        }

        public async Task<RpcResponse<object>> Upload(IEnumerable<PacHeadRow> pacHeadRows, string purchaseTaskCode)
        {
            LsFusionPacPostResponse response = null;
            Exception exception = null;
            try
            {
                var pacResultExtArray = pacHeadRows.Select(PacToPacResultExt).ToArray();

                using (var requestLogStream = new FileStream(GetRequestLogFileName(purchaseTaskCode), FileMode.CreateNew))
                {
                    response = await _httpQueryCall.RunRaw<PacResultExt[], LsFusionPacPostResponse>(
                        pacResultExtArray,
                        $"{_syncProvidersLsFusionOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.pack_post",
                        _syncProvidersLsFusionOptions.TimeoutMs,
                        _basicAuthenticationHeaderEncodedValue,
                        requestLogStream
                    );
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.Error(exception, "UploadPac: {pacHeadRow.ExtId}");
            }
            
            if (exception != null)
                return RpcResponse<object>.WithError(null, CommonErrors.Exception(exception));

            if (response == null)
                return RpcResponse<object>.WithError(null, CommonErrors.ExternalSystemError(null));

            if (response.Success != 1)
            {
                _logger.Warning($"UploadPac: {response.ErrorText}");
                return RpcResponse<object>.WithError(null, CommonErrors.ExternalSystemError(response.ErrorText));
            }
            
            return RpcResponse<object>.WithSuccess(null);
        }

        private string GetRequestLogFileName(string purchaseTaskCode)
        {
            return Path.Combine(
                _syncProvidersLsFusionOptions.SyncLogsFolderPath,
                string.Format("{0} - {1}",
                    _clock.GetCurrentInstant().InUtc().LocalDateTime.ToString("s", CultureInfo.InvariantCulture),
                    purchaseTaskCode
                )
            );
        }

        private PacResultExt PacToPacResultExt(PacHeadRow pacHeadRow)
        {
            var pacResultExt = new PacResultExt
            {
                PacId = pacHeadRow.ExtId,
                PacStatus = "ACCEPTED",
                //PacStatus = pacHeadRow.PacState.IsProcessed ? "ACCEPTED" : "DECLINED",
                ChangedAt = pacHeadRow.ChangedAt,
                StartedAt = pacHeadRow.StartedAt ?? Instant.FromUnixTimeMilliseconds(0),
                ResponsibleUserId = pacHeadRow.ResponsibleUser?.ExtId ?? string.Empty,
                Lines = new List<PacResultLineExt>()
            };
            if (pacHeadRow.PacState.IsProcessed)
            {
                foreach (var pacLineRow in pacHeadRow.Lines)
                {
                    foreach (var pacLineStateRow in pacLineRow.PacLineStates)
                    {
                        pacResultExt.Lines.Add(
                            new PacResultLineExt
                            {
                                PacLineId = pacLineRow.ExtId,
                                ProductId = pacLineRow.ProductId,
                                QtyExpected = pacLineRow.QtyExpected,
                                QtyNormal = pacLineStateRow.QtyNormal,
                                QtyBroken = pacLineStateRow.QtyBroken,
                                ProdDate = 
                                    pacLineStateRow.ExpirationDate.HasValue
                                        ? pacLineStateRow.ExpirationDaysPlus > 0 
                                            ? pacLineStateRow.ExpirationDate
                                            : null
                                        : null,
                                ExpDays = pacLineStateRow.ExpirationDaysPlus,
                                ExpDate = 
                                    pacLineStateRow.ExpirationDate.HasValue
                                        ? pacLineStateRow.ExpirationDaysPlus > 0 
                                            ? pacLineStateRow.ExpirationDate.Value.Plus(Period.FromDays(pacLineStateRow.ExpirationDaysPlus))
                                            : pacLineStateRow.ExpirationDate
                                        : null,
                                PalletCode = pacLineStateRow.PalletCode
                            }
                        );
                    }
                }
            }

            return pacResultExt;
        }
    }
}