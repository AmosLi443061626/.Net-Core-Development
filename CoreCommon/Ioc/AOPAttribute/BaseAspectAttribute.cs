using System;
using Castle.DynamicProxy;

namespace CoreCommon.Ioc.AOPAttribute
{
    public class BaseAspectAttribute : Attribute
    {
        public virtual void OnExcuting(IInvocation invocation)
        {
           
        }

        public virtual void OnExit(IInvocation invocation)
        {
        }
    }
}
