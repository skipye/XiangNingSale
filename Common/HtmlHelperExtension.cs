using System.Web.Mvc;

namespace Common
{
    public static class HtmlHelperExtension
    {
        public static string CurrentAction(this HtmlHelper html)
        {
            return (string)html.ViewContext.RouteData.Values["action"];
        }

        public static string CurrentController(this HtmlHelper html)
        {
            return (string)html.ViewContext.RouteData.Values["controller"];
        }

        public static string CurrentArea(this HtmlHelper html)
        {
            return (string)html.ViewContext.RouteData.DataTokens["area"];
        }

       
    }
}
