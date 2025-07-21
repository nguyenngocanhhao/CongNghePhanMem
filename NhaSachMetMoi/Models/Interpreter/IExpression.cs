using System.Collections.Generic;
using System.Web.Mvc;

namespace NhaSachMetMoi.Models.Interpreter
{
    public interface IExpression
    {
        IExpression SetNext(IExpression next);
        List<Sach> Interpret(List<Sach> books, FormCollection f);
    }
}
