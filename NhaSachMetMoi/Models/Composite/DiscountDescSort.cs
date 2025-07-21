using NhaSachMetMoi.Models.Composite;
using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Linq;

public class DiscountDescSort : ISortStrategy
{
    public IEnumerable<Sach> Sort(IEnumerable<Sach> books)
    {
        return books.OrderByDescending(s => s.Sale);
    }
}
