using System.Reflection;
using PIS.Profiler.Core.Aspects;
using PIS.Profiler.Core.Base;

namespace PIS.Profiler.Core.Base.Methods
{
    internal class FuncInterceptionArgs : MethodInterceptionArgs
    {
        readonly LateBoundFunc _func;
        readonly object[] _argsValues;

        public FuncInterceptionArgs(object instance, MethodInfo method, object[] argsValues, LateBoundFunc func)
            : base(instance, method, new Arguments(argsValues))
        {
            _func = func;
            _argsValues = argsValues;
        }

        public override void Proceed()
        {
            ReturnValue = _func.Invoke(_argsValues);
        }
    }
}
