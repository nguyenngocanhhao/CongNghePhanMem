using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models
{
    public class AdminUser : IUser
    {
        private Admin _admin;
        public AdminUser(Admin admin) => _admin = admin;

        public string TaiKhoan => _admin.TaiKhoan;
        public string MatKhau => _admin.MatKhau;
        public string GetRole() => "Admin";
    }
}
