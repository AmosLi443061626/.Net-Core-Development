using CoreCommon.Configs;
using CoreCommon.Extensions;
using CoreCommon.Logs;
using CoreCommon.MessageMQ.Models;
using CoreCommon.MessageMQ.MQS.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.MessageMQ.MQS
{
    public class PublishQueueFactory
    {
        PublishQueueExecutor _executor = new PublishQueueExecutor(null);

        public static PublishQueueFactory factory = new PublishQueueFactory();

        public OperateResult PublishAsync(string keyName, PublishedMessage content)
        {
            try
            {
                return _executor.PublishAsync(keyName, content.exchangeName, content.ToJson()).Result;
            }
            catch
            {
                try
                {
                    return _executor.PublishAsync(keyName, content.exchangeName, content.ToJson()).Result;
                }
                catch (Exception ex)
                {
                    return OperateResult.Failed(ex);
                }
            }
        }
    }
}
