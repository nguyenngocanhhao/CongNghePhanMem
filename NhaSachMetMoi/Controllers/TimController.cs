using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;
using PagedList.Mvc;
using PagedList;
using System.Globalization;
using System.Text;

namespace NhaSachMetMoi.Controllers
{
    public class TimController : Controller
    {
        // GET: Tim
        NhaSachEntities db = new NhaSachEntities();

        [HttpPost]
        public ActionResult Ketquatimkiem(FormCollection f, int? page)
        {
            string sTukhoa = f["txtTim"] != null ? f["txtTim"].ToString() : string.Empty;
            string tenTL = f["TenTL"] != null ? f["TenTL"].ToString() : string.Empty;
            ViewBag.Tukhoa = sTukhoa;
            ViewBag.TenTL = tenTL;

            var lsttim = db.Saches.ToList(); // Lấy danh sách sách

            if (!string.IsNullOrEmpty(sTukhoa))
            {
                string keyword = RemoveDiacritics(sTukhoa);
                lsttim = lsttim.Where(n => RemoveDiacritics(n.TenSach).Contains(keyword)).ToList();
            }

            if (!string.IsNullOrEmpty(tenTL))
            {
                string category = RemoveDiacritics(tenTL);
                lsttim = lsttim.Where(n => RemoveDiacritics(n.TL.TenTL).Contains(category)).ToList();
            }

            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(lsttim.OrderBy(n => n.TenSach).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult Ketquatimkiem(int? page, string sTukhoa, string tenTL)
        {
            ViewBag.Tukhoa = sTukhoa;
            ViewBag.TenTL = tenTL;

            var lsttim = db.Saches.ToList(); // Lấy danh sách sách

            if (!string.IsNullOrEmpty(sTukhoa))
            {
                string keyword = RemoveDiacritics(sTukhoa);
                lsttim = lsttim.Where(n => RemoveDiacritics(n.TenSach).Contains(keyword)).ToList();
            }

            if (!string.IsNullOrEmpty(tenTL))
            {
                string category = RemoveDiacritics(tenTL);
                lsttim = lsttim.Where(n => RemoveDiacritics(n.TL.TenTL).Contains(category)).ToList();
            }

            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(lsttim.OrderBy(n => n.TenSach).ToPagedList(pageNum, pageSize));
        }

        private string RemoveDiacritics(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC).ToLower();
        }
    }
}
