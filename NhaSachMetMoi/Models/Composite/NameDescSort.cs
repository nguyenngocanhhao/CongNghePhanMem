using NhaSachMetMoi.Models.Composite;
using NhaSachMetMoi.Models;
using System.Linq;
using System.Collections.Generic;

public class NameDescSort : ISortStrategy
{
    public IEnumerable<Sach> Sort(IEnumerable<Sach> books)
    {
        return books.OrderByDescending(s => s.TenSach);
    }
}
