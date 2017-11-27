using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.Logs
{
    public class LogRequest
    {

        public string Cookie { get; set; }

        public string Url { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string RequestBodys { get; set; }

        public string ResponseBodys { get; set; }

        public DateTime ExcuteStartTime { get; set; }

        public DateTime ExcuteEndTime { get; set; }
    }
}
