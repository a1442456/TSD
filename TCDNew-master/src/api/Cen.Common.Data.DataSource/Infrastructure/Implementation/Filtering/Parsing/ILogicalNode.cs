namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering.Parsing
{
    public interface ILogicalNode
    {
        FilterCompositionLogicalOperator LogicalOperator
        {
            get;
        }
    }
}
