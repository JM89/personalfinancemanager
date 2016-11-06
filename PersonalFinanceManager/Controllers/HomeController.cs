using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult SaveCurrentAccount(int accountId, int indexAccountList)
        {
            Session["PreviousAccount"] = Session["CurrentAccount"];
            Session["CurrentAccount"] = accountId;
            Session["IndexAccountList"] = indexAccountList;

            if (Session["PreviousAccount"] == null || (int)Session["PreviousAccount"] == (int)Session["CurrentAccount"])
            {
                Session["ReloadPage"] = false;
            }
            else
            {
                Session["ReloadPage"] = true;
            }
            
            return Json(new { Data = new
                {
                    accountId = accountId, 
                    reloadPage = Session["ReloadPage"] 
                 }
                }, JsonRequestBehavior.AllowGet);
        }
    }
}