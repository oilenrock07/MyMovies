namespace MyMovies.Common.Extension
{
    public static class StringExtension
    {
        public static string CleanHtml(this string str)
        {
            if (str.Contains("&nbsp;")) str = str.Replace("&nbsp;", "");
            if (str.Contains("\n")) str = str.Replace("\n", "");

            return str.Trim();
        }

        public static bool IsIMDB(this string str)
        {
            if (str.Length == 9 && str.Substring(0, 2) == "tt")
                return true;

            return false;
        }
    }
}
