using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.Bridge
{
    public interface IProductRenderer
    {
        string GetExtraCssClass();
        string GetExtraBadge();
    }

}
