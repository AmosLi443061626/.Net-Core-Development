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

namespace startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())   //指定配置文件所在的目录
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();  //指定加载的配

#if DEBUG //本地运行开发
            config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())   //指定配置文件所在的目录
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();
#endif

            #region 初始化区域模块
            //设置配置文件
            ConfigManagerConf.SetConfiguration(config);//Config配置文件注册

            #region 获取配置文件值(value), 引用类型测试 -> Etcd 及时更新
            //List<string> kd= ConfigManagerConf.GetReferenceValue("99999");
            //while (true)
            //{
            //    Console.WriteLine(kd[0]);
            //    Console.WriteLine("按任意键下一次输出");
            //    Console.ReadLine();
            //}
            #endregion

            IocContainer.Container.RegisterAssembly("Standard.Examples", "IStandard.Examples"); // Ioc注册  实现dll , 接口类
            IocContainer.Container.Build(); //Ioc 注册后编译
            IocContainer.Container.RabbitBuild();　//注册rabbitmq

            DBContextEntity.DbContext.Configure(ConfigManagerConf.GetValue("DB:mysql"));//mysql连接初始化
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
