using System.Reflection;
using PIS.Profiler.Core.Aspects;

namespace PIS.Profiler.Core.Base.Methods
{
    internal class ActionInterceptionArgs : MethodInterceptionArgs
    {
        readonly LateBoundAction _action;
        readonly object[] _argsValues;

        public ActionInterceptionArgs(object instance, MethodInfo method, object[] argsValues, LateBoundAction action) 
            : base(instance, method, new Arguments(argsValues))
        {
            _action = action;
            _argsValues = argsValues;
        }

        public override void Proceed()
        {
            _action.Invoke(_argsValues);
        }
    }
}
