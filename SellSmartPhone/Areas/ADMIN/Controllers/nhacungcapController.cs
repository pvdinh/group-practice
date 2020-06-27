using SellSmartPhone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellSmartPhone.Areas.ADMIN.Controllers
{
    public class nhacungcapController : Controller
    {
        SellphonesEntities db = new SellphonesEntities();
        // GET: ADMIN/nhacungcap
        public ActionResult Index()
        {
            var model = db.Nhacungcaps.ToList();
            return View("ListNcc",model);
        }
        public ActionResult Edit(int ? id)
        {
            var model = db.Nhacungcaps.Where(p => p.IDNhacungcap == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Nhacungcap ncc)
        {
            var model = db.Nhacungcaps.Where(p => p.IDNhacungcap == ncc.IDNhacungcap).FirstOrDefault();
            model.TenNhaungcap = ncc.TenNhaungcap;
            model.phonenumber = ncc.phonenumber;
            model.email = ncc.email;
            db.Nhacungcaps.AddOrUpdate(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult profile(int? id)
        {
            var model = db.Nhacungcaps.Where(p => p.IDNhacungcap == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult add(Nhacungcap ncc)
        {
            return View();
        }
    }
}