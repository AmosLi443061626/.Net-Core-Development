using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCommon.LinkZipkin
{
    public static class ZipkinTraceConstants
    {
        public const string HseaderTraceId = "X-B3-TraceId";

        public const string HeaderSpanId = "X-B3-SpanId";

        public const string HeaderParentSpanId = "X-B3-ParentSpanId";
    }
}
