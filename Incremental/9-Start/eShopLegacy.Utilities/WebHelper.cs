using System.Web;

namespace eShopLegacy.Utilities
{
    public class WebHelper
    {
        public static string UserAgent => HttpContext.Current.Request.UserAgent;
    }
}