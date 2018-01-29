using System;
using System.Text;
using System.Threading.Tasks;

using RabbitMQ.Client;
using CoreCommon.Extensions;
using CoreCommon.Configs;
using CoreCommon.Logs;
using CoreCommon.MessageMQ.Models;

namespace CoreCommon.MessageMQ.MQS.RabbitMQ
{
    public class PublishQueueExecutor
    {
        private readonly RabbitMQOptions _rabbitMQOptions;

        private string[] _hostNames;
        public PublishQueueExecutor(RabbitMQOptions options)
        {
            if (options != null)
            {
                _rabbitMQOptions = options;
                return;
            }
            _rabbitMQOptions = new RabbitMQOptions
            {
                Password = ConfigManagerConf.GetValue("rabbitmq:Password"),
                UserName = ConfigManagerConf.GetValue("rabbitmq:UserName"),
                VirtualHost = ConfigManagerConf.GetValue("rabbitmq:VirtualHost"),
                TopicExchangeName = ConfigManagerConf.GetValue("rabbitmq:TopicExchangeName"),
                RequestedConnectionTimeout = ConfigManagerConf.GetValue("rabbitmq:RequestedConnectionTimeout").ConvertToIntSafe(),
                SocketReadTimeout = ConfigManagerConf.GetValue("rabbitmq:SocketReadTimeout").ConvertToIntSafe(),
                SocketWriteTimeout = ConfigManagerConf.GetValue("rabbitmq:SocketWriteTimeout").ConvertToIntSafe(),
                Port = ConfigManagerConf.GetValue("rabbitmq:Port").ConvertToIntSafe(),
            };
            _hostNames = ConfigManagerConf.GetValue("rabbitmq:HostName").Split(',');
        }

        public Task<OperateResult> PublishAsync(string keyName, string group, string content)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQOptions.HostName,
                UserName = _rabbitMQOptions.UserName,
                Port = _rabbitMQOptions.Port,
                Password = _rabbitMQOptions.Password,
                VirtualHost = _rabbitMQOptions.VirtualHost,
                RequestedConnectionTimeout = _rabbitMQOptions.RequestedConnectionTimeout,
                SocketReadTimeout = _rabbitMQOptions.SocketReadTimeout,
                SocketWriteTimeout = _rabbitMQOptions.SocketWriteTimeout
            };

            try
            {
                using (var connection = factory.CreateConnection(_hostNames))
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(content);

                    channel.ExchangeDeclare(_rabbitMQOptions.TopicExchangeName, RabbitMQOptions.ExchangeType, durable: true);
                    channel.BasicPublish(exchange: _rabbitMQOptions.TopicExchangeName,
                                         routingKey: keyName,
                                         basicProperties: null,
                                         body: body);
                }
                return Task.FromResult(OperateResult.Success);
            }
            catch (Exception ex)
            {
                return Task.FromResult(OperateResult.Failed(ex,
                    new OperateError()
                    {
                        Code = ex.HResult.ToString(),
                        Description = ex.Message
                    }));
            }
        }
    }
}