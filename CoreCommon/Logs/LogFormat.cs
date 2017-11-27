using CoreCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.Logs
{
    /// <summary>
    /// Json格式
    /// </summary>
    public class LogFormat
    {
        public LogFormat()
        {
        }

        /// <summary>
        /// 系统标识
        /// </summary>
        public string _sysFlag { get; set; }

        /// <summary>
        /// 标记
        /// </summary>
        public string _flag { get; set; }
        /// <summary>
        /// 处理时间处理多少ms
        /// </summary>
        public long _handlingTime { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime _added { get; set; }

        public string _message { get; set; }

        public static LogFormat Record<T>(string flag, long handlingTime, DateTime added, T message)
        {
            return new LogFormat {
                _flag = flag,
                _handlingTime = handlingTime,
                _added = added,
                _message = message.ToJson()
            };
        }
    }
}
