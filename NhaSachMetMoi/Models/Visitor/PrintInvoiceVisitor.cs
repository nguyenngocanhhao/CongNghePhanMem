using NhaSachMetMoi.Models;
using System;

public class PrintInvoiceVisitor : IOrderVisitor
{
    public void Visit(DonHang donHang)
    {
        Console.WriteLine("=== HÓA ??N BÁN HÀNG ===");
        Console.WriteLine($"Mã đơn hàng: {donHang.MaDH}");
        Console.WriteLine($"Ngày đặt: {donHang.NgayDat}");
        Console.WriteLine($"Tổng tiền: {donHang.TongTien} VND");
        Console.WriteLine("--- Danh sách sản phẩm ---");
        foreach (var item in donHang.ChiTietDHs)
        {
            Console.WriteLine($"{item.TenSach} x{item.SoLuong} - {item.GiaBan}?");
        }
    }
}
