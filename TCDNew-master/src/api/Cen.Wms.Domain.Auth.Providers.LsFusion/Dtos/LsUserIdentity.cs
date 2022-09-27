namespace Cen.Wms.Domain.Auth.Providers.LsFusion.Dtos
{
    public class LsUserIdentity
    {
        public string UserId { get; set; }
        public short Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}