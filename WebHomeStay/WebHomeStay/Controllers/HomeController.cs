using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebHomeStay.Models;

namespace WebHomeStay.Controllers
{
    public class HomeController : Controller
    {
        DatabaseDataContext db = new DatabaseDataContext();
        public ActionResult Index()
        {
            var model = db.PHONGs.Take(4).OrderByDescending(p => p.MAPHONG);
            ViewBag.Tasks = model;
            return View();
        }

        public ActionResult About()
        {
            
            return View();
        }

        public ActionResult Contact()
        {
           

            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}