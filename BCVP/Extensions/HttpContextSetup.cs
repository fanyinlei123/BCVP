﻿using BCVP.Common.HttpContextUser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BCVP.Extensions
{
    /// <summary>
    /// Db 启动服务
    /// </summary>
    public static class HttpContextSetup
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
