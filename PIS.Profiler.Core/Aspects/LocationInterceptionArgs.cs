
using System.Reflection;

namespace PIS.Profiler.Core.Aspects
{
    public abstract class LocationInterceptionArgs : AdviceArgs
    {
        internal LocationInterceptionArgs(object instance, PropertyInfo property, object value) : base(instance)
        {
            Location = property;
            
            if (value != null) Value = value;

            else Value = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
        }
        public object Value { get; set; }

        public PropertyInfo Location { get; private set; }

        public abstract object GetCurrentValue();
        public abstract void ProceedGetValue();
        public abstract void ProceedSetValue();
        public abstract void SetNewValue(object value);
    }
}