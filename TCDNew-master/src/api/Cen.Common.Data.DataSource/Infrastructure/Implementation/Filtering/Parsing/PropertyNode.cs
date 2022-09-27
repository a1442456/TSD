namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering.Parsing
{
    public class PropertyNode : IFilterNode
    {
        public string Name
        {
            get;
            set;
        }

        public void Accept(IFilterNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
