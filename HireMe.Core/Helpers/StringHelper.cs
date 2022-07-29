namespace HireMe.Core.Helpers
{
using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringHelper 
    {
        private static readonly char[] DefaultDelimeters = new char[] { ' ', ',', '.', '-', '\n', '\t', '/', '@', '_', '=', ')', '(', '*', '&', '^', '%', '$', '#', '!', '`', '~', '+' };

        public static string GetUntilOrEmpty(this string text, string stopAt)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
        public static string GetUntil(this string text, int length)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                if (length > 0)
                {
                    return text.Substring(0, length);
                }
            }

            return String.Empty;
        }
        public static string getFirstWord(String text)
        {

            int index = text.IndexOf(' ');

            if (index > -1)
            { // Check if there is more than one word.

                return text.Substring(0, index).Trim(); // Extract first word.

            }
            else
            {

                return text; // Text is the first word itself.
            }
        }
        public static string LastWord(this string StringValue)
        {
            return LastWord(StringValue, DefaultDelimeters);
        }

        public static string LastWord(this string StringValue, char[] Delimeters)
        {
            int index = StringValue.LastIndexOfAny(Delimeters);

            if (index > -1)
                return StringValue.Substring(index);
            else
                return null;
        }

        public static bool ContainsAny(this string haystack, IList needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
        public static bool ContainsAny2(string[] haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
        public static string[] SplitAndTrim(this string text, char separator)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.Split(separator).Select(t => t.Trim()).ToArray();
        }
        public static string Filter(this string str)
        {
            int index = str.IndexOf('@');

            if (index >= 0)
                str = str.Substring(0, index);
            return str;
            //   string res = String.Concat(str?.Trim(DefaultDelimeters)?.Split(' ', StringSplitOptions.RemoveEmptyEntries));/*.Split(DefaultDelimeters))*/;//Regex.Replace(firstWordOnly ? getFirstWord(str) : str, String.Concat(charsToRemove), String.Empty);
            // return res;
        }
        public static string FilterTrimSplit(this string str)
        {
            // for using str = str.GetUntilOrEmpty("@") 
            string res = String.Concat(str?.Trim(DefaultDelimeters)?.Split(' ', StringSplitOptions.RemoveEmptyEntries));/*.Split(DefaultDelimeters))*/;//Regex.Replace(firstWordOnly ? getFirstWord(str) : str, String.Concat(charsToRemove), String.Empty);
            return res;
        }
         
        //    public static string RemoveSpecialCharacters(string value)
        //  {
        //   return new String(value.ex(DefaultDelimeters).ToArray());
        //  }
    }
	
}
