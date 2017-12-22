using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesoAuthWebTester.Controllers
{
    public class HomeController : BaseController
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

        public JsonResult RefreshToken()
        {
            if (Session["AccessToken"] != null)
            {
                ViewBag.AccessToken = Session["AccessToken"];
                ViewBag.RefreshToken = Session["RefreshToken"];
                //var jsonSerialiser = new JavaScriptSerializer();
                //object json = jsonSerialiser.Serialize(new { accessToken = ViewBag.AccessToken, success = true });
                return Json(new { accessToken = ViewBag.AccessToken, success = true }, JsonRequestBehavior.AllowGet);
                //return json;
            }
            else
            {
                return Json(new { accessToken = "", success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}