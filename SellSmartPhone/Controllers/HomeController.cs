using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellSmartPhone.Models;

namespace SellSmartPhone.Controllers
{
    public class HomeController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();
        classData data = new classData();
        List<get_product_discount_Result> products = new List<get_product_discount_Result>();

        // GET: home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult listproduct()
        {
            Session["count"] = 12; //kiểm soát số lượng hiển thị sản phẩm
            Session["temp"] = 1;  //trạng thái phát hiện đang ở loại sp nào
            products = db.get_product_discount().Where(s=>s.LoaiSP == 1111 && s.Ishot ==1).Take(8).ToList();
            return PartialView("_Viewproduct", products);
        }

        public ActionResult listproductnew()
        {
            Session["count"] = 12;
            Session["temp"] = 2; //trạng thái phát hiện đang ở loại sp nào
            products = db.get_product_discount().Where(s => s.Isnew == 1 && s.LoaiSP == 1111).Take(8).ToList();
            return PartialView("_Viewproduct", products);
        }

        public ActionResult listproductdiscount()
        {
            Session["count"] = 12;
            Session["temp"] = 3; //trạng thái phát hiện đang ở loại sp nào
            products = db.get_product_discount().Where(s=>s.Giamgia != null).Take(8).ToList();
            return PartialView("_Viewproduct", products);
        }
        public ActionResult getmore()
        {
            if(int.Parse(Session["temp"].ToString()) ==1 ) // sản phẩm nổi bật
            {
                products = db.get_product_discount().Where(s => s.LoaiSP == 1111 && s.Ishot == 1).Take(int.Parse(Session["count"].ToString())).ToList();
                return PartialView("_Viewproduct more", products);
            }
            else if (int.Parse(Session["temp"].ToString()) == 2) //sản phẩm mới
            {
                products = db.get_product_discount().Where(s => s.Isnew == 1 && s.LoaiSP == 1111).Take(int.Parse(Session["count"].ToString())).ToList();
                //nếu số sp nhỏ hơn bằng 8 thì ko cout session["count"]
                return PartialView("_Viewproduct more", products);
            }
            else if(int.Parse(Session["temp"].ToString()) == 3)    //sản phẩm giảm giá
            {
                products = db.get_product_discount().Where(s=> s.Giamgia != null).Take(int.Parse(Session["count"].ToString())).ToList();
                return PartialView("_Viewproduct more", products);
            }
            return HttpNotFound();
        }

    }
}