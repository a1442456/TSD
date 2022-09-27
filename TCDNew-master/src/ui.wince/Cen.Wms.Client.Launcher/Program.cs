using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Cen.Wms.Client.Common;
using Ionic.Zip;
using Cen.Wms.Client;

namespace Cen.Wms.Client.Launcher
{
    static class Program
    {
        public static bool DownloadFile(string url, string destination)
        {
            bool success = false;

            System.Net.HttpWebRequest request = null;
            System.Net.WebResponse response = null;
            Stream responseStream = null;
            FileStream fileStream = null;

            try
            {
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 100000; // 100 seconds
                response = request.GetResponse();

                responseStream = response.GetResponseStream();

                fileStream = File.Open(destination, FileMode.Create, FileAccess.Write, FileShare.None);

                // read up to ten kilobytes at a time
                int maxRead = 10240;
                byte[] buffer = new byte[maxRead];
                int bytesRead = 0;
                int totalBytesRead = 0;

                // loop until no data is returned
                while ((bytesRead = responseStream.Read(buffer, 0, maxRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    fileStream.Write(buffer, 0, bytesRead);
                }

                // we got to this point with no exception. Ok.
                success = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                // cleanup all potentially open streams.

                if (null != responseStream)
                    responseStream.Close();
                if (null != response)
                    response.Close();
                if (null != fileStream)
                    fileStream.Close();
            }

            // if part of the file was written and the transfer failed, delete the partial file
            if (!success && File.Exists(destination))
                File.Delete(destination);

            return success;
        }

        private static string ReadUpdateAddress()
        {
            var result = string.Empty;

            try
            {
                var settingsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                settingsPath += @"\Launcher.config.xml";

                if (!File.Exists(settingsPath))
                    return string.Empty;

                var xdoc = new XmlDocument();
                xdoc.Load(settingsPath);
                XmlElement root = xdoc.DocumentElement;
                XmlNodeList nodeList = root.ChildNodes.Item(0).ChildNodes;

                var enumerator = nodeList.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var node = ((XmlNode) enumerator.Current);
                    if (node.NodeType == XmlNodeType.Element)
                        if (node.Attributes["key"].Value == "UpdateAddress")
                            result = node.Attributes["value"].Value;
                }
            }
            catch (Exception)
            {
                
            }

            return result;
        }

        private static long ReadVersionLocal()
        {
            long result = 0;

            try
            {
                var versionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                versionPath += @"\version.txt";

                using(var streamReader = File.OpenText(versionPath))
                    result = Convert.ToInt64(streamReader.ReadLine().Trim().ToUpper());
            }
            catch (Exception)
            {
                
            }

            return result;
        }

        private static long ReadVersionRemote()
        {
            long result = 0;

            try
            {
                var versionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                versionPath += @"\version_upd";

                if (File.Exists(versionPath))
                    File.Delete(versionPath);

                DownloadFile(ReadUpdateAddress() + "version.txt", versionPath);

                using (var streamReader = File.OpenText(versionPath))
                    result = Convert.ToInt64(streamReader.ReadLine().Trim().ToUpper());
            }
            catch (Exception)
            {
                
            }

            return result;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {   
            FileStream stream;
            try
            {
                var lockPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                lockPath += @"\lock.txt";
                stream = new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                MessageBox.Show("Программа уже запущена!");
                return;
            }

            var versionLocal = ReadVersionLocal();
            var versionRemote = ReadVersionRemote();
            if (versionRemote != 0)
            {
                if (versionLocal < versionRemote)
                {
                    var updatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                    var updateFilePath = updatePath + @"\update.zip";

                    if (File.Exists(updateFilePath))
                        File.Delete(updateFilePath);

                    DownloadFile(ReadUpdateAddress() + versionRemote + ".zip", updateFilePath);

                    using (var zip = ZipFile.Read(updateFilePath))
                    {
                        zip.ExtractAll(updatePath, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }

            var asm = Assembly.Load("Cen.Wms.Client, Culture=neutral, PublicKeyToken=null");
            var type = asm.GetType("Cen.Wms.Client.App");
            var runnable =  Activator.CreateInstance(type) as IRunnable;
            
            if (runnable == null)
                return;
            runnable.Run();
            stream.Close();
            Application.Exit();
        }
    }
}