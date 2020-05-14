﻿#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-04-15 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Core.EventBus;
using Dt.Core.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
#endregion

namespace Dt.Core
{
    public class Startup
    {
        /// <summary>
        /// 定义全局服务
        /// </summary>
        /// <param name="p_services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection p_services)
        {
            Glb.ConfigureServices(p_services);
            return Silo.ConfigureServices(p_services);
        }

        /// <summary>
        /// 定义请求管道的中间件
        /// </summary>
        /// <param name="p_app"></param>
        public void Configure(IApplicationBuilder p_app)
        {
            // 添加中间件，注意先后顺序！
            // 异常处理中间件放在管道的最前端，内部try { await _next(context); }捕获异常时重定向到 /.error
            p_app.UseExceptionHandler("/.error");

            // 内置中间件
            p_app.UseMiddleware<DtMiddleware>();

            // 外部中间件
            Glb.Configure(p_app);

            // 默认页和静态页面改为在外部启用！
            //p_app.UseDefaultFiles();
            //p_app.UseStaticFiles();

            // 末尾中间件，显示自定义404页面
            p_app.UseMiddleware<EndMiddleware>();

            // 订阅事件
            RemoteEventBus.Subscribe(p_app.ApplicationServices);
        }
    }
}
