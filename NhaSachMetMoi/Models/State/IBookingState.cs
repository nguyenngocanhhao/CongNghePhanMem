// IBookingState.cs
using NhaSachMetMoi.Models;

public interface IBookingState
{
    string GetStatus();
    void NextState(BookingInfo booking);
   
}
