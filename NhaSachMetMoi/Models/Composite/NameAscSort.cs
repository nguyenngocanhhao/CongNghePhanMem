using NhaSachMetMoi.Models.Composite;
using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Linq;

public class NameAscSort : ISortStrategy
{
    public IEnumerable<Sach> Sort(IEnumerable<Sach> books)
    {
        return books.OrderBy(s => s.TenSach);
    }
}
