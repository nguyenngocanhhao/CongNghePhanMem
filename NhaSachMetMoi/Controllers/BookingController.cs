using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Controllers
{
    public class BookingController : Controller
    {
        private NhaSachEntities db = new NhaSachEntities();

        // GET: Booking
        public ActionResult Index()
        {
            var model = new BookingInfo();

            // Kiểm tra nếu người dùng đã đăng nhập
            if (Session["TaiKhoan"] != null && Session["TaiKhoan"] is KH khachHang)
            {
                // Sử dụng trực tiếp thông tin từ Session mà không truy vấn lại
                model.MaKH = khachHang.MaKH;
                model.PhoneNumber = khachHang.DienThoai;
                model.FullName = khachHang.HoTen;
                model.Email = khachHang.Email;
            }

            return View(model);
        }

        // POST: Booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BookingInfo booking)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu người dùng đã đăng nhập
                if (Session["TaiKhoan"] != null && Session["TaiKhoan"] is KH khachHang)
                {
                    // Lấy MaKH từ đối tượng KH trong Session
                    booking.MaKH = khachHang.MaKH;
                    booking.PhoneNumber = khachHang.DienThoai;
                    booking.FullName = khachHang.HoTen;
                    booking.Email = khachHang.Email;
                }

                // Tính toán TotalPrice dựa trên RoomPackage và Duration
                decimal totalPrice = 0;
                if (booking.RoomPackage == "Meeting Room")
                {
                    if (booking.Duration == 2) totalPrice = 170000;
                    else if (booking.Duration == 3) totalPrice = 220000;
                    else if (booking.Duration == 4) totalPrice = 300000;
                }
                else if (booking.RoomPackage == "Private Zone")
                {
                    if (booking.Duration == 2) totalPrice = 35000;
                    else if (booking.Duration == 3) totalPrice = 50000;
                    else if (booking.Duration == 4) totalPrice = 60000;
                }
                else if (booking.RoomPackage == "Couple Room")
                {
                    if (booking.Duration == 2) totalPrice = 45000;
                    else if (booking.Duration == 3) totalPrice = 65000;
                    else if (booking.Duration == 4) totalPrice = 80000;
                }

                // Gán TotalPrice vào đối tượng booking
                booking.TotalPrice = totalPrice;

                // Lưu thông tin booking vào database
                db.BookingInfoes.Add(booking);
                db.SaveChanges();

                // Gửi email xác nhận
                SendConfirmationEmail(booking);

                return RedirectToAction("Confirmation");
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form với thông báo lỗi
            return View(booking);
        }

        // GET: Booking/Confirmation
        public ActionResult Confirmation()
        {
            // Trang xác nhận sau khi đặt chỗ thành công
            return View();
        }
        private void SendConfirmationEmail(BookingInfo booking)
        {
            try
            {
                var fromAddress = new MailAddress("athenaathenaaaaa@gmail.com", "Athena Library");
                var toAddress = new MailAddress(booking.Email, booking.FullName);
                const string subject = "ATHENA - Xác nhận đặt phòng thành công";

                string body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2 style='color: #4CAF50;'>Xin chào {booking.FullName},</h2>
                <p>Cảm ơn bạn đã đặt phòng tại <strong>Athena Library</strong>. Thông tin đặt phòng của bạn:</p>
                <table style='width: 100%; max-width: 600px; margin: auto; border-collapse: collapse;'>
                    <tr>
                        <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Gói phòng</td>
                        <td style='padding: 8px;'>{booking.RoomPackage}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Thời lượng</td>
                        <td style='padding: 8px;'>{booking.Duration} giờ</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Ngày</td>
                        <td style='padding: 8px;'>{booking.BookingDate:dd/MM/yyyy}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Giờ đến</td>
                        <td style='padding: 8px;'>{booking.BookingTime}</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Tổng tiền</td>
                        <td style='padding: 8px;'>{booking.TotalPrice}</td>
                    </tr>
                </table>
                <p style='margin-top: 20px;'>Xin cảm ơn,<br><strong>Athena Library</strong></p>
            </body>
            </html>";

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("athenaathenaaaaa@gmail.com", "abxbhuqlszibmnhd"); // Mật khẩu ứng dụng

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true // Cho phép nội dung HTML
                    })
                    {
                        smtp.Send(message);
                    }
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Gửi email thất bại: " + ex.Message);
            }
        }




    }
}
