// Helpers/EmailBuilders/BookingEmailBuilder.cs
using System.Text;
using NhaSachMetMoi.Models;

public class BookingEmailBuilder : IEmailBuilder
{
    private StringBuilder _content;

    public void BuildHeader()
    {
        _content = new StringBuilder();
        _content.AppendLine("<html><body style='font-family: Arial, sans-serif; color: #333;'>");
    }

    public void BuildBody(BookingInfo booking)
    {
        _content.AppendLine($"<h2 style='color: #4CAF50;'>Xin chào {booking.FullName},</h2>");
        _content.AppendLine("<p>Bạn đã đặt phòng tại <strong>Athena Library</strong>. Thông tin chi tiết:</p>");
        _content.AppendLine("<table style='width: 100%; max-width: 600px; border-collapse: collapse;'>");

        void AddRow(string label, string value)
        {
            _content.AppendLine("<tr>");
            _content.AppendLine($"<td style='padding: 8px; background-color: #f2f2f2; font-weight: bold;'>{label}</td>");
            _content.AppendLine($"<td style='padding: 8px;'>{value}</td>");
            _content.AppendLine("</tr>");
        }

        AddRow("Gói phòng", booking.RoomPackage);
        AddRow("Thời lượng", booking.Duration + " giờ");
        AddRow("Ngày", booking.BookingDate.ToString("dd/MM/yyyy"));
        AddRow("Giờ đến", booking.BookingTime.ToString(@"hh\:mm"));
        AddRow("Tổng tiền", booking.TotalPrice + " VND");

        _content.AppendLine("</table>");
    }

    public void BuildFooter()
    {
        _content.AppendLine("<p style='margin-top: 20px;'>Trân trọng,<br><strong>Athena Library</strong></p>");
        _content.AppendLine("</body></html>");
    }

    public string GetResult() => _content.ToString();
}
