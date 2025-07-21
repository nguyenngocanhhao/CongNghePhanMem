using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.StrategyFactoryNull
{
    public class AdminUser : IUser
    {
        private Admin _admin;
        public AdminUser(Admin admin) => _admin = admin;
        public int? MaKH { get; set; }
        public string TaiKhoan => _admin.TaiKhoan;
        public string MatKhau => _admin.MatKhau;
        public bool IsValid => true;

        public string GetRole() => "Admin";
    }
}
