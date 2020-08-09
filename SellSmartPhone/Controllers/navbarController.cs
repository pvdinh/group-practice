using SellSmartPhone.Models;
using SellSmartPhone.Models.HandleCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Controllers
{
    public class navbarController : Controller
    {
        private SellphonesEntities db = new SellphonesEntities();
        private Cart cart = new Cart();
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

        public ActionResult addBasket(int? MaSP)
        {
            Session["idaccount"] = 1000;
            cart.ListCart = new Cart().GetCart(1000);
            cart.AddToCart((int)MaSP, 1000);
            cart.ListCart = new Cart().GetCart(1000);
            return PartialView("_ViewBasket", cart.ListCart);
        }
        public ActionResult deleteBasket(int? MaSP)
        {
            Session["idaccount"] = 1000;
            cart.ListCart = new Cart().GetCart(1000);
            cart.DeleteCart((int)MaSP, 1000);
            cart.ListCart = new Cart().GetCart(1000);
            return PartialView("_ViewBasket", cart.ListCart);
        }
        public ActionResult viewBasket()
        {
            Session["idaccount"] = 1000;
            cart.ListCart = new Cart().GetCart(1000);
            return PartialView("_ViewBasket",cart.ListCart);
        }
    }
}