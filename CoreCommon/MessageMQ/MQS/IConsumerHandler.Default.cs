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
                    using (var client = _consumerClientFactory.Create(matchGroup.Key))
                    {
                        RegisterMessageProcessor(client);

                        foreach (var item in matchGroup.Value)
                        {
                            client.Subscribe(item.Attribute.Name);
                        }

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
                    var receive = message.Content.ToModel<ReceivedMessage>();
                    try
                    {
                        if (receive.ExpiresAt > expiresTime && receive.ExpiresAt < DateTime.Now)
                        {
                            //超时未消费队列
                            Log.Info(LogFormat.Record("RabbitmqOvertime", 0, DateTime.Now, message));
                        }
                        else
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            try
                            {
                                StoreMessage(message, receive); //抛出异常 进入重试机制

                                stopwatch.Stop();
                                Log.Info(LogFormat.Record("RabbitmqQueue", stopwatch.ElapsedMilliseconds, DateTime.Now, message));
                            }
                            catch (Exception ex)
                            {
                                stopwatch.Stop();
                                Log.Info(LogFormat.Record("RabbitmqQueueException", stopwatch.ElapsedMilliseconds, DateTime.Now, new { message = message, ex = ex.ToString() }));
                                throw ex;
                            }
                        }
                    }
                    catch
                    {
                        if (receive.Retries > 0) //重试
                        {
                            receive.Retries -= 1;
                            PublishQueueFactory.factory.PublishAsync(message.Name, receive.ToJson());
                        }
                        else
                        {
                            //失败队列记录
                            Log.Warn(LogFormat.Record("RabbitmqQueueFail", 0, DateTime.Now, message));
                        }
                    }
                    client.Commit();
                }
                catch { }
            };
        }



        private Result StoreMessage(MessageContext message, ReceivedMessage receive)
        {
            var topicExector = _selector.GetTopicExector(message.Name);

            var mExecutor = topicExector[message.Group];
            if (mExecutor != null)
            {
                var mList = mExecutor as List<ConsumerExecutorDescriptor>;

                var ced = mList.FirstOrDefault(x => x.MethodInfo.Name == receive.Name);
                if (ced == null && mList.Count > 0)
                {
                    ced = mList[0];
                }
                if (ced != null)
                {
                    object reflect = Activator.CreateInstance(ced.ImplTypeInfo);

                    var methodInfo = ced.MethodInfo;

                    var parameters = methodInfo.GetParameters();

                    List<object> objParams = new List<object>();
                    objParams = receive.Content.ToModel(objParams);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        objParams[i] = objParams[i].ToJson().ToModel(parameters[i].ParameterType);
                    }
                    var result = methodInfo.Invoke(reflect, objParams.ToArray()) as Result;

                    Console.WriteLine(result.ToJson());
                    return result;
                }
            }
            return Result.Fail(0);
        }
    }
}