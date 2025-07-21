using iTextSharp.text;
using iTextSharp.text.pdf;
using NhaSachMetMoi.Models;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

public class ExportPdfVisitor : IOrderVisitor
{
    public void Visit(DonHang donHang)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"HoaDon_{donHang.MaDH}.pdf");

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, fs);
            doc.Open();

            // Font tiếng Việt
            BaseFont baseFont = BaseFont.CreateFont(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf"),
                BaseFont.IDENTITY_H,
                BaseFont.EMBEDDED
            );
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(baseFont, 12);
            // Logo
            string logoPath = HttpContext.Current.Server.MapPath("~/Content/images/logo.png");
            if (File.Exists(logoPath))
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleAbsolute(120f, 60f);
                logo.Alignment = Element.ALIGN_CENTER;
                doc.Add(logo);
                doc.Add(new Paragraph(" "));
            }

            // Tiêu đề
            Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG", fontTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 15
            };
            doc.Add(title);

            // Thông tin khách hàng
            doc.Add(new Paragraph($"Mã đơn hàng: {donHang.MaDH}", fontNormal));
            doc.Add(new Paragraph($"Ngày đặt: {donHang.NgayDat?.ToString("dd/MM/yyyy HH:mm:ss")}", fontNormal));
            doc.Add(new Paragraph($"Trạng thái: {donHang.TrangThai}", fontNormal));
            doc.Add(new Paragraph($"Khách hàng: {donHang.KH?.HoTen}", fontNormal));
            doc.Add(new Paragraph($"Email: {donHang.KH?.Email}", fontNormal));
            doc.Add(new Paragraph($"SĐT: {donHang.KH?.DienThoai}", fontNormal));
            doc.Add(new Paragraph($"Địa chỉ: {donHang.KH?.DiaChi}", fontNormal));
            doc.Add(new Paragraph(" "));

            // Bảng chi tiết
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 25f, 10f, 15f, 20f, 10f, 20f });

            AddHeaderCell(table, "Tên sách", fontNormal);
            AddHeaderCell(table, "SL", fontNormal);
            AddHeaderCell(table, "Ảnh bìa", fontNormal);
            AddHeaderCell(table, "Giá bán", fontNormal);
            AddHeaderCell(table, "KM", fontNormal);
            AddHeaderCell(table, "Thành tiền", fontNormal);

            foreach (var item in donHang.ChiTietDHs)
            {
                var sach = item.Sach;

                string tenSach = sach?.TenSach ?? "Không rõ";
                int soLuong = item.SoLuong ?? 0;
                decimal giaBan = sach?.Gia ?? 0;
                int khuyenMai = sach?.Sale ?? 0;
                string anhBia = sach?.AnhBia ?? "";
                decimal giaSauKM = giaBan * (100 - khuyenMai) / 100;
                decimal thanhTien = giaSauKM * soLuong;

                AddCell(table, tenSach, fontNormal);
                AddCell(table, soLuong.ToString(), fontNormal);

                // Ảnh bìa
                string imagePath = HttpContext.Current.Server.MapPath($"~/Content/images/sach/{anhBia}");
                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleAbsolute(40f, 50f);
                    PdfPCell imgCell = new PdfPCell(img, true);
                    imgCell.Padding = 4;
                    table.AddCell(imgCell);
                }
                else
                {
                    AddCell(table, "Không có", fontNormal);
                }

                AddCell(table, $"{giaBan:N0} VNĐ", fontNormal);
                AddCell(table, $"{khuyenMai}%", fontNormal);
                AddCell(table, $"{thanhTien:N0} VNĐ", fontNormal);
            }

            doc.Add(table);

            // Tổng và giảm giá
            doc.Add(new Paragraph(" ", fontNormal));
            int giamGiaPhanTram = (int)(donHang.MaGiamGia1?.giam ?? 0);
            decimal tienTru = donHang.TongTien * giamGiaPhanTram / 100;
            decimal tongSauGiam = donHang.TongTien - tienTru;

            doc.Add(new Paragraph($"Tổng tiền: {donHang.TongTien:N0} VNĐ", fontNormal));
            doc.Add(new Paragraph($"Thuế: {tienTru:N0} VNĐ", fontNormal));
            doc.Add(new Paragraph($"Tổng tiền phải trả: {tongSauGiam:N0} VNĐ", fontTitle)
            {   
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 10
            });

            // QR code
            string url = "http://lavierose.io.vn/";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            Bitmap qrBitmap = qrCode.GetGraphic(20);

            using (MemoryStream ms = new MemoryStream())
            {
                qrBitmap.Save(ms, ImageFormat.Png);
                iTextSharp.text.Image qrImage = iTextSharp.text.Image.GetInstance(ms.ToArray());
                qrImage.ScaleAbsolute(100f, 100f);
                qrImage.Alignment = Element.ALIGN_CENTER;

                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph("📱 Quét mã QR để truy cập trang web ATHENA", fontNormal)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 10,
                    SpacingAfter = 5
                });
                doc.Add(qrImage);
            }

            doc.Close();
        }
    }

    private void AddCell(PdfPTable table, string text, iTextSharp.text.Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font))
        {
            Padding = 5,
            HorizontalAlignment = Element.ALIGN_CENTER,
            VerticalAlignment = Element.ALIGN_MIDDLE
        };
        table.AddCell(cell);
    }

    private void AddHeaderCell(PdfPTable table, string text, iTextSharp.text.Font font)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font))
        {
            Padding = 6,
            HorizontalAlignment = Element.ALIGN_CENTER,
            BackgroundColor = new BaseColor(230, 230, 230)
        };
        table.AddCell(cell);
    }
}
