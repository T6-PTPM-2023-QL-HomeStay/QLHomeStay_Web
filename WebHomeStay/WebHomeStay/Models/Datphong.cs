using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebHomeStay.Models
{
    public class Datphong
    {
        DatabaseDataContext db = new DatabaseDataContext();
        public class ThongTinDatPhongViewModel
        {
            
            public PHONG Phong { get; set; } // Lớp chứa thông tin phòng
            public List<LOAIPHONG> LoaiPhong { get; set; } // Danh sách loại phòng
            public ThongTinDatPhong ThongTinDatPhong { get; set; } 
        }

        public class ThongTinDatPhong
        {
            [Required(ErrorMessage = "Vui lòng nhập ngày check-in")]
            public DateTime NgayCheckIn { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập ngày check-out")]
            public DateTime NgayCheckOut { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập số người")]
            public int SoNguoi { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập số trẻ em")]
            public int SoTreEm { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập họ tên")]
            public string HoTen { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
            [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
            public string SoDienThoai { get; set; }

        }
    }
}