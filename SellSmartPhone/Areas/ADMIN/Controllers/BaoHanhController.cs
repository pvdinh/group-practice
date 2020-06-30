using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SellSmartPhone.Areas.ADMIN.model;

namespace SellSmartPhone.Areas.ADMIN.Controllers
{
    public class BaoHanhController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();

        public ActionResult Index(string search)
        {
            var links = from l in db.Accounts
                        select l;
            if(!String.IsNullOrEmpty(search))
            {
                links = links.Where(s => s.Hoten.Contains(search));
            }

            return View(links);
        }


    }
}