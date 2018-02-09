using CoreCommon.Exceptions;
using CoreCommon.Extensions;
using CoreCommon.LinkZipkin;
using CoreCommon.Logs;
using CoreCommon.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace startup.Filters
{
    public class LogFilterAttribute : Attribute, IActionFilter
    {
        public LogRequest _logRequest;

        public TraceSpan traceSpan;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //初始化日志
            TraceSpan traceSpan = TraceContext.CreateTrace();
            traceSpan.parentId = context.HttpContext.Request.Headers[ZipkinTraceConstants.HeaderSpanId];
            var traceId = context.HttpContext.Request.Headers[ZipkinTraceConstants.HseaderTraceId].ToString();
            if (!traceId.IsNullOrEmpty())
            {
                traceSpan.traceId = traceId;
            }
            traceSpan.timestamp = DateTime.Now.ToUnixTimestamp();
            traceSpan.timestamp_millis = DateTime.Now.ToTimestamp();
            traceSpan.name = context.HttpContext.Request.Path;
            traceSpan.kind = "SERVER";
            traceSpan.localEndpoint.serviceName = "pay";

            #region 日志信息
            traceSpan.tags.Add("http.url", $"{context.HttpContext.Request.Host.Value}");
            traceSpan.tags.Add("http.path", $"{context.HttpContext.Request.Host.Value}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
            #endregion

            traceSpan.tags.Add("request.body", context.HttpContext.Request.ToReadAsyncJson());
            traceSpan.tags.Add("group", $"http");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            traceSpan = TraceContext.Get();
            traceSpan.duration = DateTime.Now.ToUnixTimestamp() - traceSpan.timestamp; //运行时间
            traceSpan.tags.Add("status", "200");
            if (context.Exception != null)
            {
                Result result = null;
                if (context.Exception is ChecksException ce) //验证异常(业务)
                {
                    result = Result.Fail(ce.Code, ce.Message);
                    traceSpan.tags.Add("checks", context.Exception.ToString());
                }
                else if (context.Exception is QueueException qe) //消息队列异常(业务)
                {
                    result = Result.Fail(qe.Code, qe.Message);
                    traceSpan.tags.Add("queues", context.Exception.ToString());
                    traceSpan.tags["status"] = "500";
                }
                else
                {
                    result = Result.Fail(500, "服务器连接错误");
                    traceSpan.tags.Add("error", context.Exception.ToString());
                    traceSpan.tags["status"] = "500";
                }
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(result);
            }
          
            Log.Info(traceSpan.ToJson());
        }
    }
}
