using CoreCommon.Ioc.AOPAttribute;
using System.Reflection;

namespace CoreCommon.MessageMQ.Abstractions
{
    /// <summary>
    /// A descriptor of user definition method.
    /// </summary>
    public class ConsumerExecutorDescriptor
    {
        public MethodInfo MethodInfo { get; set; }

        public TypeInfo ImplTypeInfo { get; set; }

        public TopSubscribeAttribute Attribute { get; set; }
    }
}