namespace Cen.Common.Data.DataSource.Infrastructure
{
    /// <summary>
    /// Logical operator used for filter descriptor composition.
    /// </summary>
    public enum FilterCompositionLogicalOperator
    {
        /// <summary>
        /// Combines filters with logical AND.
        /// </summary>
        And,

        /// <summary>
        /// Combines filters with logical OR.
        /// </summary>
        Or
    }
}