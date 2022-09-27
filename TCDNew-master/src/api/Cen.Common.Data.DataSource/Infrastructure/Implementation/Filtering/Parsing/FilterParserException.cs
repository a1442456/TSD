using System;

namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Filtering.Parsing
{
    public class FilterParserException : Exception
    {
        public FilterParserException()
        {
        }

        public FilterParserException(string message)
            : base(message)
        {
        }

        public FilterParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
