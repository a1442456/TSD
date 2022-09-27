using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.User.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.User.Manage.Api.Processors
{
    public class UserWhoAmIQuery: IQueryProcessor<object, RpcResponse<ViewModelSimple>>
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        
        public UserWhoAmIQuery(ILogger logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        
        public Task<RpcResponse<ViewModelSimple>> Run(IUserIdProvider userIdProvider, object request)
        {
            return _userRepository.UserReadSimple(new ByIdReq { Id = userIdProvider.UserGuid });
        }
    }
}