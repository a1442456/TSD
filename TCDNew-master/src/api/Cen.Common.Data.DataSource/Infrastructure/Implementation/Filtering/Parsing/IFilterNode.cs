namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering.Parsing
{
    public interface IFilterNode
    {
        void Accept(IFilterNodeVisitor visitor);
    }
}
