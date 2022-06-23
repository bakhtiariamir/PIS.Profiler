using System.Reflection;

namespace PIS.Profiler.Core.Aspects
{
    public class MethodExecutionArgs : MethodArgs
    {
        public MethodExecutionArgs(object instance, MethodInfo method, Arguments arguments) 
            : base(instance, method, arguments)
        { }

        public FlowBehavior FlowBehavior { get; set; }

        public Exception Exception { get; set; }
    }
}
