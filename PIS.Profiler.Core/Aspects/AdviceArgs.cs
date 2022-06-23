namespace PIS.Profiler.Core.Aspects
{
    public abstract class AdviceArgs
    {
        protected AdviceArgs(object instance)
        {
            Instance = instance;
        }

        public object Instance { get; private set; }
    }
}
