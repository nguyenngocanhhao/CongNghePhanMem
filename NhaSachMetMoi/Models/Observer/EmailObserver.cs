using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;
using System.Net;
using System;

public class EmailObserver : IOrderObserver
{
    private readonly NhaSachEntities db = new NhaSachEntities();

    public void Update(DonHang order)
    {
        var customer = db.KHs.FirstOrDefault(k => k.MaKH == order.MaKH);
        if (customer == null) return;

        var orderDetails = db.ChiTietDHs.Where(c => c.MaDH == order.MaDH).ToList();

        try
            {
                var fromAddress = new MailAddress("athenaathenaaaaa@gmail.com", "Athena Library");
                var toAddress = new MailAddress(customer.Email, customer.HoTen);
                const string subject = "ATHENA - Đơn hàng được xác nhận thành công";

                string body = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333;'>
    <h2 style='color: #2E8B57;'>Xin chào {customer.HoTen}, đơn hàng của bạn đã được xác nhận</h2>
    <p>Cảm ơn bạn đã đặt hàng tại <strong>Athena Library</strong>. Thông tin đơn hàng của bạn:</p>
    <table style='width: 100%; max-width: 600px; margin: auto; border-collapse: collapse;'>
        <tr>
            <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Mã đơn hàng</td>
            <td style='padding: 8px;'>{order.MaDH}</td>
        </tr>
        <tr>
            <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Ngày đặt hàng</td>
            <td style='padding: 8px;'>{order.NgayDat:dd/MM/yyyy HH:mm:ss}</td>
        </tr>
        <tr>
            <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Ngày giao dự kiến</td>
            <td style='padding: 8px;'>{order.NgayGiao:dd/MM/yyyy HH:mm:ss}</td>
        </tr>
        <tr>
            <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Tổng tiền phải trả</td>
            <td style='padding: 8px;'>{order.TongTien:N0} VND</td>
        </tr>
    </table>
    <h3 style='margin-top: 20px;'>Chi tiết đơn hàng:</h3>
    <table style='width: 100%; max-width: 600px; margin: auto; border-collapse: collapse;'>
        <thead>
            <tr>
                <th style='padding: 8px; background-color: #2E8B57; color: #fffacd; text-align: left;'>Tên sách</th>
                <th style='padding: 8px; background-color: #2E8B57; color: #fffacd; text-align: left;'>Số lượng</th>
                <th style='padding: 8px; background-color: #2E8B57; color: #fffacd; text-align: left;'>Đơn giá</th>
                <th style='padding: 8px; background-color: #2E8B57; color: #fffacd; text-align: left;'>Khuyến mãi</th>
                <th style='padding: 8px; background-color: #2E8B57; color: #fffacd; text-align: left;'>Thành tiền</th>
            </tr>
        </thead>
        <tbody>";

                // Thêm thông tin từng sản phẩm trong đơn hàng
                foreach (var item in orderDetails)
                {
                    var itemPrice = item.Gia * (1 - item.Sach.Sale / 100m);
                    var totalPrice = itemPrice * item.SoLuong;

                    body += $@"
        <tr>
            <td style='padding: 8px; border: 1px solid #ddd;'>{item.Sach.TenSach}</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{item.SoLuong}</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{item.Gia:N0} VND</td>
            <td style='padding: 8px; border: 1px solid #ddd;'>{item.Sach.Sale}%</td>
            <td style='padding: 8px; border: 1px solid #ddd; color: red;'>{totalPrice:N0} VND</td>
        </tr>";
                }

                body += @"
        </tbody>
    </table>
    <p style='margin-top: 20px;'>Xin cảm ơn quý khách hàng, vui lòng chờ theo dõi ngày dự kiến để nhận hàng.<br><strong>Athena Library</strong></p>
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
                        IsBodyHtml = true
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
