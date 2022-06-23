namespace PIS.Profiler.Core.Aspects
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class MethodInterceptionAspect : Aspect, IMethodAspect
    {
        public virtual void OnInvoke(MethodInterceptionArgs args)
        {
            args.Proceed();
        }
    }
}