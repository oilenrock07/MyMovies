using System.Web;
using System.Web.Optimization;

namespace MyMovies.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/analytics.js",
                        "~/Scripts/jflickrfeed.min.js",
                        "~/Scripts/jquery.debouncedresize.js",
                        "~/Scripts/jquery.hoverIntent.min.js",
                        "~/Scripts/jquery.magnific-popup.min.js",
                        "~/Scripts/jquery.nicescroll.min.js",
                        "~/Scripts/jquery.tweet.min.js",
                        "~/Scripts/main.js",
                        "~/Scripts/owl.carousel.min.js",
                        "~/Scripts/retina.min.js",
                        "~/Scripts/smoothscroll.js",
                        "~/Scripts/waypoints-sticky.min.js",
                        "~/Scripts/waypoints.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/bootstrap-colorpicker.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/animate.css",
                      "~/Content/green.css",
                      "~/Content/vspacing.min.css",
                      "~/Content/simple-line-icons.css",
                      "~/Content/custom.css",
                      "~/Content/my-movie.css"));
        }
    }
}