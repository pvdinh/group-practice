using SellSmartPhone.Models;
using SellSmartPhone.Models.HandleCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace SellSmartPhone.Controllers
{
    public class CartController : Controller
    {
        //GET: Cart
        private SellphonesEntities db = new SellphonesEntities();
        private Cart cart = new Cart();
        public ActionResult Index(int? idSP)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.ListCart = new Cart().GetCart(MaKH);
            if (idSP != null)
            {
                cart.AddToCart((int)idSP, MaKH);
                cart.ListCart = new Cart().GetCart(MaKH);
            }
            ViewBag.countCart = cart.ListCart.Count();
            return View();
        }
        public ActionResult PayCart(int? idSP)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            if (idSP != null)
            {
                var data = db.Sanphams.Where(s => s.MaSP == idSP).FirstOrDefault();
            }
            return PartialView("_AddCart");
        }
        public ActionResult Cart()
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_Cart", cart.ListCart);
        }
        public ActionResult AddCart(int idSP)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.AddToCart(idSP, MaKH);
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_Cart", cart.ListCart);
        }
        public ActionResult SubCart(int idSP)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.SubCart(MaKH, idSP);
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_Cart", cart.ListCart);
        }
        public ActionResult DeleteCart(int idSP)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.DeleteCart(idSP, MaKH);
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_Cart", cart.ListCart);
        }


        //-------------------------------------------------------
        // GET: Cart
        //public ActionResult Index()
        //{
        //    var cart = Session[cartsession];
        //    var list = new List<CartItem>();
        //    if (cart != null)
        //    {
        //        list = (List<CartItem>)cart;
        //    }
        //    return View(list);
        //}
        //public ActionResult AddCart(int productId, int quantity)
        //{
        //    var cart = Session[cartsession];
        //    if (cart != null)
        //    {
        //        var product = db.Sanphams.Where(x => x.MaSP == productId).FirstOrDefault();
        //        //ép kiểu sang list cart item, nếu chứa sp rồi thì tăng thêm hoặc thì tạo mới rồi add vào
        //        var list = (List<CartItem>)cart;
        //        if (list.Exists(x => x.sanPham.MaSP == productId))
        //        {
        //            foreach (var it in list)
        //            {
        //                if (it.sanPham.MaSP == productId)
        //                {
        //                    it.soLuong += quantity;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var itemcart = new CartItem();
        //            itemcart.sanPham = product;
        //            itemcart.soLuong = quantity;

        //            list.Add(itemcart);
        //        }
        //        Session[cartsession] = list;
        //    }
        //    else
        //    {
        //        var product = db.Sanphams.Where(x => x.MaSP == productId).FirstOrDefault();
        //        //tạo 1 cart item mới
        //        var itemcart = new CartItem();
        //        itemcart.sanPham = product;
        //        itemcart.soLuong = quantity;

        //        var list = new List<CartItem>();
        //        list.Add(itemcart);

        //        Session[cartsession] = list;
        //    }
        //    return RedirectToAction("Index");
        //}
        //public ActionResult Checkout()
        //{
        //    return View();
        //}

        //public ActionResult IconAddCart()
        //{
        //    var icon = db.ChitietDHs.ToList();
        //    return PartialView("AddCart");
        //}

    }
}