using CoreCommon.Configs;
using CoreCommon.Extensions;
using System;

namespace CoreCommon.MessageMQ.MQS.RabbitMQ
{
    public class RabbitMQConsumerClientFactory : IConsumerClientFactory
    {
        private readonly RabbitMQOptions _rabbitMQOptions;

        public RabbitMQConsumerClientFactory(RabbitMQOptions options)
        {
            if (options != null)
            {
                _rabbitMQOptions = options;
                return;
            }
            _rabbitMQOptions = new RabbitMQOptions
            {
                HostName = ConfigManagerConf.GetValue("rabbitmq:HostName"),
                Password = ConfigManagerConf.GetValue("rabbitmq:Password"),
                UserName = ConfigManagerConf.GetValue("rabbitmq:UserName"),
                VirtualHost = ConfigManagerConf.GetValue("rabbitmq:VirtualHost"),
                TopicExchangeName = ConfigManagerConf.GetValue("rabbitmq:TopicExchangeName"),
                RequestedConnectionTimeout = ConfigManagerConf.GetValue("rabbitmq:RequestedConnectionTimeout").ConvertToIntSafe(),
                SocketReadTimeout = ConfigManagerConf.GetValue("rabbitmq:SocketReadTimeout").ConvertToIntSafe(),
                SocketWriteTimeout = ConfigManagerConf.GetValue("rabbitmq:SocketWriteTimeout").ConvertToIntSafe(),
                Port = ConfigManagerConf.GetValue("rabbitmq:Port").ConvertToIntSafe(),
            };
        }

        public IConsumerClient Create(string queueName)
        {
            return new RabbitMQConsumerClient(queueName,_rabbitMQOptions);
        }
    }
}