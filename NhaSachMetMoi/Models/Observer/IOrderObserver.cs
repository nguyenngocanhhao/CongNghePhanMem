using NhaSachMetMoi.Models;
using System.Collections.Generic;

public interface IOrderObserver
{
    void Update(DonHang order);
}

public class OrderSubject
{
    private readonly List<IOrderObserver> _observers = new List<IOrderObserver>();

    public void Attach(IOrderObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IOrderObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(DonHang order)
    {
        foreach (var observer in _observers)
        {
            observer.Update(order);
        }
    }
}
