using NhaSachMetMoi.Models.Composite;
using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Linq;

public class NameFilter : IFilter
{
    private readonly string _keyword;

    public NameFilter(string keyword)
    {
        _keyword = keyword?.ToLower();
    }

    public IEnumerable<Sach> Apply(IEnumerable<Sach> books)
    {
        return string.IsNullOrWhiteSpace(_keyword)
            ? books
            : books.Where(b => b.TenSach.ToLower().Contains(_keyword));
    }
}
