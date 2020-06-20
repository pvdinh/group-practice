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
        SellphonesEntities db = new SellphonesEntities();
        private const string cartsession = "cartsession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[cartsession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddCart(int productId, int quantity)
        {
            var cart = Session[cartsession];
            if (cart != null)
            {
                var product = db.Sanphams.Where(x => x.MaSP == productId).FirstOrDefault();
                //ép kiểu sang list cart item, nếu chứa sp rồi thì tăng thêm hoặc thì tạo mới rồi add vào
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.sanPham.MaSP == productId))
                {
                    foreach(var it in list)
                    {
                        if (it.sanPham.MaSP == productId)
                        {
                            it.soLuong += quantity;
                        }
                    }
                }
                else
                {
                    var itemcart = new CartItem();
                    itemcart.sanPham = product;
                    itemcart.soLuong = quantity;

                    list.Add(itemcart);
                }
                Session[cartsession] = list;
            }
            else
            {
                var product = db.Sanphams.Where(x => x.MaSP == productId).FirstOrDefault();
                //tạo 1 cart item mới
                var itemcart = new CartItem();
                itemcart.sanPham = product;
                itemcart.soLuong = quantity;

                var list = new List<CartItem>();
                list.Add(itemcart);

                Session[cartsession] = list;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Checkout()
        {
            return View();
        }
    }
}