namespace Cen.Common.Data.DataSource.Infrastructure
{
    public interface IDescriptor
    {
        void Deserialize(string source);
        string Serialize();
    }
}
