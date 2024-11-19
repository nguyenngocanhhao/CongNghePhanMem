using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Microsoft.AspNet.Identity;
using System;

[assembly: OwinStartup(typeof(NhaSachMetMoi.Startup))]

namespace NhaSachMetMoi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cấu hình cho các dịch vụ OWIN
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/DangNhap/IndexTK"),
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = "331638002907-irbl7eovs7jrupgudi5etek0shkcfs3v.apps.googleusercontent.com",  // Thay thế bằng ClientId của bạn
                ClientSecret = "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",  // Thay thế bằng ClientSecret của bạn
                CallbackPath = new PathString("/signin-google"),  // Đường dẫn callback từ Google
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie  // Đảm bảo thuộc tính này được cấu hình
            });
        }

    }
}
