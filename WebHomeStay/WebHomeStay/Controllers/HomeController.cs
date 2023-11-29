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
            var loaiPhongList = db.LOAIPHONGs.ToList();
            ViewBag.LoaiPhong = new SelectList(loaiPhongList, "MALOAI", "TENLOAI");
            var model = db.PHONGs.Take(3).OrderByDescending(p => p.MAPHONG);
            ViewBag.Tasks = model;
            return View();
        }

       
        public ActionResult About()
        {           
            return View();
        }

        public ActionResult LoaiPhong()
        {
            var model = db.LOAIPHONGs.OrderByDescending(p => p.TENLOAI);
            return PartialView(model);
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

        [HttpPost]
        public ActionResult TimKiemPhong(string loaiPhong, decimal giaTien, int soNguoi)
        {
            // Lấy danh sách loại phòng và gán vào ViewBag
            var loaiPhongList = db.LOAIPHONGs.ToList();
            ViewBag.LoaiPhong = new SelectList(loaiPhongList, "MALOAI", "TENLOAI");

            var danhSachPhong = db.PHONGs
                .Where(p => p.TRANGTHAI == "Trống" &&
                            (loaiPhong == null || p.MALOAIPHONG == loaiPhong) &&
                            (giaTien == 0 || (decimal)p.LOAIPHONG.GIAPH <= giaTien) &&
                            p.SOLUONGNGUOIO >= soNguoi)
                .ToList();

            return View("TimKiemPhong", danhSachPhong);
        }

    }
}