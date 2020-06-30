using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SellSmartPhone.Areas.ADMIN.model;
using System.Data.Entity;

namespace SellSmartPhone.Areas.ADMIN.Controllers
{
    public class BaoHanhController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();

        public ActionResult Index(string search)
        {
            List<DonhangKH> pList = db.DonhangKHs.Where(x => x.MaKH.ToString().Contains(search)).ToList();
            return View(pList);
        }

        public ActionResult DeTails()
        {
          

            return View();
            
        }

    }
}