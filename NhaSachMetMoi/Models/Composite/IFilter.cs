using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.Composite
{
    public interface IFilter
    {
        IEnumerable<Sach> Apply(IEnumerable<Sach> books);
    }

}
