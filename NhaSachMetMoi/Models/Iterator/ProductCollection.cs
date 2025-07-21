using NhaSachMetMoi.Models.Iterator;
using NhaSachMetMoi.Models;
using System.Collections.Generic;

public class ProductCollection : IAggregate<Sach>
{
    private List<Sach> _products;

    public ProductCollection(List<Sach> products)
    {
        _products = products;
    }

    public IIterator<Sach> CreateIterator()
    {
        return new ProductIterator(_products);
    }
}