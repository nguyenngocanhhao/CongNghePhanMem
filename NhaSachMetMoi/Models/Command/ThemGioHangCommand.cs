using NhaSachMetMoi.Controllers;
using NhaSachMetMoi.Models.Command;
public class ThemGioHangCommand : IShopCommand
{
    private readonly GioHangController _controller;
    private readonly int _maSach;
    private readonly string _strURL;

    public ThemGioHangCommand(GioHangController controller, int maSach, string strURL)
    {
        _controller = controller;
        _maSach = maSach;
        _strURL = strURL;
    }

    public void Execute()
    {
        _controller.ThucHienThemGioHang(_maSach, _strURL);
    }
}
