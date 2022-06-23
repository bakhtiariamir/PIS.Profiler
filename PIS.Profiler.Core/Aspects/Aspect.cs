namespace PIS.Profiler.Core.Aspects
{
    public abstract class Aspect : Attribute, IAspect
    {
        public int AspectPriority { get; set; }
    }
}
