using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;


using NhaSachMetMoi.Models;
// Main
namespace NhaSachMetMoi.Controllers
{
    public class TrangchuController : Controller
    {
        // GET: Trangchu
        NhaSachEntities db = new NhaSachEntities();
        //public ActionResult Index()
        //{
        //    var lstSach = db.Saches.ToList();
        //    ViewBag.DanhMucs = db.TLs.ToList();
        //    return View(lstSach);


        //}
        public ActionResult Index()
        {
            var lstSach = db.Saches.ToList(); // Lấy danh sách sách từ cơ sở dữ liệu

            ViewData["IsHomePage"] = true;
            return View(lstSach); // Truyền lstSach vào view
        }


        public ActionResult TrangChu()
        {
            var lstSach = db.Saches.ToList();
            ViewData["IsHomePage"] = true;
            return View();
        }


        public ActionResult SachTL(int MaTL, int? page, string sortOrder)
        {
            ViewBag.DanhMucs = db.TLs.ToList();
            int pageSize = 12;
            int pageNum = (page ?? 1);
            TL tl = db.TLs.SingleOrDefault(n => n.MaTL == MaTL);
            List<Sach> lstSach = db.Saches.Where(n => n.MaTL == MaTL).ToList();
            switch (sortOrder)
            {
                case "PriceAsc":
                    lstSach = lstSach.OrderBy(s => s.Gia).ToList();
                    break;
                case "PriceDesc":
                    lstSach = lstSach.OrderByDescending(s => s.Gia).ToList();
                    break;
                case "NameAsc":
                    lstSach = lstSach.OrderBy(s => s.TenSach).ToList();
                    break;
                case "NameDesc":
                    lstSach = lstSach.OrderByDescending(s => s.TenSach).ToList();
                    break;
                default:
                    break;
            }
            ViewBag.Page = pageNum;
            ViewBag.CurrentSort = sortOrder;

            return View(lstSach.ToPagedList(pageNum, pageSize));

        }

        public ActionResult XemCT(int MaSach)
        {
            var sach = db.Saches.Include(s => s.TL).Include(s => s.TG).Include(s => s.NXB)
                              .FirstOrDefault(s => s.MaSach == MaSach);
            if (sach == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách các sách cùng thể loại
            sach.CungTheLoai = db.Saches.Where(s => s.TL.MaTL == sach.TL.MaTL && s.MaSach != MaSach).ToList();

            return View(sach);
        }
        public ActionResult TatCaSanPham(string sortOrder, int? page, int? category)
        {
            // Lấy danh sách tất cả sách từ cơ sở dữ liệu
            var lstSach = db.Saches.AsQueryable();

            // Lọc theo thể loại nếu được chọn
            if (category.HasValue)
            {
                lstSach = lstSach.Where(s => s.MaTL == category.Value);
            }

            // Sắp xếp dựa trên tham số sortOrder
            switch (sortOrder)
            {
                case "PriceAsc":
                    lstSach = lstSach.OrderBy(s => s.Gia);
                    break;
                case "PriceDesc":
                    lstSach = lstSach.OrderByDescending(s => s.Gia);
                    break;
                case "NameAsc":
                    lstSach = lstSach.OrderBy(s => s.TenSach);
                    break;
                case "NameDesc":
                    lstSach = lstSach.OrderByDescending(s => s.TenSach);
                    break;
                case "DiscountDesc":
                    lstSach = lstSach.OrderByDescending(s => s.Sale);
                    break;
                default:
                    lstSach = lstSach.OrderBy(s => s.TenSach); // Sắp xếp mặc định
                    break;
            }

            // Truyền danh sách thể loại và các tham số vào view
            ViewBag.SortOrder = sortOrder;
            ViewBag.CategoryList = db.TLs.ToList(); // Lấy danh sách thể loại từ cơ sở dữ liệu
            ViewBag.SelectedCategory = category;

            // Phân trang
            int pageSize = 8;
            int pageNum = (page ?? 1);

            // Sử dụng ToPagedList từ PagedList.Mvc
            return View(lstSach.ToPagedList(pageNum, pageSize));
        }



    }
}