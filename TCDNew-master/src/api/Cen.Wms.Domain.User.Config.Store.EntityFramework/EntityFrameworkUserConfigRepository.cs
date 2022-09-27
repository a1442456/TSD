using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.User;
using Cen.Wms.Domain.User.Config.Abstract;
using Cen.Wms.Domain.User.Config.Models;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.User.Config.Store.EntityFramework
{
    public class EntityFrameworkUserConfigRepository: IUserConfigRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkUserConfigRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }
        
        public async Task<RpcResponse<UserConfigEditModel>> UserConfigGet(ByIdReq userId)
        {
            var userConfigRow = await _wmsContext.UserConfig.FirstOrDefaultAsync(e => e.Id == userId.Id);
            var userConfig = _mapper.Map<UserConfigEditModel>(userConfigRow);
            
            return RpcResponse<UserConfigEditModel>.WithSuccess(userConfig);
        }

        public async Task<RpcResponse<bool>> UserConfigSet(ByIdReq userId, UserConfigEditModel userConfigEditModel)
        {
            var userConfigRow = await _wmsContext.UserConfig.FirstOrDefaultAsync(e => e.Id == userId.Id);
            if (userConfigRow == null)
            {
                if (userConfigEditModel == null)
                    return RpcResponse<bool>.WithSuccess(true);
                
                userConfigRow = new UserConfigRow { Id = userId.Id, DefaultFacilityId = userConfigEditModel.DefaultFacilityId };
                await _wmsContext.UserConfig.AddAsync(userConfigRow);
            }
            else
            {
                if (userConfigEditModel == null)
                {
                    _wmsContext.UserConfig.Remove(userConfigRow);
                }
                else
                {
                    userConfigRow.DefaultFacilityId = userConfigEditModel.DefaultFacilityId;
                    _wmsContext.UserConfig.Update(userConfigRow);                    
                }
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }
    }
}