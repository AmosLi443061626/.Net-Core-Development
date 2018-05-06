using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.LinkZipkin
{
    public class TraceSpan
    {
        /// <summary>
        /// 唯一Id(SpanId)
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// TraceId(贯穿记录)
        /// </summary>
        public string traceId { get; set; }
        /// <summary>
        /// 上级Id(parentSpanId)
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 执行时长微妙
        /// </summary>
        public long duration { get; set; }
        /// <summary>
        /// 发出方向(SpanKindConstants)
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 方法请求(get/post)
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 微妙
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        /// 秒
        /// </summary>
        public long timestamp_millis { get; set; }
        /// <summary>
        /// 其它参数
        /// </summary>
        public Dictionary<string, string> tags { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 本地节点信息
        /// </summary>
        public LocalEndpoint localEndpoint { get; set; } = new LocalEndpoint();
        /// <summary>
        /// 其它时间节点记录
        /// </summary>
        public List<Annotations> annotations { get; set; }

        public TraceSpan Create()
        {
            id = NumberUtils.EncodeLongToLowerHexString(RandomUtils.NextLong());
            traceId = NumberUtils.EncodeLongToLowerHexString(RandomUtils.NextLong());
            return this;
        }

        public TraceSpan Create(string parentSpanId)
        {
            parentId = parentSpanId;
            return Create();
        }
    }
}
