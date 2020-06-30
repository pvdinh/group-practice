using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Areas.ADMIN.Controllers
{
    public class ManagerController : Controller
    {
        // GET: ADMIN/Manager
        public ActionResult Index()
        {
            return View();
        }
    }
}