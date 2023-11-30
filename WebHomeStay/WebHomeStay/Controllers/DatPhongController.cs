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
                ViewBag.LoaiPhong = ""; // Không có phòng, đặt LoaiPhong là chuỗi trống
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

        [HttpGet]
        public ActionResult DatPhongKH()
        {
            string maPhong = RouteData.Values["id"] as string;

            if (string.IsNullOrEmpty(maPhong))
            {
                // Xử lý khi giá trị không tồn tại
                return HttpNotFound();
            }

            var phong = db.PHONGs.FirstOrDefault(s => s.MAPHONG.Equals(maPhong));

            if (phong == null)
            {
                // Xử lý khi không tìm thấy phòng
                return HttpNotFound();
            }
            ViewBag.phong = phong;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult DatPhongKH(HOPDONG hd, CTHD ct)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!User.Identity.IsAuthenticated)
            {
                // Xử lý khi người dùng chưa đăng nhập
                return RedirectToAction("Login", "Auth");
            }

            string maPhong = RouteData.Values["id"] as string;

            if (string.IsNullOrEmpty(maPhong))
            {
                // Xử lý khi giá trị không tồn tại
                return HttpNotFound();
            }

            var phong = db.PHONGs.FirstOrDefault(s => s.MAPHONG.Equals(maPhong));

            if (phong == null)
            {
                // Xử lý khi không tìm thấy phòng
                return HttpNotFound();
            }

            // Lấy thông tin khách hàng từ cơ sở dữ liệu, ví dụ:
            var khachHang = db.KHACHHANGs.FirstOrDefault();
            // Trong trường hợp thực tế, bạn cần thiết kế cách lấy thông tin khách hàng dựa trên yêu cầu của bạn.

            // Tính toán các giá trị còn thiếu
            hd.MAPHONG = phong.MAPHONG;
            hd.MAKHACH = khachHang.MAKH;
            hd.THOIGIANTAO = DateTime.Now;
            hd.TRANGTHAI = "Chưa thanh toán";

            var tongtien = db.LOAIPHONGs.FirstOrDefault();

            ct.MAHD = hd.MAHOPDONG;
            // Thực hiện các bước để tính giá trị cho ct, ví dụ:
            ct.TONGTIENTHANHTOAN = tongtien.GIAPH;

            // Thêm HOPDONG và CTHD vào cơ sở dữ liệu
            db.HOPDONGs.InsertOnSubmit(hd);
            db.CTHDs.InsertOnSubmit(ct);

            db.SubmitChanges();

            return RedirectToAction("Index");
        }


    }
}