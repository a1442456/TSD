using Cen.Wms.Client.Models.Dtos;

namespace Cen.Wms.Client.Models.State
{
    class StateAuth
    {
        private readonly UserTokenResp _userToken;

        public StateAuth(UserTokenResp userToken)
        {
            if (userToken != null)
            {
                _userToken = new UserTokenResp { AuthToken = userToken.AuthToken, DisplayName = userToken.DisplayName };
            }
        }

        public bool IsAuthorised { get { return _userToken != null; } }
        public string AuthToken { get { return _userToken.AuthToken; } }
        public string DisplayName { get { return _userToken.DisplayName; } }
    }
}
