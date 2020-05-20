using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellSmartPhone.Models;

namespace SellSmartPhone.Controllers
{
    public class ProductController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();
        // GET: Product
        public ActionResult Index(int? id)
        {
            Session["id"] = id;
            var spkm = db.SPkhuyenmais.Where(s => s.MaSP == id).FirstOrDefault();
            if (spkm != null)
            {
                var ttkm = db.Khuyenmais.Where(s => s.MaKM == spkm.MaSPKM).FirstOrDefault();
                if(ttkm != null)
                ViewBag.ttkm = ttkm;
            }
            get_product_discount_Result data = new get_product_discount_Result();
            data = db.get_product_discount().Where(s => s.MaSP == id).FirstOrDefault();
            return View(data);
        }

        public ActionResult Infoproduct()
        {
            int id = int.Parse(Session["id"].ToString());
            get_product_discount_Result data = new get_product_discount_Result();
            data = db.get_product_discount().Where(s => s.MaSP == id).FirstOrDefault();
            return PartialView("_Viewinfoproduct", data);
        }
        public ActionResult Thongso()
        {
            int id = int.Parse(Session["id"].ToString());
            var data = db.Sanphams.Where(s => s.MaSP == id).FirstOrDefault();
            List<Thongsokythuat> tttk = db.Thongsokythuats.Where(s => s.MaSP == id).ToList();
            ViewBag.data = data;
            return PartialView("_ViewThongso", tttk);
        }
        public ActionResult comment()
        {
            int id = int.Parse(Session["id"].ToString());
            get_product_discount_Result data = db.get_product_discount().Where(s => s.MaSP == id).FirstOrDefault();
            return PartialView("_Viewcomment", data);
        }
    }
}