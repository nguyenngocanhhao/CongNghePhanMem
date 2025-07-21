using NhaSachMetMoi.Models.Composite;
using NhaSachMetMoi.Models;
using System.Collections.Generic;

public class CompositeFilter : IFilter
{
    private readonly List<IFilter> _filters = new List<IFilter>();

    public void Add(IFilter filter)
    {
        _filters.Add(filter);
    }

    public IEnumerable<Sach> Apply(IEnumerable<Sach> books)
    {
        foreach (var filter in _filters)
        {
            books = filter.Apply(books);
        }
        return books;
    }
}
