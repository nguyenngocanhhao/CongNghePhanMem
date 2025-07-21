using NhaSachMetMoi.Models;
using System.ComponentModel.DataAnnotations.Schema;
namespace NhaSachMetMoi.Models { 

public class BookingInfoViewModel
{
    public BookingInfo Booking { get; set; }

    [NotMapped]
    private IBookingState _state;

        [NotMapped]
        public IBookingState State
        {
            get
            {
                if (Booking.ConfirmationStatus == "Đã xác nhận")
                    return new DaXacNhanState();
               
                return new DangChoState(); 
            }
            set
            {
                Booking.ConfirmationStatus = value.GetStatus();
            }
        }

    }
}