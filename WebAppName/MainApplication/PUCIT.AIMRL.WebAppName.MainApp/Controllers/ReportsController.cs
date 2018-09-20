using PUCIT.AIMRL.WebAppName.MainApp.Util;
using PUCIT.AIMRL.WebAppName.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PUCIT.AIMRL.WebAppName.MainApp.Controllers
{
    public class ReportsController : BaseController
    {
        //
        // GET: /Admin/

        public ActionResult UserLoginHistory()
        {
            if (PUCIT.AIMRL.WebAppName.MainApp.Security.PermissionManager.perViewLoginHistoryReport == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        public ActionResult ForgotPasswordLog()
        {
            if (PUCIT.AIMRL.WebAppName.MainApp.Security.PermissionManager.perViewLoginHistoryReport == false)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}
