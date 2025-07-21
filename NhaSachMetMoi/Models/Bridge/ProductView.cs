using NhaSachMetMoi.Models.Bridge;

public class ProductView
{
    private readonly IProductRenderer _renderer;
    public ProductView(IProductRenderer renderer)
    {
        _renderer = renderer;
    }

    public string ExtraCssClass => _renderer.GetExtraCssClass();
    public string ExtraBadge => _renderer.GetExtraBadge();
}
