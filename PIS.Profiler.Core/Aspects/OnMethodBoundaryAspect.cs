namespace PIS.Profiler.Core.Aspects
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class OnMethodBoundaryAspect : Aspect, IMethodAspect
    {
        public virtual void OnEntry(MethodExecutionArgs args) 
        { }
        public virtual void OnExit(MethodExecutionArgs args) 
        { }
        public virtual void OnSuccess(MethodExecutionArgs args)
        { }
        public virtual void OnException(MethodExecutionArgs args) 
        { }
    }
}
