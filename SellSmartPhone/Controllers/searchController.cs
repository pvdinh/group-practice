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
        //id là mã loại của sản phẩm.
        public ActionResult Index(int? id, string type, string sort, string btnsearch, int? mahang, int? searchAdvanced)
        {
            Session.Timeout = 1440;

            //lưu lại kiểu sắp xếp và loại sắp xếp
            Session["sort"] = sort;
            Session["type"] = type;

            /*lọc sản phẩm khi dùng filter (searchAdvanced)==== nếu giá trị này != null tức là đang dùng filter(searchAdvanced)*/
            Session["searchAdvanced"] = searchAdvanced;
            /*===========================*/

            //lọc khi bấm icon search
            Session["btnsearch"] = btnsearch;
            /*===========================*/

            //lọc khi click ở navbar, có thể dùng 1 phần cho fliter cùng với sesion["searchAdvanced"]
            Session["mahang"] = mahang;
            Session["id"] = id; //mã của sản phẩm
            /*===========================*/
            return View();
        }

        public JsonResult listUIproduct(string term)  //dữ liệu này được dùng cho JqueryUI Autocomplete ở header(Layout)
        {
            List<string> data = new List<string>();
            data = db.Sanphams.Where(s => s.TenSP.Contains(term)).Select(s => s.TenSP).Take(7).ToList(); //hiển thị 7 sản phẩm
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult resultproduct(int? id, int? page, string btnsearch, int? mahang)
        {
            data.allsanphams = new List<Sanpham>();

            /*==============================Tìm kiếm theo từ khoá=======================================*/

            if (Session["btnsearch"] != null && Session["id"] == null)
            {
                if (Session["type"] != null && Session["type"].ToString() != "")
                {
                    /* tuỳ chọn tăng giảm sẽ dùng luôn session["sort"] trước đó đã được lưu */
                    /* sort chỉ hoạt động khi có giá trị của session["sort"] , mặc định sắp xếp theo giá*/
                    if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                    {
                        data.allsanphams = db.Sanphams.OrderByDescending(s => s.Gia).Where(s => s.TenSP.Contains(btnsearch)).ToList();
                        if (Session["sort"] != null)
                        {
                            getgiambtnSearch(btnsearch);
                        }
                    }
                    else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                    {
                        data.allsanphams = db.Sanphams.OrderBy(s => s.Gia).Where(s => s.TenSP.Contains(btnsearch)).ToList();
                        if (Session["sort"] != null)
                        {
                            gettangbtnSearch(btnsearch);
                        }
                    }
                }
                else
                {
                    data.allsanphams = db.Sanphams.Where(s => s.TenSP.Contains(btnsearch)).ToList();
                }
            }

            /*===========================================================================================*/

            /*===================================== Tìm kiếm theo Loại sản phẩm ============================*/
            if (Session["id"] != null && Session["mahang"] == null) //session["id"] là loại sản phẩm như đã nói.
            {
                //nếu có giá trị của type (tăng,giảm)
                if (Session["type"] != null && Session["type"].ToString() != "")
                {
                    /* tuỳ chọn tăng giảm sẽ dùng luôn session["sort"] trước đó đã được lưu */
                    /* sort chỉ hoạt động khi có giá trị của session["sort"] , mặc định sắp xếp theo giá*/
                    if (Session["searchAdvanced"] != null && Session["filterresult"] != null)//đang sử dụng filter
                    {
                        data.allsanphams = (List<Sanpham>)Session["filterresult"]; //Session["filterresult"] là danh sách đã được lọc theo (min,max) giá từ trước
                        data.allsanphams = data.allsanphams.Where(s => s.LoaiSP == id).ToList();
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamloai(id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                            if (Session["sort"] != null)
                            {
                                gettangloai(id);
                            }
                        }
                    }
                    else //dùng những tuỳ chọn search khác (ở navbar, ở header)
                    {
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            data.allsanphams = db.Sanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamloai(id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            data.allsanphams = db.Sanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                            if (Session["sort"] != null)
                            {
                                gettangloai(id);
                            }
                        }
                    }
                }
                else
                {   //show mặc định khi không chọn sắp xếp
                    //nếu sesion["sort"] != null thì ko có sự thay đổi, chỉ thay đổi khi chọn kiểu sắp xếp (tăng, giảm)
                    if (Session["searchAdvanced"] != null && Session["filterresult"] != null) //lấy sản phẩm từ bộ lọc filter
                    {
                        data.allsanphams = (List<Sanpham>)Session["filterresult"];
                        data.allsanphams = data.allsanphams.Where(s => s.LoaiSP == id).ToList();
                    }
                    else
                    {
                        data.allsanphams = db.Sanphams.Where(s => s.LoaiSP == id).ToList();
                    }
                }
            }

            /*==========================================================================================*/

            /*===================================== Tìm kiếm theo Hãng sản phẩm ============================*/
            if (Session["mahang"] != null && Session["id"] == null)
            {
                //nếu có giá trị của type (tăng,giảm)
                if (Session["type"] != null && Session["type"].ToString() != "")
                {
                    /* tuỳ chọn tăng giảm sẽ dùng luôn session["sort"] trước đó đã được lưu */
                    /* sort chỉ hoạt động khi có giá trị của session["sort"] , mặc định sắp xếp theo giá*/
                    if (Session["searchAdvanced"] != null && Session["filterresult"] != null)
                    {
                        data.allsanphams = (List<Sanpham>)Session["filterresult"];//Session["filterresult"] là danh sách đã được lọc theo (min,max) giá từ trước
                        data.allsanphams = data.allsanphams.Where(s => s.HangSX == mahang).ToList();
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamhang(mahang);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                gettanghang(mahang);
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            data.allsanphams = db.Sanphams.OrderByDescending(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamhang(mahang);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            data.allsanphams = db.Sanphams.OrderBy(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                gettanghang(mahang);
                            }
                        }
                    }
                }
                else
                {   //show mặc định khi không chọn sắp xếp
                    //nếu  sắp xếp != null thì ko có sự thay đổi, chỉ thay đổi khi chọn kiểu sắp xếp (tăng, giảm)
                    if (Session["searchAdvanced"] != null && Session["filterresult"] != null)
                    {
                        data.allsanphams = (List<Sanpham>)Session["filterresult"];
                        data.allsanphams = data.allsanphams.Where(s => s.HangSX == mahang).ToList();
                    }
                    else
                        data.allsanphams = db.Sanphams.Where(s => s.HangSX == mahang).ToList();
                }
            }

            /*==========================================================================================*/


            /*==========================================================Tìm kiếm theo mã hãng và loại sản phẩm=========================================================================*/

            if (Session["mahang"] != null && Session["id"] != null) //session["id"] là loại sản phẩm như đã nói.
            {
                //nếu có giá trị của type (tăng,giảm)
                if (Session["type"] != null && Session["type"].ToString() != "")
                {
                    /* tuỳ chọn tăng giảm sẽ dùng luôn session["sort"] trước đó đã được lưu */
                    /* sort chỉ hoạt động khi có giá trị của session["sort"] , mặc định sắp xếp theo giá*/
                    if (Session["searchAdvanced"] != null && Session["filterresult"] != null)
                    {
                        data.allsanphams = (List<Sanpham>)Session["filterresult"]; //Session["filterresult"] là danh sách đã được lọc theo (min,max) giá từ trước
                        data.allsanphams = data.allsanphams.Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamNavbar(mahang, id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                gettangNavbar(mahang, id);
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            data.allsanphams = db.Sanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamNavbar(mahang, id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            data.allsanphams = db.Sanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                gettangNavbar(mahang, id);
                            }
                        }
                    }
                }
                else
                {   //show mặc định khi không chọn sắp xếp
                    //nếu  sắp xếp != null thì ko có sự thay đổi, chỉ thay đổi khi chọn kiểu sắp xếp (tăng, giảm)
                    if (Session["searchAdvanced"] != null && Session["filterresult"] != null)
                    {
                        data.allsanphams = (List<Sanpham>)Session["filterresult"];
                        data.allsanphams = data.allsanphams.Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                    }
                    else
                        data.allsanphams = db.Sanphams.Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                }
            }
            /*==============================================================================================================================================================================*/

            //đếm tống số sản phẩm tìm được
            ViewBag.countnumber = data.allsanphams != null ? data.allsanphams.Count() : 0;
            List<LoaiSP> dt = new List<LoaiSP>();
            foreach (var item in data.allsanphams)
            {
                //lấy hãng sản xuất tương ứng với sản phẩm
                var x = db.HangSXes.ToList().Where(s => s.MaHangSX == item.HangSX).FirstOrDefault();
                products.Add(new viewproductHome(item.MaSP, item.TenSP, item.Gia, item.Anh, x.tenhang, item.Ishot, item.Isnew));
                //lấy loại sản phẩm tương ứng mã loại
                var y = db.LoaiSPs.Where(s => s.MaLoai == item.LoaiSP).FirstOrDefault();
                dt.Add(y);
            }

            /*lấy tên loại để lấy source của ảnh. Dùng được cho 2 trường hơp : chỉ view 1 loại sản phẩm và nhiều loại sản phẩm của 1 hãng*/
            /*bỏ đi sẽ lỗi ảnh*/
            if (page > 1)
            {
                double x = double.Parse(page.ToString()) / 2;
                x = Math.Ceiling((double)x);      //làm tròn lên
                int y = int.Parse(x.ToString()) * 8;
                dt = dt.Skip(y).ToList();        //bỏ qua y phần tử khi chuyển trang.
            }

            ViewBag.listloaisp = dt;
            return PartialView("_Viewproductsearch", products.ToPagedList(page ?? 1, 8));
        }

        public void gettangloai(int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.HangSX).Where(s => s.LoaiSP == id).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
            }
        }
        public void getgiamloai(int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.HangSX).Where(s => s.LoaiSP == id).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
            }
        }
        public void gettanghang(int? mahang)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.HangSX).Where(s => s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
            }
        }
        public void getgiamhang(int? mahang)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.HangSX).Where(s => s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
            }
        }
        public void getgiambtnSearch(string btnsearch)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.HangSX).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
        }
        public void gettangbtnSearch(string btnsearch)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.HangSX).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
        }
        public void gettangNavbar(int? mahang, int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.HangSX).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
        }
        public void getgiamNavbar(int? mahang, int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.HangSX).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                data.allsanphams = data.allsanphams.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
        }
    }
}