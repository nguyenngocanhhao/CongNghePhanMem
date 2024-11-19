using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;

namespace NhaSachMetMoi.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        NhaSachEntities db = new NhaSachEntities();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMaSach);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        public ActionResult MuaNgay(int iMaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.iMaSach == iMaSach);

            if (sanpham == null)
            {
                sanpham = new GioHang(iMaSach);
                lstGioHang.Add(sanpham);
            }
            else
            {
                sanpham.iSoLuong++;
            }
            return RedirectToAction("GioHang", "GioHang");
        }
        public ActionResult CapNhatGioHang(int iMaSach, int txtSoLuong)
        {
            List<GioHang> gioHang = Session["GioHang"] as List<GioHang>;
            GioHang item = gioHang.FirstOrDefault(x => x.iMaSach == iMaSach);
            if (item != null)
            {
                if (txtSoLuong < 1)
                {
                    txtSoLuong = 1;
                }
                item.iSoLuong = txtSoLuong;
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGioHang(int iMaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult XacNhanDonHang()
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("IndexTK", "DangNhap");
            }
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Trangchu");
            }
            List<GioHang> lstGioHang = LayGioHang();

            ViewBag.lstGioHang = lstGioHang;

            return View(Session["TaiKhoan"]);
        }
        [HttpPost]
        public ActionResult XacNhanDonHang(FormCollection F)
        {
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("IndexTK", "DangNhap");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Trangchu");
            }

            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.lstGioHang = lstGioHang;

            if (F != null && !string.IsNullOrEmpty(F["maCode"]))
            {
                string maCode = F["maCode"].ToString();

                if (maCode.Length <= 0)
                {
                    ViewBag.ThongBao = "Nhập mã";
                    return View(Session["TaiKhoan"]);
                }

                try
                {
                    MaGiamGia maGiam = db.MaGiamGias.FirstOrDefault(item => item.ma == maCode);
                    if (maGiam != null)
                    {
                        ViewBag.giamGia = maGiam.giam;
                        Session["maGiam"] = maGiam.giam;
                    }
                    else
                    {
                        ViewBag.giamGia = null;
                        ViewBag.ThongBao = "Mã không tồn tại";
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log chi tiết lỗi hoặc hiển thị thông báo lỗi tùy chỉnh
                    ViewBag.ThongBao = "Đã xảy ra lỗi: " + ex.Message;
                    if (ex.InnerException != null)
                    {
                        ViewBag.ThongBao += " - Inner Exception: " + ex.InnerException.Message;
                    }
                    return View(Session["TaiKhoan"]);
                }
            }

            return View(Session["TaiKhoan"]);
        }

        public ActionResult SuaXacNhanTT(int id)
        {
            var khachHang = db.KHs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        public ActionResult SuaXacNhanTT(KH khachHang)
        {

            if (ModelState.IsValid)
            {
                var khachHangInDb = db.KHs.Find(khachHang.MaKH);
                if (khachHangInDb != null)
                {
                    // Chỉ cập nhật các trường cần thiết, không cập nhật TaiKhoan và MatKhau
                    khachHangInDb.HoTen = khachHang.HoTen;
                    khachHangInDb.Email = khachHang.Email;
                    khachHangInDb.DiaChi = khachHang.DiaChi;
                    khachHangInDb.DienThoai = khachHang.DienThoai;


                    db.SaveChanges();
                }
                return RedirectToAction("XacNhanDonHang", new { id = khachHang.MaKH });

            }
            return View(khachHang);
        }

        public ActionResult Dathangthanhcong()
        {
            DonHang ddh = new DonHang();
            KH kh = (KH)Session["taikhoan"];
            List<GioHang> gh = LayGioHang();
            int phanTramGiam = 0;

            if (Session["maGiam"] != null)
            {
                phanTramGiam = Convert.ToInt32(Session["maGiam"]);
                ddh.MaGiamGia = Convert.ToInt32(Session["maGiamId"]); // Lưu Id của mã giảm giá
            }

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            double tongTien = TongTien();
            double tienTru = tongTien * ((double)phanTramGiam / 100);
            double tienGiam = tongTien - tienTru;
            ddh.TongTien = (int)tienGiam;
            db.DonHangs.Add(ddh);
            db.SaveChanges();

            foreach (var item in gh)
            {
                ChiTietDH ctdh = new ChiTietDH();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.Gia = (decimal)item.dGia;
                db.ChiTietDHs.Add(ctdh);
            }
            db.SaveChanges();

            // Clear the cart after placing the order
            Session["giohang"] = null;

            // Send confirmation email
            SendOrderConfirmationEmail(ddh, kh, gh);

            return View("Dathangthanhcong");
        }


        private void SendOrderConfirmationEmail(DonHang order, KH customer, List<GioHang> cartItems)
        {
            try
            {
                var fromAddress = new MailAddress("athenaathenaaaaa@gmail.com", "Athena Library");
                var toAddress = new MailAddress(customer.Email, customer.HoTen);
                const string subject = "Athena Library - Xác nhận đặt hàng thành công";

                // Tính toán giảm giá từ mã (nếu có)

                var discountPercentage = ViewBag.giamGia != null ? $"{ViewBag.giamGia}%" : "0%";

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

    

    }
}
