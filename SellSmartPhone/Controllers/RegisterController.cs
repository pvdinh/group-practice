using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Controllers
{
    public class RegisterController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();

        protected void setAlert1(string message, string type)
        {
            TempData["AlertMessage"] = message;
            
            if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }   
        }
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AccLogin Input,Account ACC )
        {
           
            var CheckMail = db.Accounts.Where(p => p.Email == Input.EmailLogin).FirstOrDefault();
            if (CheckMail == null)
            {
                ACC.Email = Input.EmailLogin;
                ACC.Hoten = Input.name;
                if (Input.Password != Input.RePassword)
                {
                    setAlert1("Xac nhan mat khau sai !", "warning");
                    return View("Index");
                }
                else
                {
                    try
                    {
                        var IDMax = db.Database.SqlQuery<Account>("SELECT MAX(IDAccount) FROM dbo.Account").ToString();
                        ACC.IDAccount = Int32.Parse(IDMax) +1;
                        ACC.Password = Input.Password;
                        ACC.Phonenumber = Input.phone;
                        ACC.Ngaysinh = Input.DateOfBirth;
                        ACC.Username = Input.username;
                        ACC.Address = Input.address;
                        db.Accounts.Add(ACC);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Shop");
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw;
                    }
                }
                
            }
            else
            {
                setAlert1("Email da ton tai", "error");
                return View("Index");
            }           
        }

        
    }
}