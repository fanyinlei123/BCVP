﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BCVP.Model
{
    public enum ResponseEnum
    {
        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限")]
        NoPermissions = 401,
        /// <summary>
        /// 找不到指定资源
        /// </summary>
        [Description("找不到指定资源")]
        NoFound = 404,
        /// <summary>
        /// 找不到指定资源
        /// </summary>
        [Description("服务器错误")]
        ServerError = 500
    }
}
