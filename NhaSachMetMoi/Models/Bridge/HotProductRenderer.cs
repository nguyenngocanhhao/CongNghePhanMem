using NhaSachMetMoi.Models.Bridge;

public class HotProductRenderer : IProductRenderer
{
    public string GetExtraCssClass() => "hot-product";
    public string GetExtraBadge() => "<div class='hot-badge'>🔥 HOT</div>";
}
