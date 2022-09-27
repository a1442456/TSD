namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering.Parsing
{

    public class DateTimeNode : IFilterNode, IValueNode
    {
        public object Value
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
