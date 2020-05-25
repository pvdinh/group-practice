using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SellSmartPhone.Models;

namespace SellSmartPhone.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Manage()
        {
            ViewBag.user = Session["user"];
            return View();
        }

        [HttpPost]
        public ActionResult Manage(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            //ném thông tin acc đang login vào x
            Account x = (Account)Session["user"];
            //lưu vào viewbag để hiển thị bên view
            ViewBag.user = Session["user"];

            if (string.Compare(OldPassword.Replace(" ", string.Empty), x.Password.Replace(" ", string.Empty), true) == 0)
            {
                if (string.Compare(ConfirmPassword, NewPassword, true) == 0)
                {
                    using (SellphonesEntities db = new SellphonesEntities())
                    {
                        db.edit_password(x.IDAccount, NewPassword);
                    }
                    return RedirectToAction("index", "home");
                }
                else return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult EditInfo()
        {
            using (SellphonesEntities db = new SellphonesEntities())
            {
                int temp = int.Parse(Session["idaccount"].ToString());
                var x = db.Accounts.Where(s => s.IDAccount == temp).FirstOrDefault();
                ViewBag.user = x;
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditInfo(Account edit)
        {
            //ném thông tin acc đang login vào x
            Account login = (Account)Session["user"];
            string origi = "1/1/0001 12:00:00 AM";
            DateTime origin = Convert.ToDateTime(origi);
            if (edit.Ngaysinh == origin)
            {
                return RedirectToAction("EditInfo", "Account");
            }
            using (SellphonesEntities db = new SellphonesEntities())
            {
                db.update_account(login.IDAccount, edit.Email, edit.Phonenumber, edit.Hoten, edit.Ngaysinh, edit.Address);
                int temp = int.Parse(Session["idaccount"].ToString());
                var x = db.Accounts.Where(s => s.IDAccount == temp).FirstOrDefault();
                ViewBag.user = x;
            }
            return View();
        }
        public ActionResult ImageUser()
        {
            Account login = (Account)Session["user"];
            using (SellphonesEntities db = new SellphonesEntities())
            {
                var user = db.Accounts.Where(s => s.IDAccount == login.IDAccount).FirstOrDefault();
                return PartialView("_ViewImageUser", user);
            }
        }
        [HttpPost]
        public ActionResult ImageUser(HttpPostedFileBase file)
        {
            Account login = (Account)Session["user"];
            if (file != null)
            {
                //save image in folder
                string physicaPath = Server.MapPath("/template/assets/images/" + file.FileName);
                file.SaveAs(physicaPath);
                //update in DataBase
                using (SellphonesEntities db = new SellphonesEntities())
                {
                    var x = db.Accounts.Where(s => s.IDAccount == login.IDAccount).FirstOrDefault();
                    x.Avatar = file.FileName;
                    db.SaveChanges();
                    return RedirectToAction("EditInfo", "Account");
                }
            }
            else
            {
                return RedirectToAction("EditInfo", "Account");
            }

        }
    }
}