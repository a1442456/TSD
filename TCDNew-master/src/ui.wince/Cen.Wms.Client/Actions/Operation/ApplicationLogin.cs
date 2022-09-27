using Cen.Wms.Client.Actions.Data.Auth;
using Cen.Wms.Client.Actions.UI.Auth;
using Cen.Wms.Client.Models.State;
using Cen.Wms.Client.Services;

namespace Cen.Wms.Client.Actions.Operation
{
    class ApplicationLogin
    {
        public static bool Run()
        {
            var result = false;

            var credentials = AskForUserCredentials.Run();
            if (credentials != null)
            {
                var userToken = UserLogin.Run(credentials);
                GStateProvider.Instance.SetStateAuth(new StateAuth(userToken));
            }

            return GStateProvider.Instance.StateAuth.IsAuthorised;
        }
    }
}
