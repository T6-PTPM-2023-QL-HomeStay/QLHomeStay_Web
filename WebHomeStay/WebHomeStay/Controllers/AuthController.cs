using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebHomeStay.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebHomeStay.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        DatabaseDataContext db = new DatabaseDataContext();

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        private int GetNextIDFromDatabase()
        {
            int? maxID = (from kh in db.KHACHHANGs
                          select kh.MAKH)
                          .AsEnumerable()
                          .Select(id => int.TryParse(id.Substring(2), out int n) ? (int?)n : null)
                          .Max();

            return (maxID ?? 0) + 1;
        }

        private string GenerateNewAccountID(string prefix)
        {
            string newID = "";
            bool isUnique = false;

            while (!isUnique)
            {
                newID = prefix + GetNextIDFromDatabase();
                // Kiểm tra xem mã tài khoản đã tồn tại trong bảng chưa
                isUnique = !db.TAIKHOANs.Any(tk => tk.MATK == newID || tk.MATK == newID);
            }

            return newID;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KHACHHANG kh, TAIKHOAN ac, FormCollection f)
        {
            // Sử dụng hàm để sinh mã tài khoản và mã khách hàng
            string newMATK = GenerateNewAccountID("TK");
            string newMAKH = GenerateNewAccountID("KH");

            var hoten = f["HotenKH"];
            var tendn = f["TenDN"];
            var matkhau = f["MatKhau"];
            var rematkhau = f["ReMatkhau"];
            var dienthoai = f["Dienthoai"];
            var ngaysinh = f["Ngaysinh"];
            var soCMND = f["CMND"];

            if (string.IsNullOrEmpty(hoten) ||
                string.IsNullOrEmpty(tendn) ||
                string.IsNullOrEmpty(matkhau) ||
                string.IsNullOrEmpty(rematkhau) ||
                string.IsNullOrEmpty(dienthoai) ||
                string.IsNullOrEmpty(ngaysinh) ||
                string.IsNullOrEmpty(soCMND))
            {
                ViewData["Loi"] = "Vui lòng điền đầy đủ thông tin!";
                return View();
            }

            if (matkhau != rematkhau)
            {
                ViewData["Loi"] = "Mật khẩu nhập lại không khớp!";
                return View();
            }

            // Kiểm tra độ dài số điện thoại và số CMND
            if (dienthoai.Length != 10 || soCMND.Length != 12)
            {
                ViewData["Loi"] = "Số điện thoại phải có 10 số và số CMND phải có 12 số!";
                return View();
            }

            // Kiểm tra sự tồn tại của tên đăng nhập
            if (db.TAIKHOANs.Any(t => t.TENDANGNHAP == tendn))
            {
                ViewData["Loi"] = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // Thêm mới tài khoản vào bảng TAIKHOAN
            ac.MATK = newMATK;
            ac.TENDANGNHAP = tendn;
            ac.MATKHAU = HashPassword(matkhau);

            db.TAIKHOANs.InsertOnSubmit(ac);
            db.SubmitChanges();

            // Gắn mã tài khoản vào bảng KHACHHANG
            kh.HOTEN = hoten;
            kh.SDT = dienthoai;
            kh.NGAYSINH = DateTime.Parse(ngaysinh);
            kh.CCCD = soCMND;
            kh.MAKH = newMAKH; // Gán giá trị của MAKH

            // Gán mã tài khoản vào bảng KHACHHANG
            kh.MATK = newMATK;

            db.KHACHHANGs.InsertOnSubmit(kh);
            db.SubmitChanges();

            ViewBag.TB = "Đăng ký thành công!";
            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            // Khai báo biến để nhận giá trị từ form f
            var tendn = f["TenDN"];
            var matkhau = f["MatKhau"];

            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống!";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống!";
            }
            if (!string.IsNullOrEmpty(tendn) && !string.IsNullOrEmpty(matkhau))
            {
                // Thay đổi dòng này để lấy tài khoản dựa trên tên đăng nhập, không cần kiểm tra mật khẩu ở đây
                TAIKHOAN ac = db.TAIKHOANs.SingleOrDefault(t => t.TENDANGNHAP == tendn);

                // Kiểm tra nếu tài khoản tồn tại
                if (ac != null)
                {
                    // Lấy mật khẩu đã mã hóa từ cơ sở dữ liệu
                    string hashedPasswordFromDatabase = ac.MATKHAU;

                    // Kiểm tra mật khẩu với mật khẩu đã mã hóa
                    if (hashedPasswordFromDatabase == HashPassword(matkhau))
                    {
                        // Lấy khách hàng tương ứng dựa trên mã tài khoản
                        KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(t => t.MATK == ac.MATK);

                        if (kh != null)
                        {
                            ViewBag.TB = "Đăng nhập thành công!";
                            Session["KH"] = kh;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // Xử lý trường hợp không tìm thấy thông tin khách hàng
                            ViewData["Loi1"] = "Không tìm thấy thông tin khách hàng!";
                        }
                    }
                    else
                    {
                        ViewData["Loi1"] = "Mật khẩu không chính xác, vui lòng thử lại!";
                    }
                }
                else
                {
                    ViewData["Loi1"] = "Tên đăng nhập hoặc mật khẩu sai, vui lòng thử lại!";
                }
            }

            return View();
        }

    }
}