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
            ViewBag.DichVuList = new SelectList(db.DVus, "MADV", "TenDV");
            var phong = db.PHONGs.FirstOrDefault(s => s.MAPHONG.Equals(maPhong));

            if (phong == null)
            {
                // Xử lý khi không tìm thấy phòng
                return HttpNotFound();
            }
            ViewBag.phong = phong;
            ViewBag.dsnguoidung = db.KHACHHANGs.ToList();
           
            return View();
        }

        [HttpPost]
        public ActionResult DatPhongKH(HOADON hd, CTHD ct, FormCollection f)
        {
           
            var makh = f["makh"];
            var mahd = f["mahd"];
            var ngaytao = f["ngaytao"];
            var madv = f["madv"];
            var maphong = f["maph"];
            var tongtien = f["tongtien"];
            var giamgia = f["giamgia"];
            var sldichvu = f["sldichvu"];


            hd.MAKH = makh;
            hd.NGAYTAO = DateTime.Now;

            db.HOADONs.InsertOnSubmit(hd);
            db.SubmitChanges();

            
            IList<CTHD> cthds = new List<CTHD>();
            //Khúc này sẽ add từng CTHD vào 
            ct.MADV = 1;
            ct.MAHD = hd.MAHD;
            ct.MAPHONG = maphong;

            db.CTHDs.InsertOnSubmit(ct);
            db.SubmitChanges();
            ct.SLDICHVU = 1;
            ct.TONGTIENTHANHTOAN = ct.DVu.DONGIA * ct.SLDICHVU + ct.PHONG.LOAIPHONG.GIAPH;

            db.SubmitChanges();
            var phong = db.PHONGs.SingleOrDefault(p => p.MAPHONG == maphong);
            if (phong != null)
            {
                // In ra giá trị trạng thái phòng để kiểm tra
                Console.WriteLine("Trạng thái phòng trước khi cập nhật: " + phong.TRANGTHAI);

                // Kiểm tra và cập nhật trạng thái phòng chỉ khi phòng đang ở trạng thái trống
                if (phong.TRANGTHAI == "Trống")
                {
                    phong.TRANGTHAI = "Đã thuê";
                    db.SubmitChanges();
                }
            }

           
            return RedirectToAction("Index");
        }


    }
}