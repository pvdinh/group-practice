using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Controllers
{
    public class searchAdvancedController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();
        classData data = new classData();
        // GET: searchAdvanced

        [HttpGet]
        public ActionResult Index(double? minprice,double? maxprice,int? loaisp,int? hangsx)
        {
            Session.Timeout = 1440;
            if (minprice != null && maxprice != null)
            {
                //nếu có (min,max) sẽ lọc ra danh sách sản phẩm để dùng bên /search/index
                data.allsanphams = db.Sanphams.Where(s => s.Gia >= minprice && s.Gia <= maxprice).ToList();
                Session["filterresult"] = data.allsanphams; // lưu list vào session
            }
            else Session["filterresult"] = null;  //nếu filter mà ko có (min,max)
            return RedirectToAction("index","search",new { id=loaisp, mahang=hangsx, searchAdvanced = 1007 });
        }
        public ActionResult result() // bộ lọc filter
        {
            data.allloaisps = db.LoaiSPs.ToList();
            data.allhangsxs = db.HangSXes.ToList();
            return PartialView("_ViewsearchAdvanced",data);
        }
    }
}