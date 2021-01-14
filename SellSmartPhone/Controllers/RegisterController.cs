using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ActionResult Index(AccLogin Input, Account ACC)
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
                        var IDMax = db.Accounts.OrderByDescending(s => s.IDAccount).Take(1).FirstOrDefault();
                        ACC.IDAccount = IDMax.IDAccount + 1;
                        ACC.Password = Input.Password;
                        ACC.Phonenumber = Input.phone;
                        ACC.Ngaysinh = Input.DateOfBirth;
                        ACC.Username = Input.username;
                        ACC.Address = Input.address;
                        db.Accounts.Add(ACC);
                        db.SaveChanges();
                        return RedirectToAction("Index", "home");
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

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitPassword(string Email)
        {
            TempData["email"] = Email;
            int newpassword = new Random().Next(11111, 99999);
            Session["check"] = newpassword;
            if (Session["check"] != Session["newpassword"] && Session["newpassword"] != null)
            {
                TempData["warning"] = "Request time out !!";

                return View("ForgetPassword");
            }
            Session["newpassword"] = newpassword;
            var user = db.Accounts.SqlQuery("SELECT * FROM dbo.Account WHERE Email=@Email", new SqlParameter("@Email", Email)).FirstOrDefault();
            if (user != null)
            {
                MailMessage mail = new MailMessage("phamvandinhxyz@gmail.com", Email);
                mail.Subject = "Your Password !";
                mail.Body = string.Format("Hello : <h2>{0}</h2> Your password is : <h1>{1}</h1>.", user.Hoten, newpassword);
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential();
                nc.UserName = "phamvandinhxyz@gmail.com";
                nc.Password = "vodoivip12";
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Port = 587;
                smtp.Send(mail);
                TempData["alert"] = "Your password has been sent to " + user.Email;
            }
            else
            {
                TempData["warning"] = "User not Exist !!";
            }

            return View("ForgetPassword");
        }

    }


}