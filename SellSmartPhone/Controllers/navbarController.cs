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
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.ListCart = new Cart().GetCart(MaKH);
            cart.AddToCart((int)MaSP, MaKH);
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_ViewBasket", cart.ListCart);
        }
        public ActionResult deleteBasket(int? MaSP)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.ListCart = new Cart().GetCart(MaKH);
            cart.DeleteCart((int)MaSP, MaKH);
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_ViewBasket", cart.ListCart);
        }
        public ActionResult viewBasket()
        {
            if (Session["idaccount"] != null)
            {
                int MaKH = int.Parse(Session["idaccount"].ToString());
                cart.ListCart = new Cart().GetCart(MaKH);
                return PartialView("_ViewBasket", cart.ListCart);
            }
            else return PartialView("_ViewNo");

        }
    }
}