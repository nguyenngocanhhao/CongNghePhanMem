using NhaSachMetMoi.Models;

public class NormalBooking : BookingTemplate
{
    private KH Customer;

    public NormalBooking(KH customer)
    {
        Customer = customer;
    }

    protected override bool ValidateCustomer()
    {
        if (Customer == null) return false;

        Booking.MaKH = Customer.MaKH;
        Booking.PhoneNumber = Customer.DienThoai;
        Booking.FullName = Customer.HoTen;
        Booking.Email = Customer.Email;

        return true;
    }

    protected override void CalculateTotalPrice()
    {
        if (Booking.RoomPackage == "Meeting Room")
        {
            switch (Booking.Duration)
            {
                case 2:
                    Booking.TotalPrice = 190000;
                    break;
                case 3:
                    Booking.TotalPrice = 220000;
                    break;
                case 4:
                    Booking.TotalPrice = 300000;
                    break;
                default:
                    Booking.TotalPrice = 0;
                    break;
            }
        }
        else if (Booking.RoomPackage == "Private Zone")
        {
            switch (Booking.Duration)
            {
                case 2:
                    Booking.TotalPrice = 35000;
                    break;
                case 3:
                    Booking.TotalPrice = 50000;
                    break;
                case 4:
                    Booking.TotalPrice = 60000;
                    break;
                default:
                    Booking.TotalPrice = 0;
                    break;
            }
        }
        else if (Booking.RoomPackage == "Couple Room")
        {
            switch (Booking.Duration)
            {
                case 2:
                    Booking.TotalPrice = 45000;
                    break;
                case 3:
                    Booking.TotalPrice = 68000;
                    break;
                case 4:
                    Booking.TotalPrice = 89000;
                    break;
                default:
                    Booking.TotalPrice = 0;
                    break;
            }
        }
        else
        {
            Booking.TotalPrice = 0; // Nếu loại phòng không hợp lệ
        }
    }
}
