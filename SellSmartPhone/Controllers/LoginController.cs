using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SellSmartPhone.Models;
using SellSmartPhone.Models.AccountLoginGoogle;

namespace SellSmartPhone.Controllers
{
    public class LoginController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();

        public int check(Account acc)
        {
            var resultEmail = db.Accounts.Where(p => p.Email == acc.Email && p.Password == acc.Password).FirstOrDefault();
            if (resultEmail != null)
            {
                return 1;
            }
            else
            {
                var checkEmail = db.Accounts.Where(p => p.Email == acc.Email).FirstOrDefault();
                if (checkEmail != null)
                {
                    return -1; // sai pass
                }
                else
                    return 0;// sai email
            }
        }


        //GET: Login
        protected void setAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else
            {
                TempData["AlertType"] = "alert-error";
            }
        }

     



        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(AccLogin login, Account Acc)
        {
            Acc.Password = login.Password;
            Acc.Email = login.EmailLogin;
            int checkAcc = check(Acc);
            if (checkAcc == 1)
            {
                var temp = db.Accounts.Where(p => p.Email == login.EmailLogin).FirstOrDefault();
                login.id = temp.IDAccount;
                login.name = temp.Hoten;
                login.EmailLogin = temp.Email;
                login.avatar = temp.Avatar;
                if (login.avatar == null)
                {
                    login.avatar = "/image/System/avtEmpty.jpg";
                }
                if (temp.Type =="customer")
                {
                    Session["idaccount"] = login.id;

                   
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["idaccount"] = login.id;
                    return RedirectToAction("Index", "Home");
                }
                //Session["idaccount"] = login.id;

                //setAlert("dang nhap thanh cong", "success");
                //return RedirectToAction("Index","Home");
            }
            else if (checkAcc == -1)
            {
                setAlert("sai mat khau", "error");
                return View("Index");
            }
            else
            {
                setAlert("khong ton tai tai khoan", "error");
                return View("Index");
            } 
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index","home");
        }

        [HttpPost]
        public ActionResult confirm(AccGoogle recivedData)
        {
            if(recivedData != null)
            {
                Session["idaccount"] = AccGoogle.LoginGoogle(recivedData, db);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Index");
            }

        }
    }
}