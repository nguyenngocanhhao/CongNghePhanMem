using NhaSachMetMoi.Controllers;
using NhaSachMetMoi.Models;
using System;
using System.Web.Mvc;
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

public class RealAdminController : IAdminControllerProxy
{
    private readonly AdminController _controller = new AdminController();
    private readonly NhaSachEntities db = DatabaseSingleton.Instance.DbContext;


    public ActionResult IndexAd(string sortOrder, int? page)
    {
        var db = DatabaseSingleton.Instance.DbContext;

        var sortParams = new Dictionary<string, string>
        {
            ["MaSPSortParm"] = String.IsNullOrEmpty(sortOrder) ? "MaSP_desc" : "",
            ["TenSPSortParm"] = sortOrder == "TenSP" ? "TenSP_desc" : "TenSP",
            ["GiaSortParm"] = sortOrder == "Gia" ? "Gia_desc" : "Gia",
            ["NamXBSortParm"] = sortOrder == "NamXB" ? "NamXB_desc" : "NamXB",
            ["SoLuongTonSortParm"] = sortOrder == "SoLuongTon" ? "SoLuongTon_desc" : "SoLuongTon",
            ["SaleSortParm"] = sortOrder == "Sale" ? "Sale_desc" : "Sale"
        };

        var saches = db.Saches.AsQueryable();

        switch (sortOrder)
        {
            case "MaSP_desc": saches = saches.OrderByDescending(s => s.MaSach); break;
            case "TenSP": saches = saches.OrderBy(s => s.TenSach); break;
            case "TenSP_desc": saches = saches.OrderByDescending(s => s.TenSach); break;
            case "Gia": saches = saches.OrderBy(s => s.Gia); break;
            case "Gia_desc": saches = saches.OrderByDescending(s => s.Gia); break;
            case "NamXB": saches = saches.OrderBy(s => s.NamXB); break;
            case "NamXB_desc": saches = saches.OrderByDescending(s => s.NamXB); break;
            case "SoLuongTon": saches = saches.OrderBy(s => s.SoLuongTon); break;
            case "SoLuongTon_desc": saches = saches.OrderByDescending(s => s.SoLuongTon); break;
            case "Sale": saches = saches.OrderBy(s => s.Sale); break;
            case "Sale_desc": saches = saches.OrderByDescending(s => s.Sale); break;
            default: saches = saches.OrderBy(s => s.MaSach); break;
        }

        int pageSize = 8;
        int pageNum = (page ?? 1);

        var viewData = new ViewDataDictionary
        {
            ["MaSPSortParm"] = sortParams["MaSPSortParm"],
            ["TenSPSortParm"] = sortParams["TenSPSortParm"],
            ["GiaSortParm"] = sortParams["GiaSortParm"],
            ["NamXBSortParm"] = sortParams["NamXBSortParm"],
            ["SoLuongTonSortParm"] = sortParams["SoLuongTonSortParm"],
            ["SaleSortParm"] = sortParams["SaleSortParm"],
            Model = saches.ToPagedList(pageNum, pageSize)
        };

        return new ViewResult
        {
            ViewName = "IndexAd",
            ViewData = viewData
        };
    }


    public ActionResult IndexDH(string sortOrder, int? page)
    {
        return _controller.IndexDH(sortOrder, page);
    }

    public ActionResult IndexB(string sortOrder, int? page)
    {
        return _controller.IndexB(sortOrder, page);
    }
    public ActionResult IndexKH(int? page)
    {
        return _controller.IndexKH(page);   
    }
    
}
