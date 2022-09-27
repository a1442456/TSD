using System.IO;
using System.Reflection;
using Cen.Common.IO.Abstract;

namespace Cen.Common.IO
{
    public class CurrentDirectoryProviderByAssembly: ICurrentDirectoryProvider
    {
        public string GetCurrentDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        }
    }
}