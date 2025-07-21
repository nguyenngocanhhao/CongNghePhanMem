using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;

public class OrderFacade
{
    private readonly NhaSachEntities db;

    public OrderFacade()
    {
        db = DatabaseSingleton.Instance.DbContext;
    }

    public void PlaceOrder(KH customer, List<GioHang> cartItems, string discountCode)
    {
        ValidateCart(cartItems);
        int discount = ApplyDiscount(discountCode);
        var order = CreateOrder(customer, cartItems, discount);
        SaveOrder(order, cartItems);
        SendConfirmationEmail(order, customer, cartItems, discount);
    }

    private DonHang CreateOrder(KH customer, List<GioHang> cartItems, int discount)
    {
        var order = new DonHang
        {
            MaKH = customer.MaKH,
            NgayDat = DateTime.Now,
            TongTien = CalculateTotalPrice(cartItems, discount),
            MaGiamGia = discount > 0 ? (int?)discount : null
        };
        db.DonHangs.Add(order);
        db.SaveChanges();
        return order;
    }

    private int CalculateTotalPrice(List<GioHang> cartItems, int discount)
    {
        double total = cartItems.Sum(item => item.dGia * item.iSoLuong * (1 - (item.iSale / 100.0)));
        total -= (total * discount / 100.0);
        return (int)total;
    }

    private void ValidateCart(List<GioHang> cartItems)
    {
        if (cartItems == null || !cartItems.Any())
            throw new InvalidOperationException("Giỏ hàng rỗng!");
    }

    private void SaveOrder(DonHang order, List<GioHang> cartItems)
    {
        foreach (var item in cartItems)
        {
            var chiTiet = new ChiTietDH
            {
                MaDH = order.MaDH,
                MaSach = item.iMaSach,
                SoLuong = item.iSoLuong,
                Gia = (decimal)item.dGia
            };
            db.ChiTietDHs.Add(chiTiet);
        }
        db.SaveChanges();
    }

    private void SendConfirmationEmail(DonHang order, KH customer, List<GioHang> cartItems, int discountPercentage)
    {
        try
        {
            var fromAddress = new MailAddress("athenaathenaaaaa@gmail.com", "Athena Library");
            var toAddress = new MailAddress(customer.Email, customer.HoTen);
            const string subject = "Athena Library - Xác nhận đặt hàng thành công";

            // Tính toán giảm giá từ mã (nếu có)

         

            string body = $@"
        <html>
        <body style='font-family: Arial, sans-serif; color: #333;'>
            <h2 style='color: #2E8B57;'>Xin chào {customer.HoTen},</h2>
            <p>Cảm ơn bạn đã đặt hàng tại <strong>Athena Library</strong>. Thông tin đơn hàng của bạn:</p>
            <table style='width: 100%; max-width: 600px; margin: auto; border-collapse: collapse;'>
                <tr>
                    <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Mã đơn hàng</td>
                    <td style='padding: 8px;'>{order.MaDH}</td>
                </tr>
                <tr>
                    <td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>Ngày đặt</td>
                    <td style='padding: 8px;'>{order.NgayDat:dd/MM/yyyy}</td>
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

            // Thêm thông tin từng sản phẩm trong giỏ hàng
            foreach (var item in cartItems)
            {
                var itemPrice = (decimal)item.dGia * (1 - (decimal)item.iSale / 100);
                var totalPrice = itemPrice * item.iSoLuong;

                body += $@"
            <tr>
                
                <td style='padding: 8px; border: 1px solid #ddd;'>{item.sTenSach}</td>
                <td style='padding: 8px; border: 1px solid #ddd;'>{item.iSoLuong}</td>
                <td style='padding: 8px; border: 1px solid #ddd;'>{itemPrice:N0} VND</td>
                <td style='padding: 8px; border: 1px solid #ddd;'>{item.iSale}%</td>
                <td style='padding: 8px; border: 1px solid #ddd; color: red;'>{totalPrice:N0} VND</td>
            </tr>";
            }

            body += @"
                </tbody>
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

    private int ApplyDiscount(string code)
    {
        var discount = db.MaGiamGias.FirstOrDefault(mg => mg.ma == code);
        return discount != null ? (int)discount.giam.GetValueOrDefault(0) : 0;
    }

}
