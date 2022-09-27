using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace Cen.Common.Http.Client
{
    public class HttpQueryCall
    {
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpQueryCall(ILogger logger, JsonSerializerOptions jsonSerializerOptions)
        {
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<TResp> RunRaw<TReq, TResp>(
            TReq request, 
            string address, 
            int timeoutMs, 
            string basicAuthenticationHeaderEncodedValue = null,
            Stream requestLogStream = null)
        {
            var req = (HttpWebRequest)WebRequest.Create(address);
            req.ContentType = "text/json";
            req.Method = "POST";
            req.Timeout = timeoutMs;
            if (basicAuthenticationHeaderEncodedValue != null)
            {
                req.Headers.Add("Authorization", "Basic " + basicAuthenticationHeaderEncodedValue);
            }

            var data = JsonSerializer.SerializeToUtf8Bytes(request, _jsonSerializerOptions);
            req.ContentLength = data.Length;

            if (requestLogStream != null)
                await requestLogStream.WriteAsync(data);
            await using (var stream = req.GetRequestStream())
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            TResp response;
            var resp = (HttpWebResponse)req.GetResponse();
            await using (var respStream = resp.GetResponseStream())
            {
                var respBytes = ReadFully(respStream);
                if (requestLogStream != null)
                {
                    requestLogStream.WriteByte(0x0A);
                    requestLogStream.WriteByte(0x0D);
                    await requestLogStream.WriteAsync(respBytes);
                }
                
                response = JsonSerializer.Deserialize<TResp>(respBytes, _jsonSerializerOptions);
            }

            return response;
        }
        
        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}