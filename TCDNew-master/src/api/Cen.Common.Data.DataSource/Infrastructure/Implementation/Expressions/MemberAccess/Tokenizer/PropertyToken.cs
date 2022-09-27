namespace Cen.Common.Data.DataSource.Infrastructure.Implementation.Expressions.MemberAccess.Tokenizer
{
    internal class PropertyToken : IMemberAccessToken
    {
        private readonly string propertyName;

        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }

        public PropertyToken(string propertyName)
        {
            this.propertyName = propertyName;
        }
    }
}