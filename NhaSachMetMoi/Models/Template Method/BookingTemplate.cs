using NhaSachMetMoi.Models;
using System.Net.Mail;
using System.Net;
using System;
 
public abstract class BookingTemplate
{
    protected BookingInfo Booking { get; set; }
    protected NhaSachEntities Db { get; } = new NhaSachEntities();

    // Template Method (Quy trình đặt phòng)
    public void ProcessBooking(BookingInfo booking)
    {
        Booking = booking;

        if (!ValidateCustomer()) return;
        CalculateTotalPrice();
        SaveBooking();
        SendConfirmationEmail();
    }

    // Bước 1: Xác thực thông tin khách hàng (có thể thay đổi với từng loại đặt phòng)
    protected abstract bool ValidateCustomer();

    // Bước 2: Tính tổng giá tiền (được override tùy loại phòng)
    protected abstract void CalculateTotalPrice();

    // Bước 3: Lưu vào DB
    protected virtual void SaveBooking()
    {
        Db.BookingInfoes.Add(Booking);
        Db.SaveChanges();
    }

    // Bước 4: Gửi email xác nhận
    protected virtual void SendConfirmationEmail()
    {
        try
        {
            var fromAddress = new MailAddress("athenaathenaaaaa@gmail.com", "Athena Library");
            var toAddress = new MailAddress(Booking.Email, Booking.FullName);
            const string subject = "ATHENA - Lịch booking của bạn đã được xác nhận";

            var builder = new BookingEmailBuilder();
            var director = new EmailDirector(builder);
            string body = director.Construct(Booking);

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("athenaathenaaaaa@gmail.com", "abxbhuqlszibmnhd");

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
        }
        catch (SmtpException ex)
        {
            Console.WriteLine("Gửi email thất bại: " + ex.Message);
        }
    }

}
