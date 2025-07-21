    using NhaSachMetMoi.Models;
using System.Web.Mvc;

public class AdminControllerProxy : IAdminControllerProxy
{
    private readonly RealAdminController _realController = new RealAdminController();
    private readonly object _sessionTaiKhoan;

    public AdminControllerProxy(object sessionTaiKhoan)
    {
        _sessionTaiKhoan = sessionTaiKhoan;
    }

    private bool IsAdmin()
    {
        return _sessionTaiKhoan is Admin;
    }

    public ActionResult IndexAd(string sortOrder, int? page)
    {
        if (!IsAdmin())
            return new HttpStatusCodeResult(403, "Ban khong co quyen truy cap.");

        return _realController.IndexAd(sortOrder, page);
    }

    public ActionResult IndexDH(string sortOrder, int? page)
    {
        if (!IsAdmin())
            return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập.");

        return _realController.IndexDH(sortOrder, page);
    }
    public ActionResult IndexB(string sortOrder, int? page)
    {
        if (!IsAdmin())
            return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập.");

        return _realController.IndexB(sortOrder, page);
    }
    public ActionResult IndexKH(int? page)
    {
        if (!IsAdmin())
            return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập.");

        return _realController.IndexKH(page);
    }
}
