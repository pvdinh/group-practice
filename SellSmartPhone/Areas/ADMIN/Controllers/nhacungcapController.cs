using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Areas.ADMIN.Controllers
{
    public class nhacungcapController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();
        // GET: ADMIN/nhacungcap
        public ActionResult Index()
        {
            var model = db.Nhacungcaps.ToList();
            return View("ListNcc",model);
        }
    }
}