using System;
using System.Text.RegularExpressions;

namespace NAtomPub.Core.Extensions
{
    public static class StringExtensions
    {
        public static Boolean TryParseBoolean(this String source, out Boolean result)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (String.IsNullOrEmpty(source))
                throw new ArgumentOutOfRangeException("source");

            if (Boolean.TryParse(source, out result))
                return true;

            if (String.Equals("yes", source, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }

            if (String.Equals("no", source, StringComparison.OrdinalIgnoreCase))
            {
                result = false;
                return true;
            }

            return false;
        }

        public static Boolean IsBase64String(this String source)
        {
            source = source.Trim();
            return (source.Length % 4 == 0) && Regex.IsMatch(source, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}