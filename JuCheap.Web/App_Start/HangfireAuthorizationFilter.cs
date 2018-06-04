using Hangfire.Dashboard;
using Microsoft.Owin;

namespace JuCheap.Web
{
    /// <summary>
    /// Hangfire认证
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            return owinContext.Authentication.User.Identity.IsAuthenticated;
        }
    }
}