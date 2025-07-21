using System;

namespace NhaSachMetMoi.Models
{
    public partial class Sach : IPrototype<Sach>
    {
        public Sach Clone()
        {
            return new Sach
            {
                TenSach = this.TenSach + " (Copy)",
                Gia = this.Gia,
                Sale = this.Sale,
                MoTa = this.MoTa,
                SoLuongTon = this.SoLuongTon,
                AnhBia = this.AnhBia,
                MaTG = this.MaTG,
                MaNXB = this.MaNXB,
                MaTL = this.MaTL,
                NamXB = this.NamXB
            };
        }
    }
}
