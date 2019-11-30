﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCVP.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Builder;
using System.IO;
using BCVP.Common.LogHelper;
using StackExchange.Profiling;
using System.Text.RegularExpressions;
using BCVP.IServices;
using Newtonsoft.Json;
using BCVP.Common;

namespace BCVP.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录请求和响应数据
    /// </summary>
    public class RequRespLogMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly IBlogArticleServices _blogArticleServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="blogArticleServices"></param>
        public RequRespLogMildd(RequestDelegate next, IBlogArticleServices blogArticleServices)
        {
            _next = next;
            _blogArticleServices = blogArticleServices;
        }



        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "RequestResponseLog", "Enabled").ObjToBool())
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api"))
                {
                    context.Request.EnableBuffering();
                    Stream originalBody = context.Response.Body;

                    try
                    {
                        // 存储请求数据
                        RequestDataLog(context);

                        using (var ms = new MemoryStream())
                        {
                            context.Response.Body = ms;

                            await _next(context);

                            // 存储响应数据
                            ResponseDataLog(context.Response, ms);

                            ms.Position = 0;
                            await ms.CopyToAsync(originalBody);
                        }
                    }
                    catch (Exception)
                    {
                        // 记录异常
                        //ErrorLogData(context.Response, ex);
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private void RequestDataLog(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = $" QueryData:{request.Path + request.QueryString}\r\n BodyData:{sr.ReadToEndAsync()}";

            if (!string.IsNullOrEmpty(content))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLock.OutSql2Log("RequestResponseLog", new string[] { "Request Data:", content });

                });

                request.Body.Position = 0;
            }

            var requestInfo = JsonConvert.SerializeObject(new RequestInfo()
            {
                Ip = GetClientIP(context),
                Url = request.Path.ObjToString().TrimEnd('/').ToLower(),
                Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Week = GetWeek(),
            });

            if (!string.IsNullOrEmpty(requestInfo))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLock.OutSql2Log("RequestIpInfoLog", new string[] { requestInfo + "," }, false);
                });

                request.Body.Position = 0;
            }


        }
        private string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        public static string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ObjToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ObjToString();
            }
            return ip;
        }

        private void ResponseDataLog(HttpResponse response, MemoryStream ms)
        {
            ms.Position = 0;
            var ResponseBody = new StreamReader(ms).ReadToEnd();

            // 去除 Html
            var reg = "<[^>]+>";
            var isHtml = Regex.IsMatch(ResponseBody, reg);

            if (!string.IsNullOrEmpty(ResponseBody))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLock.OutSql2Log("RequestResponseLog", new string[] { "Response Data:", ResponseBody });

                });
            }
        }
    }
}

