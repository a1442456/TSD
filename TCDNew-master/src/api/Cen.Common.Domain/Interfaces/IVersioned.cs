namespace Cen.Common.Domain.Interfaces
{
    public interface IVersioned
    {
        long RowVersion { get; set; }
    }
}