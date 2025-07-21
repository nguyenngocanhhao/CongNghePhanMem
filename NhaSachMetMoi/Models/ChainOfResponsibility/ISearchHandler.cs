using System.Collections.Generic;
using System.Web.Mvc;
using NhaSachMetMoi.Models;

public interface ISearchHandler
{
    ISearchHandler SetNext(ISearchHandler next);
    List<Sach> Handle(List<Sach> saches, FormCollection f);
}
