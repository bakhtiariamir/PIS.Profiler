namespace PIS.Profiler.Core.Aspects
{
    internal interface ILocationInterceptionAspect : IAspect
    {
        void OnGetValue(LocationInterceptionArgs args);

        void OnSetValue(LocationInterceptionArgs args);
    }
}