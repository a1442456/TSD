using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;
using Serilog;

namespace Cen.Wms.Domain.Device.Manage.Api.Processors
{
    public class DeviceStateGetProcessor: IQueryProcessor<DeviceIdentifierDto, RpcResponse<DeviceStatus>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IDeviceRepository _deviceRepository;

        public DeviceStateGetProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, IDeviceRepository deviceRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _deviceRepository = deviceRepository;
        }

        public async Task<RpcResponse<DeviceStatus>> Run(IUserIdProvider userIdProvider, DeviceIdentifierDto request)
        {
            var deviceStatusGetResult = await _deviceRepository.DeviceStateGet(request);
            
            _unitOfWork.Rollback();

            return deviceStatusGetResult;
        }
    }
}