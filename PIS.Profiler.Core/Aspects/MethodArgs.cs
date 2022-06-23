using System.Reflection;

namespace PIS.Profiler.Core.Aspects
{
    public abstract class MethodArgs : AdviceArgs
    {
        protected MethodArgs(object instance, MethodInfo method, Arguments arguments) : base(instance)
        {
            Method = method;
            Arguments = arguments;
        }
        public MethodInfo Method { get; private set; }

        public Arguments Arguments { get; private set; }
        public object ReturnValue { get; set; }
    }
}
