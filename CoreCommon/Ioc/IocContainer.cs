using System;
using Autofac;
using System.Reflection;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System.Collections.Concurrent;
using CoreCommon.Ioc.AOPAttribute;
using System.Linq;
using System.Threading.Tasks;
using CoreCommon.MessageMQ.MQS;
using CoreCommon.MessageMQ.Abstractions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using CoreCommon.CacheOperation;

namespace CoreCommon.Ioc
{
    /// <summary>
    /// Ioc 
    /// 使用前请初始化
    /// 最后 _builder.Build() 更新
    /// </summary>
    public class IocContainer
    {
        private static ContainerBuilder _builder = new ContainerBuilder();

        private static IContainer _container;

        public static IocContainer Container = new IocContainer();

        private static List<ConsumerExecutorDescriptor> _consumerExecutorDescriptor = new List<ConsumerExecutorDescriptor>();

        static ConsumerHandler _consumer;
        public void Build()
        {
            //注册AOP
            _builder.Register(c => new Call()).Named<IInterceptor>("aspect").AsSelf().InstancePerLifetimeScope();

            _builder.RegisterType<RedisCache>().As<ICache>();

            _container = _builder.Build();
        }

        public void RabbitBuild()
        {
            if (_consumerExecutorDescriptor.Count == 0) return;
            //注册Rabbit
            Task.Factory.StartNew(() =>
            {
                _consumer = new ConsumerHandler(_consumerExecutorDescriptor);
                _consumer.Start();
            });
        }

        public void RabbitDispose()
        {
            if (_consumer != null)
                _consumer.Dispose();
        }

        /// <summary>
        /// 注册应用项目
        /// </summary>
        /// <param name="interfaceAssem">项目接口实现</param>
        /// <param name="implAssem">项目接口</param>
        public void RegisterAssembly(string implAssem, string interfaceAssem)

        {
            try
            {
                RegisterAssembly(Assembly.Load(interfaceAssem), Assembly.Load(implAssem));
            }
            catch (Exception ex)
            {
                throw new Exception("注册失败" + implAssem + ex.Message);
            }
        }

        private static void RegisterAssembly(Assembly interfaceAssembly, Assembly impAssembly)
        {
            foreach (TypeInfo interfaceType in interfaceAssembly.DefinedTypes)
            {
                foreach (TypeInfo implType in impAssembly.DefinedTypes)
                {
                    if (interfaceType.IsAssignableFrom(implType))
                    {
                        _builder.RegisterType(implType).InstancePerLifetimeScope().As(interfaceType).EnableInterfaceInterceptors().InterceptedBy(typeof(Call));
                    }
                }
            }

            InitSubscribeAttribute(impAssembly);
        }

        private static void InitSubscribeAttribute(Assembly impAssembly)
        {
            var impTypes = impAssembly.GetTypes();
            foreach (var imp in impTypes)
            {
                var methodInfos = imp.GetMethods();
                var typeInfo = imp.GetTypeInfo();
                foreach (var methodInfo in methodInfos)
                {
                    var attrs = methodInfo.GetCustomAttributes();
                    var subs = attrs.Where(x => x.GetType() == typeof(TopSubscribeAttribute));
                    foreach (var item in subs)
                    {
                        if (((TopSubscribeAttribute)item).IsStart)
                            _consumerExecutorDescriptor.Add(new ConsumerExecutorDescriptor { MethodInfo = methodInfo, Attribute = (TopSubscribeAttribute)item, ImplTypeInfo = typeInfo });
                    }
                }
            }
        }

        public T Get<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (Exception ex)
            {
                throw new Exception("Ioc 配置失败:" + typeof(T).Namespace +" "+ex.ToString());
            }
        }
    }
}
