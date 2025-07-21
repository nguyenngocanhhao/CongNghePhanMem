using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhaSachMetMoi.Models.StrategyFactoryNull
{
    public interface ILoginStrategy
    {
        IUser Login(NhaSachEntities db, string username, string password);
    }
}
