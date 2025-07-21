namespace NhaSachMetMoi.Models.StrategyFactoryNull
{
    public class NullUser : IUser
    {
        public string TaiKhoan => "guest";
        public string MatKhau => "";
        public int? MaKH => null;
        public bool IsValid => false;
        public string GetRole() => "None";
    }
}
