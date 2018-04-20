using ClientUI.Attributes;
using System.Web;
using System.Web.Mvc;

namespace ClientUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AbsAuthorize2());
        }
    }
}
