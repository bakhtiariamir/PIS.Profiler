namespace PIS.Profiler.Core.Aspects
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class LocationInterceptionAspect : Aspect, ILocationInterceptionAspect
    {
        public virtual void OnGetValue(LocationInterceptionArgs args)
        {
            args.ProceedGetValue();
        }
        public virtual void OnSetValue(LocationInterceptionArgs args)
        {
            args.ProceedSetValue();
        }
    }
}