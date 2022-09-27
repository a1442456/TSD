using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using Cen.Wms.Client.Models.Dtos.GosZNKAK;
using System.Net;
using Newtonsoft.Json;
using Cen.Wms.Client.Utils;
using NLog;
using Cen.Wms.Client.Common;
using Cen.Wms.Client.Actions.UI.Utility;
using System.Drawing;

namespace Cen.Wms.Client.Services
{
    class GosZNAK
    {
        private string _url = @"https://api.datamark.by/";
        private string _authMethodUrl = "auth";
        private string _labelMethodUrl = "labels";
        private static readonly Encoding encoding = Encoding.UTF8;

        public GosZNAK()
        {
            ReadParams();
        }

        private void ReadParams()
        {
            var result = string.Empty;

            try
            {
                var settingsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                settingsPath += @"\Launcher.config.xml";

                var xdoc = new XmlDocument();
                xdoc.Load(settingsPath);
                XmlElement root = xdoc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes.Item(0).ChildNodes;

                var enumerator = nodeList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var node = ((XmlNode) enumerator.Current);
                    if (node.NodeType == XmlNodeType.Element)
                        if (node.Attributes["key"].Value == "GosZNAKUrl")
                            _url = node.Attributes["value"].Value;
                }
            }
            catch (Exception)
            {
                
            }
        }

        private string GetJsonResponse<T>(T sendedObj, string token, string url, ref bool isError)
        {
            isError = false;
            string resp = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Headers.Add("Token", token);
            httpWebRequest.Timeout = 45000;

            httpWebRequest.Method = "POST";
            string serializedObj = JsonConvert.SerializeObject(sendedObj);
            byte[] data = Encoding.UTF8.GetBytes(serializedObj);
            httpWebRequest.ContentLength = data.Length;
            using (Stream writer = httpWebRequest.GetRequestStream())
                writer.Write(data, 0, data.Length);

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    resp = streamReader.ReadToEnd();
            }
            catch (WebException exx)
            {   
                using (StreamReader streamReader = new StreamReader(exx.Response.GetResponseStream()))
                    resp = streamReader.ReadToEnd();
                isError = true;
                return resp;
                
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //System.Windows.Forms.MessageBox.Show("Ошибка при получении ответа от маркировки");
                return string.Empty;
            }

            return resp;
        }

        public void GetLabels(string barcode, string token)
        {
            bool isError = false;
            GosZNAKLabels labelInfo = new GosZNAKLabels();
            try
            {
                lblDTO lbl = new lblDTO() { label = barcode };
                string response = GetJsonResponse<lblDTO>(lbl,
                   token,
                    _url + _labelMethodUrl, ref isError);

                if (!isError)
                {
                    labelInfo = JsonConvert.DeserializeObject<GosZNAKLabels>(response);
                    ShowModalMessage.Run("Маркировка",
                    labelInfo,
                    labelInfo.label.status.code == 50 ? System.Drawing.Color.PaleGreen : Color.Red);
                }
                else
                {
                    GosZnakError err = new GosZnakError();
                    err = JsonConvert.DeserializeObject<GosZnakError>(response);
                    ShowModalMessage.Run("Маркировка(Ошибка) " + err.error,
                    string.Format("{0}", err.message),
                    Color.Coral);
                }
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetLogger(Messages.LoggerNetName);
                logger.Error(ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
                 ShowModalMessage.Run("Маркировка(Ошибка)",
                    "Превышено время ожидания ответа от сервиса.",
                    Color.Coral);
            }
        }

        public string GetToken()
        {
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("username", "AMalevich@willesden.by");
            postParameters.Add("password", "sw23");
            postParameters.Add("is_rules_agree", "false");

            HttpWebResponse webResponse = MultipartFormDataPost(_url + _authMethodUrl, postParameters);

            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            GosZNAKAuth GosZNAKAuthResp = new GosZNAKAuth();
            try
            {
                GosZNAKAuthResp = JsonConvert.DeserializeObject<GosZNAKAuth>(fullResponse);
            }
            catch (Exception ex)
            {
                ShowModalMessage.Run("Маркировка(ошибка)", 
                    "Ошибка при сериализации ответа сервера маркировки");
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            
            return GosZNAKAuthResp.token;
           
        }

        private static HttpWebResponse MultipartFormDataPost(string postUrl, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = String.Format("multipart/form-data; boundary={0}", formDataBoundary);
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, contentType, formData);
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            const string crlf = "\r\n";
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCRLF = false;

            foreach (var param in postParameters)
            {   
                if (needsCRLF)
                    formDataStream.Write(encoding.GetBytes(crlf), 0, encoding.GetByteCount(crlf));

                needsCRLF = true;


                string postData = string.Format("--{0}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}",
                    boundary,
                    param.Key,
                    param.Value,
                    crlf);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));

            }

            // Add the end of the request.  Start with a newline
            string footer = crlf + "--" + boundary + "--" + crlf;
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        private static HttpWebResponse PostForm(string postUrl, string contentType, byte[] formData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream())
                requestStream.Write(formData, 0, formData.Length);

            return (HttpWebResponse)request.GetResponse();
        }
    }
}
