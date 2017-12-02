using CoreCommon.Configs;
using CoreCommon.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace startup.Filters
{
    /// <summary>
    /// 鉴权授权
    /// </summary>
    public class AuthFilterAttribute : Attribute, IActionFilter
    {
        private static List<string> authKey = ConfigManagerConf.GetReferenceValue("authKey");

        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var auth = context.HttpContext.Request.Headers["auth-key"];
            if (auth.Count == 0 || authKey[0] != auth[0])
            {
                context.Result = new ObjectResult(Result.Fail(1012,"权限不足"));
                return;
            }
        }
    }
}
