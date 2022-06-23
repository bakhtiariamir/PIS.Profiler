using System.Reflection;
using PIS.Profiler.Core.Aspects;

namespace PIS.Profiler.Core.Base.Properties
{
    internal class PropertyInterceptionArgs : LocationInterceptionArgs
    {
        private readonly LateBoundGetter _getter;
        private readonly LateBoundSetter _setter;

        public PropertyInterceptionArgs(object instance, PropertyInfo property, object value) 
            : base(instance, property, value)
        {
            var getter = property.GetGetMethod(nonPublic: true);
            
            if (getter != null) _getter = DelegateFactory.CreateGetter(instance, getter);

            var setter = property.GetSetMethod(nonPublic: true);
            
            if (setter != null) _setter = DelegateFactory.CreateSetter(instance, setter);
        }

        public override object GetCurrentValue()
        {
            throw new NotImplementedException();
        }

        public override void ProceedGetValue()
        {
            Value = _getter();
        }

        public override void ProceedSetValue()
        {
            _setter(Value);
        }

        public override void SetNewValue(object value)
        {
            throw new NotImplementedException();
        }
    }
}
