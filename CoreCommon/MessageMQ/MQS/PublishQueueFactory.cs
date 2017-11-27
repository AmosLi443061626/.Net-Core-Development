using CoreCommon.Logs;
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

        public OperateResult PublishAsync(string keyName, string content)
        {
            try
            {
                return _executor.PublishAsync(keyName, content).Result;
            }
            catch
            {
                try
                {
                    return _executor.PublishAsync(keyName, content).Result;
                }
                catch (Exception ex)
                {
                    Log.Fatal(LogFormat.Record("RabbitmqPublishException", 0, DateTime.Now, new { keyName = keyName, content = content, exception = ex.ToString() }));
                    return OperateResult.Failed(ex);
                }
            }
        }
    }
}
