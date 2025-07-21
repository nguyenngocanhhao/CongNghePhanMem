using System;
using System.ComponentModel.DataAnnotations;

namespace NhaSachMetMoi.Models
{
    public class KHMetadata
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(50, ErrorMessage = "Họ tên tối đa 50 ký tự")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(50, ErrorMessage = "Tên tài khoản tối đa 50 ký tự")]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 20 ký tự")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,20}$",
        ErrorMessage = "Mật khẩu phải chứa ít nhất 1 chữ in hoa, 1 số và 1 ký tự đặc biệt (@$!%*?&)")]

        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$",
      ErrorMessage = "Email không hợp lệ, vui lòng nhập đúng định dạng (example@domain.com)")]
        [StringLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"(0[3|5|7|8|9])+([0-9]{8})", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string DienThoai { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime? NgaySinh { get; set; }
    }
}
