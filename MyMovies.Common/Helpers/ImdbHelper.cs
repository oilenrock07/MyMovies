namespace MyMovies.Common.Helpers
{
    public static class ImdbHelper
    {
        public static string GetImdbId(string url)
        {
            var splitted = url.Split('/');
            if (splitted.Length >= 4)
                return splitted[4];

            return "";
        }
    }
}
