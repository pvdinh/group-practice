using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Controllers
{
    public class navbarController : Controller
    {
        // GET: navbar
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult categoryproduct()
        {
            using (SellphonesEntities db = new SellphonesEntities())
            {
                List<LoaiSP> listcategory = db.LoaiSPs.ToList();
                return PartialView("_Viewcategoryproduct", listcategory);
            }
        }
        public ActionResult categoryproductWithBrand()
        {
            using (SellphonesEntities db = new SellphonesEntities())
            {
                List<LoaiSP> listcategory = db.LoaiSPs.ToList();
                ViewBag.brand = db.HangSXes.ToList();
                return PartialView("_ViewcategoryproductWithBrand", listcategory);
            }
        }
        public ActionResult viewbefore()
        {
            return PartialView("_Viewbefore");
        }
    }
}