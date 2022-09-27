using System.IO;
using System.Reflection;

namespace Cen.Common.Data.DataSource.Infrastructure.Implementation
{
    using System.Runtime.Loader;

    internal class FactoryLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return Default.LoadFromAssemblyName(assemblyName);
        }

        public Assembly Load(Stream assembly)
        {
            return LoadFromStream(assembly, null);
        }
    }
}
