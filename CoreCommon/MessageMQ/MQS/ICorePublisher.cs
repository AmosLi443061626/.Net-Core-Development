using System.Data;
using System.Threading.Tasks;

namespace CoreCommon.MessageMQ.MQS
{
    /// <summary>
    /// A publish service for publish a message to .
    /// </summary>
    public interface ICorePublisher
    {
        Task PublishAsync(string name, string content);

        void Publish(string name, string content);

        Task PublishAsync<T>(string name, T contentObj);

        void Publish<T>(string name, T contentObj);

        Task PublishAsync(string name, string content, IDbConnection dbConnection, IDbTransaction dbTransaction = null);

        void Publish(string name, string content, IDbConnection dbConnection, IDbTransaction dbTransaction = null);

        Task PublishAsync<T>(string name, T contentObj, IDbConnection dbConnection, IDbTransaction dbTransaction = null);

        void Publish<T>(string name, T contentObj, IDbConnection dbConnection, IDbTransaction dbTransaction = null);
    }
}