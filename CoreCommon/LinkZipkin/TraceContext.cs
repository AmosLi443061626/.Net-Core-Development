using System;

using System.Threading;


namespace CoreCommon.LinkZipkin
{
    public static class TraceContext
    {
        private const string TraceCallContextKey = "crto_trace";

        private static readonly AsyncLocal<TraceSpan> AsyncLocalTrace = new AsyncLocal<TraceSpan>();
       
        public static TraceSpan Get()
        {
            return AsyncLocalTrace.Value;
        }

        public static void Set(TraceSpan trace)
        {
            AsyncLocalTrace.Value = trace;
        }

        public static TraceSpan CreateTrace()
        {
            var trace = new TraceSpan().Create();
            AsyncLocalTrace.Value = trace;
            return trace;
        }

        public static TraceSpan CreateTrace(string parentSpanId)
        {
            var trace = new TraceSpan().Create(parentSpanId);
            AsyncLocalTrace.Value = trace;
            return trace;
        }

        public static void Clear()
        {
            AsyncLocalTrace.Value = null;
        }
    }
}
