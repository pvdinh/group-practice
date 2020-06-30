using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellSmartPhone.Models;
using PagedList.Mvc;
using PagedList;

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
            //kiểm tra hàng còn hay không
            var status = db.Sanphams.Where(s => s.MaSP == id).Select(s => s.SoLuong).FirstOrDefault();
            ViewBag.status = status;
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
            Session["countcmt"] = 6;
            ViewBag.user = Session["user"];
            int id = int.Parse(Session["id"].ToString());
            List<get_comment_Result1> listcomment = db.get_comment(id).Take(4).ToList();
            return PartialView("_Viewcomment",listcomment);
        }
        public ActionResult commentMore()
        {
            ViewBag.user = Session["user"];
            int id = int.Parse(Session["id"].ToString());
            List<get_comment_Result1> listcomment = db.get_comment(id).Take(int.Parse(Session["countcmt"].ToString())).ToList();
            return PartialView("_ViewcommentMore", listcomment);
        }
        public ActionResult productReative()
        {
            int id = int.Parse(Session["id"].ToString());
            //lay ma loai san pham thong qua id san pham
            var x = db.Sanphams.Where(s => s.MaSP == id).Select(s => s.LoaiSP).FirstOrDefault();
            //lay danh sach san pham thong qua ma loa san pham
            List<get_product_discount_Result> products = new List<get_product_discount_Result>();
            products = db.get_product_discount().Where(s=>s.LoaiSP == x && s.MaSP != id).ToList();
            products = products.OrderBy(s => Guid.NewGuid()).Take(6).ToList();
            return PartialView("_ViewproductRelate",products);
        }
        [HttpPost]
        public ActionResult addcomment(string HoTen,string Email,string NoiDung)
        {
            ViewBag.user = Session["user"];
            int id = int.Parse(Session["id"].ToString());
            int lastest = db.Binhluans.OrderByDescending(s => s.MaBL).Take(1).Select(s=>s.MaBL).FirstOrDefault();
            if(Session["user"] == null)
            {
                DateTime ngaydang = DateTime.Now;
                db.add_comment(lastest + 1, id, null, NoiDung, ngaydang, HoTen, Email);
            }
            else
            {
                DateTime ngaydang = DateTime.Now;
                Account x = (Account)Session["user"];
                db.add_comment(lastest + 1, id,x.IDAccount, NoiDung, ngaydang, HoTen, Email);
            }
            List<get_comment_Result1> listcomment = db.get_comment(id).ToList();
            return PartialView("_Viewcomment", listcomment);
        }

    }
}