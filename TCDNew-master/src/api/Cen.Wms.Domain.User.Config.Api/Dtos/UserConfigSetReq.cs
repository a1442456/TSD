using Cen.Common.Domain.Models;
using Cen.Wms.Domain.User.Config.Models;

namespace Cen.Wms.Domain.User.Config.Api.Dtos
{
    public class UserConfigSetReq
    {
        public ByIdReq UserId { get; set; }
        public UserConfigEditModel UserConfigEditModel { get; set; }
    }
}