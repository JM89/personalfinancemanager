using PersonalFinanceManager.Helpers.Filters;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UnitOrWorkAttribute());
        }
    }
}
