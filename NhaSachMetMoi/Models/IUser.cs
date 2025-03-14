using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models
{
    public interface IUser
    {
        string TaiKhoan { get; }
        string MatKhau { get; }
        string GetRole(); // Trả về quyền của người dùng (Admin, Customer)
    }
}
