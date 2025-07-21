// BookingMediator.cs
using NhaSachMetMoi.Models.Mediator;
using NhaSachMetMoi.Models;

public class BookingMediator : IBookingMediator
{
    public void HandleBooking(BookingInfo booking, KH khachHang)
    {
        var bookingProcess = new NormalBooking(khachHang);
        bookingProcess.ProcessBooking(booking);
    }
}
