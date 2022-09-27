using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.EntityFramework;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.User.Manage.Abstract;
using Cen.Wms.Domain.User.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.User.Manage.Api.Processors
{
    public class UserListQuery: IQueryProcessor<TableRowsReq, RpcResponse<DataSourceResult<UserListModel>>>
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        
        public UserListQuery(ILogger logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        
        public Task<RpcResponse<DataSourceResult<UserListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsReq request)
        {
            return _userRepository.UserList(request);
        }
    }
}