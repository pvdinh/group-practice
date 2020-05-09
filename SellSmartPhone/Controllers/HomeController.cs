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
        List<viewproductHome> products = new List<viewproductHome>();

        // GET: home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult listproduct()
        {
            Session["count"] = 12; //kiểm soát số lượng hiển thị sản phẩm
            Session["temp"] = 1;  //trạng thái phát hiện đang ở loại sp nào
            data.allsanphams = db.Sanphams.Where(s=>s.LoaiSP == 1111 && s.Ishot ==1).Take(8).ToList();
            foreach(var item in data.allsanphams)
            {
                //lấy hãng sản xuất tương ứng với sản phẩm
                var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang,item.Ishot,item.Isnew));
            }
            return PartialView("_Viewproduct", products);
        }

        public ActionResult listproductnew()
        {
            Session["count"] = 12;
            Session["temp"] = 2; //trạng thái phát hiện đang ở loại sp nào
            data.allsanphams = db.Sanphams.Where(s => s.Isnew == 1 && s.LoaiSP == 1111).Take(8).ToList();
            foreach (var item in data.allsanphams)
            {
                //lấy hãng sản xuất tương ứng với sản phẩm
                var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
            }
            return PartialView("_Viewproduct", products);
        }

        public ActionResult listproductdiscount()
        {
            Session["count"] = 12;
            Session["temp"] = 3; //trạng thái phát hiện đang ở loại sp nào
            data.allsanphams = db.Sanphams.Where(s => s.LoaiSP == 1111).Take(8).ToList();
            foreach (var item in data.allsanphams)
            {
                //lấy hãng sản xuất tương ứng với sản phẩm
                var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
            }
            return PartialView("_Viewproduct", products);
        }
        public ActionResult getmore()
        {
            if(int.Parse(Session["temp"].ToString()) ==1 ) // sản phẩm nổi bật
            {
                data.allsanphams = db.Sanphams.Where(s => s.LoaiSP == 1111 && s.Ishot == 1).Take(int.Parse(Session["count"].ToString())).ToList();
                foreach (var item in data.allsanphams)
                {
                    //lấy hãng sản xuất tương ứng với sản phẩm
                    var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                    products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
                }
                return PartialView("_Viewproduct more", products);
            }
            if (int.Parse(Session["temp"].ToString()) == 2) //sản phẩm mới
            {
                data.allsanphams = db.Sanphams.Where(s => s.Isnew == 1 && s.LoaiSP == 1111).Take(int.Parse(Session["count"].ToString())).ToList();
                foreach (var item in data.allsanphams)
                {
                    //lấy hãng sản xuất tương ứng với sản phẩm
                    var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                    products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
                }
                //nếu số sp nhỏ hơn bằng 8 thì ko cout session["count"]
                if(products.Count() <= 8 )
                {
                    return PartialView("_Viewproduct", products);
                }
                else return PartialView("_Viewproduct more", products);
            }
            else                                         //sản phẩm giảm giá
            {
                data.allsanphams = db.Sanphams.Where(s=>s.LoaiSP == 1111).Take(8).ToList();
                foreach (var item in data.allsanphams)
                {
                    //lấy hãng sản xuất tương ứng với sản phẩm
                    var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                    products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
                }
                if (products.Count() <= 8)
                {
                    return PartialView("_Viewproduct", products);
                }
                else return PartialView("_Viewproduct more", products);
            }

        }

    }
}