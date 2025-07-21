using NhaSachMetMoi.Models;
using Microsoft.Owin.Security;
using NhaSachMetMoi.Models.StrategyFactoryNull;


public static class LoginStrategyFactory
{
    public static ILoginStrategy GetLoginStrategy(string method, IAuthenticationManager authManager = null)
    {
        if (method == "Google") return new GoogleLoginStrategy(authManager);
        return new NormalLoginStrategy();
    }
}
