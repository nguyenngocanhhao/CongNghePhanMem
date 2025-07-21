using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.StrategyFactoryNull
{
    public class CustomerUser : IUser
    {
        private KH _customer;
        public CustomerUser(KH customer) => _customer = customer;
        public int? MaKH { get; set; }
        public string TaiKhoan => _customer.TaiKhoan;
        public string MatKhau => _customer.MatKhau;
        public bool IsValid => true;

        public string GetRole() => "Customer";
    }
}
