namespace Cen.Wms.Domain.Sync.Providers.Fake
{
    public class SyncProvidersFakeOptions
    {
        public static string SectionName => "Sync:Providers:Fake";
        public int PacsCount { get; set; }
        public int FacilitiesCount { get; set; }
        public int UsersCount { get; set; }
    }
}