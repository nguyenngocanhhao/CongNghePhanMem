 using NhaSachMetMoi.Models;

public interface IEmailBuilder
{
    void BuildHeader();
    void BuildBody(BookingInfo booking);
    void BuildFooter();
    string GetResult();
}
