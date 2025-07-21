using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.Iterator
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
}
