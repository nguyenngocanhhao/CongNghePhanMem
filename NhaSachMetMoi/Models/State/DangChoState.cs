using NhaSachMetMoi.Models;

public class DangChoState : IBookingState
{
    public string GetStatus() => "Đang chờ xác nhận";

    public void NextState(BookingInfo booking)
    {
        booking.ConfirmationStatus = new DaXacNhanState().GetStatus();
    }

 
}
