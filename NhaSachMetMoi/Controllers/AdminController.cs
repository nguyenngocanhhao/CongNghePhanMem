using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Net.Mail;
using System.Data.Entity;
using System.Net;

namespace NhaSachMetMoi.Controllers
{
    public class AdminController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();

        // GET: Admin
        public ActionResult IndexAd(string sortOrder, int? page)
        {
            ViewBag.MaSPSortParm = String.IsNullOrEmpty(sortOrder) ? "MaSP_desc" : "";
            ViewBag.TenSPSortParm = sortOrder == "TenSP" ? "TenSP_desc" : "TenSP";
            ViewBag.GiaSortParm = sortOrder == "Gia" ? "Gia_desc" : "Gia";
            ViewBag.NamXBSortParm = sortOrder == "NamXB" ? "NamXB_desc" : "NamXB";
            ViewBag.SoLuongTonSortParm = sortOrder == "SoLuongTon" ? "SoLuongTon_desc" : "SoLuongTon";
            ViewBag.SaleSortParm = sortOrder == "Sale" ? "Sale_desc" : "Sale";

            var saches = from s in db.Saches
                         select s;

            switch (sortOrder)
            {
                case "MaSP_desc":
                    saches = saches.OrderByDescending(s => s.MaSach);
                    break;
                case "TenSP":
                    saches = saches.OrderBy(s => s.TenSach);
                    break;
                case "TenSP_desc":
                    saches = saches.OrderByDescending(s => s.TenSach);
                    break;
                case "Gia":
                    saches = saches.OrderBy(s => s.Gia);
                    break;
                case "Gia_desc":
                    saches = saches.OrderByDescending(s => s.Gia);
                    break;
                case "NamXB":
                    saches = saches.OrderBy(s => s.NamXB);
                    break;
                case "NamXB_desc":
                    saches = saches.OrderByDescending(s => s.NamXB);
                    break;
                case "SoLuongTon":
                    saches = saches.OrderBy(s => s.SoLuongTon);
                    break;
                case "SoLuongTon_desc":
                    saches = saches.OrderByDescending(s => s.SoLuongTon);
                    break;
                case "Sale":
                    saches = saches.OrderBy(s => s.Sale);
                    break;
                case "Sale_desc":
                    saches = saches.OrderByDescending(s => s.Sale);
                    break;
                default:
                    saches = saches.OrderBy(s => s.MaSach);
                    break;
            }

            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(saches.ToPagedList(pageNum, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL");
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sach sach, HttpPostedFileBase fileupload)
        {
            if (sach.Gia < 0)
            {
                ModelState.AddModelError("Gia", "Giá không thể là số âm.");
            }

            if (sach.Sale < 0)
            {
                ModelState.AddModelError("Sale", "Khuyến mãi không thể là số âm.");
            }

            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/sach"), fileName);
                fileupload.SaveAs(path);
                sach.AnhBia = fileupload.FileName;

                db.Saches.Add(sach);

                try
                {
                    db.SaveChanges();
                    return RedirectToAction("IndexAd");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL");
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG");
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB");

            return View(sach);
        }

        [HttpGet]
        public ActionResult Edit(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL", sach.MaTL);
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG", sach.MaTG);
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }

        [HttpPost]
        public ActionResult Edit(Sach sach, HttpPostedFileBase fileupload)
        {
            if (sach.Gia < 0)
            {
                ModelState.AddModelError("Gia", "Giá không thể là số âm.");
            }

            if (sach.Sale < 0)
            {
                ModelState.AddModelError("Sale", "Khuyến mãi không thể là số âm.");
            }

            if (ModelState.IsValid)
            {
                if (fileupload != null && fileupload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/sach"), fileName);
                    fileupload.SaveAs(path);
                    sach.AnhBia = fileName;
                }
                else
                {
                    // Giữ nguyên ảnh bìa hiện tại nếu không có tệp mới được chọn
                    var currentSach = db.Saches.AsNoTracking().FirstOrDefault(s => s.MaSach == sach.MaSach);
                    if (currentSach != null)
                    {
                        sach.AnhBia = currentSach.AnhBia;
                    }
                }

                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAd");
            }

            // Thiết lập lại ViewBag để giữ giá trị của các dropdown list nếu ModelState không hợp lệ
            ViewBag.MaTL = new SelectList(db.TLs.ToList(), "MaTL", "TenTL", sach.MaTL);
            ViewBag.MaTG = new SelectList(db.TGs.ToList(), "MaTG", "TenTG", sach.MaTG);
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList(), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }



        public ActionResult Details(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        public ActionResult Delete(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhan(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Xóa tất cả các bản ghi liên quan trong bảng ChiTietDH
            var chiTietDonHang = db.ChiTietDHs.Where(c => c.MaSach == MaSach).ToList();
            foreach (var chiTiet in chiTietDonHang)
            {
                db.ChiTietDHs.Remove(chiTiet);
            }

            db.Saches.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("IndexAd");
        }

        public ActionResult IndexDH(string sortOrder, int? page)
        {
            // Xác định các tham số sắp xếp từ ViewBag
            ViewBag.OrderIdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.OrderDateSortParm = sortOrder == "OrderDate" ? "date_desc" : "OrderDate";
            ViewBag.DeliveryDateSortParm = sortOrder == "DeliveryDate" ? "delivery_date_desc" : "DeliveryDate";
            ViewBag.CustomerIdSortParm = sortOrder == "CustomerId" ? "customer_id_desc" : "CustomerId";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";

            // Lấy danh sách đơn hàng từ cơ sở dữ liệu
            var donHangs = from dh in db.DonHangs
                           select dh;

            // Sắp xếp theo tham số được chỉ định
            switch (sortOrder)
            {
                case "id_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.MaDH);
                    break;
                case "OrderDate":
                    donHangs = donHangs.OrderBy(dh => dh.NgayDat);
                    break;
                case "date_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.NgayDat);
                    break;
                case "DeliveryDate":
                    donHangs = donHangs.OrderBy(dh => dh.NgayGiao);
                    break;
                case "delivery_date_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.NgayGiao);
                    break;
                case "CustomerId":
                    donHangs = donHangs.OrderBy(dh => dh.MaKH);
                    break;
                case "customer_id_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.MaKH);
                    break;
                case "Status":
                    donHangs = donHangs.OrderBy(dh => dh.TrangThai);
                    break;
                case "status_desc":
                    donHangs = donHangs.OrderByDescending(dh => dh.TrangThai);
                    break;
                default:
                    donHangs = donHangs.OrderBy(dh => dh.MaDH);
                    break;
            }

            // Thiết lập phân trang
            int pageSize = 8;
            int pageNum = (page ?? 1);

            return View(donHangs.ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult EditDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
            }

            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDH(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexDH");
            }
            return View(donHang);
        }

        public ActionResult DetailDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donHang);
        }

        [HttpPost]
        public ActionResult DetailDH(int MaDH, bool xacnhan, DateTime? ngaygiao)
        {
            var order = db.DonHangs.Find(MaDH);

            if (order != null)
            {
                if (xacnhan)
                {
                    // Update the order status and delivery date
                    order.NgayGiao = ngaygiao;
                    db.SaveChanges();

                    // Lấy thông tin khách hàng và chi tiết đơn hàng
                    var customer = order.KH;
                    var orderDetails = db.ChiTietDHs.Where(ct => ct.MaDH == MaDH).ToList();

                    // Send confirmation email
                    SendOrderConfirmationEmail(order, customer, orderDetails);
                }
            }

            return RedirectToAction("IndexDH");
        }


        public ActionResult DeleteDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);
            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donHang);
        }
        [HttpPost, ActionName("DeleteDH")]
        public ActionResult XacNhanDH(int MaDH)
        {
            DonHang donHang = db.DonHangs.SingleOrDefault(n => n.MaDH == MaDH);

            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var chiTietDHs = db.ChiTietDHs.Where(ct => ct.MaDH == MaDH);
            foreach (var chiTietDH in chiTietDHs)
            {
                db.ChiTietDHs.Remove(chiTietDH);
            }
            db.DonHangs.Remove(donHang);
            db.SaveChanges();

            return RedirectToAction("IndexDH");
        }
        [HttpPost]
        public JsonResult UpdateOrderStatus(int MaDH, string TrangThai)
        {
            try
            {
                var order = db.DonHangs.Find(MaDH);
                if (order != null)
                {
                    order.TrangThai = TrangThai;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(new { success = false });
        }

        public ActionResult IndexKH(int? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(db.KHs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNum, pageSize));
        }

        public ActionResult EditKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);

