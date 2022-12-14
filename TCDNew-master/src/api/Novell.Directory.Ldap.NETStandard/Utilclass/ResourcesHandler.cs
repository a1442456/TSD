/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
*
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to  permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/

//
// Novell.Directory.Ldap.Utilclass.ResourcesHandler.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using System.Globalization;
using System.Text;

namespace Novell.Directory.Ldap.Utilclass
{
    /// <summary>
    ///     A utility class to get strings from the ExceptionMessages and
    ///     ResultCodeMessages resources.
    /// </summary>
    public class ResourcesHandler
    {
        /// <summary> The default Locale.</summary>
        private static CultureInfo _defaultLocale;

        static ResourcesHandler()
        {
            _defaultLocale = CultureInfo.CurrentUICulture;
        }

        // Cannot create an instance of this class
        private ResourcesHandler()
        {
        }

        /// <summary>
        ///     Returns a string using the MessageOrKey as a key into
        ///     ExceptionMessages or, if the Key does not exist, returns the
        ///     string messageOrKey.  In addition it formats the arguments into the message
        ///     according to MessageFormat.
        /// </summary>
        /// <param name="messageOrKey">
        ///     Key string for the resource.
        /// </param>
        /// <param name="">
        ///     arguments.
        /// </param>
        /// <returns>
        ///     the text for the message specified by the MessageKey or the Key
        ///     if it there is no message for that key.
        /// </returns>
        public static string GetMessage(string messageOrKey, object[] arguments)
        {
            return GetMessage(messageOrKey, arguments, null);
        }

        /// <summary>
        ///     Returns the message stored in the ExceptionMessages resource for the
        ///     specified locale using messageOrKey and argments passed into the
        ///     constructor.  If no string exists in the resource then this returns
        ///     the string stored in message.  (This method is identical to
        ///     getLdapErrorMessage(Locale locale).).
        /// </summary>
        /// <param name="messageOrKey">
        ///     Key string for the resource.
        /// </param>
        /// <param name="">
        ///     arguments.
        /// </param>
        /// <param name="locale">
        ///     The Locale that should be used to pull message
        ///     strings out of ExceptionMessages.
        /// </param>
        /// <returns>
        ///     the text for the message specified by the MessageKey or the Key
        ///     if it there is no message for that key.
        /// </returns>
        public static string GetMessage(string messageOrKey, object[] arguments, CultureInfo locale)
        {
            if (_defaultLocale == null)
            {
                _defaultLocale = CultureInfo.CurrentUICulture;
            }

            if (locale == null)
            {
                locale = _defaultLocale;
            }

            if (messageOrKey == null)
            {
                messageOrKey = string.Empty;
            }

            var pattern = ExceptionMessages.GetErrorMessage(messageOrKey);

            // Format the message if arguments were passed
            if (arguments != null)
            {
                var strB = new StringBuilder();
                strB.AppendFormat(pattern, arguments);
                pattern = strB.ToString();

                // MessageFormat mf = new MessageFormat(pattern);
                // pattern=System.String.Format(locale,pattern,arguments);
                // mf.setLocale(locale);
                // this needs to be reset with the new local - i18n defect in java
                // mf.applyPattern(pattern);
                // pattern = mf.format(arguments);
            }

            return pattern;
        }

        /// <summary>
        ///     Returns a string representing the Ldap result code from the
        ///     default ResultCodeMessages resource.
        /// </summary>
        /// <param name="code">
        ///     the result code.
        /// </param>
        /// <returns>
        ///     the String representing the result code.
        /// </returns>
        public static string GetResultString(int code)
        {
            return GetResultString(code, null);
        }

        /// <summary>
        ///     Returns a string representing the Ldap result code.  The message
        ///     is obtained from the locale specific ResultCodeMessage resource.
        /// </summary>
        /// <param name="code">
        ///     the result code.
        /// </param>
        /// <param name="locale">
        ///     The Locale that should be used to pull message
        ///     strings out of ResultMessages.
        /// </param>
        /// <returns>
        ///     the String representing the result code.
        /// </returns>
        public static string GetResultString(int code, CultureInfo locale)
        {
            string result;
            try
            {
                result = ResultCodeMessages.GetResultCode(Convert.ToString(code));
            }
            catch (ArgumentNullException)
            {
                result = GetMessage(ExceptionMessages.UnknownResult, new object[] {code }, locale);
            }

            return result;
        }
    } // end class ResourcesHandler
}