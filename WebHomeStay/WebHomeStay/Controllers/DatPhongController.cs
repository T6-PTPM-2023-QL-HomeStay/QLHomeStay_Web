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
            var listPhong = db.PHONGs.Where(s => s.MALOAIPHONG == Maloai).ToList();

            if (listPhong.Count == 0)
            {
                ViewBag.TB = "Không tìm thấy phòng nào thuộc mã loại " + Maloai;
                ViewBag.LoaiAo = ""; // Không có phòng, đặt LoaiPhong là chuỗi trống
            }
            else
            {
                // Lấy tên loại phòng từ db.LOAIPHONGs
                string tenLoai = db.LOAIPHONGs
                    .Where(s => s.MALOAI == Maloai)
                    .Select(s => s.TENLOAI)
                    .FirstOrDefault();

                ViewBag.LoaiPhong = tenLoai;
            }

            return View(listPhong);
        }

        public ActionResult CTPhong(string ma)
        {
            var phong = db.PHONGs.SingleOrDefault(t => t.MAPHONG.Equals(ma));

            if (phong == null)
            {
                return HttpNotFound();
            }

            return View(phong);
        }



    }
}