namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering.Parsing
{
    public enum FilterTokenType
    {
        Property,
        ComparisonOperator,
        Or,
        And,
        Not,
        Function,
        Number,
        String,
        Boolean,
        DateTime,
        LeftParenthesis,
        RightParenthesis,
        Comma,
        Null
    }
}
