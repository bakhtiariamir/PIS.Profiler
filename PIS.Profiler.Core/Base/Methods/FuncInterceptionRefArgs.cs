using System.Reflection;
using PIS.Profiler.Core.Aspects;

namespace PIS.Profiler.Core.Base.Methods
{
    internal class FuncInterceptionRefArgs : MethodInterceptionArgs
    {
        private readonly Delegate _func;
        private readonly object[] _argsValues;

        public FuncInterceptionRefArgs(object instance, MethodInfo method, object[] argsValues, Delegate func)
            : base(instance, method, new Arguments(argsValues))
        {
            _func = func;
            _argsValues = argsValues;
        }
        public override void Proceed()
        {
            ReturnValue = _func.DynamicInvoke(_argsValues);
        }
    }
}
