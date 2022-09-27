using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.User;
using Cen.Wms.Domain.User.Manage.Abstract;
using Cen.Wms.Domain.User.Manage.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.User.Manage.Store.EntityFramework
{
    public class EntityFrameworkUserRepository: IUserRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkUserRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }

        public async Task<RpcResponse<Guid>> UserStore(UserEditModel userEditModel)
        {
            Guid result;
            
            if (userEditModel.Id == Guid.Empty)
            {
                var userIdNew = NewId.NextGuid();
                var userRow = new UserRow();
                _mapper.Map(userEditModel, userRow);
                userRow.Id = userIdNew;
                var userStateRow = new UserStateRow();
                userStateRow.Id = userIdNew;
                userStateRow.IsLocked = true;
                
                await _wmsContext.User.AddAsync(userRow);
                await _wmsContext.UserState.AddAsync(userStateRow);

                result = userIdNew;
            }
            else
            {
                var userRow = await _wmsContext.User.FirstOrDefaultAsync(e => e.Id == userEditModel.Id);
                _mapper.Map(userEditModel, userRow);
                
                _wmsContext.User.Update(userRow);
                
                // ReSharper disable once PossibleInvalidOperationException
                result = userRow.Id.Value;
            }

            return RpcResponse<Guid>.WithSuccess(result);
        }

        public async Task<RpcResponse<ViewModelSimple>> UserReadSimple(ByIdReq request)
        {
            var userRow = await _wmsContext.User.FirstOrDefaultAsync(e => e.Id == request.Id);
            var userViewModelSimple =
                userRow != null
                    ? new ViewModelSimple { Id = userRow.Id?.ToString("D"), Code = userRow.Code, Name = userRow.Name }
                    : null;
            return RpcResponse<ViewModelSimple>.WithSuccess(userViewModelSimple);
        }

        public async Task<RpcResponse<bool>> UserExists(ByIdReq request)
        {
            var userExists = await _wmsContext.User.AnyAsync(e => e.Id == request.Id);
            
            return RpcResponse<bool>.WithSuccess(userExists);
        }

        public async Task<RpcResponse<Guid>> UserIdByExtId(string extId)
        {
            var userRow = await _wmsContext.User.FirstOrDefaultAsync(e => e.ExtId == extId);
            
            return RpcResponse<Guid>.WithSuccess(userRow?.Id ?? Guid.Empty);
        }

        public async Task<RpcResponse<UserEditModel>> UserRead(ByIdReq request)
        {
            var userRow = await _wmsContext.User.FirstOrDefaultAsync(e => e.Id == request.Id);
            var userEditModel = _mapper.Map<UserEditModel>(userRow);

            return RpcResponse<UserEditModel>.WithSuccess(userEditModel);
        }

        public async Task<RpcResponse<UserStateEditModel>> UserStateGet(ByIdReq request)
        {
            var userStateRow = await _wmsContext.UserState.FirstOrDefaultAsync(e => e.Id == request.Id);
            var userState = _mapper.Map<UserStateEditModel>(userStateRow);
            
            return RpcResponse<UserStateEditModel>.WithSuccess(userState);
        }

        public async Task<RpcResponse<bool>> UserStateSet(ByIdReq request, UserStateEditModel userState)
        {
            var userStateRow = await _wmsContext.UserState.FirstOrDefaultAsync(e => e.Id == request.Id);
            if (userStateRow == null)
            {
                userStateRow = new UserStateRow();
                _mapper.Map(userState, userStateRow);
                userStateRow.Id = request.Id;
                
                await _wmsContext.UserState.AddAsync(userStateRow);
            }
            else
            {
                _mapper.Map(userState, userStateRow);
                _wmsContext.Update(userStateRow);
            }

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<DataSourceResult<UserListModel>>> UserList(TableRowsReq request)
        {
            var dataSourceRequest = request.GetDataSourceRequest();
            var dataSourceResult = await _wmsContext.User
                .ProjectTo<UserListModel>(_mapper.ConfigurationProvider)
                .ToDataSourceResultAsync(dataSourceRequest);
            
            return RpcResponse<DataSourceResult<UserListModel>>.WithSuccess(
                dataSourceResult.AsTyped<UserListModel>()
            );
        }
    }
}