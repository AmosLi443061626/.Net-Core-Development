using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CoreCommon.Configs;
using CoreCommon.Ioc;
using Microsoft.AspNetCore.Hosting.Internal;

namespace startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var core_env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //根据 <环境变量> 读取不同的配置文件
            //VS设置调试环境变量: 项目->属性->调试->环境变量
            //docker -env key=value 设置<环境变量>
            var config = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)   //指定配置文件所在的目录
            .AddJsonFile($"appsettings.{(string.IsNullOrEmpty(core_env) ? core_env : core_env + ".")}json", optional: true, reloadOnChange: true)
            .Build();  //加载指定的配置文件<环境变量:ASPNETCORE_ENVIRONMENT>未配置,则读取 appsettings.json
            Console.WriteLine($"read setting json: appsettings.{(string.IsNullOrEmpty(core_env) ? core_env : core_env + ".")}json");
            
            core_env = null;
            #region 初始化区域模块


            //配置中心: 读取appsettings.json -> 可扩展配置中心
            //读取配置 ConfigManagerConf.GetValue("key")   ,key 使用冒号寻找下级(Core配置文件):  Logging:Debug
            ConfigManagerConf.SetConfiguration(config);//Config配置文件注册
            Console.WriteLine("read setting end.");

            Console.WriteLine("ioc register start... attribute aop");
            IocContainer.Container.RegisterAssembly("Standard.Examples", "IStandard.Examples"); // Ioc注册  实现dll , 接口类
            IocContainer.Container.Build(); //Ioc 注册后编译
            Console.WriteLine("ioc register end.");


            Console.WriteLine("rabbitmq customer start...");
            Console.WriteLine("attribute TopSubscribeAttribute loding...");
            IocContainer.Container.RabbitBuild();　//注册rabbitmq
            Console.WriteLine("rabbitmq customer end.");


            Console.WriteLine("set mysql connection  start.");
            DBContextEntity.DbContext.Configure(ConfigManagerConf.GetValue("DB:mysql"));//mysql连接初始化
            Console.WriteLine("set mysql connection  end.");
            #endregion

            var build = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();


            //启动同步监控
            build.Run();


            #region 初始化释放模块
            IocContainer.Container.RabbitDispose();

            #endregion

        }
    }
}
