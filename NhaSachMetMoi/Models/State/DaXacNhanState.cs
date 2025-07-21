using NhaSachMetMoi.Models;

public class DaXacNhanState : IBookingState
{
    public string GetStatus() => "Đã xác nhận";

    public void NextState(BookingInfo booking) { /* Không làm gì */ }

  
}