namespace HireMe.Core.Helpers
{
using System;
    using System.Collections;
    using System.Linq;

    public static class StringHelper 
    {
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
        //bool anyLuck = s.ContainsAny("a", "b", "c");
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

    }
	
}
