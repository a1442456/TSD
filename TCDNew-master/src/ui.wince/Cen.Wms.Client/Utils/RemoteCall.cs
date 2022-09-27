using System.Net;
using Cen.Wms.Client.Models.Rpc;

namespace Cen.Wms.Client.Utils
{
    public static class RemoteCall
    {
        private const int _timeout = 45000;

        public static RpcResponse<O> Invoke<I, O>(string address, I request, string bearer)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(address);
            req.ContentType = "text/json";
            req.Method = "POST";
            req.Timeout = _timeout;
            if (!string.IsNullOrEmpty(bearer))
                req.Headers.Add("Authorization", string.Format("Bearer {0}", bearer));

            var data = JsonHelpers.Serialize(request);
            req.ContentLength = data.Length;

            using (var stream = req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            RpcResponse<O> response;
            using (HttpWebResponse resp = (HttpWebResponse) req.GetResponse())
            {
                using (var respStream = resp.GetResponseStream())
                {
                    response = JsonHelpers.Deserialize<RpcResponse<O>>(respStream);
                }
            }

            return response;
        }
    }
}
