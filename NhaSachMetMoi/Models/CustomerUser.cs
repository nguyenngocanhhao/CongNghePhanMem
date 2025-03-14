using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models
{
    public class CustomerUser : IUser
    {
        private KH _customer;
        public CustomerUser(KH customer) => _customer = customer;

        public string TaiKhoan => _customer.TaiKhoan;
        public string MatKhau => _customer.MatKhau;
        public string GetRole() => "Customer";
    }
}
