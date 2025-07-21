using NhaSachMetMoi.Models;

public interface IOrderVisitor
{
    void Visit(DonHang donHang);
}
