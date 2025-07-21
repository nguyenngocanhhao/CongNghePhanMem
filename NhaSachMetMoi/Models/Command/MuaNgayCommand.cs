using NhaSachMetMoi.Controllers;
using NhaSachMetMoi.Models.Command;

public class MuaNgayCommand : IShopCommand
{
    private readonly GioHangController _controller;
    private readonly int _maSach;

    public MuaNgayCommand(GioHangController controller, int maSach)
    {
        _controller = controller;
        _maSach = maSach;
    }

    public void Execute()
    {
        _controller.ThucHienMuaNgay(_maSach);
    }
}