            if (kh == null)
            {
                Response.StatusCode = 404;
            }

            return View(kh);
        }

        [HttpPost]
        public ActionResult EditKH(KH kh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexKH");
            }
            return View(kh);
        }

        public ActionResult DetailKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);

            if (kh == null)
            {
                Response.StatusCode = 404;
            }
            return View(kh);
        }

        public ActionResult DeleteKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("DeleteKH")]
        public ActionResult XacNhanKH(int MaKH)
        {
            KH kh = db.KHs.SingleOrDefault(n => n.MaKH == MaKH);

            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var donHangs = db.DonHangs.Where(dh => dh.MaKH == MaKH);
            foreach (var donHang in donHangs)
            {
                var chiTietDHs = db.ChiTietDHs.Where(ct => ct.MaDH == donHang.MaDH);
                foreach (var chiTietDH in chiTietDHs)
                {
                    db.ChiTietDHs.Remove(chiTietDH);
                }
                db.DonHangs.Remove(donHang);
            }
            db.KHs.Remove(kh);
            db.SaveChanges();
            return RedirectToAction("IndexKH");
        }

        [HttpPost]
        public ActionResult AddMaTL(string TenTL)
        {
            var loaiSach = new TL { TenTL = TenTL };
            db.TLs.Add(loaiSach);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult AddMaTG(string TenTG)
        {
            var tacGia = new TG { TenTG = TenTG };
            db.TGs.Add(tacGia);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult AddMaNXB(string TenNXB)
        {
            var nhaXuatBan = new NXB { TenNXB = TenNXB };
            db.NXBs.Add(nhaXuatBan);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
        private void SendOrderConfirmationEmail(DonHang order, KH customer, List<ChiTietDH> orderDetails)
        {
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
        [HttpPost]
        public ActionResult DisableDH(int MaDH, bool isDisabled)
        {
            var donHang = db.DonHangs.FirstOrDefault(d => d.MaDH == MaDH);
            if (donHang == null)
            {
                return Json(new { success = false, message = "Đơn hàng không tồn tại." });
            }

            // Kiểm tra trạng thái hiện tại của đơn hàng trước khi cập nhật
            if (donHang.TrangThai == "Đã giao")
            {
                return Json(new { success = false, message = "Không thể hủy đơn hàng đã giao." });
            }

            // Cập nhật trạng thái của đơn hàng
            donHang.TrangThai = isDisabled ? "Bị hủy" : "Chưa giao";
            db.SaveChanges();

            return Json(new { success = true });
        }


       public ActionResult IndexB(string sortOrder, int? page)
{
    ViewBag.BookingIdSortParm = String.IsNullOrEmpty(sortOrder) ? "bookingId_desc" : "";
    ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
    ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
    ViewBag.RoomPackageSortParm = sortOrder == "RoomPackage" ? "roomPackage_desc" : "RoomPackage";
    ViewBag.DurationSortParm = sortOrder == "Duration" ? "duration_desc" : "Duration";
    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
    ViewBag.TotalPriceSortParm = sortOrder == "TotalPrice" ? "totalPrice_desc" : "TotalPrice";
    ViewBag.StatusSortParm = sortOrder == "Status" ? "status_desc" : "Status";

    var bookings = from b in db.BookingInfoes
                   select b;

    switch (sortOrder)
    {
        case "bookingId_desc":
            bookings = bookings.OrderByDescending(b => b.BookingID);
            break;
        case "Name":
            bookings = bookings.OrderBy(b => b.FullName);
            break;
        case "name_desc":
            bookings = bookings.OrderByDescending(b => b.FullName);
            break;
        case "Email":
            bookings = bookings.OrderBy(b => b.Email);
            break;
        case "email_desc":
            bookings = bookings.OrderByDescending(b => b.Email);
            break;
        case "RoomPackage":
            bookings = bookings.OrderBy(b => b.RoomPackage);
            break;
        case "roomPackage_desc":
            bookings = bookings.OrderByDescending(b => b.RoomPackage);
            break;
        case "Duration":
            bookings = bookings.OrderBy(b => b.Duration);
            break;
        case "duration_desc":
            bookings = bookings.OrderByDescending(b => b.Duration);
            break;
        case "Date":
            bookings = bookings.OrderBy(b => b.BookingDate);
            break;
        case "date_desc":
            bookings = bookings.OrderByDescending(b => b.BookingDate);
            break;
        case "TotalPrice":
            bookings = bookings.OrderBy(b => b.TotalPrice);
            break;
        case "totalPrice_desc":
            bookings = bookings.OrderByDescending(b => b.TotalPrice);
            break;
        case "Status":
            bookings = bookings.OrderBy(b => b.ConfirmationStatus);
            break;
        case "status_desc":
            bookings = bookings.OrderByDescending(b => b.ConfirmationStatus);
            break;
        default:
            bookings = bookings.OrderBy(b => b.BookingID);
            break;
    }

    int pageSize = 10;
    int pageNumber = (page ?? 1);
    return View(bookings.ToPagedList(pageNumber, pageSize));
}



        // GET: BookingAdmin/Details/5
        public ActionResult DetailsB(int id)
        {
            // Lấy chi tiết booking theo ID
            var booking = db.BookingInfoes.FirstOrDefault(b => b.BookingID == id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: BookingAdmin/Edit/5
        public ActionResult EditB(int id)
        {
            var booking = db.BookingInfoes.FirstOrDefault(b => b.BookingID == id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            // Tạo danh sách gói phòng cho dropdown list
            ViewBag.RoomPackages = new List<SelectListItem>
    {
        new SelectListItem { Value = "Meeting Room", Text = "Meeting Room" },
        new SelectListItem { Value = "Private Zone", Text = "Private Zone" },
        new SelectListItem { Value = "Couple Room", Text = "Couple Room" }
    };

            return View(booking);
        }

        // POST: BookingAdmin/Edit/5
        [HttpPost]
        public ActionResult EditB(BookingInfo model)
        {
            if (ModelState.IsValid)
            {
                var booking = db.BookingInfoes.FirstOrDefault(b => b.BookingID == model.BookingID);
                if (booking != null)
                {
                    // Cập nhật các thuộc tính từ model
                    booking.FullName = model.FullName;
                    booking.Email = model.Email;
                    booking.PhoneNumber = model.PhoneNumber;
                    booking.RoomPackage = model.RoomPackage;
                    booking.Duration = model.Duration;
                    booking.BookingDate = model.BookingDate;
                    booking.BookingTime = model.BookingTime;
                    booking.TotalPrice = model.TotalPrice;
                    booking.Note = model.Note;

                    db.SaveChanges();

                    return RedirectToAction("IndexB");
                }
            }

            // Nếu không hợp lệ, trả lại View với model hiện tại
            return View(model);
        }

        public ActionResult DeleteB(int id)
        {
            // Lấy booking cần xóa
            BookingInfo booking = db.BookingInfoes.SingleOrDefault(b => b.BookingID == id);
            if (booking == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(booking);
        }

        [HttpPost, ActionName("DeleteB")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int BookingID)
        {
            // Xóa booking khỏi cơ sở dữ liệu
            BookingInfo booking = db.BookingInfoes.SingleOrDefault(b => b.BookingID == BookingID);
            if (booking != null)
            {
                db.BookingInfoes.Remove(booking);
                db.SaveChanges();
            }
            return RedirectToAction("IndexB");
        }



        // GET: BookingAdmin/Confirm/5
        public ActionResult ConfirmB(int id)
        {
            // Xác nhận booking
            var booking = db.BookingInfoes.FirstOrDefault(b => b.BookingID == id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            booking.ConfirmationStatus = "Đã xác nhận";
            db.SaveChanges();

            // Gửi email xác nhận
            SendConfirmationEmail(booking);

            return RedirectToAction("IndexB");
        }


        private void SendConfirmationEmail(BookingInfo booking)
        {
            try
            {
                var fromAddress = new MailAddress("athenaathenaaaaa@gmail.com", "Athena Library");
                var toAddress = new MailAddress(booking.Email, booking.FullName);
                const string subject = "ATHENA - Lịch booking của bạn đã được xác nhận";

                string body = $@"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <h2 style='color: #4CAF50;'>Xin chào {booking.FullName},</h2>
        <p>Lịch booking của bạn tại <strong>Athena Library</strong> đã được xác nhận thành công. Dưới đây là thông tin chi tiết của booking:</p>
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
                <td style='padding: 8px;'>{booking.TotalPrice} VNĐ</td>
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
        public ActionResult ThongKeDoanhThu(string period = "day")
        {
            DateTime startDate = DateTime.Now;

            switch (period.ToLower())
            {
                case "day":
                    startDate = DateTime.Today;
                    break;
                case "week":
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    break;
                case "month":
                    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    break;
                case "year":
                    startDate = new DateTime(DateTime.Now.Year, 1, 1);
                    break;
                default:
                    startDate = DateTime.Today;
                    break;
            }

            // Lấy tổng doanh thu từ các đơn hàng đã xác nhận
            var totalRevenue = db.DonHangs
                      .Where(dh => dh.NgayDat >= startDate && dh.TrangThai == "đã giao")
                      .Select(dh => dh.TongTien)
                      .DefaultIfEmpty(0)
                      .Sum();


            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.Period = period;

            return View();
        }

        public JsonResult GetRevenueData(string period)
        {
            var data = new List<decimal>();
            var labels = new List<string>();
            var orderCounts = new List<int>(); // Số lượng đơn hàng
            var confirmedBookingCounts = new List<int>(); // Số lượng booking đã xác nhận
            decimal totalRevenue = 0; // Tổng doanh thu

            switch (period.ToLower())
            {
                case "day":
                    for (int i = 0; i < 24; i++) // 24 giờ trong ngày
                    {
                        var startHour = DateTime.Today.AddHours(i);
                        var endHour = startHour.AddHours(1);

                        var revenue = db.DonHangs
                                        .Where(dh => dh.NgayDat >= startHour && dh.NgayDat < endHour && dh.TrangThai == "đã giao")
                                        .Sum(dh => (decimal?)dh.TongTien) ?? 0;

                        totalRevenue += revenue;

                        var orderCount = db.DonHangs
                                        .Count(dh => dh.NgayDat >= startHour && dh.NgayDat < endHour && dh.TrangThai == "đã giao");

                        var confirmedBookingCount = db.BookingInfoes
                                        .Count(b => b.BookingDate >= startHour && b.BookingDate < endHour && b.ConfirmationStatus == "Đã xác nhận");

                        data.Add(revenue);
                        orderCounts.Add(orderCount);
                        confirmedBookingCounts.Add(confirmedBookingCount);
                        labels.Add(startHour.ToString("HH:mm"));
                    }
                    break;

                case "week":
                    DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1); // Thứ 2 của tuần hiện tại
                    string[] weekDays = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
                    for (int i = 0; i < 7; i++)
                    {
                        var startDay = startOfWeek.AddDays(i);

                        var revenue = db.DonHangs
                                        .Where(dh => DbFunctions.TruncateTime(dh.NgayDat) == DbFunctions.TruncateTime(startDay) && dh.TrangThai == "đã giao")
                                        .Sum(dh => (decimal?)dh.TongTien) ?? 0;

                        totalRevenue += revenue;

                        var orderCount = db.DonHangs
                                        .Count(dh => DbFunctions.TruncateTime(dh.NgayDat) == DbFunctions.TruncateTime(startDay) && dh.TrangThai == "đã giao");

                        var confirmedBookingCount = db.BookingInfoes
                                        .Count(b => DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(startDay) && b.ConfirmationStatus == "Đã xác nhận");

                        data.Add(revenue);
                        orderCounts.Add(orderCount);
                        confirmedBookingCounts.Add(confirmedBookingCount);
                        labels.Add(weekDays[i]);
                    }
                    break;

                case "month":
                    for (int i = 1; i <= 12; i++) // Các tháng từ 1 đến 12
                    {
                        DateTime startDate = new DateTime(DateTime.Now.Year, i, 1);
                        DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                        var revenue = db.DonHangs
                                        .Where(dh => dh.NgayDat >= startDate && dh.NgayDat <= endDate && dh.TrangThai == "đã giao")
                                        .Sum(dh => (decimal?)dh.TongTien) ?? 0;

                        totalRevenue += revenue;

                        var orderCount = db.DonHangs
                                        .Count(dh => dh.NgayDat >= startDate && dh.NgayDat <= endDate && dh.TrangThai == "đã giao");

                        var confirmedBookingCount = db.BookingInfoes
                                        .Count(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.ConfirmationStatus == "Đã xác nhận");

                        data.Add(revenue);
                        orderCounts.Add(orderCount);
                        confirmedBookingCounts.Add(confirmedBookingCount);
                        labels.Add($"Tháng {i}");
                    }
                    break;

                case "year":
                    int currentYear = DateTime.Now.Year;
                    for (int i = currentYear - 2; i <= currentYear + 2; i++) // Từ năm hiện tại - 2 đến năm hiện tại + 2
                    {
                        DateTime startDate = new DateTime(i, 1, 1);
                        DateTime endDate = new DateTime(i, 12, 31);

                        var revenue = db.DonHangs
                                        .Where(dh => dh.NgayDat >= startDate && dh.NgayDat <= endDate && dh.TrangThai == "đã giao")
                                        .Sum(dh => (decimal?)dh.TongTien) ?? 0;

                        totalRevenue += revenue;

                        var orderCount = db.DonHangs
                                        .Count(dh => dh.NgayDat >= startDate && dh.NgayDat <= endDate && dh.TrangThai == "đã giao");

                        var confirmedBookingCount = db.BookingInfoes
                                        .Count(b => b.BookingDate >= startDate && b.BookingDate <= endDate && b.ConfirmationStatus == "Đã xác nhận");

                        data.Add(revenue);
                        orderCounts.Add(orderCount);
                        confirmedBookingCounts.Add(confirmedBookingCount);
                        labels.Add($"Năm {i}");
                    }
                    break;
            }

            // Gán tổng doanh thu vào ViewBag để sử dụng trên View
            ViewBag.TotalRevenue = totalRevenue;

            return Json(new { data, labels, orderCounts, confirmedBookingCounts }, JsonRequestBehavior.AllowGet);
        }






    }
}
