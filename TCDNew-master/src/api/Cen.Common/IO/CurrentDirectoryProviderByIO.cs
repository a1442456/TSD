using System.IO;
using Cen.Common.IO.Abstract;

namespace Cen.Common.IO
{
    public class CurrentDirectoryProviderByIO: ICurrentDirectoryProvider
    {
        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}