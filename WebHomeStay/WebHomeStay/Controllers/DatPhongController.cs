using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebHomeStay.Models;
namespace WebHomeStay.Controllers
{
    public class DatPhongController : Controller
    {
        DatabaseDataContext db = new DatabaseDataContext();
        // GET: DatPhong
        public ActionResult Index()
        {
            List<PHONG> p = db.PHONGs.ToList();
            
            return View(p);
        }

        public ActionResult LoaiPhong(string Maloai)
        {
            var list = db.PHONGs.Where(s => s.MALOAIPHONG.Equals(Maloai)).ToList();

            if (list.Count == 0)
            {
                ViewBag.TB = "Khong tim thay";
            }
            else
            {
                // Lấy ra tên từ đối tượng LOAIPHONG
                string ten = db.LOAIPHONGs
                    .Where(lp => lp.MALOAI.Equals(Maloai))
                    .Select(lp => lp.TENLOAI)
                    .FirstOrDefault();

                ViewBag.Loai = ten;
            }
            return View();
        }

    

    }
}