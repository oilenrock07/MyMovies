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
    }
}
