using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using NhaSachMetMoi.Models;

namespace NhaSachMetMoi.Controllers
{
    public class DangNhapController : Controller
    {
        NhaSachEntities db = new NhaSachEntities();

        // GET: DangNhap
        public ActionResult IndexTK()
        {
            return View();
        }

        // Đăng nhập bằng tài khoản thông thường
        [HttpPost]
        public ActionResult DangNhap(FormCollection F)
        {
            string sTaiKhoan = F["txtTaiKhoan"].ToString();
            string sMatKhau = F["txtMatKhau"].ToString();

            Admin admin = db.Admins.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);

            if (admin != null)
            {
                Session["TaiKhoan"] = admin;
                return RedirectToAction("IndexAd", "Admin");
            }

            KH kh = db.KHs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);

            if (kh != null)
            {
                ViewBag.ThongBao = "Đăng nhập thành công";
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index", "TrangChu");
            }

            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng";
            return View("IndexTK");
        }

        // Đăng ký tài khoản mới
        [HttpPost]
        public ActionResult DangKy(KH kh)
        {
            if (ModelState.IsValid)
            {
                db.KHs.Add(kh);
                db.SaveChanges();
                ViewBag.ThongBao = "Đăng ký thành công";
                return View("IndexTK");
            }

            return View("IndexTK");
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("IndexTK");
        }

        // Đăng nhập bằng Google
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", "DangNhap", new { ReturnUrl = returnUrl })
            };
            return new ChallengeResult("Google", properties);
        }

        // Callback từ Google sau khi đăng nhập
        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            // Ghi log để kiểm tra giá trị loginInfo
            if (loginInfo == null)
            {
                System.Diagnostics.Debug.WriteLine("GoogleLoginCallback: loginInfo is null.");
                return RedirectToAction("IndexTK", "DangNhap");
            }
            else
            {
                // Ghi log thông tin từ Google để xem có gì bất thường không
                System.Diagnostics.Debug.WriteLine($"GoogleLoginCallback: loginInfo is not null. Email: {loginInfo.Email}, Name: {loginInfo.ExternalIdentity.Name}");
            }

            // Xử lý việc đăng nhập/đăng ký dựa trên thông tin từ Google
            var user = db.KHs.SingleOrDefault(k => k.Email == loginInfo.Email);

            if (user == null)
            {
                // Nếu người dùng chưa tồn tại, đăng ký tự động
                user = new KH
                {
                    HoTen = loginInfo.ExternalIdentity.Name,
                    Email = loginInfo.Email,
                    TaiKhoan = loginInfo.Email,
                    MatKhau = "GeneratedRandomPassword"
                };

                db.KHs.Add(user);
                db.SaveChanges();
            }

            // Đăng nhập thành công
            Session["TaiKhoan"] = user;

            // Chuyển hướng về trang chủ hoặc trang mong muốn
            return RedirectToAction("Index", "TrangChu");
        }


        // Property để lấy Authentication Manager
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // Lớp hỗ trợ việc xử lý khi thách thức đăng nhập qua Google
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, AuthenticationProperties properties)
            {
                LoginProvider = provider;
                Properties = properties;
            }

            public string LoginProvider { get; set; }
            public AuthenticationProperties Properties { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.GetOwinContext().Authentication.Challenge(Properties, LoginProvider);
            }
        }
    }
}
