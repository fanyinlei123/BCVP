using BCVP.AuthHelper;
using Microsoft.AspNetCore.Builder;

namespace BCVP.Middlewares
{
    public static class MiddlewareHelpers
    {
        public static IApplicationBuilder UseJwtTokenAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenAuth>();
        }
        public static IApplicationBuilder UseReuestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMildd>();
        }
        public static IApplicationBuilder UseSignalRSendMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SignalRSendMildd>();
        }
    }
}
