﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using BCVP.AOP;
using BCVP.Common;
using BCVP.Common.LogHelper;
using BCVP.Extensions;
using BCVP.Filter;
using BCVP.Hubs;
using BCVP.IServices;
using BCVP.Middlewares;
using BCVP.Model;
using log4net;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static BCVP.SwaggerHelper.CustomApiVersion;

namespace BCVP
{
    public class Startup
    {

        /// <summary>
        /// log4net 仓储库
        /// </summary>
        public static ILoggerRepository Repository { get; set; }
        private static readonly ILog log =
        LogManager.GetLogger(typeof(GlobalExceptionsFilter));
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 以下code可能与文章中不一样,对代码做了封装,具体查看右侧 Extensions 文件夹.
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            services.AddSingleton(new Appsettings(Env.ContentRootPath));
            services.AddSingleton(new LogLock(Env.ContentRootPath));

            services.AddMemoryCacheSetup();
            services.AddSqlsugarSetup();
            services.AddDbSetup();
            services.AddAutoMapperSetup();
            services.AddCorsSetup();
            services.AddMiniProfilerSetup();
            services.AddSwaggerSetup();
            services.AddJobSetup();
            services.AddHttpContextSetup();
            services.AddAuthorizationSetup();

            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.AddScoped<UseServiceDIAttribute>();

            services.AddControllers(o =>
            {
                // 全局异常过滤
                o.Filters.Add(typeof(GlobalExceptionsFilter));
                // 全局路由权限公约
                //o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
                // 全局路由前缀，统一修改路由
                o.Conventions.Insert(0, new GlobalRoutePrefixFilter(new RouteAttribute(RoutePrefix.Name)));
            })
            //全局配置Json序列化处理
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            //注册要通过反射创建的组件
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            builder.RegisterType<BlogCacheAOP>();//可以直接替换其他拦截器
            builder.RegisterType<BlogRedisCacheAOP>();//可以直接替换其他拦截器
            builder.RegisterType<BlogLogAOP>();//这样可以注入第二个
            builder.RegisterType<BlogTranAOP>();

            // ※※★※※ 如果你是第一次下载项目，请先F6编译，然后再F5执行，※※★※※

            #region 带有接口层的服务注入

            #region Service.dll 注入，有对应接口
            //获取项目绝对路径，请注意，这个是实现类的dll文件，不是接口 IService.dll ，注入容器当然是Activatore
            try
            {
                var servicesDllFile = Path.Combine(basePath, "BCVP.Services.dll");
                var assemblysServices = Assembly.LoadFrom(servicesDllFile);//直接采用加载文件的方法  ※※★※※ 如果你是第一次下载项目，请先F6编译，然后再F5执行，※※★※※

                //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();//指定已扫描程序集中的类型注册为提供所有其实现的接口。


                // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
                var cacheType = new List<Type>();
                if (Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
                {
                    cacheType.Add(typeof(BlogRedisCacheAOP));
                }
                if (Appsettings.app(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
                {
                    cacheType.Add(typeof(BlogCacheAOP));
                }
                if (Appsettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
                {
                    cacheType.Add(typeof(BlogTranAOP));
                }
                if (Appsettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
                {
                    cacheType.Add(typeof(BlogLogAOP));
                }

                builder.RegisterAssemblyTypes(assemblysServices)
                          .AsImplementedInterfaces()
                          .InstancePerLifetimeScope()
                          .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                                                        // 如果你想注入两个，就这么写  InterceptedBy(typeof(BlogCacheAOP), typeof(BlogLogAOP));
                                                        // 如果想使用Redis缓存，请必须开启 redis 服务，端口号我的是6319，如果不一样还是无效，否则请使用memory缓存 BlogCacheAOP
                          .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。 
                #endregion

                #region Repository.dll 注入，有对应接口
                var repositoryDllFile = Path.Combine(basePath, "BCVP.Repository.dll");
                var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
                builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();
            }
            catch (Exception ex)
            {
                log.Error("Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查并拷贝。\n" + ex.Message);

                //throw new Exception("※※★※※ 如果你是第一次下载项目，请先对整个解决方案dotnet build（F6编译），然后再对api层 dotnet run（F5执行），\n因为解耦了，如果你是发布的模式，请检查bin文件夹是否存在Repository.dll和service.dll ※※★※※" + ex.Message + "\n" + ex.InnerException);
            }
            #endregion

            #endregion

            #region 没有接口层的服务层注入

            ////因为没有接口层，所以不能实现解耦，只能用 Load 方法。
            ////注意如果使用没有接口的服务，并想对其使用 AOP 拦截，就必须设置为虚方法
            ////var assemblysServicesNoInterfaces = Assembly.Load("BCVP.Services");
            ////builder.RegisterAssemblyTypes(assemblysServicesNoInterfaces);

            #endregion

            #region 没有接口的单独类 class 注入
            ////只能注入该类中的虚方法
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Love)))
                .EnableClassInterceptors()
                .InterceptedBy(typeof(BlogLogAOP));

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBlogArticleServices _blogArticleServices, ILoggerFactory loggerFactory)
        {

            //记录所有的访问记录
            loggerFactory.AddLog4Net();
            //记录请求与返回数据 
            app.UseReuestResponseLog();
            // signalr 
            app.UseSignalRSendMildd();

            #region Environment
            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();

                //app.Use(async (context, next) =>
                //{
                //    //这里会多次调用，这里测试一下就行，不要打开注释
                //    //var blogs =await _blogArticleServices.GetBlogs();
                //    var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                //    Console.WriteLine(processName);
                //    await next();
                //});
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();

            }
            #endregion

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                var ApiName = Appsettings.app(new string[] { "Startup", "ApiName" });
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });
                // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：解决方案名.index.html
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("BCVP.index.html");//这里是配合MiniProfiler进行性能监控的，《文章：完美基于AOP的接口性能分析》，如果你不需要，可以暂时先注释掉，不影响大局。
                c.RoutePrefix = ""; //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
            });
            #endregion



            // ↓↓↓↓↓↓ 注意下边这些中间件的顺序，很重要 ↓↓↓↓↓↓
            app.UseMiniProfiler();

            app.UseCors("LimitRequests");


            // 跳转https
            //app.UseHttpsRedirection();
            // 使用静态文件
            app.UseStaticFiles();
            // 使用cookie
            app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();//把错误码返回前台，比如是404
            // Routing
            app.UseRouting();
            // 这种自定义授权中间件，可以尝试，但不推荐
            // app.UseJwtTokenAuth();
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/api2/chatHub");
            });


        }

    }
}
