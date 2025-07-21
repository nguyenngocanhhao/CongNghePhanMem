using System.Linq;

namespace NhaSachMetMoi.Models.StrategyFactoryNull
{
    public static class UserFactory
    {
        public static IUser CreateUser(NhaSachEntities db, string username, string password)
        {
            var admin = db.Admins.SingleOrDefault(a => a.TaiKhoan == username && a.MatKhau == password);
            if (admin != null) return new AdminUser(admin);

            var customer = db.KHs.SingleOrDefault(k => k.TaiKhoan == username && k.MatKhau == password);
            if (customer != null) return new CustomerUser(customer);

            return new NullUser(); // Trả về Null Object
        }

    }
}
