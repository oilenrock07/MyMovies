using HtmlAgilityPack;

namespace MyMovies.Common.Extension
{
    public static class HtmlNodeExtension
    {
        public static string OuterHtml(this HtmlNode node)
        {
            return node == null ? "" : node.OuterHtml;
        }

        public static string OuterHtmlClean(this HtmlNode node)
        {
            return node == null ? "" : node.OuterHtml.CleanHtml();
        }

        public static string InnerText(this HtmlNode node)
        {
            return node == null ? "" : node.InnerText;
        }

        public static string InnerTextClean(this HtmlNode node)
        {
            return node == null ? "" : node.InnerText.CleanHtml();
        }

        public static string InnerHtml(this HtmlNode node)
        {
            return node == null ? "" : node.InnerHtml;
        }

        public static string InnerHtmlClean(this HtmlNode node)
        {
            return node == null ? "" : node.InnerHtml.CleanHtml();
        }

        public static string Attribute(this HtmlNode node, string attribute)
        {
            return node == null ? "" : node.Attributes[attribute].Value;
        }
    }
}
