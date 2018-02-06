using System;
using System.Threading;
using System.Threading.Tasks;
using CoreCommon.MessageMQ.MQS.RabbitMQ;
using System.Collections.Generic;
using CoreCommon.MessageMQ.Abstractions;
using System.Collections.Concurrent;
using CoreCommon.Extensions;
using CoreCommon.MessageMQ.Models;
using System.Linq;
using CoreCommon.Results;
using CoreCommon.Logs;
using System.Diagnostics;
using CoreCommon.CacheOperation;
using CoreCommon.Ioc;

namespace CoreCommon.MessageMQ.MQS
{
    public class ConsumerHandler : IDisposable
    {
        private readonly MethodMatcherCache _selector;

        private readonly IConsumerClientFactory _consumerClientFactory;
        private readonly CancellationTokenSource _cts;

        private readonly TimeSpan _pollingDelay = TimeSpan.FromSeconds(1);

        private Task _compositeTask;
        private bool _disposed;

        List<ConsumerExecutorDescriptor> _listConsumerExecutorDescriptor;
        private ConcurrentDictionary<string, IList<ConsumerExecutorDescriptor>> Entries { get; }

        public ConsumerHandler(List<ConsumerExecutorDescriptor> consumerExecutorDescriptor)
        {
            _cts = new CancellationTokenSource();
            _selector = new MethodMatcherCache();
            _consumerClientFactory = new RabbitMQConsumerClientFactory(null);

            _listConsumerExecutorDescriptor = consumerExecutorDescriptor;
        }

        public void Start()
        {
            var groupingMatchs = _selector.GetCandidatesMethodsOfGroupNameGrouped(_listConsumerExecutorDescriptor);

            foreach (var matchGroup in groupingMatchs)
            {
                Task.Factory.StartNew(() =>
                {
                    using (var client = _consumerClientFactory.Create(matchGroup.Key, matchGroup.Value.Attribute.ExchangeName))
                    {
                        RegisterMessageProcessor(client);

                        client.Subscribe(matchGroup.Value.Attribute.ExchangeName, matchGroup.Value.Attribute.Group);

                        client.Listening(_pollingDelay, _cts.Token);
                    }
                }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            _compositeTask = Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            _cts.Cancel();

            try
            {
                _compositeTask.Wait(TimeSpan.FromSeconds(60));
            }
            catch (AggregateException ex)
            {
                var innerEx = ex.InnerExceptions[0];
                if (!(innerEx is OperationCanceledException))
                {
                }
            }
        }

        DateTime expiresTime = new DateTime(1949, 10, 01);

        private void RegisterMessageProcessor(IConsumerClient client)
        {
            client.MessageReceieved += (sender, message) =>
            {
                try
                {
                    var receive = message.Content.ToModel<PublishedMessage>();
                    try
                    {
                        StoreMessage(message, receive); //抛出异常 进入重试机制
                    }
                    catch
                    {
                        receive.consumerNum++;
                        if (receive.consumerNum >= receive.maxNum)
                        {
                            //写入缓存
                            IocContainer.Container.Get<ICache>().Set($"rabbimq:fail:{receive.queueName}:{receive.rid}", receive.ToJson(), -1);
                        }
                        else
                            PublishQueueFactory.factory.PublishAsync(message.Group, receive);
                    }
                    client.Commit();
                }
                catch { }
            };
        }



        private Result StoreMessage(MessageContext message, PublishedMessage receive)
        {
            var queueExector = _selector.GetTopicExector(message.Name);

            if (queueExector != null)
            {
                object reflect = Activator.CreateInstance(queueExector.ImplTypeInfo);
                var methodInfo = queueExector.MethodInfo;
                var parameters = methodInfo.GetParameters();
                List<object> objParams = new List<object>();
                objParams = receive.msgBeanJson.ToModel(objParams);
                for (int i = 0; i < parameters.Length; i++)
                {
                    objParams[i] = objParams[i].ToJson().ToModel(parameters[i].ParameterType);
                }
                var result = methodInfo.Invoke(reflect, objParams.ToArray()) as Result;

                return result;
            }
            return Result.Fail(0);
        }
    }
}