using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Cen.Common.Data.DataSource.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex NameExpression = new Regex("([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)", RegexOptions.Compiled);
        private static readonly Regex EntityExpression = new Regex("(&amp;|&)#([0-9]+;)", RegexOptions.Compiled);

        /// <summary>
        /// Replaces the format item in a specified System.String with the text equivalent of the value of a corresponding System.Object instance in a specified array.
        /// </summary>
        /// <param name="instance">A string to format.</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the System.String equivalent of the corresponding instances of System.Object in args.</returns>
        public static string FormatWith(this string instance, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, instance, args);
        }

        public static string EscapeHtmlEntities(this string html)
        {
            return EntityExpression.Replace(html, "$1\\\\#$2");
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="instance">The string to check equality.</param>
        /// <param name="comparing">The comparing with string.</param>
        /// <returns>
        /// <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCaseSensitiveEqual(this string instance, string comparing)
        {
            return string.CompareOrdinal(instance, comparing) == 0;
        }

        /// <summary>
        /// Determines whether this instance and another specified System.String object have the same value.
        /// </summary>
        /// <param name="instance">The string to check equality.</param>
        /// <param name="comparing">The comparing with string.</param>
        /// <returns>
        /// <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCaseInsensitiveEqual(this string instance, string comparing)
        {
            return string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string ToCamelCase(this string instance)
        {

            return instance[0].ToString().ToLowerInvariant() + instance.Substring(1);
        }

        public static string AsTitle(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            int lastIndex = value.LastIndexOf(".", StringComparison.Ordinal);

            if (lastIndex > -1)
            {
                value = value.Substring(lastIndex + 1);
            }

            return value.SplitPascalCase();
        }

        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (!value.HasValue())
            {
                return defaultValue;
            }

            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        public static string SplitPascalCase(this string value)
        {
            return NameExpression.Replace(value, " $1").Trim();
        }

        public static string JavaScriptStringEncode(this string value, bool addDoubleQuotes)
        {
            string str = JavaScriptStringEncode(value);
            if (!addDoubleQuotes)
            {
                return str;
            }
            return string.Concat("\"", str, "\"");
        }

        public static string JavaScriptStringEncode(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = null; 
            int num = 0;
            int num1 = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char chr = value[i];
                if (CharRequiresJavaScriptEncoding(chr))
                {
                    if (stringBuilder == null)
                    {
                        stringBuilder = new StringBuilder(value.Length + 5);
                    }
                    if (num1 > 0)
                    {
                        stringBuilder.Append(value, num, num1);
                    }
                    num = i + 1;
                    num1 = 0;
                }
                char chr1 = chr;
                switch (chr1)
                {
                    case '\b':
                        {
                            stringBuilder.Append("\\b");
                            break;
                        }
                    case '\t':
                        {
                            stringBuilder.Append("\\t");
                            break;
                        }
                    case '\n':
                        {
                            stringBuilder.Append("\\n");
                            break;
                        }
                    case '\v':
                        {
                            if (!CharRequiresJavaScriptEncoding(chr))
                            {
                                num1++;
                                break;
                            }
                            else
                            {
                                AppendCharAsUnicodeJavaScript(stringBuilder, chr);
                                break;
                            }
                        }
                    case '\f':
                        {
                            stringBuilder.Append("\\f");
                            break;
                        }
                    case '\r':
                        {
                            stringBuilder.Append("\\r");
                            break;
                        }
                    default:
                        {
                            if (chr1 == '\"')
                            {
                                stringBuilder.Append("\\\"");
                                break;
                            }
                            else if (chr1 == '\\')
                            {
                                stringBuilder.Append("\\\\");
                                break;
                            }
                            else
                            {
                                goto case '\v';
                            }
                        }
                }
            }
            if (stringBuilder == null)
            {
                return value;
            }
            if (num1 > 0)
            {
                stringBuilder.Append(value, num, num1);
            }
            return stringBuilder.ToString();
        }

        private static bool CharRequiresJavaScriptEncoding(char c)
        {
            if (c < ' ' || c == '\"' || c == '\\' || c == '\'' || c == '<' || c == '>' || c == '&' || c == '\u0085' || c == '\u2028')
            {
                return true;
            }
            return c == '\u2029';
        }

        private static void AppendCharAsUnicodeJavaScript(StringBuilder builder, char c)
        {
            builder.Append("\\u");
            int num = c;
            builder.Append(num.ToString("x4", CultureInfo.InvariantCulture));
        }
    }
}