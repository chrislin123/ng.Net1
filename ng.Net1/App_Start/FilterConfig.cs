using System.Web;
using System.Web.Mvc;
using ng.Net1.Controllers;

namespace ng.Net1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
