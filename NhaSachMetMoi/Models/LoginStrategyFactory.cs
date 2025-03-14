using Microsoft.Owin.Security;
using NhaSachMetMoi.Models;

public static class LoginStrategyFactory
{
    public static ILoginStrategy GetLoginStrategy(string method, IAuthenticationManager authManager = null)
    {
        if (method == "Google") return new GoogleLoginStrategy(authManager);
        return new NormalLoginStrategy();
    }
}
