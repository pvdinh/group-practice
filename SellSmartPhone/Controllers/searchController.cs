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
        List<get_product_discount_Result> listproducts = new List<get_product_discount_Result>();
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
                        data.allsanphams = db.Sanphams.Where(s => s.TenSP.Contains(btnsearch)).ToList();
                        foreach (var item in data.allsanphams)
                        {
                            var x = db.get_product_discount().Where(s => s.MaSP == item.MaSP).FirstOrDefault();
                            listproducts.Add(x);
                        }
                        listproducts = listproducts.OrderByDescending(s => s.Gia).ToList();
                        if (Session["sort"] != null)
                        {
                            getgiambtnSearch(btnsearch);
                        }
                    }
                    else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                    {
                        data.allsanphams = db.Sanphams.Where(s => s.TenSP.Contains(btnsearch)).ToList();
                        foreach (var item in data.allsanphams)
                        {
                            var x = db.get_product_discount().Where(s => s.MaSP == item.MaSP).FirstOrDefault();
                            listproducts.Add(x);
                        }
                        listproducts = listproducts.OrderBy(s => s.Gia).ToList();
                        if (Session["sort"] != null)
                        {
                            gettangbtnSearch(btnsearch);
                        }
                    }
                }
                else
                {
                    data.allsanphams = db.Sanphams.Where(s => s.TenSP.Contains(btnsearch)).ToList();
                    foreach (var item in data.allsanphams)
                    {
                        var x = db.get_product_discount().Where(s => s.MaSP == item.MaSP).FirstOrDefault();
                        listproducts.Add(x);
                    }
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
                        listproducts = (List<get_product_discount_Result>)Session["filterresult"]; //Session["filterresult"] là danh sách đã được lọc theo (min,max) giá từ trước
                        listproducts = listproducts.Where(s => s.LoaiSP == id).ToList();
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamloai(id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
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
                            listproducts = db.get_product_discount().OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamloai(id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            listproducts = db.get_product_discount().OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
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
                        listproducts = (List<get_product_discount_Result>)Session["filterresult"];
                        listproducts = listproducts.Where(s => s.LoaiSP == id).ToList();
                    }
                    else
                    {
                        listproducts = db.get_product_discount().Where(s => s.LoaiSP == id).ToList();
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
                        listproducts = (List<get_product_discount_Result>)Session["filterresult"];//Session["filterresult"] là danh sách đã được lọc theo (min,max) giá từ trước
                        listproducts = listproducts.Where(s => s.HangSX == mahang).ToList();
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamhang(mahang);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
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
                            listproducts = db.get_product_discount().OrderByDescending(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamhang(mahang);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            listproducts = db.get_product_discount().OrderBy(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
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
                        listproducts = (List<get_product_discount_Result>)Session["filterresult"];
                        listproducts = listproducts.Where(s => s.HangSX == mahang).ToList();
                    }
                    else
                        listproducts = db.get_product_discount().Where(s => s.HangSX == mahang).ToList();
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
                        listproducts = (List<get_product_discount_Result>)Session["filterresult"]; //Session["filterresult"] là danh sách đã được lọc theo (min,max) giá từ trước
                        listproducts = listproducts.Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                        if (string.Compare(Session["type"].ToString(), "giam", true) == 0)
                        {
                            listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamNavbar(mahang, id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
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
                            listproducts = db.get_product_discount().OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                            if (Session["sort"] != null)
                            {
                                getgiamNavbar(mahang, id);
                            }
                        }
                        else if (string.Compare(Session["type"].ToString(), "tang", true) == 0)
                        {
                            listproducts = db.get_product_discount().OrderBy(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
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
                        listproducts = (List<get_product_discount_Result>)Session["filterresult"];
                        listproducts = listproducts.Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                    }
                    else
                        listproducts = db.get_product_discount().Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
                }
            }
            /*==============================================================================================================================================================================*/

            //đếm tống số sản phẩm tìm được
            ViewBag.countnumber = listproducts != null ? listproducts.Count() : 0;
            return PartialView("_Viewproductsearch", listproducts.ToPagedList(page ?? 1, 8));
        }

        public void gettangloai(int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.HangSX).Where(s => s.LoaiSP == id).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
            }
        }
        public void getgiamloai(int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.HangSX).Where(s => s.LoaiSP == id).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id).ToList();
            }
        }
        public void gettanghang(int? mahang)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.HangSX).Where(s => s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
            }
        }
        public void getgiamhang(int? mahang)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.HangSX).Where(s => s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.HangSX == mahang).ToList();
            }
        }
        public void getgiambtnSearch(string btnsearch)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.HangSX).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
        }
        public void gettangbtnSearch(string btnsearch)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.HangSX).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.TenSP.Contains(btnsearch)).ToList();
            }
        }
        public void gettangNavbar(int? mahang, int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.HangSX).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderBy(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
        }
        public void getgiamNavbar(int? mahang, int? id)
        {
            if (string.Compare(Session["sort"].ToString(), "hang", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.HangSX).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
            else if (string.Compare(Session["sort"].ToString(), "gia", true) == 0)
            {
                listproducts = listproducts.OrderByDescending(s => s.Gia).Where(s => s.LoaiSP == id && s.HangSX == mahang).ToList();
            }
        }
    }
}