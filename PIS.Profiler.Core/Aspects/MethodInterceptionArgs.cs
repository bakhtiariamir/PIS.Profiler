using System.Reflection;

namespace PIS.Profiler.Core.Aspects
{
    public abstract class MethodInterceptionArgs : MethodArgs
    {
        internal MethodInterceptionArgs(object instance, MethodInfo method, Arguments arguments) 
            : base(instance, method, arguments)
        { }

        public abstract void Proceed();
    }
}
