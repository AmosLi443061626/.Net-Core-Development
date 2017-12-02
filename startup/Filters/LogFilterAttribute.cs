using CoreCommon.Exceptions;
using CoreCommon.Extensions;
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
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logRequest = new LogRequest();
            _logRequest.Url = $"{context.HttpContext.Request.Host.Value}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
            _logRequest.Headers = context.HttpContext.Request.Headers.ToDictionary(k => k.Key, v => string.Join(";", v.Value.ToList()));
            _logRequest.ExcuteStartTime = DateTime.Now;
            _logRequest.RequestBodys = context.HttpContext.Request.ToReadAsyncJson();
            _logRequest.Cookie = context.HttpContext.Request.Cookies.ToJson();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logRequest.ExcuteEndTime = DateTime.Now;

            if (context.Exception == null)
            {
                _logRequest.ResponseBodys = (context.Result as ObjectResult).Value.ToJson();
                LogSuccess();
            }
            else
            {
                Result result = null;
                if (context.Exception is ChecksException ce) //验证异常(业务)
                {
                    result = Result.Fail(ce.Code, ce.Message);
                    _logRequest.ResponseBodys = result.ToJson();
                    LogSuccess();
                }
                else if (context.Exception is QueueException qe) //消息队列异常(业务)
                {
                    result = Result.Fail(qe.Code, qe.Message);
                    _logRequest.ResponseBodys = result.ToJson();
                    LogSuccess();
                }
                else
                {
                    _logRequest.ResponseBodys = context.Exception.ToString();
                    result = Result.Fail(500, "服务器连接错误");
                    Log.Error(LogFormat.Record("startupExpection", (_logRequest.ExcuteEndTime - _logRequest.ExcuteStartTime).Milliseconds, DateTime.Now, _logRequest));
                }
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(result.ToJson());
            }
        }

        /// <summary>
        /// 记录成功信息
        /// </summary>
        private void LogSuccess()
        {
            Log.Info(LogFormat.Record("startup", (_logRequest.ExcuteEndTime - _logRequest.ExcuteStartTime).Milliseconds, DateTime.Now, _logRequest));
        }
    }
}
