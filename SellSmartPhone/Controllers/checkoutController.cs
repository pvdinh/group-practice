using Microsoft.Ajax.Utilities;
using SellSmartPhone.Models;
using SellSmartPhone.Models.HandleCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Controllers
{
    public class checkoutController : Controller
    {
        // GET: checkout
        private SellphonesEntities db = new SellphonesEntities();
        private Cart cart = new Cart();
        public ActionResult Index()
        {
            if (Session["idaccount"] != null)
            {
                int MaKH = int.Parse(Session["idaccount"].ToString());
                cart.ListCart = new Cart().GetCart(MaKH);
                ViewBag.countincart = cart.ListCart.Count();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Cart()
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            cart.ListCart = new Cart().GetCart(MaKH);
            return PartialView("_ViewCart", cart.ListCart);
        }

        public ActionResult InfoCustomer()
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            Account User = db.Accounts.Where(s => s.IDAccount == MaKH).FirstOrDefault();
            return PartialView("_ViewInfoCustomer", User);
        }
        [HttpPost]
        public ActionResult result(Account acc, string ghichu, string group2)
        {
            int MaKH = int.Parse(Session["idaccount"].ToString());
            /*cập nhật lại thông tin khách hàng*/
            Account account = db.Accounts.Where(s => s.IDAccount == MaKH).FirstOrDefault();
            account.Hoten = acc.Hoten;
            account.Email = acc.Email;
            account.Phonenumber = acc.Phonenumber;
            account.Address = acc.Address;
            db.SaveChanges();
            /*===================================*/

            /*cập nhật lại thông tin đơn hàng*/
            if (group2 == null)
            {
                TempData["alertcheckout"] = "Bạn chưa chọn phương thức thanh toán";
                TempData["AlertType"] = "alert-warning";
                return RedirectToAction("index", "Checkout");
            }
            else
            {
                DonhangKH donhangKH = db.DonhangKHs.Where(s => s.MaKH == MaKH && s.Tinhtrangdonhang == 0).FirstOrDefault();
                donhangKH.Tinhtrangdonhang = 1;
                donhangKH.Phivanchuyen = 0;
                donhangKH.Ngaydatmua = DateTime.Now;
                donhangKH.TongTien = new Cart().TotalCart(MaKH);
                donhangKH.ghichu = ghichu;
                donhangKH.PhuongthucTT = group2;
                db.SaveChanges();
                TempData["checkout"] = "Thanh toán thành công !!";
                return RedirectToAction("index", "checkout");
            }
        }
    }
}