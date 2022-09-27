using System;

namespace Cen.Common.Http.Client
{
    public static class BasicAuthHelper
    {
        public static string GetBasicAuthHeaderString(string userName, string userPassword)
        {
            return Convert.ToBase64String(
                System.Text.Encoding
                    .GetEncoding("ISO-8859-1")
                    .GetBytes(
                        $"{userName}:{userPassword}"
                    )
            );
        }
    }
}