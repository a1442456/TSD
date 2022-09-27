using System;
using System.Linq.Expressions;

namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Expressions.Filtering
{
    internal abstract class FilterExpressionBuilder : ExpressionBuilderBase
    {
        protected FilterExpressionBuilder(ParameterExpression parameterExpression) :
            base(parameterExpression.Type)
        {
            this.ParameterExpression = parameterExpression;
        }

        public abstract Expression CreateBodyExpression();

        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        public LambdaExpression CreateFilterExpression()
        {
            Expression bodyExpression = this.CreateBodyExpression();
            return Expression.Lambda(bodyExpression, this.ParameterExpression);
        }
    }
}