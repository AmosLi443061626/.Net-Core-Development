using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CoreCommon.MessageMQ.Models;

namespace CoreCommon.MessageMQ.MQS.RabbitMQ
{
    public class RabbitMQConsumerClient : IConsumerClient
    {
        private readonly string _exchageName;
        private readonly string _queueName;
        private readonly RabbitMQOptions _rabbitMQOptions;

        private IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private ulong _deliveryTag;

        public event EventHandler<MessageContext> MessageReceieved;

        private string[] _hostNames;

        public RabbitMQConsumerClient(string queueName, RabbitMQOptions options)
        {
            _queueName = queueName;
            _rabbitMQOptions = options;
            _exchageName = options.TopicExchangeName;

            InitClient();
        }

        private void InitClient()
        {
            _connectionFactory = new ConnectionFactory()
            {
                UserName = _rabbitMQOptions.UserName,
                Port = _rabbitMQOptions.Port,
                Password = _rabbitMQOptions.Password,
                VirtualHost = _rabbitMQOptions.VirtualHost,
                RequestedConnectionTimeout = _rabbitMQOptions.RequestedConnectionTimeout,
                SocketReadTimeout = _rabbitMQOptions.SocketReadTimeout,
                SocketWriteTimeout = _rabbitMQOptions.SocketWriteTimeout,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = 60
            };
            _hostNames = _rabbitMQOptions.HostName.Split(',');
            _connection = _connectionFactory.CreateConnection(_hostNames);
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchageName, type: RabbitMQOptions.ExchangeType);
            _channel.QueueDeclare(_queueName, exclusive: false, durable: true, autoDelete: false);
        }

        public void Listening(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnConsumerReceived;
            _channel.BasicConsume(_queueName, false, consumer);
            while (!cancellationToken.IsCancellationRequested)
            {
                try { 
                Task.Delay(timeout, cancellationToken).Wait();
                }
                catch { }
            }
        }

        public void Subscribe(string topic)
        {
            _channel.QueueBind(_queueName, _exchageName, topic);
        }

        public void Subscribe(string topic, int partition)
        {
            _channel.QueueBind(_queueName, _exchageName, topic);
        }

        public void Commit()
        {
            _channel.BasicAck(_deliveryTag, false);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            _deliveryTag = e.DeliveryTag;
            var message = new MessageContext
            {
                Group = _queueName,
                Name = e.RoutingKey,
                Content = Encoding.UTF8.GetString(e.Body)
            };
            MessageReceieved?.Invoke(sender, message);
        }
    }
}