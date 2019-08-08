#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2019-04-15 创建
******************************************************************************/
#endregion

#region 引用命名
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.IO;
#endregion

namespace Dts.Core
{
    /// <summary>
    /// 启动微服务
    /// </summary>
    public static class Launcher
    {
        /// <summary>
        /// 初始化服务，任一环节失败即启动失败
        /// + 创建日志对象
        /// + 读取配置
        /// + 缓存默认库表结构、同步时间
        /// + 缓存Sql语句
        /// + 启动http服务器
        /// 此方法不可异步，否则启动有问题！！！
        /// </summary>
        /// <param name="p_stub">服务存根</param>
        /// <param name="p_args">启动参数</param>
        public static void Run(ISvcStub p_stub, string[] p_args)
        {
            Glb.Stub = p_stub ?? throw new ArgumentNullException(nameof(p_stub));
            CreateLogger();
            LoadConfig();
            DbSchema.Init();
            Silo.CacheSql();
            RunWebHost(p_args);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// 创建日志对象
        /// </summary>
        static void CreateLogger()
        {
            try
            {
                // 支持动态调整
                var cfg = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "etc/config"))
                    .AddJsonFile("logger.json", false, true)
                    .Build();

                // 日志文件命名：
                // k8s：服务名 -服务实例ID-日期.txt，避免部署在k8s挂载宿主目录时文件名重复
                // windows：服务名-日期.txt
                string fileName = Glb.IsInDocker ? $"{Glb.SvcName}-{Glb.ID}-.txt" : $"{Glb.SvcName}-.txt";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "etc/log", fileName);
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(cfg)
                    .WriteTo.Console()
                    // 输出json文件，默认最大1G、最多保存31个文件、实时写、文件独占方式
                    .WriteTo.File(
                        new CompactJsonFormatter(),
                        path,
                        rollingInterval: RollingInterval.Day, // 文件名末尾加日期
                        rollOnFileSizeLimit: true) // 超过1G时新文件名末尾加序号
                    .CreateLogger();
            }
            catch (Exception e)
            {
                Console.WriteLine($"创建日志对象失败：{e.Message}");
                throw;
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        static void LoadConfig()
        {
            try
            {
                Glb.Config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "etc/config"))
                    .AddJsonFile("service.json", false, true)
                    .AddJsonFile("global.json", false, true)
                    .Build();
                Log.Information("读取配置成功");
            }
            catch (Exception e)
            {
                Log.Fatal($"读取配置失败：{e.Message}");
                throw;
            }
        }

        /// <summary>
        /// 启动Kestrel
        /// </summary>
        /// <param name="p_args">启动参数</param>
        static void RunWebHost(string[] p_args)
        {
            try
            {
                var host = WebHost.CreateDefaultBuilder(p_args)
                    .ConfigureKestrel(options =>
                    {
                        // 设置http2为默认监听协议
                        // 未使用Listen方法，因无法应用外部设置的端口！
                        options.ConfigureEndpointDefaults(listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                            listenOptions.UseHttps(Path.Combine(Directory.GetCurrentDirectory(), "etc/cert.pfx"), "test");
                        });
                    })
                    .UseStartup<Startup>()
                    // 内部注入AddSingleton<ILoggerFactory>(new SerilogLoggerFactory())
                    .UseSerilog()
                    // 实例化WebHost并初始化，调用Startup.ConfigureServices和Configure
                    .Build();
                Log.Information($"启动 {Glb.SvcName} 成功");

                // 内部调用WebHost.StartAsync()
                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"启动 {Glb.SvcName} 失败");
                throw;
            }
        }
    }
}