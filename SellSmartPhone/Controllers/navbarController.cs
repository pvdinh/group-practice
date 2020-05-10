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
        public ActionResult categoryproduct() //xem tất cả các loại sản phẩm phần header
        {
            using (SellphonesEntities db = new SellphonesEntities())
            {
                // lấy danh sách loại sản phẩm
                List<LoaiSP> listcategory = db.LoaiSPs.ToList();
                return PartialView("_Viewcategoryproduct", listcategory);
            }
        }
        public ActionResult categoryproductWithBrand() //hiển thị navbar gồm list product và list brand
        {
            using (SellphonesEntities db = new SellphonesEntities())
            {
                classData data = new classData();
                data.allloaisps = db.LoaiSPs.ToList();
                data.allhangsxs = db.HangSXes.ToList();
                return PartialView("_ViewcategoryproductWithBrand", data);
            }
        }
    }
}