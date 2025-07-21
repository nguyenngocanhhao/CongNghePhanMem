 using NhaSachMetMoi.Models;

public class EmailDirector
{
    private readonly IEmailBuilder _builder;

    public EmailDirector(IEmailBuilder builder)
    {
        _builder = builder;
    }

    public string Construct(BookingInfo booking)
    {
        _builder.BuildHeader();
        _builder.BuildBody(booking);
        _builder.BuildFooter();
        return _builder.GetResult();
    }
}
