using NhaSachMetMoi.Models.Iterator;
using NhaSachMetMoi.Models;
using System.Collections.Generic;

public class ProductIterator : IIterator<Sach>
{
    private List<Sach> _products;
    private int _position = 0;

    public ProductIterator(List<Sach> products)
    {
        _products = products;
    }

    public bool HasNext()
    {
        return _position < _products.Count;
    }

    public Sach Next()
    {
        if (!HasNext()) return null;
        return _products[_position++];
    }
}