using System.Diagnostics;
using PIS.Profiler.Core.Aspects;

namespace PIS.Profiler.Core.Action

{
    public class ActionProfiler : MethodInterceptionAspect
    {
        private Stopwatch _stopWatch;
        private long _bkBefore;
        private long _bkAfter;
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            _stopWatch = new Stopwatch();
            BeforeExecute();
            args.Proceed();
            AfterExecute();
        }


        void BeforeExecute()
        {
            _bkBefore = GC.GetTotalMemory(false) / 1024;
            _stopWatch.Start();
            Console.WriteLine("----StartInvoke");
            Console.WriteLine(_bkBefore + " Started with this kb.");
        }

        void AfterExecute()
        {
            _stopWatch.Stop();
            _bkAfter = GC.GetTotalMemory(true) / 1024;
            Console.WriteLine(_bkAfter + " Amt. After Collection");
            Console.WriteLine(_bkAfter - _bkBefore + " Amt. Collected by GC.");
            TimeSpan ts = _stopWatch.Elapsed;

            Console.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine("----EndInvoke");

        }
    }
}
