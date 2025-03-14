using Microsoft.Owin.Security;
using NhaSachMetMoi.Models;
using System;
using System.Linq;


public class GoogleLoginStrategy : ILoginStrategy
{
    private IAuthenticationManager _authManager;

    public GoogleLoginStrategy(IAuthenticationManager authManager)
    {
        _authManager = authManager;
    }
    private string GenerateValidPhoneNumber()
    {
        Random rnd = new Random();
        string[] prefixes = { "032", "033", "034", "035", "036", "037", "038", "039", // Viettel
                          "052", "056", "058", // Vietnamobile
                          "070", "076", "077", "078", "079", // Mobifone
                          "081", "082", "083", "084", "085", "086", "087", "088", "089" // Vinaphone
                        };
        string prefix = prefixes[rnd.Next(prefixes.Length)];
        string suffix = rnd.Next(1000000, 9999999).ToString();
        return prefix + suffix; // Tạo số điện thoại hợp lệ
    }


    public IUser Login(NhaSachEntities db, string username, string password)
    {
        var loginInfo = _authManager.GetExternalLoginInfo();
        if (loginInfo == null) return null;

        var user = db.KHs.SingleOrDefault(k => k.Email == loginInfo.Email);
        if (user == null)
        {
            user = new KH
            {
                HoTen = loginInfo.ExternalIdentity.Name,
                Email = loginInfo.Email,
                TaiKhoan = loginInfo.Email,
                MatKhau = "Athena123!@",
                DiaChi = "Chưa cập nhật", // Nếu ràng buộc không được để trống
                DienThoai = GenerateValidPhoneNumber(), // Nếu cần số điện thoại
                NgaySinh = DateTime.Now // Nếu có ràng buộc ngày sinh
            };
            try
            {
                db.KHs.Add(user);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                throw;
            }
        }
       

        return new CustomerUser(user);
    }
}