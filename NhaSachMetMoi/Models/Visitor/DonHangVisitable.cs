using NhaSachMetMoi.Models;

public class DonHangVisitable : IVisitableOrder
{
    private DonHang _donHang;

    public DonHangVisitable(DonHang donHang)
    {
        _donHang = donHang;
    }

    public DonHang DonHang => _donHang;

    public void Accept(IOrderVisitor visitor)
    {
        visitor.Visit(_donHang);
    }
}
