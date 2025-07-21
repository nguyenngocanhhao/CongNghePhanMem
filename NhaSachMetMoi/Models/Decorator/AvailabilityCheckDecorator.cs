using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhaSachMetMoi.Models;
using NhaSachMetMoi.Models.Command;

namespace NhaSachMetMoi.Models.Command.Decorator
{
    public class AvailabilityCheckDecorator : IShopCommand
    {
        private readonly IShopCommand _wrappedCommand;
        private readonly int _maSach;
        private readonly Controller _controller;
        private readonly NhaSachEntities _db = DatabaseSingleton.Instance.DbContext;

        public AvailabilityCheckDecorator(IShopCommand command, int maSach, Controller controller)
        {
            _wrappedCommand = command;
            _maSach = maSach;
            _controller = controller;
        }

        public void Execute()
        {
            var sach = _db.Saches.FirstOrDefault(s => s.MaSach == _maSach);

            if (sach == null || sach.SoLuongTon <= 0)
            {
                // Hiển thị alert khi hết hàng
                _controller.TempData["AlertMessage"] = "Sản phẩm đã hết hàng!";
                return;
            }

            _wrappedCommand.Execute();
        }
    }
}
