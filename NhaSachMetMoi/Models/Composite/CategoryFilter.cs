using NhaSachMetMoi.Models.Composite;
using NhaSachMetMoi.Models;
using System.Collections.Generic;
using System.Linq;


public class CategoryFilter : IFilter
{
    private readonly int? _categoryId;

    public CategoryFilter(int? categoryId)
    {
        _categoryId = categoryId;
    }

    public IEnumerable<Sach> Apply(IEnumerable<Sach> books)
    {
        return _categoryId.HasValue
            ? books.Where(s => s.MaTL == _categoryId.Value)
            : books;
    }
}
