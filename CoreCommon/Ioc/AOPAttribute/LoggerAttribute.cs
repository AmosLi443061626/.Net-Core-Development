using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CoreCommon.Ioc.AOPAttribute
{
    /// <summary>
    /// 日志属性
    /// </summary>
    public class LoggerAttribute : BaseAspectAttribute
    {

        public override void OnExcuting(IInvocation invocation)
        {
            Debug.WriteLine("日志执行前");
        }

        public override void OnExit(IInvocation invocation)
        {
            Debug.WriteLine("日志执行后");
        }
    }
}
