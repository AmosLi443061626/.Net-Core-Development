using Castle.DynamicProxy;
using CoreCommon.Extensions;
using CoreCommon.MessageMQ.Models;
using CoreCommon.MessageMQ.MQS;
using CoreCommon.MessageMQ.MQS.RabbitMQ;
using CoreCommon.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoreCommon.Ioc.AOPAttribute
{
    /// <summary>
    /// 队列属性
    /// </summary>
    public class TopSubscribeAttribute : BaseAspectAttribute
    {
        /// <summary>
        /// topic or exchange route key name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// kafak --> groups.id
        /// rabbitmq --> queue.name
        /// </summary>
        public string Group { get; set; } = "core.default.group";
        /// <summary>
        /// 过期时间秒
        /// </summary>
        public int ExpiresAtSecond { get; set; }
        /// <summary>
        /// 重试次数
        /// </summary>
        public int Retries { get; set; }

        public TopSubscribeAttribute(string name, string group = null, int expiresSecond = 0, int retries = 0)
        {
            Name = name;
            ExpiresAtSecond = expiresSecond;
            if (!string.IsNullOrEmpty(group))
            {
                Group = group;
            }
            Retries = retries;
        }

        public override void OnExcuting(IInvocation invocation)
        {
            var result = PublishQueueFactory.factory.PublishAsync(Name, new PublishedMessage
            {
                Id = Guid.NewGuid().GenerateUniqueID(),
                Name = invocation.Method.Name,
                Content = invocation.Arguments.ToJson(),
                ExpiresAt = ExpiresAtSecond > 0 ? DateTime.Now.AddSeconds(ExpiresAtSecond) : new DateTime(1949, 10, 01),
                Retries = Retries
            }.ToJson());

            invocation.ReturnValue = result.Succeeded ? Result.Success() : Result.Fail(88888, "消息发送失败");
            Debug.WriteLine("消息队列执行前");
        }

        public override void OnExit(IInvocation invocation)
        {
            Debug.WriteLine("消息队列执行后");
        }
    }
}
