using NhaSachMetMoi.Models;

public class NormalLoginStrategy : ILoginStrategy
{
    public IUser Login(NhaSachEntities db, string username, string password)
    {
        return UserFactory.CreateUser(db, username, password);
    }
}