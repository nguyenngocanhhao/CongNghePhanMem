using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.Mediator
{
    // IBookingMediator.cs
    public interface IBookingMediator
    {
        void HandleBooking(BookingInfo booking, KH khachHang);
    }

}
