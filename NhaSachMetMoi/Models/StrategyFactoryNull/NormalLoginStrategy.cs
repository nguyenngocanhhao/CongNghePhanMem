using NhaSachMetMoi.Models;
using NhaSachMetMoi.Models.StrategyFactoryNull;


public class NormalLoginStrategy : ILoginStrategy
{
    public IUser Login(NhaSachEntities db, string username, string password)
    {
        return UserFactory.CreateUser(db, username, password);
    }
}