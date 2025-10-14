using System.Web;
using System.Web.Mvc;
using B_M.Filters;

namespace B_M
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
