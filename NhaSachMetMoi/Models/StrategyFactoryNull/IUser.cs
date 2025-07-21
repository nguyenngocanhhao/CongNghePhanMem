using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.StrategyFactoryNull
{
    public interface IUser
    {
        string TaiKhoan { get; }
        string MatKhau { get; }
        int? MaKH { get; }
        bool IsValid { get; }

        string GetRole(); // Trả về quyền của người dùng (Admin, Customer)
    }
}
