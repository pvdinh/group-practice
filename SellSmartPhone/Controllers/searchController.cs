using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace SellSmartPhone.Controllers
{
    public class searchController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();
        classData data = new classData();
        List<viewproductHome> products = new List<viewproductHome>();
        // GET: search
        public ActionResult Index(int? id, string type, string sort, string btnsearch)
        {
            //lấy thông tin để lọc sản phẩm
            Session["sort"] = sort;
            Session["type"] = type;
            Session["id"] = id;
            /*===========================*/
            Session["btnsearch"] = btnsearch;

            return View();
        }

        public JsonResult listUIproduct(string term)
        {
            List<string> data = new List<string>();
            data = db.Sanphams.Where(s => s.TenSP.Contains(term)).Select(s => s.TenSP).Take(7).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult resultproduct(int? id, int? page, string btnsearch)
        {
            /*===================================== Tìm kiếm theo Loại sản phẩm ============================*/
            if (Session["id"] != null)
            {
                if (Session["type"] != null && Session["type"].ToString() != "")
                {
                    /* tuỳ chọn tăng giảm sẽ dùng luôn session["sort"] trước đó đã được lưu */
                    if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                    {
                        data.allsanphams = db.Sanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                        if (Session["sort"] != null)
                        {
                            getgiam(id);
                        }
                    }
                    else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                    {
                        data.allsanphams = db.Sanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                        if (Session["sort"] != null)
                        {
                            gettang(id);
                        }
                    }
                }
                else
                {   //show mặc định khi không chọn sắp xếp
                    //nếu  sắp xếp != null thì ko có sự thay đổi, chỉ thay đổi khi chọn kiểu sắp xếp (tăng, giảm)
                    data.allsanphams = db.Sanphams.Where(s => s.LoaiSP == id).ToList();
                }
            }

            /*==========================================================================================*/

            /*==============================Tìm kiếm theo từ khoá=======================================*/

            if (Session["btnsearch"] != null && Session["btnsearch"].ToString() != "")
            {
                data.allsanphams = db.Sanphams.Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }

            /*===========================================================================================*/
            //đếm tống số sản phẩm tìm được
            ViewBag.countnumber = data.allsanphams.Count();
            List<LoaiSP> dt = new List<LoaiSP>();
            foreach (var item in data.allsanphams)
            {
                //lấy hãng sản xuất tương ứng với sản phẩm
                var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
                //lấy tên loại cho mỗi sản phẩm
                var y = db.LoaiSPs.Where(s => s.MaLoai == item.LoaiSP).FirstOrDefault();
                dt.Add(y);
            }
            //lấy tên loại để lấy source của ảnh.
            if (page > 1)
            {
                double? x = (double)8 * (page / 2);
                x = Math.Ceiling((double)x);
                int y = int.Parse(x.ToString());
                dt = dt.Skip(y).ToList();
            }

            ViewBag.listloaisp = dt;

            return PartialView("_Viewproductsearch", products.ToPagedList(page ?? 1, 8));
        }

        public void gettang(int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = db.Sanphams.OrderBy(s => s.HangSX).Where(s => s.LoaiSP == id).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = db.Sanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
            }
        }

        public void getgiam(int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = db.Sanphams.OrderByDescending(s => s.HangSX).Where(s => s.LoaiSP == id).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = db.Sanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
            }
        }

    }
}